using Robust.Shared.Prototypes;

namespace Content.Shared._FEC14.Roadmap;

[Prototype("fecRoadmapSection")]
public sealed partial class FECRoadmapSectionPrototype : IPrototype
{
    [IdDataField]
    public string ID { get; private set; } = default!;

    [DataField(required: true)]
    public LocId Name;

    [DataField]
    public int Order;

    [DataField]
    public List<FECRoadmapEntry> Items = new();
}

[DataDefinition]
public sealed partial class FECRoadmapEntry
{
    [DataField(required: true)]
    public LocId Name;

    [DataField]
    public LocId? Text;

    [DataField]
    public FECRoadmapState State = FECRoadmapState.Planned;
}

public enum FECRoadmapState
{
    Planned,
    InProgress,
    Partial,
    Complete,
}
