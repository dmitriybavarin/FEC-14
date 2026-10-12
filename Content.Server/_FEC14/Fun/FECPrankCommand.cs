using Content.Server.Administration;
using Content.Server.Administration.Logs;
using Content.Shared.Administration;
using Content.Shared.Database;
using Robust.Server.Player;
using Robust.Shared.Console;

namespace Content.Server._FEC14.Fun;

[AdminCommand(AdminFlags.Fun)]
public sealed class FECPrankCommand : LocalizedEntityCommands
{
    [Dependency] private readonly IAdminLogManager _adminLog = default!;
    [Dependency] private readonly FECFunSystem _fun = default!;
    [Dependency] private readonly IPlayerManager _players = default!;

    private static readonly string[] Pranks = { "say", "honk", "balloon", "spin" };

    public override string Command => "fecprank";

    public override void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length != 2)
        {
            shell.WriteError(Loc.GetString("shell-wrong-arguments-number"));
            shell.WriteLine(Help);
            return;
        }

        if (!_players.TryGetSessionByUsername(args[0], out var session) || session.AttachedEntity is not { } target)
        {
            shell.WriteError(Loc.GetString("fec-tp-no-player", ("player", args[0])));
            return;
        }

        if (!_fun.TryPrank(target, args[1]))
        {
            shell.WriteError(Loc.GetString("fec-prank-unknown", ("prank", args[1])));
            return;
        }

        _adminLog.Add(LogType.AdminCommands, LogImpact.Low, $"{shell.Player?.Name ?? "console"} pranked {EntityManager.ToPrettyString(target)} with {args[1]}");
        shell.WriteLine(Loc.GetString("fec-prank-done", ("player", session.Name), ("prank", Loc.GetString($"fec-prank-{args[1]}"))));
    }

    public override CompletionResult GetCompletion(IConsoleShell shell, string[] args)
    {
        return args.Length switch
        {
            1 => CompletionResult.FromHintOptions(CompletionHelper.SessionNames(players: _players), Loc.GetString("fec-tp-hint-player")),
            2 => CompletionResult.FromOptions(Pranks),
            _ => CompletionResult.Empty,
        };
    }
}
