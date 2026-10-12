using Content.Server._RMC14.Rules.DistressSignal;
using Content.Shared._FEC14.Weather;
using Content.Shared._RMC14.Rules;
using Content.Shared._RMC14.Weather;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;

namespace Content.Server._FEC14.Weather;

public sealed class FECPlanetWeatherSystem : EntitySystem
{
    [Dependency] private readonly CMDistressSignalRuleSystem _distress = default!;
    [Dependency] private readonly IPrototypeManager _prototypes = default!;
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly RMCWeatherSystem _weather = default!;

    private readonly List<(EntityUid Map, string Planet)> _pending = new();

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<RMCPlanetComponent, RMCPlanetAddedEvent>(OnPlanetAdded);
    }

    private void OnPlanetAdded(Entity<RMCPlanetComponent> ent, ref RMCPlanetAddedEvent args)
    {
        if (_distress.FECSelectedPlanet is { } planet)
            _pending.Add((ent.Owner, planet.Proto.ID));
    }

    public override void Update(float frameTime)
    {
        if (_pending.Count == 0)
            return;

        foreach (var (map, planet) in _pending)
        {
            if (!TerminatingOrDeleted(map) && _prototypes.TryIndex<FECPlanetWeatherPrototype>(planet, out var profile))
                Apply(map, profile);
        }

        _pending.Clear();
    }

    private void Apply(EntityUid map, FECPlanetWeatherPrototype profile)
    {
        var clearRound = profile.ClearRoundChance is { } clear ? _random.Prob(Math.Clamp(clear, 0, 1)) : (bool?) null;
        _weather.FECApplyPlanetProfile(map, profile, clearRound);
        Log.Info($"Applied planet weather profile {profile.ID} to {ToPrettyString(map)}, clear round: {clearRound?.ToString() ?? "default"}");
    }
}
