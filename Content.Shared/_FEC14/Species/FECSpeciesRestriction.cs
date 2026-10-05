using Content.Shared._FEC14.CCVar;
using Content.Shared.Humanoid.Prototypes;
using Robust.Shared.Configuration;

namespace Content.Shared._FEC14.Species;

public static class FECSpeciesRestriction
{
    public static bool IsAllowed(SpeciesPrototype species, IConfigurationManager? config = null)
    {
        if (!species.RoundStart)
            return false;

        config ??= IoCManager.Resolve<IConfigurationManager>();
        var whitelist = config.GetCVar(FECCVars.SpeciesWhitelist);
        if (string.IsNullOrWhiteSpace(whitelist))
            return true;

        foreach (var id in whitelist.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            if (id == species.ID)
                return true;
        }

        return false;
    }
}
