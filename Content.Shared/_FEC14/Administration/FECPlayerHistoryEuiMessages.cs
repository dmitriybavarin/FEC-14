using Content.Shared.Eui;
using Robust.Shared.Serialization;

namespace Content.Shared._FEC14.Administration;

[Serializable, NetSerializable]
public enum FECPlayerHistoryKind : byte
{
    Ban,
    RoleBan,
    ChatBan,
    Note,
    Watchlist,
    Message,
    Whitelist,
}

[Serializable, NetSerializable]
public sealed class FECPlayerHistoryEntry(FECPlayerHistoryKind kind, DateTime? date, string text)
{
    public readonly FECPlayerHistoryKind Kind = kind;
    public readonly DateTime? Date = date;
    public readonly string Text = text;
}

[Serializable, NetSerializable]
public sealed class FECPlayerHistoryEuiState(string player, bool loading, List<string> summary, List<FECPlayerHistoryEntry> entries) : EuiStateBase
{
    public readonly string Player = player;
    public readonly bool Loading = loading;
    public readonly List<string> Summary = summary;
    public readonly List<FECPlayerHistoryEntry> Entries = entries;
}
