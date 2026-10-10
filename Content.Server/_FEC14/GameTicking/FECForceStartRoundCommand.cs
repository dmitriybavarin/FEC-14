using System.Linq;
using Content.Server._RMC14.Rules.DistressSignal;
using Content.Server.Administration;
using Content.Server.GameTicking;
using Content.Shared._RMC14.Rules;
using Content.Shared.Administration;
using Robust.Shared.Console;
using Robust.Shared.Random;

namespace Content.Server._FEC14.GameTicking;

[AdminCommand(AdminFlags.Round)]
public sealed class FECForceStartRoundCommand : LocalizedEntityCommands
{
    [Dependency] private readonly GameTicker _ticker = default!;
    [Dependency] private readonly CMDistressSignalRuleSystem _distress = default!;
    [Dependency] private readonly RMCPlanetSystem _planets = default!;
    [Dependency] private readonly IRobustRandom _random = default!;

    public override string Command => "forcestartround";

    public override void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (_ticker.RunLevel != GameRunLevel.PreRoundLobby)
        {
            shell.WriteError(Loc.GetString("fec-cmd-forcestartround-not-lobby"));
            return;
        }

        if (args.Length > 1)
        {
            shell.WriteError(Loc.GetString("shell-wrong-arguments-number"));
            return;
        }

        if (args.Length == 1)
        {
            var planet = _planets.GetAllPlanets().FirstOrDefault(p => p.Proto.ID == args[0]);
            if (planet.Proto == null)
            {
                shell.WriteError(Loc.GetString("fec-cmd-forcestartround-no-planet", ("planet", args[0])));
                return;
            }

            _distress.CancelPlanetVote();
            _distress.SetPlanet(planet);
        }
        else if (_distress.FECSelectedPlanet == null && _planets.GetCandidatesInRotation().Count == 0)
        {
            var all = _planets.GetAllPlanetsInRotation();
            if (all.Count > 0)
            {
                _distress.CancelPlanetVote();
                _distress.SetPlanet(_random.Pick(all));
            }
        }

        shell.WriteLine(Loc.GetString("fec-cmd-forcestartround-started"));
        _ticker.StartRound(true);
    }

    public override CompletionResult GetCompletion(IConsoleShell shell, string[] args)
    {
        if (args.Length == 1)
        {
            return CompletionResult.FromHintOptions(
                _planets.GetAllPlanets().Select(p => new CompletionOption(p.Proto.ID, p.Proto.Name)),
                Loc.GetString("fec-cmd-forcestartround-hint"));
        }

        return CompletionResult.Empty;
    }
}
