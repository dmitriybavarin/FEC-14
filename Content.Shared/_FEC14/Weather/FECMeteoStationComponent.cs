namespace Content.Shared._FEC14.Weather;

[RegisterComponent]
public sealed partial class FECMeteoStationComponent : Component
{
    [DataField]
    public TimeSpan UseCooldown = TimeSpan.FromSeconds(2);

    [ViewVariables]
    public TimeSpan NextUse;
}
