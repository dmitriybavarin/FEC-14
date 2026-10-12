using System.Linq;
using Content.Server._FEC14.GameTicking;
using Content.Server.GameTicking.Rules;
using Content.Shared._RMC14.Marines.Announce;
using Content.Shared._RMC14.Weather;
using Content.Shared.GameTicking.Components;
using Robust.Shared.Random;

namespace Content.Server._FEC14.Events;

public sealed class FECWeatherStormRuleSystem : GameRuleSystem<FECWeatherStormRuleComponent>
{
    [Dependency] private readonly SharedMarineAnnounceSystem _announce = default!;
    [Dependency] private readonly FECEventsSystem _events = default!;
    [Dependency] private readonly FECRoundSummarySystem _summary = default!;
    [Dependency] private readonly RMCWeatherSystem _weather = default!;

    protected override void Started(EntityUid uid, FECWeatherStormRuleComponent comp, GameRuleComponent gameRule, GameRuleStartedEvent args)
    {
        base.Started(uid, comp, gameRule, args);

        if (_events.GetPlanetMap() is { } planet)
        {
            var options = _weather.GetWeatherEventCompletionOptions(planet).Select(o => o.Value).ToList();
            RobustRandom.Shuffle(options);
            foreach (var option in options)
            {
                if (!_weather.TryStartWeatherEvent(planet, option, false, false, out _))
                    continue;

                _announce.AnnounceHighCommand(Loc.GetString(comp.HighCommandAnnouncement));
                _summary.AddLine(Loc.GetString(comp.Summary));
                break;
            }
        }

        GameTicker.EndGameRule(uid, gameRule);
    }
}
