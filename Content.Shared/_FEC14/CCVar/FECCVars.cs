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

    public static readonly CVarDef<string> TempJobWhitelistsFile =
        CVarDef.Create("fec.temp_job_whitelists_file", ".CONFIG/temp_job_whitelists.json", CVar.SERVERONLY);

    public static readonly CVarDef<string> AdminAuditWebhook =
        CVarDef.Create("fec.admin_audit_webhook", string.Empty, CVar.SERVERONLY | CVar.CONFIDENTIAL);

    public static readonly CVarDef<bool> EventsEnabled =
        CVarDef.Create("fec.events_enabled", true, CVar.SERVERONLY);

    public static readonly CVarDef<float> EventsChance =
        CVarDef.Create("fec.events_chance", 0.3f, CVar.SERVERONLY);

    public static readonly CVarDef<int> EventsMaxPerRound =
        CVarDef.Create("fec.events_max_per_round", 2, CVar.SERVERONLY);

    public static readonly CVarDef<string> EventsBlocksFile =
        CVarDef.Create("fec.events_blocks_file", ".CONFIG/event_blocks.json", CVar.SERVERONLY);

    public static readonly CVarDef<string> EventsRotationFile =
        CVarDef.Create("fec.events_rotation_file", ".CONFIG/event_rotation.json", CVar.SERVERONLY);

    public static readonly CVarDef<bool> EventsVoteEnabled =
        CVarDef.Create("fec.events_vote_enabled", true, CVar.SERVERONLY);

    public static readonly CVarDef<int> EventsVoteOptions =
        CVarDef.Create("fec.events_vote_options", 3, CVar.SERVERONLY);

    public static readonly CVarDef<int> EventsVoteSeconds =
        CVarDef.Create("fec.events_vote_seconds", 60, CVar.SERVERONLY);

    public static readonly CVarDef<int> EventsConfirmSeconds =
        CVarDef.Create("fec.events_confirm_seconds", 60, CVar.SERVERONLY);

    public static readonly CVarDef<int> EventsLowOnlineWarnMinutes =
        CVarDef.Create("fec.events_low_online_warn_minutes", 5, CVar.SERVERONLY);

    public static readonly CVarDef<int> EventsRoleCooldownRounds =
        CVarDef.Create("fec.events_role_cooldown_rounds", 3, CVar.SERVERONLY);

    public static readonly CVarDef<string> EventsRolesFile =
        CVarDef.Create("fec.events_roles_file", ".CONFIG/event_roles.json", CVar.SERVERONLY);

    public static readonly CVarDef<bool> FunEnabled =
        CVarDef.Create("fec.fun_enabled", true, CVar.SERVERONLY);

    public static readonly CVarDef<float> FunDeathQuoteChance =
        CVarDef.Create("fec.fun_death_quote_chance", 0.5f, CVar.SERVERONLY);

    public static readonly CVarDef<bool> FunRoundStartQuote =
        CVarDef.Create("fec.fun_round_start_quote", true, CVar.SERVERONLY);

    public static readonly CVarDef<bool> MappingNewUi =
        CVarDef.Create("fec.mapping_new_ui", false, CVar.CLIENTONLY | CVar.ARCHIVE);

    public static readonly CVarDef<float> WeatherClearRoundChance =
        CVarDef.Create("fec.weather_clear_round_chance", 0.25f, CVar.SERVERONLY);

    public static readonly CVarDef<float> WeatherMaxStartChance =
        CVarDef.Create("fec.weather_max_start_chance", 0.5f, CVar.SERVERONLY);

    public static readonly CVarDef<float> WeatherMinCheckMinutes =
        CVarDef.Create("fec.weather_min_check_minutes", 5f, CVar.SERVERONLY);

    public static readonly CVarDef<float> WeatherMinClearMinutes =
        CVarDef.Create("fec.weather_min_clear_minutes", 10f, CVar.SERVERONLY);
}
