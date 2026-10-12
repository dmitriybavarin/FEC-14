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
            .Bind(ContentKeyFunctions.FECMappingUndo, InputCmdHandler.FromDelegate(_ => Send(new FECMappingUndoRequestEvent()), handle: false))
            .Bind(ContentKeyFunctions.FECMappingRedo, InputCmdHandler.FromDelegate(_ => Send(new FECMappingRedoRequestEvent()), handle: false))
            .Register<FECMappingHistorySystem>();
    }

    public override void Shutdown()
    {
        base.Shutdown();
        CommandBinds.Unregister<FECMappingHistorySystem>();
    }

    private void Send(EntityEventArgs ev)
    {
        if (_admin.CanAdminPlace())
            RaiseNetworkEvent(ev);
    }
}
