using Content.Server.Administration;
using Content.Server.Administration.Logs;
using Content.Shared.Administration;
using Content.Shared.Database;
using Robust.Shared.Console;

namespace Content.Server._FEC14.Night;

[AdminCommand(AdminFlags.Weather)]
public sealed class FECNightCommand : LocalizedCommands
{
    [Dependency] private readonly IAdminLogManager _adminLog = default!;
    [Dependency] private readonly FECNightSystem _night = default!;

    public override string Command => "fecnight";

    public override void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length != 1 || args[0] is not ("start" or "end"))
        {
            shell.WriteError(Help);
            return;
        }

        var start = args[0] == "start";
        if (!_night.TryForce(start, out var message))
        {
            shell.WriteError(message);
            return;
        }

        shell.WriteLine(message);
        _adminLog.Add(LogType.AdminCommands, LogImpact.Medium, $"{shell.Player?.Name ?? "Server"} {(start ? "forced night" : "ended night")}");
    }

    public override CompletionResult GetCompletion(IConsoleShell shell, string[] args)
    {
        return args.Length == 1
            ? CompletionResult.FromOptions(new[] { new CompletionOption("start"), new CompletionOption("end") })
            : CompletionResult.Empty;
    }
}
