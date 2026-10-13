using System.Linq;
using Content.Server.Administration;
using Content.Shared.Administration;
using Content.Shared.Roles;
using Robust.Shared.Console;
using Robust.Shared.Prototypes;

namespace Content.Server._FEC14.JobWhitelist;

[AdminCommand(AdminFlags.JobWhitelist)]
public sealed class FECTempJobWhitelistCommand : LocalizedEntityCommands
{
    [Dependency] private readonly IPlayerLocator _locator = default!;
    [Dependency] private readonly IPrototypeManager _prototypes = default!;
    [Dependency] private readonly FECTempJobWhitelistSystem _temp = default!;

    public override string Command => "jobwhitelisttemp";

    public override async void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length != 3)
        {
            shell.WriteError(Loc.GetString("shell-wrong-arguments-number"));
            shell.WriteLine(Help);
            return;
        }

        if (!_prototypes.TryIndex<JobPrototype>(args[1], out var job))
        {
            shell.WriteError(Loc.GetString("cmd-jobwhitelist-job-does-not-exist", ("job", args[1])));
            return;
        }

        if (!FECTempJobWhitelistSystem.TryParseDuration(args[2], out var duration))
        {
            shell.WriteError(Loc.GetString("fec-temp-whitelist-bad-duration", ("duration", args[2])));
            return;
        }

        var data = await _locator.LookupIdByNameOrIdAsync(args[0]);
        if (data == null)
        {
            shell.WriteError(Loc.GetString("cmd-jobwhitelist-player-not-found", ("player", args[0])));
            return;
        }

        _temp.Add(data.UserId, data.Username, job.ID, duration, shell.Player?.Name ?? "console");
        shell.WriteLine(Loc.GetString("fec-temp-whitelist-added",
            ("player", data.Username),
            ("job", job.LocalizedName),
            ("date", $"{DateTime.Now + duration:dd.MM.yyyy HH:mm}")));
    }

    public override CompletionResult GetCompletion(IConsoleShell shell, string[] args)
    {
        return args.Length switch
        {
            1 => CompletionResult.FromHint(Loc.GetString("cmd-jobwhitelisttemp-hint-player")),
            2 => CompletionResult.FromHintOptions(
                _prototypes.EnumeratePrototypes<JobPrototype>().Select(j => j.ID).OrderBy(id => id),
                Loc.GetString("cmd-jobwhitelisttemp-hint-job")),
            3 => CompletionResult.FromHint(Loc.GetString("cmd-jobwhitelisttemp-hint-duration")),
            _ => CompletionResult.Empty,
        };
    }
}
