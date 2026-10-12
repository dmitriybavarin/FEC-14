using Robust.Shared.Prototypes;

namespace Content.Shared._FEC14.Weather;

[Prototype("fecWeatherTactics")]
public sealed partial class FECWeatherTacticsPrototype : IPrototype
{
    [IdDataField]
    public string ID { get; private set; } = default!;

    [DataField]
    public bool BlocksAir;

    [DataField]
    public int MortarDeviation;

    [DataField]
    public float OpticsRangeMultiplier = 1f;
}
