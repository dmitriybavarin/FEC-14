using Content.Server.Administration;
using Content.Server.RoundEnd;
using Content.Shared.Administration;
using Robust.Shared.Console;

namespace Content.Server.GameTicking.Commands
{
    [AdminCommand(AdminFlags.Round)]
    public sealed class RestartRoundCommand : IConsoleCommand
    {
        [Dependency] private readonly IEntityManager _e = default!;

        public string Command => "restartround";
        public string Description => Loc.GetString("fec-cmd-restart-round-desc"); // FEC14
        public string Help => string.Empty;

        public void Execute(IConsoleShell shell, string argStr, string[] args)
        {
            var ticker = _e.System<GameTicker>();

            if (ticker.RunLevel != GameRunLevel.InRound)
            {
                shell.WriteLine("This can only be executed while the game is in a round - try restartroundnow");
                return;
            }

            _e.System<RoundEndSystem>().EndRound();
        }
    }

    [AdminCommand(AdminFlags.Round)]
    public sealed class RestartRoundNowCommand : IConsoleCommand
    {
        [Dependency] private readonly IEntityManager _e = default!;

        public string Command => "restartroundnow";
        public string Description => Loc.GetString("fec-cmd-restart-round-desc-2"); // FEC14
        public string Help => String.Empty;

        public void Execute(IConsoleShell shell, string argStr, string[] args)
        {
            _e.System<GameTicker>().RestartRound();
        }
    }
}
