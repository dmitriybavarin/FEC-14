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
                    BeginFall((map, night), profile, now);

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

    private void BeginFall(Entity<FECNightComponent> night, FECPlanetNightPrototype profile, TimeSpan now)
    {
        var light = EnsureComp<MapLightComponent>(night);
        var ambient = EnsureComp<RMCAmbientLightComponent>(night);
        if (ambient.IsAnimating)
            return;

        if (Luminance(light.AmbientLightColor) <= Luminance(profile.Color))
        {
            night.Comp.Phase = FECNightPhase.Done;
            return;
        }

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
