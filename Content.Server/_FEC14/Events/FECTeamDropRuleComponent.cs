using Robust.Shared.Prototypes;

namespace Content.Server._FEC14.Events;

[RegisterComponent, Access(typeof(FECTeamDropRuleSystem))]
public sealed partial class FECTeamDropRuleComponent : Component
{
    [DataField(required: true)]
    public List<EntProtoId> Spawners = new();

    [DataField]
    public FECEventLocation Location = FECEventLocation.Planet;

    [DataField]
    public float Scatter = 1.5f;

    [DataField]
    public LocId? AresAnnouncement;

    [DataField]
    public LocId? HighCommandAnnouncement;

    [DataField]
    public LocId? XenoAnnouncement;

    [DataField]
    public LocId? Summary;

    [DataField]
    public string? ExplosionType;

    [DataField]
    public float ExplosionIntensity = 100f;

    [DataField]
    public string Outcome = "landed";
}

public enum FECEventLocation : byte
{
    Planet,
    Ship,
}
