using System.Globalization;
using System.Linq;
using Content.Server.Administration;
using Content.Server.Administration.Logs;
using Content.Server.Administration.Managers;
using Content.Server.EUI;
using Content.Shared._FEC14.Administration;
using Content.Shared._FEC14.CCVar;
using Content.Shared.Administration;
using Content.Shared.Database;
using Content.Shared.Eui;
using Robust.Shared.Configuration;

namespace Content.Server._FEC14.Administration;

public sealed class FECSettingsEui : BaseEui
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
        new("lobby", FECCVars.SpeciesWhitelist, FECSettingType.String),
    };

    private string? _status;

    public FECSettingsEui()
    {
        IoCManager.InjectDependencies(this);
    }

    public override void Opened()
    {
        _admin.OnPermsChanged += OnPermsChanged;
        StateDirty();
    }

    public override void Closed()
    {
        _admin.OnPermsChanged -= OnPermsChanged;
    }

    private void OnPermsChanged(AdminPermsChangedEventArgs args)
    {
        if (args.Player == Player && !CanUse())
            Close();
    }

    private bool CanUse()
    {
        return _admin.HasAdminFlag(Player, AdminFlags.Server);
    }

    public override EuiStateBase GetNewState()
    {
        var entries = Settings
            .Select(s => new FECSettingEntry(
                s.Category,
                s.Def.Name,
                s.Type,
                Format(_cfg.GetCVar(s.Def.Name)),
                Format(s.Def.DefaultValue),
                s.Min,
                s.Max))
            .ToList();

        return new FECSettingsEuiState(entries, _status);
    }

    public override void HandleMessage(EuiMessageBase msg)
    {
        base.HandleMessage(msg);

        if (!CanUse())
        {
            Close();
            return;
        }

        switch (msg)
        {
            case FECSettingsSetMsg set:
                Set(set.Key, set.Value.Trim());
                break;
            case FECSettingsResetMsg reset when Settings.FirstOrDefault(s => s.Def.Name == reset.Key) is { } setting:
                Set(setting.Def.Name, Format(setting.Def.DefaultValue));
                break;
        }

        StateDirty();
    }

    private void Set(string key, string text)
    {
        if (Settings.FirstOrDefault(s => s.Def.Name == key) is not { } setting)
            return;

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
                _status = Loc.GetString("fec-settings-bad-value", ("key", key), ("value", text));
                return;
        }

        var old = Format(_cfg.GetCVar(key));
        _cfg.SetCVar(key, value);
        var now = Format(_cfg.GetCVar(key));
        _adminLog.Add(LogType.AdminCommands, LogImpact.High, $"{Player.Name} changed FEC setting {key} from {old} to {now}");
        _status = Loc.GetString("fec-settings-changed", ("key", key), ("old", old), ("value", now));
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
