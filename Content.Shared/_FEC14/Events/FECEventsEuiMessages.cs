using Content.Shared.Eui;
using Robust.Shared.Serialization;

namespace Content.Shared._FEC14.Events;

[Serializable, NetSerializable]
public sealed class FECEventsEuiState(List<FECEventsEuiEntry> entries, List<string> log, string info, string? status) : EuiStateBase
{
    public readonly List<FECEventsEuiEntry> Entries = entries;
    public readonly List<string> Log = log;
    public readonly string Info = info;
    public readonly string? Status = status;
}

[Serializable, NetSerializable]
public sealed class FECEventsEuiEntry(string id, string name, string status, int minPlayers, float weight, bool blocked)
{
    public readonly string Id = id;
    public readonly string Name = name;
    public readonly string Status = status;
    public readonly int MinPlayers = minPlayers;
    public readonly float Weight = weight;
    public readonly bool Blocked = blocked;
}

[Serializable, NetSerializable]
public sealed class FECEventsStartMsg(string id) : EuiMessageBase
{
    public readonly string Id = id;
}

[Serializable, NetSerializable]
public sealed class FECEventsBlockMsg(string id, string duration) : EuiMessageBase
{
    public readonly string Id = id;
    public readonly string Duration = duration;
}

[Serializable, NetSerializable]
public sealed class FECEventsUnblockMsg(string id) : EuiMessageBase
{
    public readonly string Id = id;
}

[Serializable, NetSerializable]
public sealed class FECEventsResetRotationMsg : EuiMessageBase;
