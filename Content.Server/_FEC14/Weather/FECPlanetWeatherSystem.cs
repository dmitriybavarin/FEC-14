using Content.Server._RMC14.Rules.DistressSignal;
using Content.Shared._FEC14.Weather;
using Content.Shared._RMC14.Rules;
using Content.Shared._RMC14.Weather;
using Content.Shared.GameTicking;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;

namespace Content.Server._FEC14.Weather;

public sealed class FECPlanetWeatherSystem : EntitySystem
{
    [Dependency] private readonly CMDistressSignalRuleSystem _distress = default!;
    [Dependency] private readonly IPrototypeManager _prototypes = default!;
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly RMCWeatherSystem _weather = default!;

    private readonly HashSet<EntityUid> _handled = new();

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<RoundRestartCleanupEvent>(_ => _handled.Clear());
    }

    public override void Update(float frameTime)
    {
        var query = EntityQueryEnumerator<RMCPlanetComponent, MetaDataComponent>();
        while (query.MoveNext(out var uid, out _, out var meta))
        {
            if (meta.EntityLifeStage < EntityLifeStage.MapInitialized || !_handled.Add(uid))
                continue;

            if (_distress.FECSelectedPlanet is { } planet &&
                _prototypes.TryIndex<FECPlanetWeatherPrototype>(planet.Proto.ID, out var profile))
            {
                Apply(uid, profile);
            }
        }
    }

    private void Apply(EntityUid map, FECPlanetWeatherPrototype profile)
    {
        var clearRound = profile.ClearRoundChance is { } clear ? _random.Prob(Math.Clamp(clear, 0, 1)) : (bool?) null;
        _weather.FECApplyPlanetProfile(map, profile, clearRound);
        Log.Info($"Applied planet weather profile {profile.ID} to {ToPrettyString(map)}, clear round: {clearRound?.ToString() ?? "default"}");
    }
}
