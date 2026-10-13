using Robust.Shared.Prototypes;

namespace Content.Shared._FEC14.Night;

[Prototype("fecPlanetNight")]
public sealed partial class FECPlanetNightPrototype : IPrototype
{
    [IdDataField]
    public string ID { get; private set; } = default!;

    [DataField]
    public float? Chance;

    [DataField]
    public TimeSpan StartDelay = TimeSpan.FromMinutes(40);

    [DataField]
    public TimeSpan Duration = TimeSpan.FromMinutes(12);

    [DataField]
    public TimeSpan Fade = TimeSpan.FromMinutes(2);

    [DataField]
    public Color Color = Color.FromHex("#38477a");
}
