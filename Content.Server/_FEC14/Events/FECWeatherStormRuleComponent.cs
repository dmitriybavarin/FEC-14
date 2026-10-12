namespace Content.Server._FEC14.Events;

[RegisterComponent, Access(typeof(FECWeatherStormRuleSystem))]
public sealed partial class FECWeatherStormRuleComponent : Component
{
    [DataField]
    public LocId HighCommandAnnouncement = "fec-storm-hc-warning";

    [DataField]
    public LocId Summary = "fec-storm-summary";
}
