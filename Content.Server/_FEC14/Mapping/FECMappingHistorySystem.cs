using Content.Server.Administration.Managers;
using Content.Server.Popups;
using Content.Shared._FEC14.Mapping;
using Content.Shared.Administration;
using Content.Shared.Popups;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;
using Robust.Shared.Network;
using Robust.Shared.Placement;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;

namespace Content.Server._FEC14.Mapping;

public sealed partial class FECMappingHistorySystem : EntitySystem
{
    [Dependency] private readonly IAdminManager _admin = default!;
    [Dependency] private readonly SharedMapSystem _map = default!;
    [Dependency] private readonly PopupSystem _popup = default!;
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;

    private const int MaxBatches = 100;

    private readonly Dictionary<NetUserId, LinkedList<Batch>> _undo = new();
    private readonly Dictionary<NetUserId, LinkedList<Batch>> _redo = new();
    private readonly Dictionary<(EntityUid Grid, Vector2i Indices), Tile> _pendingOldTiles = new();
    private bool _replaying;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<PlacementEntityEvent>(OnEntityPlacement);
        SubscribeLocalEvent<PlacementTileEvent>(OnTilePlacement);
        SubscribeLocalEvent<TileChangedEvent>(OnTileChanged);
        SubscribeNetworkEvent<FECMappingUndoRequestEvent>(OnUndoRequest);
        SubscribeNetworkEvent<FECMappingRedoRequestEvent>(OnRedoRequest);
        InitializeClipboard();
    }

    public override void Update(float frameTime)
    {
        _pendingOldTiles.Clear();
    }

    private void OnTileChanged(ref TileChangedEvent ev)
    {
        if (_replaying)
            return;

        foreach (var change in ev.Changes)
        {
            _pendingOldTiles.TryAdd((ev.Entity.Owner, change.GridIndices), change.OldTile);
        }
    }

    private void OnEntityPlacement(PlacementEntityEvent ev)
    {
        if (_replaying || ev.PlacerNetUserId is not { } user)
            return;

        switch (ev.PlacementEventAction)
        {
            case PlacementEventAction.Create:
                Record(user, Step.Spawned(ev.EditedEntity));
                break;
            case PlacementEventAction.Erase:
                if (Describe(ev.EditedEntity) is { } erased)
                    Record(user, erased);
                break;
        }
    }

    private void OnTilePlacement(PlacementTileEvent ev)
    {
        if (_replaying || ev.PlacerNetUserId is not { } user)
            return;

        if (!TryGetGridTile(ev.Coordinates, out var grid, out var indices))
            return;

        if (!_pendingOldTiles.TryGetValue((grid, indices), out var oldTile))
            return;

        Record(user, Step.TileChange(grid, indices, oldTile));
    }

    private void OnUndoRequest(FECMappingUndoRequestEvent ev, EntitySessionEventArgs args)
    {
        Replay(args.SenderSession, _undo, _redo, "fec-mapping-undo", "fec-mapping-undo-empty");
    }

    private void OnRedoRequest(FECMappingRedoRequestEvent ev, EntitySessionEventArgs args)
    {
        Replay(args.SenderSession, _redo, _undo, "fec-mapping-redo", "fec-mapping-redo-empty");
    }

    private void Replay(
        ICommonSession session,
        Dictionary<NetUserId, LinkedList<Batch>> from,
        Dictionary<NetUserId, LinkedList<Batch>> to,
        string doneKey,
        string emptyKey)
    {
        if (!_admin.HasAdminFlag(session, AdminFlags.Spawn))
            return;

        if (!from.TryGetValue(session.UserId, out var stack) || stack.Last is not { } node)
        {
            _popup.PopupCursor(Loc.GetString(emptyKey), session);
            return;
        }

        stack.RemoveLast();
        var inverse = new Batch(_timing.CurTick);
        _replaying = true;
        try
        {
            for (var i = node.Value.Steps.Count - 1; i >= 0; i--)
            {
                if (Revert(node.Value.Steps[i]) is { } reverted)
                    inverse.Steps.Add(reverted);
            }
        }
        finally
        {
            _replaying = false;
        }

        if (inverse.Steps.Count > 0)
            Push(to, session.UserId, inverse);

        _popup.PopupCursor(Loc.GetString(doneKey, ("count", node.Value.Steps.Count)), session);
    }

    private Step? Revert(Step step)
    {
        switch (step.Kind)
        {
            case StepKind.Spawned:
                if (step.Entity is not { } uid || TerminatingOrDeleted(uid))
                    return null;

                var erased = Describe(uid);
                QueueDel(uid);
                return erased;

            case StepKind.Erased:
                if (step.Prototype is not { } proto || step.Coordinates is not { } coords || !coords.IsValid(EntityManager))
                    return null;

                var spawned = Spawn(proto.Id, coords);
                _transform.SetLocalRotation(spawned, step.Rotation);
                return Step.Spawned(spawned);

            case StepKind.Tile:
                if (step.Grid is not { } gridUid || !TryComp(gridUid, out MapGridComponent? grid))
                    return null;

                var current = _map.GetTileRef(gridUid, grid, step.Indices).Tile;
                _map.SetTile(gridUid, grid, step.Indices, step.Tile);
                return Step.TileChange(gridUid, step.Indices, current);
        }

        return null;
    }

    private void Record(NetUserId user, Step step)
    {
        _redo.Remove(user);
        var stack = GetStack(_undo, user);
        if (stack.Last is { } last && last.Value.Tick == _timing.CurTick)
        {
            last.Value.Steps.Add(step);
            return;
        }

        var batch = new Batch(_timing.CurTick);
        batch.Steps.Add(step);
        Push(_undo, user, batch);
    }

    private void Push(Dictionary<NetUserId, LinkedList<Batch>> stacks, NetUserId user, Batch batch)
    {
        var stack = GetStack(stacks, user);
        stack.AddLast(batch);
        while (stack.Count > MaxBatches)
        {
            stack.RemoveFirst();
        }
    }

    private static LinkedList<Batch> GetStack(Dictionary<NetUserId, LinkedList<Batch>> stacks, NetUserId user)
    {
        if (!stacks.TryGetValue(user, out var stack))
        {
            stack = new LinkedList<Batch>();
            stacks[user] = stack;
        }

        return stack;
    }

    private Step? Describe(EntityUid uid)
    {
        if (MetaData(uid).EntityPrototype is not { } proto)
            return null;

        var xform = Transform(uid);
        return Step.Erased(proto.ID, xform.Coordinates, xform.LocalRotation);
    }

    private bool TryGetGridTile(EntityCoordinates coordinates, out EntityUid gridUid, out Vector2i indices)
    {
        gridUid = default;
        indices = default;
        if (_transform.GetGrid(coordinates) is not { } grid || !TryComp(grid, out MapGridComponent? gridComp))
            return false;

        gridUid = grid;
        indices = _map.TileIndicesFor(grid, gridComp, coordinates);
        return true;
    }

    private enum StepKind : byte
    {
        Spawned,
        Erased,
        Tile,
    }

    private sealed class Step
    {
        public StepKind Kind;
        public EntityUid? Entity;
        public EntProtoId? Prototype;
        public EntityCoordinates? Coordinates;
        public Angle Rotation;
        public EntityUid? Grid;
        public Vector2i Indices;
        public Tile Tile;

        public static Step Spawned(EntityUid uid) => new() { Kind = StepKind.Spawned, Entity = uid };

        public static Step Erased(EntProtoId proto, EntityCoordinates coordinates, Angle rotation) =>
            new() { Kind = StepKind.Erased, Prototype = proto, Coordinates = coordinates, Rotation = rotation };

        public static Step TileChange(EntityUid grid, Vector2i indices, Tile tile) =>
            new() { Kind = StepKind.Tile, Grid = grid, Indices = indices, Tile = tile };
    }

    private sealed class Batch(GameTick tick)
    {
        public readonly GameTick Tick = tick;
        public readonly List<Step> Steps = new();
    }
}
