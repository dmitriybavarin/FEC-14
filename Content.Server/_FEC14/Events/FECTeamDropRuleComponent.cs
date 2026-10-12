using Robust.Shared.Prototypes;

namespace Content.Server._FEC14.Events;

[RegisterComponent, Access(typeof(FECTeamDropRuleSystem))]
public sealed partial class FECTeamDropRuleComponent : Component
{
    [DataField(required: true)]
    public List<EntProtoId> Spawners = new();

    [DataField]
    public LocId? AresAnnouncement;

    [DataField]
    public LocId? HighCommandAnnouncement;

    [DataField]
    public LocId? Summary;
}
