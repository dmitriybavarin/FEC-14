using System.Numerics;
using Content.Shared._FEC14.CCVar;
using Content.Shared._FEC14.Weather;
using Content.Shared._FEC14.Wet;
using Content.Shared._RMC14.Weather;
using Content.Shared.Humanoid;
using Content.Shared.Mobs.Components;
using Content.Shared.Mobs.Systems;
using Content.Shared.Popups;
using Robust.Shared.Configuration;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;
using Robust.Shared.Timing;

namespace Content.Server._FEC14.Wet;

public sealed class FECWetSystem : EntitySystem
{
    private const string PrintPrototype = "FECWetFootprint";
    private const float StepDistance = 0.85f;
    private const float FootOffset = 0.1f;
    private static readonly TimeSpan SoakInterval = TimeSpan.FromSeconds(1);
    private static readonly string[] WetSurfaces = ["Wet", "Water"];
    private static readonly string[] PrintSurfaces = ["Sand", "Dirt", "Mud", "Snow"];

    [Dependency] private readonly IConfigurationManager _config = default!;
    [Dependency] private readonly SharedMapSystem _map = default!;
    [Dependency] private readonly MobStateSystem _mobState = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;
    [Dependency] private readonly FECWeatherTacticsSystem _tactics = default!;
    [Dependency] private readonly ITileDefinitionManager _tileDefs = default!;
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly RMCWeatherSystem _weather = default!;

    private TimeSpan _nextSoak;

    public override void Update(float frameTime)
    {
        if (!_config.GetCVar(FECCVars.WetEnabled))
            return;

        var now = _timing.CurTime;
        if (now >= _nextSoak)
        {
            _nextSoak = now + SoakInterval;
            Soak(now);
        }

        var wet = EntityQueryEnumerator<FECWetComponent, TransformComponent>();
        while (wet.MoveNext(out var uid, out var comp, out var xform))
        {
            if (now >= comp.ExpiresAt)
            {
                _popup.PopupEntity(Loc.GetString("fec-wet-dried"), uid, uid);
                RemCompDeferred<FECWetComponent>(uid);
                continue;
            }

            LeavePrint(uid, comp, xform);
        }
    }

    private void Soak(TimeSpan now)
    {
        var duration = TimeSpan.FromSeconds(Math.Max(_config.GetCVar(FECCVars.WetDurationSeconds), 1f));
        var query = EntityQueryEnumerator<HumanoidAppearanceComponent, MobStateComponent, TransformComponent>();
        while (query.MoveNext(out var uid, out _, out var mobState, out var xform))
        {
            if (!_mobState.IsAlive(uid, mobState))
                continue;

            var rained = _tactics.IsWetWeather(xform.MapID) && _weather.IsWeatherExposed(uid);
            if (!rained && !IsOnSurface(xform, WetSurfaces))
                continue;

            if (!TryComp(uid, out FECWetComponent? wet))
            {
                wet = EnsureComp<FECWetComponent>(uid);
                _popup.PopupEntity(Loc.GetString("fec-wet-soaked"), uid, uid);
            }

            wet.ExpiresAt = now + duration;
        }
    }

    private void LeavePrint(EntityUid uid, FECWetComponent wet, TransformComponent xform)
    {
        var position = xform.LocalPosition;
        if (wet.LastPrint is not { } last)
        {
            wet.LastPrint = position;
            return;
        }

        var delta = position - last;
        var distance = delta.Length();
        if (distance < StepDistance)
            return;

        wet.LastPrint = position;
        if (xform.GridUid is not { } gridUid ||
            xform.ParentUid != gridUid ||
            !IsOnSurface(xform, PrintSurfaces))
        {
            return;
        }

        wet.LeftFoot = !wet.LeftFoot;
        var direction = delta / distance;
        var side = new Vector2(-direction.Y, direction.X) * (wet.LeftFoot ? FootOffset : -FootOffset);
        var print = Spawn(PrintPrototype, xform.Coordinates.Offset(side));
        Transform(print).LocalRotation = delta.ToWorldAngle();
    }

    private bool IsOnSurface(TransformComponent xform, string[] surfaces)
    {
        if (xform.GridUid is not { } gridUid || !TryComp(gridUid, out MapGridComponent? grid))
            return false;

        var indices = _map.CoordinatesToTile(gridUid, grid, xform.Coordinates);
        var tile = _map.GetTileRef(gridUid, grid, indices).Tile;
        if (tile.IsEmpty)
            return false;

        var id = _tileDefs[tile.TypeId].ID;
        foreach (var surface in surfaces)
        {
            if (id.Contains(surface, StringComparison.OrdinalIgnoreCase))
                return true;
        }

        return false;
    }
}
