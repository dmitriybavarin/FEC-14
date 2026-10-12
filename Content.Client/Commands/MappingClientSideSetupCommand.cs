using Content.Client.Mapping;
using Content.Client.Markers;
using Content.Shared._FEC14.CCVar; // FEC14
using JetBrains.Annotations;
using Robust.Client.Graphics;
using Robust.Client.State;
using Robust.Shared.Configuration; // FEC14
using Robust.Shared.Console;

namespace Content.Client.Commands;

[UsedImplicitly]
internal sealed class MappingClientSideSetupCommand : LocalizedCommands
{
    [Dependency] private readonly IConfigurationManager _cfg = default!; // FEC14
    [Dependency] private readonly IEntitySystemManager _entitySystemManager = default!;
    [Dependency] private readonly ILightManager _lightManager = default!;

    public override string Command => "mappingclientsidesetup";

    public override string Help => LocalizationManager.GetString($"cmd-{Command}-help", ("command", Command));

    public override void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (!_lightManager.LockConsoleAccess)
        {
            _entitySystemManager.GetEntitySystem<MarkerSystem>().MarkersVisible = true;
            _lightManager.Enabled = false;
            shell.ExecuteCommand("showsubfloor");
            if (_cfg.GetCVar(FECCVars.MappingNewUi)) // FEC14
                IoCManager.Resolve<IStateManager>().RequestStateChange<MappingState>();
        }
    }
}

