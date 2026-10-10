using Content.Server.Administration;
using Content.Server.Atmos.EntitySystems;
using Content.Shared.Administration;
using Robust.Shared.Console;

namespace Content.Server.Atmos.Commands
{
    [AdminCommand(AdminFlags.Debug)]
    public sealed class ListGasesCommand : IConsoleCommand
    {
        [Dependency] private readonly IEntityManager _e = default!;

        public string Command => "listgases";
        public string Description => Loc.GetString("fec-cmd-list-gases-desc"); // FEC14
        public string Help => "listgases";

        public void Execute(IConsoleShell shell, string argStr, string[] args)
        {
            var atmosSystem = _e.System<AtmosphereSystem>();

            foreach (var gasPrototype in atmosSystem.Gases)
            {
                var gasName = Loc.GetString(gasPrototype.Name);
                shell.WriteLine($"{gasName} ID: {gasPrototype.ID}");
            }
        }
    }

}
