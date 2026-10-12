using Content.Shared._RMC14.Weather;
using Robust.Shared.Prototypes;

namespace Content.Shared._FEC14.Weather;

[Prototype("fecPlanetWeather")]
public sealed partial class FECPlanetWeatherPrototype : IPrototype
{
    [IdDataField]
    public string ID { get; private set; } = default!;

    [DataField]
    public float? ClearRoundChance;

    [DataField]
    public float? StartChance;

    [DataField]
    public TimeSpan? MinTimeBetweenChecks;

    [DataField]
    public TimeSpan? MinTimeBetweenEvents;

    [DataField]
    public List<RMCWeatherEvent> ExtraEvents = new();
}
