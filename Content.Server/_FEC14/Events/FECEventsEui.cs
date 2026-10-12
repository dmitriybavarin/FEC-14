using System.Linq;
using Content.Server._FEC14.JobWhitelist;
using Content.Server.Administration;
using Content.Server.Administration.Managers;
using Content.Server.EUI;
using Content.Shared._FEC14.Events;
using Content.Shared.Administration;
using Content.Shared.Eui;
using Robust.Server.Player;
using Robust.Shared.Prototypes;

namespace Content.Server._FEC14.Events;

public sealed class FECEventsEui : BaseEui
{
    [Dependency] private readonly IAdminManager _admin = default!;
    [Dependency] private readonly IEntityManager _entities = default!;
    [Dependency] private readonly IPlayerManager _players = default!;
    [Dependency] private readonly IPrototypeManager _prototypes = default!;

    private readonly FECEventsSystem _events;
    private string? _status;

    public FECEventsEui()
    {
        IoCManager.InjectDependencies(this);
        _events = _entities.System<FECEventsSystem>();
    }

    public override void Opened()
    {
        _admin.OnPermsChanged += OnPermsChanged;
        _events.Changed += StateDirty;
        StateDirty();
    }

    public override void Closed()
    {
        _admin.OnPermsChanged -= OnPermsChanged;
        _events.Changed -= StateDirty;
    }

    private void OnPermsChanged(AdminPermsChangedEventArgs args)
    {
        if (args.Player == Player && !CanUse())
            Close();
    }

    private bool CanUse()
    {
        return _admin.HasAdminFlag(Player, AdminFlags.Round);
    }

    public override EuiStateBase GetNewState()
    {
        var entries = _prototypes.EnumeratePrototypes<FECEventPrototype>()
            .OrderBy(p => p.ID)
            .Select(p => new FECEventsEuiEntry(
                p.ID,
                Loc.GetString(p.Name),
                _events.GetStatus(p),
                p.MinPlayers,
                p.Weight,
                _events.IsBlocked(p.ID, out _)))
            .ToList();

        var log = _events.EventLog
            .Select(l => Loc.GetString("fec-events-ui-log-line",
                ("time", $"{(int) l.At.TotalMinutes:00}:{l.At.Seconds:00}"),
                ("text", l.Text)))
            .ToList();

        var info = Loc.GetString("fec-events-ui-info",
            ("players", _players.PlayerCount),
            ("slots", string.Join(", ", _events.Slots.Select(s => (int) s.TotalMinutes))),
            ("slotCount", _events.Slots.Count),
            ("vote", _events.VoteRunning ? 1 : 0));

        return new FECEventsEuiState(entries, log, info, _status);
    }

    public override void HandleMessage(EuiMessageBase msg)
    {
        base.HandleMessage(msg);

        if (!CanUse())
        {
            Close();
            return;
        }

        var admin = Player.Name;
        switch (msg)
        {
            case FECEventsStartMsg start:
                _status = _events.TryStart(start.Id, true, out var reason)
                    ? Loc.GetString("fec-events-started", ("id", start.Id))
                    : reason;
                break;
            case FECEventsBlockMsg block:
            {
                TimeSpan? duration = null;
                var text = block.Duration.Trim();
                if (text.Length > 0 && text != "forever")
                {
                    if (!FECTempJobWhitelistSystem.TryParseDuration(text, out var parsed))
                    {
                        _status = Loc.GetString("fec-temp-whitelist-bad-duration", ("duration", text));
                        break;
                    }

                    duration = parsed;
                }

                if (!_prototypes.HasIndex<FECEventPrototype>(block.Id))
                {
                    _status = Loc.GetString("fec-events-unknown", ("id", block.Id));
                    break;
                }

                _events.Block(block.Id, duration, admin);
                _status = Loc.GetString("fec-events-blocked-done", ("id", block.Id));
                break;
            }
            case FECEventsUnblockMsg unblock:
                _status = Loc.GetString(_events.Unblock(unblock.Id, admin) ? "fec-events-unblocked" : "fec-events-not-blocked", ("id", unblock.Id));
                break;
            case FECEventsResetRotationMsg:
                _events.ResetRotation(admin);
                _status = Loc.GetString("fec-events-rotation-reset");
                break;
        }

        StateDirty();
    }
}
