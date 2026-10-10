using Content.Shared._RMC14.Xenonids;
using Content.Shared.Mind;

namespace Content.Client._FEC14.Roles;

public static class FECRoleTypeName
{
    public static string Get(IEntityManager entities, EntityUid? player, RoleTypePrototype? proto)
    {
        switch (proto?.ID)
        {
            case "SoloAntagonist":
            case "TeamAntagonist":
                return Loc.GetString("fec-role-type-antagonists");
            case null:
            case "Neutral":
                return entities.HasComponent<XenoComponent>(player)
                    ? Loc.GetString("fec-role-type-xenos")
                    : Loc.GetString("fec-role-type-marines");
            default:
                return Loc.GetString(proto.Name);
        }
    }
}
