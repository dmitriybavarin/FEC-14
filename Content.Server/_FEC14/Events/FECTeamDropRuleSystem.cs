using Content.Server._FEC14.GameTicking;
using Content.Server.Explosion.EntitySystems;
using Content.Server.GameTicking.Rules;
using Content.Shared._RMC14.Marines.Announce;
using Content.Shared._RMC14.Xenonids.Announce;
using Content.Shared.GameTicking.Components;
using Robust.Shared.Map;
using Robust.Shared.Random;

namespace Content.Server._FEC14.Events;

public sealed class FECTeamDropRuleSystem : GameRuleSystem<FECTeamDropRuleComponent>
{
    [Dependency] private readonly SharedMarineAnnounceSystem _announce = default!;
    [Dependency] private readonly FECEventsSystem _events = default!;
    [Dependency] private readonly ExplosionSystem _explosion = default!;
    [Dependency] private readonly FECRoundSummarySystem _summary = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;
    [Dependency] private readonly SharedXenoAnnounceSystem _xenoAnnounce = default!;

    protected override void Started(EntityUid uid, FECTeamDropRuleComponent comp, GameRuleComponent gameRule, GameRuleStartedEvent args)
    {
        base.Started(uid, comp, gameRule, args);

        if (TryGetSpawn(comp.Location, out var coordinates))
        {
            if (comp.ExplosionType is { } explosion)
                _explosion.QueueExplosion(_transform.ToMapCoordinates(coordinates), explosion, comp.ExplosionIntensity, 5f, 30f, null);

            foreach (var spawner in comp.Spawners)
            {
                Spawn(spawner, coordinates.Offset(RobustRandom.NextVector2(comp.Scatter)));
            }

            if (comp.AresAnnouncement is { } ares)
                _announce.AnnounceARES(null, Loc.GetString(ares));

            if (comp.HighCommandAnnouncement is { } highCommand)
                _announce.AnnounceHighCommand(Loc.GetString(highCommand));

            if (comp.XenoAnnouncement is { } xeno)
                _xenoAnnounce.AnnounceQueenMother(Loc.GetString(xeno));

            if (comp.Summary is { } summary)
                _summary.AddLine(Loc.GetString(summary));

            _events.ReportOutcome(uid, comp.Outcome);
        }
        else
        {
            Log.Info($"FEC team drop event {ToPrettyString(uid)} found no {comp.Location} spawn point");
            _events.ReportOutcome(uid, "failed");
        }

        GameTicker.EndGameRule(uid, gameRule);
    }

    private bool TryGetSpawn(FECEventLocation location, out EntityCoordinates coordinates)
    {
        return location == FECEventLocation.Ship
            ? _events.TryGetShipSpawn(out coordinates)
            : _events.TryGetPlanetSpawn(out coordinates);
    }
}
