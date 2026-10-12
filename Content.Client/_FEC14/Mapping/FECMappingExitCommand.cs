using Content.Client.Gameplay;
using Robust.Client.Graphics;
using Robust.Client.State;
using Robust.Shared.Console;

namespace Content.Client._FEC14.Mapping;

public sealed class FECMappingExitCommand : LocalizedCommands
{
    [Dependency] private readonly ILightManager _light = default!;
    [Dependency] private readonly IStateManager _state = default!;

    public override string Command => "mappingexit";

    public override void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        _light.Enabled = true;
        _state.RequestStateChange<GameplayState>();
    }
}
