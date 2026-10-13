using System.Linq;
using Content.Server.Administration;
using Content.Server.Administration.Managers;
using Content.Server.EUI;
using Content.Shared._FEC14.Weather;
using Content.Shared._RMC14.Weather;
using Content.Shared.Administration;
using Content.Shared.Eui;
using Content.Shared.Weather;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;

namespace Content.Server._FEC14.Weather;

public sealed class FECWeatherEui : BaseEui
{
    [Dependency] private readonly IAdminManager _admin = default!;
    [Dependency] private readonly IEntityManager _entities = default!;

    private string? _message;

    public FECWeatherEui()
    {
        IoCManager.InjectDependencies(this);
    }

    public override void Opened()
    {
        _admin.OnPermsChanged += OnPermsChanged;
        _entities.System<FECWeatherAdminSystem>().Track(this);
        StateDirty();
    }

    public override void Closed()
    {
        _admin.OnPermsChanged -= OnPermsChanged;
        _entities.System<FECWeatherAdminSystem>().Untrack(this);
    }

    private void OnPermsChanged(AdminPermsChangedEventArgs args)
    {
        if (args.Player == Player && !CanUse())
            Close();
    }

    private bool CanUse()
    {
        return _admin.HasAdminFlag(Player, AdminFlags.Weather);
    }

    public override EuiStateBase GetNewState()
    {
        var maps = new List<FECWeatherMapEntry>();
        var query = _entities.EntityQueryEnumerator<MapComponent>();
        while (query.MoveNext(out var uid, out var map))
        {
            if (map.MapId == MapId.Nullspace)
                continue;

            maps.Add(CreateEntry(uid, map.MapId));
        }

        maps.Sort((a, b) => a.MapId.CompareTo(b.MapId));

        return new FECWeatherEuiState(maps, _message);
    }

    private FECWeatherMapEntry CreateEntry(EntityUid mapUid, MapId mapId)
    {
        var name = _entities.GetComponent<MetaDataComponent>(mapUid).EntityName;
        if (string.IsNullOrWhiteSpace(name))
        {
            var grids = new List<Entity<MapGridComponent>>();
            var gridQuery = _entities.EntityQueryEnumerator<MapGridComponent, TransformComponent>();
            while (gridQuery.MoveNext(out var gridUid, out var grid, out var xform))
            {
                if (xform.MapID == mapId)
                    grids.Add((gridUid, grid));
            }

            name = grids
                .Select(g => _entities.GetComponent<MetaDataComponent>(g).EntityName)
                .FirstOrDefault(n => !string.IsNullOrWhiteSpace(n)) ?? string.Empty;
        }

        string? currentWeather = null;
        if (_entities.TryGetComponent(mapUid, out WeatherComponent? weather) && weather.Weather.Count > 0)
            currentWeather = weather.Weather.Keys.First().Id;

        var events = new List<FECWeatherEventEntry>();
        var state = "None";
        string? currentEvent = null;
        var remaining = 0;
        var hasCycle = _entities.TryGetComponent(mapUid, out RMCWeatherCycleComponent? cycle);
        if (cycle != null)
        {
            var cycleState = cycle.State;
            state = cycleState.ToString();
            var left = cycleState == RMCWeatherCycleState.Warning ? cycle.WarningRemaining : cycle.EventRemaining;
            remaining = (int) Math.Max(0, Math.Ceiling(left.TotalSeconds));
            for (var i = 0; i < cycle.WeatherEvents.Count; i++)
            {
                var ev = cycle.WeatherEvents[i];
                events.Add(new FECWeatherEventEntry(i, EventName(ev), (int) Math.Max(0, ev.Duration.TotalSeconds), ev.WeatherType.Id));
            }

            if (cycleState is RMCWeatherCycleState.Warning or RMCWeatherCycleState.Running)
            {
                if (cycle.ForcedEvent is { } forced)
                    currentEvent = EventName(forced);
                else if (cycle.CurrentEventIndex is { } index && index >= 0 && index < cycle.WeatherEvents.Count)
                    currentEvent = EventName(cycle.WeatherEvents[index]);
            }
        }

        return new FECWeatherMapEntry((int) mapId, name, hasCycle, state, currentEvent, remaining, currentWeather, events);
    }

    private static string EventName(RMCWeatherEvent ev)
    {
        return ev.DisplayName ?? ev.Name;
    }

    public override void HandleMessage(EuiMessageBase msg)
    {
        base.HandleMessage(msg);

        if (!CanUse())
        {
            Close();
            return;
        }

        var weather = _entities.System<RMCWeatherSystem>();
        switch (msg)
        {
            case FECWeatherStartMessage start:
                weather.TryStartWeatherEvent((MapId) start.MapId, start.EventIndex.ToString(), start.Instant, true, out _message);
                break;
            case FECWeatherEndMessage end:
                weather.TryEndWeather((MapId) end.MapId, out _message);
                break;
            default:
                return;
        }

        StateDirty();
    }
}
