using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Content.Server.Administration;
using Content.Shared.Administration;
using Robust.Server.Player;
using Robust.Shared.Console;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;

namespace Content.Server._FEC14.Administration;

[AdminCommand(AdminFlags.Admin)]
public sealed class FECTeleportCommand : LocalizedEntityCommands
{
    [Dependency] private readonly IMapManager _mapManager = default!;
    [Dependency] private readonly SharedMapSystem _maps = default!;
    [Dependency] private readonly IPlayerManager _players = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;

    public override string Command => "fectp";

    public override void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length is < 1 or > 2)
        {
            shell.WriteError(Loc.GetString("shell-wrong-arguments-number"));
            shell.WriteLine(Help);
            return;
        }

        EntityUid who;
        if (args.Length == 2)
        {
            if (!_players.TryGetSessionByUsername(args[1], out var session) || session.AttachedEntity is not { } attached)
            {
                shell.WriteError(Loc.GetString("fec-tp-no-player", ("player", args[1])));
                return;
            }

            who = attached;
        }
        else if (shell.Player?.AttachedEntity is { } self)
        {
            who = self;
        }
        else
        {
            shell.WriteError(Loc.GetString("shell-only-players-can-run-this-command"));
            return;
        }

        if (!TryResolve(args[0], out var destination, out var description))
        {
            shell.WriteError(Loc.GetString("fec-tp-not-found", ("target", args[0])));
            return;
        }

        _transform.SetMapCoordinates(who, destination.Value);
        _transform.AttachToGridOrMap(who);
        shell.WriteLine(Loc.GetString("fec-tp-done", ("target", description),
            ("map", destination.Value.MapId), ("x", $"{destination.Value.X:0.#}"), ("y", $"{destination.Value.Y:0.#}")));
    }

    private bool TryResolve(string text, [NotNullWhen(true)] out MapCoordinates? destination, out string description)
    {
        destination = null;
        description = text;

        if (text.StartsWith("map:", StringComparison.OrdinalIgnoreCase) || text.StartsWith("m:", StringComparison.OrdinalIgnoreCase))
        {
            var number = text[(text.IndexOf(':') + 1)..];
            if (!int.TryParse(number, out var mapNumber) || !_maps.TryGetMap(new MapId(mapNumber), out var mapUid))
                return false;

            return TryResolveEntity(mapUid.Value, out destination, out description);
        }

        if (int.TryParse(text, out var id))
        {
            var local = new EntityUid(id);
            if (EntityManager.EntityExists(local) && (EntityManager.HasComponent<MapGridComponent>(local) || EntityManager.HasComponent<MapComponent>(local)))
                return TryResolveEntity(local, out destination, out description);

            if (EntityManager.TryGetEntity(new NetEntity(id), out var net))
                return TryResolveEntity(net.Value, out destination, out description);

            if (EntityManager.EntityExists(local))
                return TryResolveEntity(local, out destination, out description);
        }

        if (_players.TryGetSessionByUsername(text, out var player) && player.AttachedEntity is { } attached)
            return TryResolveEntity(attached, out destination, out description);

        return false;
    }

    private bool TryResolveEntity(EntityUid uid, [NotNullWhen(true)] out MapCoordinates? destination, out string description)
    {
        description = EntityManager.ToPrettyString(uid).ToString();
        destination = null;

        if (EntityManager.TryGetComponent(uid, out MapComponent? map))
        {
            var grids = _mapManager.GetAllGrids(map.MapId).ToList();
            if (grids.Count == 0)
            {
                destination = new MapCoordinates(0, 0, map.MapId);
                return true;
            }

            var largest = grids.MaxBy(g => g.Comp.LocalAABB.Width * g.Comp.LocalAABB.Height);
            description = EntityManager.ToPrettyString(largest.Owner).ToString();
            uid = largest.Owner;
        }

        if (EntityManager.TryGetComponent(uid, out MapGridComponent? grid))
        {
            destination = _transform.ToMapCoordinates(new EntityCoordinates(uid, grid.LocalAABB.Center));
            return true;
        }

        destination = _transform.GetMapCoordinates(uid);
        return destination.Value.MapId != MapId.Nullspace;
    }

    public override CompletionResult GetCompletion(IConsoleShell shell, string[] args)
    {
        return args.Length switch
        {
            1 => CompletionResult.FromHintOptions(
                _maps.GetAllMapIds().OrderBy(m => (int) m).Select(m => $"map:{(int) m}")
                    .Concat(_players.Sessions.Select(s => s.Name)),
                Loc.GetString("fec-tp-hint-target")),
            2 => CompletionResult.FromHintOptions(_players.Sessions.Select(s => s.Name), Loc.GetString("fec-tp-hint-player")),
            _ => CompletionResult.Empty,
        };
    }
}
