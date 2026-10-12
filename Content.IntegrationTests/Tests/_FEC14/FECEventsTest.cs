using System.Linq;
using Content.Server._FEC14.Events;
using Content.Server.GameTicking;
using Content.Shared._FEC14.Events;
using Content.Shared.GameTicking.Components;
using Content.Shared.Roles;
using Robust.Shared.GameObjects;
using Robust.Shared.Localization;
using Robust.Shared.Prototypes;

namespace Content.IntegrationTests.Tests._FEC14;

[TestFixture]
public sealed class FECEventsTest
{
    [Test]
    public async Task EventPrototypesAreValid()
    {
        await using var pair = await PoolManager.GetServerClient();
        var server = pair.Server;
        var prototypes = server.ResolveDependency<IPrototypeManager>();
        var loc = server.ResolveDependency<ILocalizationManager>();

        await server.WaitAssertion(() =>
        {
            var events = prototypes.EnumeratePrototypes<FECEventPrototype>().ToList();
            Assert.That(events, Is.Not.Empty);

            Assert.Multiple(() =>
            {
                foreach (var proto in events)
                {
                    Assert.That(prototypes.TryIndex(proto.Rule, out var rule), $"{proto.ID}: rule {proto.Rule} does not exist");
                    Assert.That(rule?.HasComponent<GameRuleComponent>() ?? false, $"{proto.ID}: rule {proto.Rule} has no GameRule component");
                    Assert.That(loc.HasString(proto.Name), $"{proto.ID}: missing locale {proto.Name}");
                    if (proto.Description is { } description)
                        Assert.That(loc.HasString(description), $"{proto.ID}: missing locale {description}");

                    Assert.That(proto.EarliestStart, Is.LessThanOrEqualTo(proto.LatestStart), $"{proto.ID}: earliestStart is after latestStart");
                    Assert.That(proto.Weight, Is.GreaterThanOrEqualTo(0), $"{proto.ID}: negative weight");
                    Assert.That(proto.MinPlayers, Is.GreaterThanOrEqualTo(0), $"{proto.ID}: negative minPlayers");

                    foreach (var chain in proto.Chains)
                    {
                        Assert.That(prototypes.HasIndex(chain.Event), $"{proto.ID}: chain to unknown event {chain.Event}");
                        Assert.That(chain.Event.Id, Is.Not.EqualTo(proto.ID), $"{proto.ID}: chain to itself");
                        Assert.That(chain.Chance, Is.InRange(0f, 1f), $"{proto.ID}: chain chance out of range");
                        Assert.That(loc.HasString($"fec-events-outcome-{chain.Outcome}"), $"{proto.ID}: missing locale for outcome {chain.Outcome}");
                    }
                }

                foreach (var antag in prototypes.EnumeratePrototypes<AntagPrototype>().Where(a => a.ID.StartsWith("FECEvent")))
                {
                    Assert.That(antag.SetPreference, $"{antag.ID}: setPreference must be true to show in the events tab");
                    Assert.That(loc.HasString(antag.Name), $"{antag.ID}: missing locale {antag.Name}");
                    Assert.That(loc.HasString(antag.Objective), $"{antag.ID}: missing locale {antag.Objective}");
                }
            });
        });

        await pair.CleanReturnAsync();
    }

    [Test]
    public async Task EventRulesStartAndEndWithoutRound()
    {
        await using var pair = await PoolManager.GetServerClient(new PoolSettings
        {
            Dirty = true,
            DummyTicker = false,
        });
        var server = pair.Server;
        var prototypes = server.ResolveDependency<IPrototypeManager>();
        var ticker = server.System<GameTicker>();
        var events = server.System<FECEventsSystem>();

        await server.WaitAssertion(() =>
        {
            Assert.That(events.TryStart("FECEventThatDoesNotExist", true, out _), Is.False);

            foreach (var proto in prototypes.EnumeratePrototypes<FECEventPrototype>().OrderBy(p => p.ID))
            {
                ticker.StartGameRule(proto.Rule);
            }
        });

        await server.WaitRunTicks(10);

        await server.WaitAssertion(() =>
        {
            ticker.ClearGameRules();
            Assert.That(ticker.GetAddedGameRules().Any(), Is.False);
        });

        await pair.CleanReturnAsync();
    }
}
