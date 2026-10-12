using System.Threading.Tasks;
using Content.Server._FEC14.JobWhitelist;
using Content.Server.Administration;
using Content.Server.Administration.Managers;
using Content.Server.Administration.Notes;
using Content.Server.Database;
using Content.Server.EUI;
using Content.Shared._FEC14.Administration;
using Content.Shared.Administration;
using Content.Shared.Eui;
using Robust.Server.Player;

namespace Content.Server._FEC14.Administration;

public sealed class FECPlayerHistoryEui : BaseEui
{
    [Dependency] private readonly IAdminManager _admin = default!;
    [Dependency] private readonly IServerDbManager _db = default!;
    [Dependency] private readonly IEntityManager _entities = default!;
    [Dependency] private readonly IAdminNotesManager _notes = default!;
    [Dependency] private readonly IPlayerManager _players = default!;
    [Dependency] private readonly ILogManager _log = default!;

    private readonly LocatedPlayerData _target;
    private readonly List<string> _summary = new();
    private readonly List<FECPlayerHistoryEntry> _entries = new();
    private bool _loading = true;

    public FECPlayerHistoryEui(LocatedPlayerData target)
    {
        IoCManager.InjectDependencies(this);
        _target = target;
    }

    public override async void Opened()
    {
        base.Opened();
        _admin.OnPermsChanged += OnPermsChanged;
        StateDirty();

        try
        {
            await Load();
        }
        catch (Exception e)
        {
            _log.GetSawmill("fec.history").Error($"Failed to load history of {_target.Username}: {e}");
            _summary.Add(Loc.GetString("fec-history-load-error"));
        }

        _loading = false;
        if (!IsShutDown)
            StateDirty();
    }

    public override void Closed()
    {
        base.Closed();
        _admin.OnPermsChanged -= OnPermsChanged;
    }

    private void OnPermsChanged(AdminPermsChangedEventArgs args)
    {
        if (args.Player == Player && !_admin.HasAdminFlag(Player, AdminFlags.ViewNotes))
            Close();
    }

    public override EuiStateBase GetNewState()
    {
        return new FECPlayerHistoryEuiState(_target.Username, _loading, _summary, _entries);
    }

    private async Task Load()
    {
        var userId = _target.UserId;
        var playtime = (await _db.GetPlayTimes(userId.UserId)).Find(p => p.Tracker == "Overall")?.TimeSpent ?? TimeSpan.Zero;
        var whitelisted = await _db.GetWhitelistStatusAsync(userId);
        var record = await _db.GetPlayerRecordByUserId(userId);

        _summary.Add(Loc.GetString("fec-history-summary-online", ("online", _players.TryGetSessionById(userId, out _) ? 1 : 0)));
        _summary.Add(Loc.GetString("fec-history-summary-playtime", ("hours", (int) playtime.TotalHours), ("minutes", playtime.Minutes)));
        _summary.Add(Loc.GetString("fec-history-summary-whitelist", ("whitelisted", whitelisted ? 1 : 0)));
        if (record != null)
        {
            _summary.Add(Loc.GetString("fec-history-summary-seen",
                ("first", $"{record.FirstSeenTime.LocalDateTime:dd.MM.yyyy}"),
                ("last", $"{record.LastSeenTime.LocalDateTime:dd.MM.yyyy HH:mm}")));
        }

        foreach (var remark in await _notes.GetAllAdminRemarks(userId.UserId))
        {
            var admin = remark.CreatedBy?.LastSeenUserName ?? Loc.GetString("fec-history-system");
            var (kind, extra) = remark switch
            {
                ServerBanNoteRecord ban => (FECPlayerHistoryKind.Ban, Expiry(ban.ExpirationTime, ban.UnbanTime)),
                ServerRoleBanNoteRecord roleBan => (FECPlayerHistoryKind.RoleBan, string.Join(", ", roleBan.Roles) + " " + Expiry(roleBan.ExpirationTime, roleBan.UnbanTime)),
                AdminWatchlistRecord => (FECPlayerHistoryKind.Watchlist, string.Empty),
                AdminMessageRecord => (FECPlayerHistoryKind.Message, string.Empty),
                _ => (FECPlayerHistoryKind.Note, string.Empty),
            };

            _entries.Add(new FECPlayerHistoryEntry(kind, remark.CreatedAt.LocalDateTime,
                Entry(admin, remark.Message, extra.Trim())));
        }

        foreach (var chatBan in await _db.GetAllChatBans(userId.UserId))
        {
            var extra = chatBan.UnbannedAt != null
                ? Loc.GetString("fec-history-pardoned")
                : chatBan.ExpiresAt is { } expires ? Loc.GetString("fec-history-until", ("date", $"{expires.ToLocalTime():dd.MM.yyyy HH:mm}")) : Loc.GetString("fec-history-permanent");

            _entries.Add(new FECPlayerHistoryEntry(FECPlayerHistoryKind.ChatBan, chatBan.BannedAt.ToLocalTime(),
                Entry(chatBan.BanningAdmin?.LastSeenUserName ?? Loc.GetString("fec-history-system"), $"[{chatBan.Type}] {chatBan.Reason}", extra)));
        }

        var temp = _entities.System<FECTempJobWhitelistSystem>();
        foreach (var (user, _, role) in await _db.FECGetAllJobWhitelists())
        {
            if (user != userId.UserId)
                continue;

            var expiry = temp.GetExpiry(user, role);
            _entries.Add(new FECPlayerHistoryEntry(FECPlayerHistoryKind.Whitelist, null,
                Loc.GetString("fec-history-whitelist", ("job", role), ("temp", expiry != null ? 1 : 0),
                    ("date", expiry is { } date ? $"{date.ToLocalTime():dd.MM.yyyy HH:mm}" : string.Empty))));
        }

        _entries.Sort((a, b) => Nullable.Compare(b.Date, a.Date));
    }

    private static string Entry(string admin, string text, string extra)
    {
        return extra.Length == 0
            ? Loc.GetString("fec-history-entry", ("admin", admin), ("text", text))
            : Loc.GetString("fec-history-entry-extra", ("admin", admin), ("text", text), ("extra", extra));
    }

    private string Expiry(DateTimeOffset? expiration, DateTime? unban)
    {
        if (unban != null)
            return Loc.GetString("fec-history-pardoned");

        return expiration is { } expires
            ? Loc.GetString("fec-history-until", ("date", $"{expires.LocalDateTime:dd.MM.yyyy HH:mm}"))
            : Loc.GetString("fec-history-permanent");
    }
}
