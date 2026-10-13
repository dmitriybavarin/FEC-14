using System.Linq;
using System.Numerics;
using Content.Shared._FEC14.Mapping;
using Content.Shared.Administration;
using Content.Shared.Mobs.Components;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;
using Robust.Shared.Network;
using Robust.Shared.Player;

namespace Content.Server._FEC14.Mapping;

public sealed partial class FECMappingHistorySystem
{
    [Dependency] private readonly EntityLookupSystem _lookup = default!;

    private const int MaxClipboardSide = 64;

    private readonly Dictionary<NetUserId, Clipboard> _clipboards = new();

    private void InitializeClipboard()
    {
        SubscribeNetworkEvent<FECMappingCopyRequestEvent>(OnCopyRequest);
        SubscribeNetworkEvent<FECMappingPasteRequestEvent>(OnPasteRequest);
    }

    private void OnCopyRequest(FECMappingCopyRequestEvent ev, EntitySessionEventArgs args)
    {
        var session = args.SenderSession;
        if (!_admin.HasAdminFlag(session, AdminFlags.Spawn))
            return;

        var first = GetCoordinates(ev.First);
        var second = GetCoordinates(ev.Second);
        if (!TryGetGridTile(first, out var grid, out var a) ||
            !TryGetGridTile(second, out var otherGrid, out var b) ||
            grid != otherGrid ||
            !TryComp(grid, out MapGridComponent? gridComp))
        {
            _popup.PopupCursor(Loc.GetString("fec-mapping-copy-bad-grid"), session);
            return;
        }

        var min = new Vector2i(Math.Min(a.X, b.X), Math.Min(a.Y, b.Y));
        var max = new Vector2i(Math.Max(a.X, b.X), Math.Max(a.Y, b.Y));
        if (max.X - min.X + 1 > MaxClipboardSide || max.Y - min.Y + 1 > MaxClipboardSide)
        {
            _popup.PopupCursor(Loc.GetString("fec-mapping-copy-too-big", ("max", MaxClipboardSide)), session);
            return;
        }

        var clipboard = new Clipboard();
        var indices = new List<Vector2i>();
        for (var x = min.X; x <= max.X; x++)
        {
            for (var y = min.Y; y <= max.Y; y++)
            {
                var index = new Vector2i(x, y);
                indices.Add(index);
                clipboard.Tiles.Add((index - min, _map.GetTileRef(grid, gridComp, index).Tile));
            }
        }

        var origin = (Vector2) min * gridComp.TileSize;
        foreach (var uid in _lookup.GetLocalEntitiesIntersecting(grid, indices))
        {
            if (!CanCopy(uid, grid))
                continue;

            var xform = Transform(uid);
            var tile = _map.TileIndicesFor(grid, gridComp, xform.Coordinates);
            if (tile.X < min.X || tile.Y < min.Y || tile.X > max.X || tile.Y > max.Y)
                continue;

            clipboard.Entities.Add((MetaData(uid).EntityPrototype!.ID, xform.LocalPosition - origin, xform.LocalRotation));
        }

        _clipboards[session.UserId] = clipboard;
        _popup.PopupCursor(Loc.GetString("fec-mapping-copied",
            ("tiles", clipboard.Tiles.Count),
            ("entities", clipboard.Entities.Count)), session);
    }

    private void OnPasteRequest(FECMappingPasteRequestEvent ev, EntitySessionEventArgs args)
    {
        var session = args.SenderSession;
        if (!_admin.HasAdminFlag(session, AdminFlags.Spawn))
            return;

        if (!_clipboards.TryGetValue(session.UserId, out var clipboard))
        {
            _popup.PopupCursor(Loc.GetString("fec-mapping-clipboard-empty"), session);
            return;
        }

        var target = GetCoordinates(ev.Target);
        if (!TryGetGridTile(target, out var grid, out var baseIndex) || !TryComp(grid, out MapGridComponent? gridComp))
        {
            _popup.PopupCursor(Loc.GetString("fec-mapping-copy-bad-grid"), session);
            return;
        }

        var batch = new Batch(_timing.CurTick);
        _replaying = true;
        try
        {
            foreach (var (offset, tile) in clipboard.Tiles)
            {
                var index = baseIndex + offset;
                var old = _map.GetTileRef(grid, gridComp, index).Tile;
                _map.SetTile(grid, gridComp, index, tile);
                Record(session.UserId, Step.TileChange(grid, index, old), batch);
            }

            var origin = (Vector2) baseIndex * gridComp.TileSize;
            foreach (var (proto, offset, rotation) in clipboard.Entities)
            {
                var spawned = Spawn(proto, new EntityCoordinates(grid, origin + offset));
                _transform.SetLocalRotation(spawned, rotation);
                Record(session.UserId, Step.Spawned(spawned), batch);
            }
        }
        finally
        {
            _replaying = false;
        }

        if (batch.Steps.Count > 0)
            Push(_undo, session.UserId, batch);

        _popup.PopupCursor(Loc.GetString("fec-mapping-pasted",
            ("tiles", clipboard.Tiles.Count),
            ("entities", clipboard.Entities.Count)), session);
    }

    private bool CanCopy(EntityUid uid, EntityUid grid)
    {
        if (TerminatingOrDeleted(uid) ||
            HasComp<MapGridComponent>(uid) ||
            HasComp<ActorComponent>(uid) ||
            HasComp<MobStateComponent>(uid) ||
            MetaData(uid).EntityPrototype == null)
        {
            return false;
        }

        return Transform(uid).ParentUid == grid;
    }

    private sealed class Clipboard
    {
        public readonly List<(Vector2i Offset, Tile Tile)> Tiles = new();
        public readonly List<(string Prototype, Vector2 Offset, Angle Rotation)> Entities = new();
    }
}
