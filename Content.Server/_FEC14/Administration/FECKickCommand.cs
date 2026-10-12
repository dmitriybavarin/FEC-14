using System.Linq;
using Content.Server.Administration;
using Content.Shared.Administration;
using Robust.Server.Player;
using Robust.Shared.Console;
using Robust.Shared.Network;

namespace Content.Server._FEC14.Administration;

[AdminCommand(AdminFlags.Moderator)]
public sealed class FECKickCommand : LocalizedCommands
{
    [Dependency] private readonly FECAdminWeightManager _weight = default!;
    [Dependency] private readonly IPlayerManager _players = default!;
    [Dependency] private readonly IServerNetManager _net = default!;

    public override string Command => "feckick";

    public override void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length < 1)
        {
            shell.WriteError(Loc.GetString("shell-wrong-arguments-number"));
            return;
        }

        if (!_players.TryGetSessionByUsername(args[0], out var target))
        {
            shell.WriteError(Loc.GetString("cmd-feckick-not-found", ("player", args[0])));
            return;
        }

        if (shell.Player is { } actor && !_weight.CanTarget(actor, target))
        {
            shell.WriteError(Loc.GetString("fec-admin-weight-blocked"));
            return;
        }

        var reason = args.Length >= 2
            ? Loc.GetString("cmd-feckick-reason", ("reason", string.Join(' ', args[1..])))
            : Loc.GetString("cmd-feckick-no-reason");

        _net.DisconnectChannel(target.Channel, reason);
        shell.WriteLine(Loc.GetString("cmd-feckick-success", ("player", target.Name)));
    }

    public override CompletionResult GetCompletion(IConsoleShell shell, string[] args)
    {
        return args.Length switch
        {
            1 => CompletionResult.FromHintOptions(
                _players.Sessions.OrderBy(s => s.Name).Select(s => s.Name),
                Loc.GetString("cmd-feckick-hint-player")),
            2 => CompletionResult.FromHint(Loc.GetString("cmd-feckick-hint-reason")),
            _ => CompletionResult.Empty,
        };
    }
}
