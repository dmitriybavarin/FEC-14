using System.IO;
using System.Linq;
using System.Text.Json;
using Content.Server._FEC14.GameTicking;
using Content.Server.Administration.Logs;
using Content.Server.GameTicking;
using Content.Server.Spawners.Components;
using Content.Shared._FEC14.CCVar;
using Content.Shared._FEC14.Events;
using Content.Shared._RMC14.Rules;
using Content.Shared.Database;
using Content.Shared.GameTicking;
using Robust.Server.Player;
using Robust.Shared.Configuration;
using Robust.Shared.Map;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;

namespace Content.Server._FEC14.Events;

public sealed class FECEventsSystem : EntitySystem
{
    [Dependency] private readonly IAdminLogManager _adminLog = default!;
    [Dependency] private readonly IConfigurationManager _cfg = default!;
    [Dependency] private readonly IPlayerManager _players = default!;
    [Dependency] private readonly IPrototypeManager _prototypes = default!;
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly FECRoundSummarySystem _summary = default!;
    [Dependency] private readonly GameTicker _ticker = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;

    private readonly List<(string Id, TimeSpan At)> _planned = new();
    private readonly HashSet<string> _started = new();
    private readonly Dictionary<string, DateTime?> _blocks = new();
    private string? _blocksPath;
    private bool _planDone;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<RoundRestartCleanupEvent>(OnCleanup);
    }

    private void OnCleanup(RoundRestartCleanupEvent ev)
    {
        _planned.Clear();
        _started.Clear();
        _planDone = false;
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

        if (_planned.Count == 0)
            return;

        var now = _ticker.RoundDuration();
        for (var i = _planned.Count - 1; i >= 0; i--)
        {
            if (_planned[i].At > now)
                continue;

            var id = _planned[i].Id;
            _planned.RemoveAt(i);
            if (!TryStart(id, false, out var reason))
                Log.Info($"FEC event {id} skipped: {reason}");
        }
    }

    private void Plan()
    {
        if (!_cfg.GetCVar(FECCVars.EventsEnabled) || !_random.Prob(Math.Clamp(_cfg.GetCVar(FECCVars.EventsChance), 0, 1)))
            return;

        var pool = _prototypes.EnumeratePrototypes<FECEventPrototype>()
            .Where(e => !IsBlocked(e.ID, out _) && e.Weight > 0)
            .ToList();

        var max = _cfg.GetCVar(FECCVars.EventsMaxPerRound);
        while (pool.Count > 0 && _planned.Count < max)
        {
            var pick = PickWeighted(pool);
            pool.Remove(pick);

            var spread = Math.Max(0, (pick.LatestStart - pick.EarliestStart).TotalSeconds);
            var at = pick.EarliestStart + TimeSpan.FromSeconds(_random.NextDouble() * spread);
            _planned.Add((pick.ID, at));
            Log.Info($"FEC event {pick.ID} planned at {at}");
        }
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

    public bool TryStart(string id, bool forced, out string reason)
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

        if (!_ticker.StartGameRule(proto.Rule, out _))
        {
            reason = Loc.GetString("fec-events-rule-failed");
            return false;
        }

        _started.Add(id);
        _adminLog.Add(LogType.EventStarted, LogImpact.High, $"FEC event {id} started{(forced ? " by admin" : string.Empty)}");
        _summary.AddLine(Loc.GetString("fec-events-summary-started", ("name", Loc.GetString(proto.Name))));
        return true;
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

    public void Block(string id, TimeSpan? duration)
    {
        LoadBlocks();
        _blocks[id] = duration is { } time ? DateTime.UtcNow + time : null;
        SaveBlocks();
        _planned.RemoveAll(p => p.Id == id);
    }

    public bool Unblock(string id)
    {
        LoadBlocks();
        if (!_blocks.Remove(id))
            return false;

        SaveBlocks();
        return true;
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

        foreach (var (id, at) in _planned)
        {
            if (id == proto.ID)
                return Loc.GetString("fec-events-status-planned", ("minutes", (int) at.TotalMinutes));
        }

        return Loc.GetString("fec-events-status-available");
    }

    private void LoadBlocks()
    {
        var path = _cfg.GetCVar(FECCVars.EventsBlocksFile);
        if (path == _blocksPath)
            return;

        _blocksPath = path;
        _blocks.Clear();
        if (!File.Exists(path))
            return;

        try
        {
            var loaded = JsonSerializer.Deserialize<Dictionary<string, DateTime?>>(File.ReadAllText(path));
            if (loaded == null)
                return;

            foreach (var (key, value) in loaded)
            {
                _blocks[key] = value;
            }
        }
        catch (Exception e)
        {
            Log.Error($"Failed to read {path}: {e.Message}");
        }
    }

    private void SaveBlocks()
    {
        var path = _cfg.GetCVar(FECCVars.EventsBlocksFile);
        var dir = Path.GetDirectoryName(path);
        if (!string.IsNullOrEmpty(dir))
            Directory.CreateDirectory(dir);

        File.WriteAllText(path, JsonSerializer.Serialize(_blocks, new JsonSerializerOptions { WriteIndented = true }));
        _blocksPath = path;
    }
}
