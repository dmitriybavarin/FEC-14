using Content.Shared._FEC14.Weather;
using Content.Shared._RMC14.Power;
using Content.Shared._RMC14.Weather;
using Content.Shared.Examine;
using Content.Shared.Interaction;
using Content.Shared.Popups;
using Robust.Shared.Timing;
using Robust.Shared.Utility;

namespace Content.Server._FEC14.Weather;

public sealed class FECMeteoStationSystem : EntitySystem
{
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;
    [Dependency] private readonly SharedRMCPowerSystem _power = default!;
    [Dependency] private readonly RMCWeatherSystem _weather = default!;
    [Dependency] private readonly FECWeatherTowerSystem _tower = default!;
    [Dependency] private readonly FECWeatherTacticsSystem _tactics = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<FECMeteoStationComponent, ExaminedEvent>(OnExamined);
        SubscribeLocalEvent<FECMeteoStationComponent, ActivateInWorldEvent>(OnActivate);
    }

    private void OnExamined(Entity<FECMeteoStationComponent> ent, ref ExaminedEvent args)
    {
        if (!args.IsInDetailsRange)
            return;

        using (args.PushGroup(nameof(FECMeteoStationComponent)))
        {
            args.PushMarkup(GetForecastText(ent));
        }
    }

    private void OnActivate(Entity<FECMeteoStationComponent> ent, ref ActivateInWorldEvent args)
    {
        if (args.Handled || !args.Complex)
            return;

        args.Handled = true;

        var now = _timing.CurTime;
        if (now < ent.Comp.NextUse)
            return;

        ent.Comp.NextUse = now + ent.Comp.UseCooldown;
        _popup.PopupEntity(FormattedMessage.RemoveMarkupPermissive(GetForecastText(ent)), ent, args.User, PopupType.Medium);
    }

    public string GetForecastText(EntityUid station)
    {
        if (!_power.IsPowered(station))
            return Loc.GetString("fec-meteo-station-no-power");

        if (!_weather.TryGetForecast(Transform(station).MapID, out var forecast))
            return Loc.GetString("fec-meteo-station-no-data");

        if (!_tower.IsForecastAvailable(forecast.MapId))
            return Loc.GetString("fec-meteo-station-no-tower");

        var minutes = (int) Math.Ceiling(forecast.Remaining.TotalMinutes);
        var seconds = (int) Math.Ceiling(forecast.Remaining.TotalSeconds);
        var location = string.IsNullOrWhiteSpace(forecast.MapName)
            ? Loc.GetString("fec-meteo-station-location-unknown")
            : forecast.MapName;

        var body = forecast.State switch
        {
            RMCWeatherCycleState.Warning => Loc.GetString("fec-meteo-station-warning",
                ("event", forecast.EventName ?? string.Empty),
                ("seconds", seconds)),
            RMCWeatherCycleState.Running when forecast.Remaining > TimeSpan.Zero => Loc.GetString("fec-meteo-station-running",
                ("event", forecast.EventName ?? string.Empty),
                ("minutes", minutes)),
            RMCWeatherCycleState.Running => Loc.GetString("fec-meteo-station-running-permanent",
                ("event", forecast.EventName ?? string.Empty)),
            RMCWeatherCycleState.Cooldown => Loc.GetString("fec-meteo-station-cooldown",
                ("minutes", minutes)),
            _ when forecast.ClearRound => Loc.GetString("fec-meteo-station-idle-clear"),
            _ => Loc.GetString("fec-meteo-station-idle",
                ("minutes", minutes)),
        };

        if (_tactics.BlocksAir(forecast.MapId))
            body += "\n" + Loc.GetString("fec-meteo-station-no-fly");

        return Loc.GetString("fec-meteo-station-forecast", ("location", location), ("body", body));
    }
}
