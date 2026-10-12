using Content.Shared.Weather;
using Robust.Shared.Map;
using Robust.Shared.Prototypes;

namespace Content.Shared._FEC14.Weather;

public sealed class FECWeatherTacticsSystem : EntitySystem
{
    [Dependency] private readonly SharedMapSystem _map = default!;
    [Dependency] private readonly IPrototypeManager _prototypes = default!;

    public bool BlocksAir(MapId mapId)
    {
        return GetTactics(mapId).BlocksAir;
    }

    public int GetMortarDeviation(MapId mapId)
    {
        return GetTactics(mapId).MortarDeviation;
    }

    public float GetOpticsRangeMultiplier(MapId mapId)
    {
        return GetTactics(mapId).OpticsRangeMultiplier;
    }

    public (bool BlocksAir, int MortarDeviation, float OpticsRangeMultiplier) GetTactics(MapId mapId)
    {
        var result = (BlocksAir: false, MortarDeviation: 0, OpticsRangeMultiplier: 1f);
        if (!_map.TryGetMap(mapId, out var mapUid) || !TryComp(mapUid, out WeatherComponent? weather))
            return result;

        foreach (var id in weather.Weather.Keys)
        {
            if (!_prototypes.TryIndex<FECWeatherTacticsPrototype>(id.Id, out var tactics))
                continue;

            result.BlocksAir |= tactics.BlocksAir;
            result.MortarDeviation = Math.Max(result.MortarDeviation, tactics.MortarDeviation);
            result.OpticsRangeMultiplier = Math.Min(result.OpticsRangeMultiplier, tactics.OpticsRangeMultiplier);
        }

        return result;
    }
}
