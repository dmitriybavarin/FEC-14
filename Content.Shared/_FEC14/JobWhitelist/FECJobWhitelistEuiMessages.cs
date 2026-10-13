using Content.Shared.Eui;
using Robust.Shared.Serialization;

namespace Content.Shared._FEC14.JobWhitelist;

[Serializable, NetSerializable]
public sealed class FECJobWhitelistEuiState(List<FECJobWhitelistEntry> entries, string? status) : EuiStateBase
{
    public readonly List<FECJobWhitelistEntry> Entries = entries;
    public readonly string? Status = status;
}

[Serializable, NetSerializable]
public sealed class FECJobWhitelistEntry(Guid userId, string playerName, string jobId, bool online, DateTime? expiresAt)
{
    public readonly Guid UserId = userId;
    public readonly string PlayerName = playerName;
    public readonly string JobId = jobId;
    public readonly bool Online = online;
    public readonly DateTime? ExpiresAt = expiresAt;
}

[Serializable, NetSerializable]
public sealed class FECJobWhitelistAddMsg(string player, string jobId, string duration) : EuiMessageBase
{
    public readonly string Player = player;
    public readonly string JobId = jobId;
    public readonly string Duration = duration;
}

[Serializable, NetSerializable]
public sealed class FECJobWhitelistRemoveMsg(Guid userId, string jobId) : EuiMessageBase
{
    public readonly Guid UserId = userId;
    public readonly string JobId = jobId;
}

[Serializable, NetSerializable]
public sealed class FECJobWhitelistRefreshMsg : EuiMessageBase;
