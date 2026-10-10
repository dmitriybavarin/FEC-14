using Content.Shared.NPC.Components;

namespace Content.Server._FEC14.Administration;

public sealed class FECPlayerFactionSystem : EntitySystem
{
    [Dependency] private readonly ILocalizationManager _loc = default!;

    public LocId? GetSubtype(string? roleType, EntityUid? ent)
    {
        switch (roleType)
        {
            case "SoloAntagonist":
            case "TeamAntagonist":
                return "fec-role-type-antagonists";
            case null:
            case "Neutral":
                return GetFactionSubtype(ent);
            default:
                return null;
        }
    }

    public LocId? GetFactionSubtype(EntityUid? ent)
    {
        if (ent == null || !TryComp(ent, out NpcFactionMemberComponent? member))
            return null;

        foreach (var faction in member.Factions)
        {
            var key = $"fec-faction-{faction.Id.ToLowerInvariant()}";
            if (_loc.HasString(key))
                return key;
        }

        return null;
    }
}
