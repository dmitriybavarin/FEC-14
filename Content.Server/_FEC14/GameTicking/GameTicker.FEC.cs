using Content.Shared.Database;
using Robust.Shared.Player;

namespace Content.Server.GameTicking;

public sealed partial class GameTicker
{
    public void FECReturnToLobby(ICommonSession player)
    {
        _mind.WipeMind(player);
        _adminLogger.Add(LogType.Respawn, LogImpact.Medium, $"Player {player} returned to the lobby.");
        PlayerJoinLobby(player);
    }
}
