using System.IO;
using System.Linq;
using System.Text.Json;
using Content.Server._FEC14.GameTicking;
using Content.Server.Administration.Logs;
using Content.Server.Chat.Managers;
using Content.Server.GameTicking;
using Content.Server.Spawners.Components;
using Content.Server.Voting;
using Content.Server.Voting.Managers;
using Content.Shared._FEC14.CCVar;
using Content.Shared._FEC14.Events;
using Content.Shared._RMC14.Marines;
using Content.Shared._RMC14.Rules;
using Content.Shared.Database;
using Content.Shared.GameTicking;
using Robust.Server.Player;
using Robust.Shared.Configuration;
using Robust.Shared.Map;
using Robust.Shared.Network;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;

namespace Content.Server._FEC14.Events;

public sealed class FECEventsSystem : EntitySystem
{
    [Dependency] private readonly IAdminLogManager _adminLog = default!;
    [Dependency] private readonly IChatManager _chat = default!;
    [Dependency] private readonly IConfigurationManager _cfg = default!;
    [Dependency] private readonly IPlayerManager _players = default!;
    [Dependency] private readonly IPrototypeManager _prototypes = default!;
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly FECRoundSummarySystem _summary = default!;
    [Dependency] private readonly GameTicker _ticker = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;
    [Dependency] private readonly IVoteManager _votes = default!;

    private readonly List<TimeSpan> _slots = new();
    private readonly List<(string Id, TimeSpan At)> _queued = new();
    private readonly HashSet<string> _started = new();
    private readonly Dictionary<EntityUid, string> _ruleEvents = new();
    private readonly List<(TimeSpan At, string Text)> _log = new();
    private readonly Dictionary<string, DateTime?> _blocks = new();
    private readonly HashSet<string> _rotationUsed = new();
    private readonly List<(string Id, TimeSpan At, bool Chained)> _awaiting = new();
    private readonly HashSet<TimeSpan> _warnedSlots = new();
    private readonly Dictionary<string, Dictionary<string, int>> _roles = new();
    private string? _rolesPath;
    private string? _blocksPath;
    private string? _rotationPath;
    private IVoteHandle? _vote;
    private bool _planDone;

    public event Action? Changed;

    public IReadOnlyList<(TimeSpan At, string Text)> EventLog => _log;
    public IReadOnlyList<TimeSpan> Slots => _slots;
    public IReadOnlyList<(string Id, TimeSpan At)> Queued => _queued;
    public bool VoteRunning => _vote != null;
    public IReadOnlyList<(string Id, TimeSpan At, bool Chained)> Awaiting => _awaiting;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<RoundRestartCleanupEvent>(OnCleanup);
    }

    private void OnCleanup(RoundRestartCleanupEvent ev)
    {
        var vote = _vote;
        _vote = null;
        vote?.Cancel();

        _slots.Clear();
        _queued.Clear();
        _started.Clear();
        _ruleEvents.Clear();
        _log.Clear();
        _awaiting.Clear();
        _warnedSlots.Clear();
        _planDone = false;
        Changed?.Invoke();
    }

    public override void Update(float frameTime)
    {
        if (_ticker.RunLevel != GameRunLevel.InRound)
            return;

        if (!_planDone)
        {
            _planDone = true;
            Plan();
        }

        if (_slots.Count == 0 && _queued.Count == 0 && _awaiting.Count == 0)
            return;

        var now = _ticker.RoundDuration();
        WarnLowOnline(now);
        for (var i = _awaiting.Count - 1; i >= 0; i--)
        {
            if (_awaiting[i].At > now)
                continue;

            var (awaitingId, _, awaitingChained) = _awaiting[i];
            _awaiting.RemoveAt(i);
            StartNow(awaitingId, awaitingChained);
        }

        for (var i = _slots.Count - 1; i >= 0; i--)
        {
            if (_slots[i] > now)
                continue;

            _slots.RemoveAt(i);
            RunSlot(now);
        }

        for (var i = _queued.Count - 1; i >= 0; i--)
        {
            if (_queued[i].At > now)
                continue;

            var id = _queued[i].Id;
            _queued.RemoveAt(i);
            Await(id, true);
        }
    }

    private void Plan()
    {
        if (!_cfg.GetCVar(FECCVars.EventsEnabled))
            return;

        var rotation = RotationPrototypes().ToList();
        if (rotation.Count == 0)
            return;

        var earliest = rotation.Min(e => e.EarliestStart);
        var latest = rotation.Max(e => e.LatestStart);
        var chance = Math.Clamp(_cfg.GetCVar(FECCVars.EventsChance), 0, 1);
        var max = _cfg.GetCVar(FECCVars.EventsMaxPerRound);
        for (var i = 0; i < max; i++)
        {
            if (!_random.Prob(chance))
                continue;

            var spread = Math.Max(0, (latest - earliest).TotalSeconds);
            _slots.Add(earliest + TimeSpan.FromSeconds(_random.NextDouble() * spread));
        }

        _slots.Sort();
        foreach (var slot in _slots)
        {
            AddLog(Loc.GetString("fec-events-log-slot", ("minutes", (int) slot.TotalMinutes)));
        }

        if (_slots.Count == 0)
            AddLog(Loc.GetString("fec-events-log-no-roll"));
    }

    private void RunSlot(TimeSpan now)
    {
        var candidates = GetCandidates(now);
        if (candidates.Count == 0)
        {
            AddLog(Loc.GetString("fec-events-log-no-candidates"));
            return;
        }

        if (candidates.Count == 1 || _vote != null || !_cfg.GetCVar(FECCVars.EventsVoteEnabled))
        {
            Launch(PickWeighted(candidates).ID);
            return;
        }

        var options = new List<FECEventPrototype>();
        var count = Math.Max(2, _cfg.GetCVar(FECCVars.EventsVoteOptions));
        while (candidates.Count > 0 && options.Count < count)
        {
            var pick = PickWeighted(candidates);
            candidates.Remove(pick);
            options.Add(pick);
        }

        StartVote(options);
    }

    private void StartVote(List<FECEventPrototype> options)
    {
        var voteOptions = new VoteOptions
        {
            Title = Loc.GetString("fec-events-vote-title"),
            InitiatorText = Loc.GetString("fec-events-vote-initiator"),
            Duration = TimeSpan.FromSeconds(Math.Max(10, _cfg.GetCVar(FECCVars.EventsVoteSeconds))),
            VoterEligibility = VoteManager.VoterEligibility.Ghost,
            DisplayVotes = true,
        };

        foreach (var option in options)
        {
            voteOptions.Options.Add((Loc.GetString(option.Name), option.ID));
        }

        var ids = options.Select(o => o.ID).ToList();
        var vote = _votes.CreateVote(voteOptions);
        _vote = vote;
        AddLog(Loc.GetString("fec-events-log-vote", ("names", string.Join(", ", ids.Select(EventName)))));

        vote.OnFinished += (_, args) =>
        {
            if (_vote != vote)
                return;

            _vote = null;
            var winner = args.Winner as string ?? (args.Winners.Length > 0 ? (string) _random.Pick(args.Winners) : _random.Pick(ids));
            AddLog(Loc.GetString("fec-events-log-vote-result", ("name", EventName(winner))));
            Launch(winner);
        };

        vote.OnCancelled += _ =>
        {
            if (_vote != vote)
                return;

            _vote = null;
            var pick = _random.Pick(ids);
            AddLog(Loc.GetString("fec-events-log-vote-cancelled", ("name", EventName(pick))));
            Launch(pick);
        };
    }

    private void Launch(string id)
    {
        if (_ticker.RunLevel != GameRunLevel.InRound)
            return;

        Await(id, false);
    }

    private void Await(string id, bool chained)
    {
        var seconds = _cfg.GetCVar(FECCVars.EventsConfirmSeconds);
        if (seconds <= 0)
        {
            StartNow(id, chained);
            return;
        }

        _awaiting.Add((id, _ticker.RoundDuration() + TimeSpan.FromSeconds(seconds), chained));
        var text = Loc.GetString("fec-events-admin-awaiting", ("name", EventName(id)), ("seconds", seconds));
        _chat.SendAdminAlert(text);
        AddLog(text);
    }

    private void StartNow(string id, bool chained)
    {
        if (!TryStart(id, false, out var reason, chained))
        {
            AddLog(Loc.GetString(chained ? "fec-events-log-chain-skipped" : "fec-events-log-skipped", ("name", EventName(id)), ("reason", reason)));
            return;
        }

        if (!chained)
            MarkRotation(id);
    }

    public bool CancelAwaiting(string id, string admin)
    {
        var removed = _awaiting.RemoveAll(a => a.Id == id) + _queued.RemoveAll(q => q.Id == id);
        if (removed == 0)
            return false;

        var text = Loc.GetString("fec-events-log-cancelled", ("name", EventName(id)), ("admin", admin));
        _chat.SendAdminAlert(text);
        AddLog(text);
        return true;
    }

    public bool ConfirmAwaiting(string id)
    {
        var index = _awaiting.FindIndex(a => a.Id == id);
        if (index < 0)
            return false;

        var chained = _awaiting[index].Chained;
        _awaiting.RemoveAt(index);
        StartNow(id, chained);
        return true;
    }

    private void WarnLowOnline(TimeSpan now)
    {
        var lead = TimeSpan.FromMinutes(Math.Max(0, _cfg.GetCVar(FECCVars.EventsLowOnlineWarnMinutes)));
        foreach (var slot in _slots)
        {
            if (slot - lead > now || !_warnedSlots.Add(slot))
                continue;

            if (GetCandidates(slot).Count > 0)
                continue;

            var needed = GetCandidates(slot, true).Select(e => e.MinPlayers).DefaultIfEmpty(0).Min();
            var text = needed > _players.PlayerCount
                ? Loc.GetString("fec-events-admin-low-online", ("minutes", (int) slot.TotalMinutes), ("players", _players.PlayerCount), ("needed", needed))
                : Loc.GetString("fec-events-admin-no-candidates", ("minutes", (int) slot.TotalMinutes));
            _chat.SendAdminAlert(text);
            AddLog(text);
        }

        foreach (var (id, at) in _queued)
        {
            if (at - lead > now || !_warnedSlots.Add(at) ||
                !_prototypes.TryIndex<FECEventPrototype>(id, out var proto) ||
                _players.PlayerCount >= proto.MinPlayers)
            {
                continue;
            }

            var text = Loc.GetString("fec-events-admin-low-online-chain", ("name", Loc.GetString(proto.Name)), ("players", _players.PlayerCount), ("needed", proto.MinPlayers));
            _chat.SendAdminAlert(text);
            AddLog(text);
        }
    }

    private List<FECEventPrototype> GetCandidates(TimeSpan now, bool ignorePlayers = false)
    {
        LoadRotation();
        var hasPlanet = HasPlanet();
        return RotationPrototypes()
            .Where(e => !_rotationUsed.Contains(e.ID) &&
                        !_started.Contains(e.ID) &&
                        e.EarliestStart <= now && now <= e.LatestStart &&
                        (ignorePlayers || _players.PlayerCount >= e.MinPlayers) &&
                        (!e.RequiresPlanet || hasPlanet))
            .ToList();
    }

    private IEnumerable<FECEventPrototype> RotationPrototypes()
    {
        return _prototypes.EnumeratePrototypes<FECEventPrototype>()
            .Where(e => e.InRotation && e.Weight > 0 && !IsBlocked(e.ID, out _));
    }

    private FECEventPrototype PickWeighted(List<FECEventPrototype> pool)
    {
        var total = pool.Sum(e => e.Weight);
        var roll = _random.NextFloat() * total;
        foreach (var e in pool)
        {
            roll -= e.Weight;
            if (roll <= 0)
                return e;
        }

        return pool[^1];
    }

    public bool TryStart(string id, bool forced, out string reason, bool chained = false)
    {
        reason = string.Empty;
        if (!_prototypes.TryIndex<FECEventPrototype>(id, out var proto))
        {
            reason = Loc.GetString("fec-events-unknown", ("id", id));
            return false;
        }

        if (!forced)
        {
            if (!_cfg.GetCVar(FECCVars.EventsEnabled))
            {
                reason = Loc.GetString("fec-events-disabled");
                return false;
            }

            if (IsBlocked(id, out _))
            {
                reason = Loc.GetString("fec-events-blocked", ("id", id));
                return false;
            }

            if (_players.PlayerCount < proto.MinPlayers)
            {
                reason = Loc.GetString("fec-events-not-enough-players", ("min", proto.MinPlayers), ("current", _players.PlayerCount));
                return false;
            }
        }

        if (proto.RequiresPlanet && !HasPlanet())
        {
            reason = Loc.GetString("fec-events-no-planet");
            return false;
        }

        var rule = _ticker.AddGameRule(proto.Rule);
        _ruleEvents[rule] = id;
        _started.Add(id);
        AddLog(Loc.GetString(forced ? "fec-events-log-started-admin" : chained ? "fec-events-log-started-chain" : "fec-events-log-started",
            ("name", Loc.GetString(proto.Name))));

        if (!_ticker.StartGameRule(rule))
        {
            _ruleEvents.Remove(rule);
            _started.Remove(id);
            QueueDel(rule);
            reason = Loc.GetString("fec-events-rule-failed");
            return false;
        }

        _adminLog.Add(LogType.EventStarted, LogImpact.High, $"FEC event {id} started{(forced ? " by admin" : chained ? " by chain" : string.Empty)}");
        _summary.AddLine(Loc.GetString("fec-events-summary-started", ("name", Loc.GetString(proto.Name))));
        return true;
    }

    public void ReportOutcome(EntityUid rule, string outcome)
    {
        if (!_ruleEvents.TryGetValue(rule, out var id) ||
            !_prototypes.TryIndex<FECEventPrototype>(id, out var proto))
        {
            return;
        }

        var outcomeText = Loc.TryGetString($"fec-events-outcome-{outcome}", out var text) ? text : outcome;
        AddLog(Loc.GetString("fec-events-log-outcome", ("name", Loc.GetString(proto.Name)), ("outcome", outcomeText)));

        var now = _ticker.RoundDuration();
        foreach (var chain in proto.Chains)
        {
            if (chain.Outcome != outcome || !_random.Prob(Math.Clamp(chain.Chance, 0, 1)))
                continue;

            if (IsBlocked(chain.Event, out _))
            {
                AddLog(Loc.GetString("fec-events-log-chain-blocked", ("name", EventName(chain.Event))));
                continue;
            }

            var at = now + chain.Delay;
            _queued.Add((chain.Event, at));
            AddLog(Loc.GetString("fec-events-log-chain", ("name", EventName(chain.Event)), ("minutes", (int) at.TotalMinutes)));
        }
    }

    public bool HasPlanet()
    {
        return GetPlanetMap() != null;
    }

    public MapId? GetPlanetMap()
    {
        var query = EntityQueryEnumerator<CMDistressSignalRuleComponent>();
        while (query.MoveNext(out var rule))
        {
            if (rule.XenoMap is { } xenoMap && !TerminatingOrDeleted(xenoMap))
                return Transform(xenoMap).MapID;
        }

        return null;
    }

    public bool TryGetPlanetSpawn(out EntityCoordinates coordinates)
    {
        coordinates = default;
        if (GetPlanetMap() is not { } planet)
            return false;

        var points = new List<EntityUid>();
        var query = EntityQueryEnumerator<SpawnPointComponent, TransformComponent>();
        while (query.MoveNext(out var uid, out var point, out var xform))
        {
            if (xform.MapID == planet && point.Job != null)
                points.Add(uid);
        }

        if (points.Count == 0)
            return false;

        coordinates = _transform.GetMoverCoordinates(_random.Pick(points));
        return true;
    }

    public bool TryGetShipSpawn(out EntityCoordinates coordinates)
    {
        coordinates = default;
        var points = new List<EntityUid>();
        var query = EntityQueryEnumerator<SpawnPointComponent, TransformComponent>();
        while (query.MoveNext(out var uid, out _, out var xform))
        {
            if (xform.GridUid is { } grid && HasComp<AlmayerComponent>(grid))
                points.Add(uid);
        }

        if (points.Count == 0)
            return false;

        coordinates = _transform.GetMoverCoordinates(_random.Pick(points));
        return true;
    }

    public bool IsBlocked(string id, out DateTime? until)
    {
        LoadBlocks();
        until = null;
        if (!_blocks.TryGetValue(id, out until))
            return false;

        if (until is { } date && date <= DateTime.UtcNow)
        {
            _blocks.Remove(id);
            SaveBlocks();
            until = null;
            return false;
        }

        return true;
    }

    public void Block(string id, TimeSpan? duration, string? admin = null)
    {
        LoadBlocks();
        _blocks[id] = duration is { } time ? DateTime.UtcNow + time : null;
        SaveBlocks();
        _queued.RemoveAll(p => p.Id == id);
        AddLog(Loc.GetString("fec-events-log-blocked", ("name", EventName(id)), ("admin", admin ?? "?")));
    }

    public bool Unblock(string id, string? admin = null)
    {
        LoadBlocks();
        if (!_blocks.Remove(id))
            return false;

        SaveBlocks();
        AddLog(Loc.GetString("fec-events-log-unblocked", ("name", EventName(id)), ("admin", admin ?? "?")));
        return true;
    }

    public bool IsRotationUsed(string id)
    {
        LoadRotation();
        return _rotationUsed.Contains(id);
    }

    public void ResetRotation(string? admin = null)
    {
        LoadRotation();
        _rotationUsed.Clear();
        SaveRotation();
        AddLog(Loc.GetString("fec-events-log-rotation-reset", ("admin", admin ?? "?")));
    }

    private void MarkRotation(string id)
    {
        if (!_prototypes.TryIndex<FECEventPrototype>(id, out var proto) || !proto.InRotation)
            return;

        LoadRotation();
        _rotationUsed.Add(id);
        var pool = RotationPrototypes().Select(e => e.ID).ToList();
        if (pool.Count > 0 && pool.All(_rotationUsed.Contains))
        {
            _rotationUsed.Clear();
            AddLog(Loc.GetString("fec-events-log-rotation-done"));
        }

        SaveRotation();
    }

    public string GetStatus(FECEventPrototype proto)
    {
        if (_started.Contains(proto.ID))
            return Loc.GetString("fec-events-status-started");

        if (IsBlocked(proto.ID, out var until))
        {
            return until is { } date
                ? Loc.GetString("fec-events-status-blocked-until", ("date", $"{date.ToLocalTime():dd.MM.yyyy HH:mm}"))
                : Loc.GetString("fec-events-status-blocked");
        }

        foreach (var (id, at) in _queued)
        {
            if (id == proto.ID)
                return Loc.GetString("fec-events-status-planned", ("minutes", (int) at.TotalMinutes));
        }

        foreach (var (id, at, _) in _awaiting)
        {
            if (id == proto.ID)
                return Loc.GetString("fec-events-status-awaiting", ("time", $"{(int) at.TotalMinutes:00}:{at.Seconds:00}"));
        }

        if (proto.InRotation && IsRotationUsed(proto.ID))
            return Loc.GetString("fec-events-status-rotation");

        return Loc.GetString("fec-events-status-available");
    }

    public bool IsOnRoleCooldown(NetUserId user, string role)
    {
        LoadRoles();
        var rounds = _cfg.GetCVar(FECCVars.EventsRoleCooldownRounds);
        return rounds > 0 &&
               _roles.TryGetValue(user.ToString(), out var roles) &&
               roles.TryGetValue(role, out var round) &&
               _ticker.RoundId - round <= rounds;
    }

    public void RecordRole(NetUserId user, string role)
    {
        LoadRoles();
        if (!_roles.TryGetValue(user.ToString(), out var roles))
        {
            roles = new Dictionary<string, int>();
            _roles[user.ToString()] = roles;
        }

        roles[role] = _ticker.RoundId;
        WriteJson(_cfg.GetCVar(FECCVars.EventsRolesFile), _roles);
    }

    private void LoadRoles()
    {
        var path = _cfg.GetCVar(FECCVars.EventsRolesFile);
        if (path == _rolesPath)
            return;

        _rolesPath = path;
        _roles.Clear();
        if (ReadJson<Dictionary<string, Dictionary<string, int>>>(path) is not { } loaded)
            return;

        foreach (var (user, roles) in loaded)
        {
            _roles[user] = roles;
        }
    }

    private string EventName(string id)
    {
        return _prototypes.TryIndex<FECEventPrototype>(id, out var proto) ? Loc.GetString(proto.Name) : id;
    }

    private void AddLog(string text)
    {
        var at = _ticker.RunLevel == GameRunLevel.InRound ? _ticker.RoundDuration() : TimeSpan.Zero;
        _log.Add((at, text));
        if (_log.Count > 200)
            _log.RemoveAt(0);

        Log.Info(text);
        Changed?.Invoke();
    }

    private void LoadBlocks()
    {
        var path = _cfg.GetCVar(FECCVars.EventsBlocksFile);
        if (path == _blocksPath)
            return;

        _blocksPath = path;
        _blocks.Clear();
        var loaded = ReadJson<Dictionary<string, DateTime?>>(path);
        if (loaded == null)
            return;

        foreach (var (key, value) in loaded)
        {
            _blocks[key] = value;
        }
    }

    private void SaveBlocks()
    {
        var path = _cfg.GetCVar(FECCVars.EventsBlocksFile);
        WriteJson(path, _blocks);
        _blocksPath = path;
    }

    private void LoadRotation()
    {
        var path = _cfg.GetCVar(FECCVars.EventsRotationFile);
        if (path == _rotationPath)
            return;

        _rotationPath = path;
        _rotationUsed.Clear();
        if (ReadJson<List<string>>(path) is { } loaded)
            _rotationUsed.UnionWith(loaded);
    }

    private void SaveRotation()
    {
        var path = _cfg.GetCVar(FECCVars.EventsRotationFile);
        WriteJson(path, _rotationUsed.OrderBy(x => x).ToList());
        _rotationPath = path;
    }

    private T? ReadJson<T>(string path) where T : class
    {
        if (!File.Exists(path))
            return null;

        try
        {
            return JsonSerializer.Deserialize<T>(File.ReadAllText(path));
        }
        catch (Exception e)
        {
            Log.Error($"Failed to read {path}: {e.Message}");
            return null;
        }
    }

    private void WriteJson<T>(string path, T value)
    {
        try
        {
            var dir = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(dir))
                Directory.CreateDirectory(dir);

            File.WriteAllText(path, JsonSerializer.Serialize(value, new JsonSerializerOptions { WriteIndented = true }));
        }
        catch (Exception e)
        {
            Log.Error($"Failed to write {path}: {e.Message}");
        }
    }
}
