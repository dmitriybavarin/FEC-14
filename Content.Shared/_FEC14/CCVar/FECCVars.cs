using Robust.Shared;
using Robust.Shared.Configuration;

namespace Content.Shared._FEC14.CCVar;

[CVarDefs]
public sealed class FECCVars : CVars
{
    public static readonly CVarDef<string> SpeciesWhitelist =
        CVarDef.Create("fec.species_whitelist", "Human", CVar.REPLICATED | CVar.SERVER);

    public static readonly CVarDef<string> OocColors =
        CVarDef.Create("fec.ooc_colors", "", CVar.SERVERONLY);

    public static readonly CVarDef<string> AdminWeightsFile =
        CVarDef.Create("fec.admin_weights_file", ".CONFIG/admin_weights.json", CVar.SERVERONLY);

    public static readonly CVarDef<string> OocColorsFile =
        CVarDef.Create("fec.ooc_colors_file", ".CONFIG/ooc_colors.json", CVar.SERVERONLY);
}
