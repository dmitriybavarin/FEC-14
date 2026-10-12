using Content.Client._RMC14.TacticalMap;
using Content.Client.Administration.Managers;
using Content.Shared._FEC14.Mapping;
using Content.Shared.Input;
using Content.Shared.Popups;
using Robust.Shared.Input;
using Robust.Shared.Input.Binding;
using Robust.Shared.Map;

namespace Content.Client._FEC14.Mapping;

public sealed class FECMappingHistorySystem : EntitySystem
{
    [Dependency] private readonly IClientAdminManager _admin = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;

    private EntityCoordinates? _copyCorner;

    public override void Initialize()
    {
        base.Initialize();

        CommandBinds.Builder
            .Bind(ContentKeyFunctions.FECMappingUndo, InputCmdHandler.FromDelegate(_ => Undo(false), handle: false))
            .Bind(ContentKeyFunctions.FECMappingRedo, InputCmdHandler.FromDelegate(_ => Undo(true), handle: false))
            .Bind(ContentKeyFunctions.FECMappingCopy, new PointerInputCmdHandler(HandleCopy, outsidePrediction: true))
            .Bind(ContentKeyFunctions.FECMappingPaste, new PointerInputCmdHandler(HandlePaste, outsidePrediction: true))
            .Register<FECMappingHistorySystem>();
    }

    public override void Shutdown()
    {
        base.Shutdown();
        CommandBinds.Unregister<FECMappingHistorySystem>();
    }

    private void Undo(bool redo)
    {
        if (TacticalMapControl.FECTryHistory(redo) || !_admin.CanAdminPlace())
            return;

        if (redo)
            RaiseNetworkEvent(new FECMappingRedoRequestEvent());
        else
            RaiseNetworkEvent(new FECMappingUndoRequestEvent());
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
