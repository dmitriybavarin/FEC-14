using System.Globalization;
using System.Linq;
using Content.Server.Administration.Logs;
using Content.Server.Administration.Managers;
using Content.Shared._FEC14.Administration;
using Content.Shared._FEC14.CCVar;
using Content.Shared.Administration;
using Content.Shared.Database;
using Robust.Shared.Configuration;
using Robust.Shared.Player;

namespace Content.Server._FEC14.Administration;

public sealed class FECSettingsSystem : EntitySystem
{
    [Dependency] private readonly IAdminManager _admin = default!;
    [Dependency] private readonly IAdminLogManager _adminLog = default!;
    [Dependency] private readonly IConfigurationManager _cfg = default!;

    private sealed record Setting(string Category, CVarDef Def, FECSettingType Type, double Min = 0, double Max = 0);

    private static readonly Setting[] Settings =
    {
        new("events", FECCVars.EventsEnabled, FECSettingType.Bool),
        new("events", FECCVars.EventsChance, FECSettingType.Float, 0, 1),
        new("events", FECCVars.EventsMaxPerRound, FECSettingType.Int, 0, 5),
        new("events", FECCVars.EventsVoteEnabled, FECSettingType.Bool),
        new("events", FECCVars.EventsVoteOptions, FECSettingType.Int, 2, 5),
        new("events", FECCVars.EventsVoteSeconds, FECSettingType.Int, 10, 300),
        new("events", FECCVars.EventsConfirmSeconds, FECSettingType.Int, 0, 600),
        new("events", FECCVars.EventsLowOnlineWarnMinutes, FECSettingType.Int, 0, 30),
        new("events", FECCVars.EventsRoleCooldownRounds, FECSettingType.Int, 0, 20),
        new("weather", FECCVars.WeatherClearRoundChance, FECSettingType.Float, 0, 1),
        new("weather", FECCVars.WeatherMaxStartChance, FECSettingType.Float, 0, 1),
        new("weather", FECCVars.WeatherMinCheckMinutes, FECSettingType.Float, 1, 60),
        new("weather", FECCVars.WeatherMinClearMinutes, FECSettingType.Float, 0, 120),
        new("night", FECCVars.NightEnabled, FECSettingType.Bool),
        new("night", FECCVars.NightChance, FECSettingType.Float, 0, 1),
        new("wet", FECCVars.WetEnabled, FECSettingType.Bool),
        new("wet", FECCVars.WetMaxSeconds, FECSettingType.Float, 10, 600),
        new("wet", FECCVars.WetGainSeconds, FECSettingType.Float, 0, 30),
        new("radio", FECCVars.RadioNoiseEnabled, FECSettingType.Bool),
        new("lobby", FECCVars.SpeciesWhitelist, FECSettingType.String),
    };

    public override void Initialize()
    {
        base.Initialize();
        SubscribeNetworkEvent<FECSettingsRequestEvent>(OnRequest);
        SubscribeNetworkEvent<FECSettingsSetEvent>(OnSet);
        SubscribeNetworkEvent<FECSettingsResetEvent>(OnReset);
    }

    public bool CanUse(ICommonSession session)
    {
        return _admin.HasAdminFlag(session, AdminFlags.Server);
    }

    private void OnRequest(FECSettingsRequestEvent ev, EntitySessionEventArgs args)
    {
        if (CanUse(args.SenderSession))
            Send(args.SenderSession, null);
    }

    private void OnSet(FECSettingsSetEvent ev, EntitySessionEventArgs args)
    {
        if (!CanUse(args.SenderSession))
            return;

        TrySet(args.SenderSession, ev.Key, ev.Value, out var status);
        Send(args.SenderSession, status);
    }

    private void OnReset(FECSettingsResetEvent ev, EntitySessionEventArgs args)
    {
        if (!CanUse(args.SenderSession))
            return;

        Reset(args.SenderSession, ev.Key, out var status);
        Send(args.SenderSession, status);
    }

    private void Send(ICommonSession session, string? status)
    {
        RaiseNetworkEvent(new FECSettingsStateEvent(GetEntries(), status), session);
    }

    public List<FECSettingEntry> GetEntries()
    {
        return Settings
            .Select(s => new FECSettingEntry(
                s.Category,
                s.Def.Name,
                s.Type,
                Format(_cfg.GetCVar(s.Def.Name)),
                Format(s.Def.DefaultValue),
                s.Min,
                s.Max))
            .ToList();
    }

    public bool Reset(ICommonSession admin, string key, out string? status)
    {
        status = null;
        return Settings.FirstOrDefault(s => s.Def.Name == key) is { } setting &&
               TrySet(admin, key, Format(setting.Def.DefaultValue), out status);
    }

    public bool TrySet(ICommonSession admin, string key, string text, out string? status)
    {
        status = null;
        if (Settings.FirstOrDefault(s => s.Def.Name == key) is not { } setting)
            return false;

        text = text.Trim();
        object value;
        switch (setting.Type)
        {
            case FECSettingType.Bool when bool.TryParse(text, out var b):
                value = b;
                break;
            case FECSettingType.Int when int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out var i):
                value = (int) Math.Clamp(i, setting.Min, setting.Max);
                break;
            case FECSettingType.Float when float.TryParse(text.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out var f):
                value = (float) Math.Clamp(f, setting.Min, setting.Max);
                break;
            case FECSettingType.String:
                value = text;
                break;
            default:
                status = Loc.GetString("fec-settings-bad-value", ("key", key), ("value", text));
                return false;
        }

        var old = Format(_cfg.GetCVar(key));
        _cfg.SetCVar(key, value);
        var now = Format(_cfg.GetCVar(key));
        _adminLog.Add(LogType.AdminCommands, LogImpact.High, $"{admin.Name} changed FEC setting {key} from {old} to {now}");
        status = Loc.GetString("fec-settings-changed", ("key", key), ("old", old), ("value", now));
        return true;
    }

    private static string Format(object? value)
    {
        return value switch
        {
            float f => f.ToString("0.###", CultureInfo.InvariantCulture),
            double d => d.ToString("0.###", CultureInfo.InvariantCulture),
            bool b => b ? "true" : "false",
            null => string.Empty,
            _ => value.ToString() ?? string.Empty,
        };
    }
}
