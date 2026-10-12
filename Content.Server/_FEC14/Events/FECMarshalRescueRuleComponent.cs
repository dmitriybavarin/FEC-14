using Content.Shared._RMC14.Weapons.Ranged.IFF;
using Content.Shared.Roles;
using Robust.Shared.Prototypes;

namespace Content.Server._FEC14.Events;

[RegisterComponent, Access(typeof(FECMarshalRescueRuleSystem))]
public sealed partial class FECMarshalRescueRuleComponent : Component
{
    [DataField]
    public ProtoId<AntagPrototype> Preference = "FECEventCMBMarshal";

    [DataField]
    public ProtoId<JobPrototype> Job = "CMBureauMarshal";

    [DataField]
    public TimeSpan CheckInterval = TimeSpan.FromMinutes(60);

    [DataField]
    public TimeSpan HighCommandDelayMin = TimeSpan.FromMinutes(2);

    [DataField]
    public TimeSpan HighCommandDelayMax = TimeSpan.FromMinutes(4);

    [DataField]
    public TimeSpan InboundMin = TimeSpan.FromMinutes(10);

    [DataField]
    public TimeSpan InboundMax = TimeSpan.FromMinutes(20);

    [DataField]
    public TimeSpan AirDefenseDelay = TimeSpan.FromSeconds(40);

    [DataField]
    public TimeSpan UnclaimedTimeout = TimeSpan.FromMinutes(5);

    [DataField]
    public string LeaderSettings = "RMCMarineRaiderLeader";

    [DataField]
    public string MemberSettings = "RMCMarineRaider";

    [DataField]
    public int MinSquad = 4;

    [DataField]
    public int MaxSquad = 10;

    [DataField]
    public int PlayersPerMember = 12;

    [DataField]
    public EntProtoId<IFFFactionComponent> Faction = "FECFactionPunitive";

    [DataField]
    public string ExplosionType = "Default";

    [DataField]
    public float ExplosionIntensity = 400f;

    public FECMarshalStage Stage = FECMarshalStage.Spawning;
    public EntityUid? Marshal;
    public string MarshalName = string.Empty;
    public TimeSpan NextCheck;
    public TimeSpan NextStage;
    public TimeSpan NextAssaultCheck;
    public TimeSpan ClaimDeadline;
    public bool AnyClaimed;
    public readonly HashSet<EntityUid> Squad = new();
    public readonly HashSet<EntityUid> Briefed = new();
}

public enum FECMarshalStage : byte
{
    Spawning,
    Monitoring,
    ReportSent,
    Inbound,
    AirDefense,
    Assault,
    Finished,
}
