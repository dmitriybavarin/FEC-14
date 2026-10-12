using Content.Client._RMC14.TacticalMap;
using Content.Client.Administration.Managers;
using Content.Shared._FEC14.Mapping;
using Content.Shared.Input;
using Robust.Shared.Input.Binding;

namespace Content.Client._FEC14.Mapping;

public sealed class FECMappingHistorySystem : EntitySystem
{
    [Dependency] private readonly IClientAdminManager _admin = default!;

    public override void Initialize()
    {
        base.Initialize();

        CommandBinds.Builder
            .Bind(ContentKeyFunctions.FECMappingUndo, InputCmdHandler.FromDelegate(_ => Undo(false), handle: false))
            .Bind(ContentKeyFunctions.FECMappingRedo, InputCmdHandler.FromDelegate(_ => Undo(true), handle: false))
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
}
