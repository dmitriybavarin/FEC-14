using System.Linq;
using System.Text;
using Content.Server._FEC14.JobWhitelist;
using Content.Server.Administration;
using Content.Shared._FEC14.Events;
using Content.Shared.Administration;
using Robust.Shared.Console;
using Robust.Shared.Prototypes;

namespace Content.Server._FEC14.Events;

[AdminCommand(AdminFlags.Round)]
public sealed class FECEventsCommand : LocalizedEntityCommands
{
    [Dependency] private readonly FECEventsSystem _events = default!;
    [Dependency] private readonly IPrototypeManager _prototypes = default!;

    public override string Command => "fecevents";

    public override void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length == 0 || args[0] == "list")
        {
            var sb = new StringBuilder();
            foreach (var proto in _prototypes.EnumeratePrototypes<FECEventPrototype>().OrderBy(p => p.ID))
            {
                sb.AppendLine(Loc.GetString("fec-events-list-entry",
                    ("id", proto.ID),
                    ("name", Loc.GetString(proto.Name)),
                    ("status", _events.GetStatus(proto)),
                    ("players", proto.MinPlayers)));
            }

            shell.WriteLine(sb.Length == 0 ? Loc.GetString("fec-events-none") : sb.ToString().TrimEnd());
            return;
        }

        if (args[0] == "rotation")
        {
            _events.ResetRotation(shell.Player?.Name ?? "console");
            shell.WriteLine(Loc.GetString("fec-events-rotation-reset"));
            return;
        }

        if (args.Length < 2)
        {
            shell.WriteError(Loc.GetString("shell-wrong-arguments-number"));
            shell.WriteLine(Help);
            return;
        }

        var id = args[1];
        if (!_prototypes.HasIndex<FECEventPrototype>(id))
        {
            shell.WriteError(Loc.GetString("fec-events-unknown", ("id", id)));
            return;
        }

        switch (args[0])
        {
            case "block":
            {
                TimeSpan? duration = null;
                if (args.Length > 2 && args[2] != "forever")
                {
                    if (!FECTempJobWhitelistSystem.TryParseDuration(args[2], out var parsed))
                    {
                        shell.WriteError(Loc.GetString("fec-temp-whitelist-bad-duration", ("duration", args[2])));
                        return;
                    }

                    duration = parsed;
                }

                _events.Block(id, duration, shell.Player?.Name ?? "console");
                shell.WriteLine(Loc.GetString("fec-events-blocked-done", ("id", id)));
                return;
            }
            case "unblock":
                shell.WriteLine(Loc.GetString(_events.Unblock(id, shell.Player?.Name ?? "console") ? "fec-events-unblocked" : "fec-events-not-blocked", ("id", id)));
                return;
            case "start":
                if (_events.TryStart(id, true, out var reason))
                    shell.WriteLine(Loc.GetString("fec-events-started", ("id", id)));
                else
                    shell.WriteError(reason);
                return;
            default:
                shell.WriteLine(Help);
                return;
        }
    }

    public override CompletionResult GetCompletion(IConsoleShell shell, string[] args)
    {
        return args.Length switch
        {
            1 => CompletionResult.FromOptions(new[] { "list", "block", "unblock", "start", "rotation" }),
            2 => CompletionResult.FromHintOptions(
                _prototypes.EnumeratePrototypes<FECEventPrototype>().Select(p => p.ID).OrderBy(i => i),
                Loc.GetString("fec-events-hint-id")),
            3 when args[0] == "block" => CompletionResult.FromHint(Loc.GetString("fec-events-hint-duration")),
            _ => CompletionResult.Empty,
        };
    }
}
