using System.Linq;
using System.Threading.Tasks;
using Content.Server.Administration;
using Content.Server.Administration.Managers;
using Content.Server.Database;
using Content.Server.EUI;
using Content.Server.Players.JobWhitelist;
using Content.Shared._FEC14.JobWhitelist;
using Content.Shared.Administration;
using Content.Shared.Administration.Logs;
using Content.Shared.Database;
using Content.Shared.Eui;
using Content.Shared.Roles;
using Robust.Server.Player;
using Robust.Shared.Network;
using Robust.Shared.Prototypes;

namespace Content.Server._FEC14.JobWhitelist;

public sealed class FECJobWhitelistEui : BaseEui
{
    [Dependency] private readonly IAdminManager _admin = default!;
    [Dependency] private readonly ISharedAdminLogManager _adminLog = default!;
    [Dependency] private readonly IServerDbManager _db = default!;
    [Dependency] private readonly JobWhitelistManager _jobWhitelist = default!;
    [Dependency] private readonly IPlayerLocator _locator = default!;
    [Dependency] private readonly IPlayerManager _players = default!;
    [Dependency] private readonly IPrototypeManager _prototypes = default!;

    private readonly List<(Guid UserId, string Name, string RoleId)> _entries = new();
    private string? _status;

    public FECJobWhitelistEui()
    {
        IoCManager.InjectDependencies(this);
    }

    public override async void Opened()
    {
        _admin.OnPermsChanged += OnPermsChanged;
        await Load();
    }

    public override void Closed()
    {
        _admin.OnPermsChanged -= OnPermsChanged;
    }

    private void OnPermsChanged(AdminPermsChangedEventArgs args)
    {
        if (args.Player == Player && !CanUse())
            Close();
    }

    private bool CanUse()
    {
        return _admin.HasAdminFlag(Player, AdminFlags.JobWhitelist);
    }

    private async Task Load()
    {
        var rows = await _db.FECGetAllJobWhitelists();
        if (IsShutDown)
            return;

        _entries.Clear();
        _entries.AddRange(rows);
        StateDirty();
    }

    public override EuiStateBase GetNewState()
    {
        var entries = _entries
            .Select(e => new FECJobWhitelistEntry(
                e.UserId,
                e.Name,
                e.RoleId,
                _players.TryGetSessionById(new NetUserId(e.UserId), out _)))
            .ToList();

        return new FECJobWhitelistEuiState(entries, _status);
    }

    public override async void HandleMessage(EuiMessageBase msg)
    {
        base.HandleMessage(msg);

        if (!CanUse())
        {
            Close();
            return;
        }

        switch (msg)
        {
            case FECJobWhitelistAddMsg add:
                await Add(add.Player.Trim(), add.JobId);
                break;
            case FECJobWhitelistRemoveMsg remove:
                Remove(remove.UserId, remove.JobId);
                break;
            case FECJobWhitelistRefreshMsg:
                _status = null;
                await Load();
                break;
        }
    }

    private async Task Add(string player, string jobId)
    {
        if (!_prototypes.TryIndex<JobPrototype>(jobId, out var job))
        {
            SetStatus(Loc.GetString("cmd-jobwhitelist-job-does-not-exist", ("job", jobId)));
            return;
        }

        if (player.Length == 0)
        {
            SetStatus(Loc.GetString("fec-job-whitelist-ui-no-player"));
            return;
        }

        var data = await _locator.LookupIdByNameOrIdAsync(player);
        if (data == null)
        {
            SetStatus(Loc.GetString("cmd-jobwhitelist-player-not-found", ("player", player)));
            return;
        }

        var userId = data.UserId.UserId;
        if (_entries.Any(e => e.UserId == userId && e.RoleId == job.ID))
        {
            SetStatus(Loc.GetString("fec-job-whitelist-ui-already", ("player", data.Username), ("job", job.LocalizedName)));
            return;
        }

        _jobWhitelist.AddWhitelist(data.UserId, job.ID);
        _entries.Add((userId, data.Username, job.ID));
        _adminLog.Add(LogType.Action, LogImpact.Medium, $"{Player:player} added job whitelist {job.ID} to {data.Username}");
        SetStatus(Loc.GetString("fec-job-whitelist-ui-added", ("player", data.Username), ("job", job.LocalizedName)));
    }

    private void Remove(Guid userId, string jobId)
    {
        var index = _entries.FindIndex(e => e.UserId == userId && e.RoleId == jobId);
        if (index < 0)
            return;

        var name = _entries[index].Name;
        _jobWhitelist.RemoveWhitelist(new NetUserId(userId), jobId);
        _entries.RemoveAt(index);
        _adminLog.Add(LogType.Action, LogImpact.Medium, $"{Player:player} removed job whitelist {jobId} from {name}");
        SetStatus(Loc.GetString("fec-job-whitelist-ui-removed", ("player", name), ("job", JobName(jobId))));
    }

    private string JobName(string jobId)
    {
        return _prototypes.TryIndex<JobPrototype>(jobId, out var job) ? job.LocalizedName : jobId;
    }

    private void SetStatus(string status)
    {
        _status = status;
        if (!IsShutDown)
            StateDirty();
    }
}
