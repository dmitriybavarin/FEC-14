using Robust.Shared.Map;
using Robust.Shared.Serialization;

namespace Content.Shared._FEC14.Mapping;

[Serializable, NetSerializable]
public sealed class FECMappingUndoRequestEvent : EntityEventArgs;

[Serializable, NetSerializable]
public sealed class FECMappingRedoRequestEvent : EntityEventArgs;

[Serializable, NetSerializable]
public sealed class FECMappingCopyRequestEvent(NetCoordinates first, NetCoordinates second) : EntityEventArgs
{
    public readonly NetCoordinates First = first;
    public readonly NetCoordinates Second = second;
}

[Serializable, NetSerializable]
public sealed class FECMappingPasteRequestEvent(NetCoordinates target) : EntityEventArgs
{
    public readonly NetCoordinates Target = target;
}
