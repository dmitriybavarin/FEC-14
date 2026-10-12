using Content.Server._FEC14.GameTicking;
using Content.Server.GameTicking.Rules;
using Content.Shared._RMC14.Marines.Announce;
using Content.Shared.GameTicking.Components;
using Robust.Shared.Random;

namespace Content.Server._FEC14.Events;

public sealed class FECTeamDropRuleSystem : GameRuleSystem<FECTeamDropRuleComponent>
{
    [Dependency] private readonly SharedMarineAnnounceSystem _announce = default!;
    [Dependency] private readonly FECEventsSystem _events = default!;
    [Dependency] private readonly FECRoundSummarySystem _summary = default!;

    protected override void Started(EntityUid uid, FECTeamDropRuleComponent comp, GameRuleComponent gameRule, GameRuleStartedEvent args)
    {
        base.Started(uid, comp, gameRule, args);

        if (_events.TryGetPlanetSpawn(out var coordinates))
        {
            foreach (var spawner in comp.Spawners)
            {
                Spawn(spawner, coordinates.Offset(RobustRandom.NextVector2(1.5f)));
            }

            if (comp.AresAnnouncement is { } ares)
                _announce.AnnounceARES(null, Loc.GetString(ares));

            if (comp.HighCommandAnnouncement is { } highCommand)
                _announce.AnnounceHighCommand(Loc.GetString(highCommand));

            if (comp.Summary is { } summary)
                _summary.AddLine(Loc.GetString(summary));

            _events.ReportOutcome(uid, "landed");
        }
        else
        {
            Log.Info($"FEC team drop event {ToPrettyString(uid)} found no planet spawn point");
            _events.ReportOutcome(uid, "failed");
        }

        GameTicker.EndGameRule(uid, gameRule);
    }
}
