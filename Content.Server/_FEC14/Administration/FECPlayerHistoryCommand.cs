using Content.Server.Administration;
using Content.Server.EUI;
using Content.Shared.Administration;
using Robust.Shared.Console;

namespace Content.Server._FEC14.Administration;

[AdminCommand(AdminFlags.ViewNotes)]
public sealed class FECPlayerHistoryCommand : LocalizedCommands
{
    [Dependency] private readonly EuiManager _eui = default!;
    [Dependency] private readonly IPlayerLocator _locator = default!;

    public override string Command => "playerhistory";

    public override async void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (shell.Player is not { } player)
        {
            shell.WriteError(Loc.GetString("shell-only-players-can-run-this-command"));
            return;
        }

        if (args.Length != 1)
        {
            shell.WriteError(Loc.GetString("shell-wrong-arguments-number"));
            return;
        }

        var located = await _locator.LookupIdByNameOrIdAsync(args[0]);
        if (located == null)
        {
            shell.WriteError(Loc.GetString("cmd-jobwhitelist-player-not-found", ("player", args[0])));
            return;
        }

        _eui.OpenEui(new FECPlayerHistoryEui(located), player);
    }
}
