using Content.Client._RMC14.TacticalMap;
using Content.Client.Administration.Managers;
using Content.Shared._FEC14.Mapping;
using Content.Shared.Input;
using Content.Shared.Interaction;
using Content.Shared.Maps;
using Content.Shared.Popups;
using Robust.Client.Placement;
using Robust.Shared.Enums;
using Robust.Shared.Input;
using Robust.Shared.Input.Binding;
using Robust.Shared.Map;

namespace Content.Client._FEC14.Mapping;

public sealed class FECMappingHistorySystem : EntitySystem
{
    [Dependency] private readonly IClientAdminManager _admin = default!;
    [Dependency] private readonly SharedMapSystem _map = default!;
    [Dependency] private readonly IMapManager _mapManager = default!;
    [Dependency] private readonly IPlacementManager _placement = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;
    [Dependency] private readonly TurfSystem _turf = default!;

    private EntityCoordinates? _copyCorner;

    public FECMappingTool Tool { get; private set; }

    public event Action? ToolChanged;

    public override void Initialize()
    {
        base.Initialize();

        CommandBinds.Builder
            .Bind(ContentKeyFunctions.FECMappingUndo, InputCmdHandler.FromDelegate(_ => Undo(false), handle: false))
            .Bind(ContentKeyFunctions.FECMappingRedo, InputCmdHandler.FromDelegate(_ => Undo(true), handle: false))
            .Bind(ContentKeyFunctions.FECMappingCopy, new PointerInputCmdHandler(HandleCopy, outsidePrediction: true))
            .Bind(ContentKeyFunctions.FECMappingPaste, new PointerInputCmdHandler(HandlePaste, outsidePrediction: true))
            .BindBefore(EngineKeyFunctions.Use, new PointerInputCmdHandler(HandleToolClick, outsidePrediction: true), typeof(SharedInteractionSystem))
            .BindBefore(EngineKeyFunctions.UIRightClick, new PointerInputCmdHandler(HandleToolCancel, outsidePrediction: true))
            .Register<FECMappingHistorySystem>();
    }

    public override void Shutdown()
    {
        base.Shutdown();
        CommandBinds.Unregister<FECMappingHistorySystem>();
    }

    public void Undo(bool redo)
    {
        if (TacticalMapControl.FECTryHistory(redo) || !_admin.CanAdminPlace())
            return;

        if (redo)
            RaiseNetworkEvent(new FECMappingRedoRequestEvent());
        else
            RaiseNetworkEvent(new FECMappingUndoRequestEvent());
    }

    public void SetTool(FECMappingTool tool)
    {
        Tool = Tool == tool ? FECMappingTool.None : tool;
        _copyCorner = null;
        ToolChanged?.Invoke();

        if (Tool == FECMappingTool.Pick)
            _popup.PopupCursor(Loc.GetString("fec-mapping-tool-pick-start"));
        else if (Tool == FECMappingTool.Select)
            _popup.PopupCursor(Loc.GetString("fec-mapping-tool-select-start"));
        else if (Tool == FECMappingTool.Paste)
            _popup.PopupCursor(Loc.GetString("fec-mapping-tool-paste-start"));
    }

    private bool HandleToolClick(in PointerInputCmdHandler.PointerInputCmdArgs args)
    {
        if (Tool == FECMappingTool.None || args.State != BoundKeyState.Down)
            return false;

        if (!_admin.CanAdminPlace() || !args.Coordinates.IsValid(EntityManager))
        {
            SetTool(FECMappingTool.None);
            return false;
        }

        if (Tool == FECMappingTool.Pick)
        {
            Pick(args.EntityUid, args.Coordinates);
            SetTool(FECMappingTool.None);
            return true;
        }

        if (Tool == FECMappingTool.Select)
        {
            HandleCopy(args);
            if (_copyCorner == null)
                SetTool(FECMappingTool.None);

            return true;
        }

        HandlePaste(args);
        return true;
    }

    private void Pick(EntityUid uid, EntityCoordinates coordinates)
    {
        if (uid.IsValid() && MetaData(uid).EntityPrototype is { } prototype)
        {
            _placement.BeginPlacing(new PlacementInformation
            {
                PlacementOption = prototype.PlacementMode,
                EntityType = prototype.ID,
                IsTile = false,
            });
            _placement.Direction = Transform(uid).LocalRotation.GetDir();
            return;
        }

        var mapCoordinates = _transform.ToMapCoordinates(coordinates);
        if (!_mapManager.TryFindGridAt(mapCoordinates, out var gridUid, out var grid) ||
            !_map.TryGetTileRef(gridUid, grid, coordinates, out var tileRef) ||
            tileRef.Tile.IsEmpty)
        {
            return;
        }

        _placement.BeginPlacing(new PlacementInformation
        {
            PlacementOption = "AlignTileAny",
            TileType = _turf.GetContentTileDefinition(tileRef).TileId,
            IsTile = true,
        });
    }

    private bool HandleToolCancel(in PointerInputCmdHandler.PointerInputCmdArgs args)
    {
        if (Tool == FECMappingTool.None || args.State != BoundKeyState.Down)
            return false;

        SetTool(FECMappingTool.None);
        return true;
    }

    private bool HandleCopy(in PointerInputCmdHandler.PointerInputCmdArgs args)
    {
        if (args.State != BoundKeyState.Down || !_admin.CanAdminPlace() || !args.Coordinates.IsValid(EntityManager))
            return false;

        if (_copyCorner is not { } first)
        {
            _copyCorner = args.Coordinates;
            _popup.PopupCursor(Loc.GetString("fec-mapping-copy-first-corner"));
            return true;
        }

        _copyCorner = null;
        RaiseNetworkEvent(new FECMappingCopyRequestEvent(GetNetCoordinates(first), GetNetCoordinates(args.Coordinates)));
        return true;
    }

    private bool HandlePaste(in PointerInputCmdHandler.PointerInputCmdArgs args)
    {
        if (args.State != BoundKeyState.Down || !_admin.CanAdminPlace() || !args.Coordinates.IsValid(EntityManager))
            return false;

        RaiseNetworkEvent(new FECMappingPasteRequestEvent(GetNetCoordinates(args.Coordinates)));
        return true;
    }
}

public enum FECMappingTool : byte
{
    None,
    Pick,
    Select,
    Paste,
}
