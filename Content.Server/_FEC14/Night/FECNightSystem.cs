using Content.Server._RMC14.Marines;
using Content.Server._RMC14.Rules.DistressSignal;
using Content.Shared._FEC14.CCVar;
using Content.Shared._FEC14.Night;
using Content.Shared._RMC14.Light;
using Content.Shared._RMC14.Rules;
using Robust.Shared.Configuration;
using Robust.Shared.Map.Components;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Robust.Shared.Timing;

namespace Content.Server._FEC14.Night;

public sealed class FECNightSystem : EntitySystem
{
    [Dependency] private readonly RMCAmbientLightSystem _ambientLight = default!;
    [Dependency] private readonly IConfigurationManager _config = default!;
    [Dependency] private readonly CMDistressSignalRuleSystem _distress = default!;
    [Dependency] private readonly MarineAnnounceSystem _marineAnnounce = default!;
    [Dependency] private readonly IPrototypeManager _prototypes = default!;
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly IGameTiming _timing = default!;

    private float _accumulator;

    public override void Update(float frameTime)
    {
        _accumulator += frameTime;
        if (_accumulator < 1f)
            return;

        _accumulator = 0;
        if (!_config.GetCVar(FECCVars.NightEnabled))
            return;

        var query = EntityQueryEnumerator<CMDistressSignalRuleComponent>();
        while (query.MoveNext(out _, out var rule))
        {
            Tick(rule);
        }
    }

    private void Tick(CMDistressSignalRuleComponent rule)
    {
        if (rule.Result != null ||
            rule.StartTime is not { } start ||
            rule.XenoMap is not { } grid ||
            Transform(grid).MapUid is not { } map ||
            _distress.FECSelectedPlanet is not { } planet ||
            !_prototypes.TryIndex<FECPlanetNightPrototype>(planet.Proto.ID, out var profile))
        {
            return;
        }

        var night = EnsureComp<FECNightComponent>(map);
        var now = _timing.CurTime;

        switch (night.Phase)
        {
            case FECNightPhase.Pending:
                var chance = profile.Chance ?? _config.GetCVar(FECCVars.NightChance);
                if (_random.Prob(Math.Clamp(chance, 0f, 1f)))
                {
                    night.Phase = FECNightPhase.Waiting;
                    night.NextPhaseAt = start + profile.StartDelay;
                }
                else
                {
                    night.Phase = FECNightPhase.Done;
                }

                break;
            case FECNightPhase.Waiting:
                if (now >= night.NextPhaseAt)
                    BeginFall((map, night), profile, now, false);

                break;
            case FECNightPhase.Falling:
                if (now >= night.NextPhaseAt)
                {
                    night.Phase = FECNightPhase.Night;
                    night.NextPhaseAt = now + profile.Duration;
                }

                break;
            case FECNightPhase.Night:
                if (now >= night.NextPhaseAt)
                    BeginRise((map, night), profile, now);

                break;
            case FECNightPhase.Rising:
                if (now >= night.NextPhaseAt)
                    night.Phase = FECNightPhase.Done;

                break;
        }
    }

    public bool TryForce(bool start, out string message)
    {
        var query = EntityQueryEnumerator<CMDistressSignalRuleComponent>();
        while (query.MoveNext(out _, out var rule))
        {
            if (rule.Result != null ||
                rule.XenoMap is not { } grid ||
                Transform(grid).MapUid is not { } map)
            {
                continue;
            }

            var profile = _distress.FECSelectedPlanet is { } planet &&
                          _prototypes.TryIndex<FECPlanetNightPrototype>(planet.Proto.ID, out var found)
                ? found
                : new FECPlanetNightPrototype();
            var night = EnsureComp<FECNightComponent>(map);
            var active = night.Phase is FECNightPhase.Falling or FECNightPhase.Night;
            if (start == active)
            {
                message = Loc.GetString(start ? "fec-night-force-already" : "fec-night-force-none");
                return false;
            }

            if (start)
                BeginFall((map, night), profile, _timing.CurTime, true);
            else
                BeginRise((map, night), profile, _timing.CurTime);

            message = Loc.GetString(start ? "fec-night-force-started" : "fec-night-force-ended");
            return true;
        }

        message = Loc.GetString("fec-night-force-no-round");
        return false;
    }

    private void BeginFall(Entity<FECNightComponent> night, FECPlanetNightPrototype profile, TimeSpan now, bool force)
    {
        var light = EnsureComp<MapLightComponent>(night);
        var ambient = EnsureComp<RMCAmbientLightComponent>(night);
        if (!force)
        {
            if (ambient.IsAnimating)
                return;

            if (Luminance(light.AmbientLightColor) <= Luminance(profile.Color))
            {
                night.Comp.Phase = FECNightPhase.Done;
                return;
            }
        }

        if (night.Comp.Phase != FECNightPhase.Rising)
            night.Comp.DayColor = light.AmbientLightColor;

        night.Comp.Phase = FECNightPhase.Falling;
        night.Comp.NextPhaseAt = now + profile.Fade;
        _ambientLight.SetColor((night.Owner, ambient), profile.Color, profile.Fade);
        _marineAnnounce.AnnounceARES(default, Loc.GetString("fec-night-falling"));
    }

    private void BeginRise(Entity<FECNightComponent> night, FECPlanetNightPrototype profile, TimeSpan now)
    {
        var ambient = EnsureComp<RMCAmbientLightComponent>(night);
        night.Comp.Phase = FECNightPhase.Rising;
        night.Comp.NextPhaseAt = now + profile.Fade;
        _ambientLight.SetColor((night.Owner, ambient), night.Comp.DayColor, profile.Fade);
        _marineAnnounce.AnnounceARES(default, Loc.GetString("fec-night-rising"));
    }

    private static float Luminance(Color color)
    {
        return 0.2126f * color.R + 0.7152f * color.G + 0.0722f * color.B;
    }
}
