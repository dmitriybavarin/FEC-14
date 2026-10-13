using Content.Server.Administration;
using Content.Server.Chat.Managers;
using Content.Shared._RMC14.CCVar;
using Content.Shared.Administration;
using Content.Shared.Chat;
using Robust.Shared.Configuration;
using Robust.Shared.Console;

namespace Content.Server._RMC14.Admin;

/// <summary>
/// Delays the round end even after the round end screen.
/// </summary>
[AdminCommand(AdminFlags.RMCMaintainer)] //RMC14
[AdminCommand(AdminFlags.Fun)]
public sealed class RMCDelayRoundEndCommand : LocalizedEntityCommands
{
    [Dependency] private readonly IChatManager _chatManager = default!;
    [Dependency] private readonly IConfigurationManager _cfg = default!;

    public override string Command => "rmcdelayroundend";
    public override string Description => Loc.GetString("fec-cmd-r-m-c-delay-round-end-desc"); // FEC14
    public override void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        var currentValue = _cfg.GetCVar(RMCCVars.RMCDelayRoundEnd);
        var chatMsg = "";
        var silent = false;

        if (args.Length >= 3)
        {
            shell.WriteError(Loc.GetString("fec-cmd-r-m-c-delay-round-end-wrong-args"));
            return;
        }

        if (args.Length >= 2)
            chatMsg = args[1];

        if (args.Length >= 1)
        {
            if (!bool.TryParse(args[0], out var silentParsed))
            {
                shell.WriteError(Loc.GetString("shell-invalid-bool") + ": " + Loc.GetString("fec-cmd-r-m-c-delay-round-end-silent-bool"));
                return;
            }
            silent = silentParsed;
        }

        _chatManager.SendAdminAnnouncement(Loc.GetString("fec-cmd-r-m-c-delay-round-end-admin", ("player", shell.Player?.Name ?? "Server"), ("value", (!currentValue).ToString().ToLowerInvariant())));

        _cfg.SetCVar(RMCCVars.RMCDelayRoundEnd, !currentValue);

        if (silent)
            return;

        if (chatMsg == "")
        {
            if (currentValue)
            {
                shell.WriteLine(Loc.GetString("fec-cmd-r-m-c-delay-round-end-removed-shell"));
                chatMsg = "\n" + Loc.GetString("fec-cmd-r-m-c-delay-round-end-removed") + "\n";
            }
            else
            {
                shell.WriteLine(Loc.GetString("fec-cmd-r-m-c-delay-round-end-delayed-shell"));
                chatMsg = "\n" + Loc.GetString("fec-cmd-r-m-c-delay-round-end-delayed") + "\n";
            }
        }

        _chatManager.ChatMessageToAll(ChatChannel.Local, chatMsg, chatMsg, default, false, true);
    }

    public override CompletionResult GetCompletion(IConsoleShell shell, string[] args)
    {
        return args.Length switch
        {
            1 => CompletionResult.FromHint(Loc.GetString("fec-cmd-r-m-c-delay-round-end-hint-silent")),
            2 => CompletionResult.FromHint(Loc.GetString("fec-cmd-r-m-c-delay-round-end-hint-message")),
            _ => CompletionResult.Empty,
        };
    }
}
