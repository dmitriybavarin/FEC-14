using Robust.Shared.Serialization;

namespace Content.Shared._FEC14.Mapping;

[Serializable, NetSerializable]
public sealed class FECMappingUndoRequestEvent : EntityEventArgs;

[Serializable, NetSerializable]
public sealed class FECMappingRedoRequestEvent : EntityEventArgs;
