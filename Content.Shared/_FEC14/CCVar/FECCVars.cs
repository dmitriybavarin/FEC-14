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

    public static readonly CVarDef<string> AdminAuditWebhook =
        CVarDef.Create("fec.admin_audit_webhook", string.Empty, CVar.SERVERONLY | CVar.CONFIDENTIAL);

    public static readonly CVarDef<float> WeatherClearRoundChance =
        CVarDef.Create("fec.weather_clear_round_chance", 0.25f, CVar.SERVERONLY);

    public static readonly CVarDef<float> WeatherMaxStartChance =
        CVarDef.Create("fec.weather_max_start_chance", 0.5f, CVar.SERVERONLY);

    public static readonly CVarDef<float> WeatherMinCheckMinutes =
        CVarDef.Create("fec.weather_min_check_minutes", 5f, CVar.SERVERONLY);

    public static readonly CVarDef<float> WeatherMinClearMinutes =
        CVarDef.Create("fec.weather_min_clear_minutes", 10f, CVar.SERVERONLY);
}
