using Robust.Shared.Prototypes;

namespace Content.Shared._FEC14.Events;

[Prototype("fecEvent")]
public sealed partial class FECEventPrototype : IPrototype
{
    [IdDataField]
    public string ID { get; private set; } = default!;

    [DataField(required: true)]
    public LocId Name;

    [DataField]
    public LocId? Description;

    [DataField(required: true)]
    public EntProtoId Rule;

    [DataField]
    public int MinPlayers;

    [DataField]
    public float Weight = 1f;

    [DataField]
    public TimeSpan EarliestStart = TimeSpan.FromMinutes(20);

    [DataField]
    public TimeSpan LatestStart = TimeSpan.FromMinutes(60);

    [DataField]
    public bool RequiresPlanet = true;
}
