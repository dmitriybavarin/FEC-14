using Content.Shared._RMC14.Power;
using Content.Shared._RMC14.Weather;
using Content.Shared.Administration.Logs;
using Content.Shared.Damage;
using Content.Shared.Database;
using Content.Shared.Destructible;
using Content.Shared.Examine;
using Content.Shared.FixedPoint;
using Content.Shared.Interaction;
using Content.Shared.Popups;
using Content.Shared.Power;
using Content.Shared.Weather;
using Robust.Shared.Map;
using Robust.Shared.Network;
using Robust.Shared.Timing;

namespace Content.Shared._FEC14.Weather;

public sealed class FECWeatherTowerSystem : EntitySystem
{
    [Dependency] private readonly ISharedAdminLogManager _adminLog = default!;
    [Dependency] private readonly SharedAppearanceSystem _appearance = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;
    [Dependency] private readonly SharedRMCPowerSystem _power = default!;
    [Dependency] private readonly INetManager _net = default!;
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly RMCWeatherSystem _rmcWeather = default!;

    public override void Initialize()
    {
        SubscribeLocalEvent<FECWeatherTowerComponent, MapInitEvent>(OnMapInit);
        SubscribeLocalEvent<FECWeatherTowerComponent, DamageChangedEvent>(OnDamageChanged);
        SubscribeLocalEvent<FECWeatherTowerComponent, BreakageEventArgs>(OnBreakage);
        SubscribeLocalEvent<FECWeatherTowerComponent, ExaminedEvent>(OnExamined);
        SubscribeLocalEvent<FECWeatherTowerComponent, InteractHandEvent>(OnInteractHand);
        SubscribeLocalEvent<FECWeatherTowerComponent, PowerChangedEvent>(OnPowerChanged);
    }

    private void OnMapInit(Entity<FECWeatherTowerComponent> ent, ref MapInitEvent args)
    {
        UpdateAppearance(ent);
        _appearance.SetData(ent, FECWeatherTowerLayers.Screen, ent.Comp.Screen);
        UpdateScreen(ent);
    }

    private void OnDamageChanged(Entity<FECWeatherTowerComponent> ent, ref DamageChangedEvent args)
    {
        if (args.Damageable.TotalDamage > FixedPoint2.Zero || ent.Comp.State != FECWeatherTowerState.Broken)
            return;

        SetState(ent, FECWeatherTowerState.Off);
    }

    private void OnBreakage(Entity<FECWeatherTowerComponent> ent, ref BreakageEventArgs args)
    {
        SetState(ent, FECWeatherTowerState.Broken);
    }

    private void OnExamined(Entity<FECWeatherTowerComponent> ent, ref ExaminedEvent args)
    {
        using (args.PushGroup(nameof(FECWeatherTowerComponent)))
        {
            args.PushMarkup(Loc.GetString(ent.Comp.State switch
            {
                FECWeatherTowerState.Broken => "fec-weather-tower-examine-broken",
                FECWeatherTowerState.On => "fec-weather-tower-examine-on",
                _ => "fec-weather-tower-examine-off",
            }));
        }
    }

    private void OnInteractHand(Entity<FECWeatherTowerComponent> ent, ref InteractHandEvent args)
    {
        if (args.Handled)
            return;

        args.Handled = true;

        if (ent.Comp.State == FECWeatherTowerState.Broken)
        {
            _popup.PopupClient(Loc.GetString("fec-weather-tower-needs-repairs"), ent, args.User, PopupType.MediumCaution);
            return;
        }

        if (!_power.IsPowered(ent))
        {
            _popup.PopupClient(Loc.GetString("fec-weather-tower-no-power"), ent, args.User, PopupType.MediumCaution);
            return;
        }

        var state = ent.Comp.State == FECWeatherTowerState.On ? FECWeatherTowerState.Off : FECWeatherTowerState.On;
        _adminLog.Add(LogType.Action, $"{ToPrettyString(args.User):user} turned {ToPrettyString(ent):tower} {state}.");
        SetState(ent, state);

        _popup.PopupClient(Loc.GetString(state == FECWeatherTowerState.On
                ? "fec-weather-tower-turned-on"
                : "fec-weather-tower-turned-off"),
            ent,
            args.User,
            PopupType.Medium);
    }

    private void OnPowerChanged(Entity<FECWeatherTowerComponent> ent, ref PowerChangedEvent args)
    {
        if (ent.Comp.State == FECWeatherTowerState.On && !args.Powered)
            SetState(ent, FECWeatherTowerState.Off);
    }

    public bool IsForecastAvailable(MapId mapId)
    {
        var found = false;
        var query = EntityQueryEnumerator<FECWeatherTowerComponent, TransformComponent>();
        while (query.MoveNext(out var tower, out var xform))
        {
            if (xform.MapID != mapId)
                continue;

            if (tower.State == FECWeatherTowerState.On)
                return true;

            found = true;
        }

        return !found;
    }

    private void SetState(Entity<FECWeatherTowerComponent> ent, FECWeatherTowerState state)
    {
        if (ent.Comp.State == state)
            return;

        ent.Comp.State = state;
        Dirty(ent);
        UpdateAppearance(ent);
        UpdateScreen(ent);
    }

    public override void Update(float frameTime)
    {
        if (_net.IsClient)
            return;

        var now = _timing.CurTime;
        var query = EntityQueryEnumerator<FECWeatherTowerComponent>();
        while (query.MoveNext(out var uid, out var tower))
        {
            if (now < tower.NextScreenUpdate)
                continue;

            tower.NextScreenUpdate = now + tower.ScreenUpdateInterval;
            UpdateScreen((uid, tower));
        }
    }

    private void UpdateScreen(Entity<FECWeatherTowerComponent> ent)
    {
        if (_net.IsClient)
            return;

        var screen = GetScreen(ent);
        if (ent.Comp.Screen == screen)
            return;

        ent.Comp.Screen = screen;
        _appearance.SetData(ent, FECWeatherTowerLayers.Screen, screen);
    }

    private FECWeatherTowerScreen GetScreen(Entity<FECWeatherTowerComponent> ent)
    {
        if (ent.Comp.State != FECWeatherTowerState.On)
            return FECWeatherTowerScreen.Off;

        var xform = Transform(ent);
        if (_rmcWeather.TryGetForecast(xform.MapID, out var forecast) &&
            forecast.MapId == xform.MapID &&
            forecast.State == RMCWeatherCycleState.Warning)
        {
            return FECWeatherTowerScreen.Warning;
        }

        if (xform.MapUid is not { } mapUid ||
            !TryComp(mapUid, out WeatherComponent? weather) ||
            weather.Weather.Count == 0)
        {
            return FECWeatherTowerScreen.Clear;
        }

        foreach (var id in weather.Weather.Keys)
        {
            if (ent.Comp.Screens.TryGetValue(id, out var screen))
                return screen;
        }

        return ent.Comp.UnknownWeatherScreen;
    }

    private void UpdateAppearance(Entity<FECWeatherTowerComponent> ent)
    {
        _appearance.SetData(ent, FECWeatherTowerLayers.Layer, ent.Comp.State);
    }
}
