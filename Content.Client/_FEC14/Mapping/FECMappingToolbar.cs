using Content.Client.Gameplay;
using Content.Client.SubFloor;
using Robust.Client.Console;
using Robust.Client.Graphics;
using Robust.Client.Player;
using Robust.Client.State;
using Robust.Client.UserInterface.Controls;

namespace Content.Client._FEC14.Mapping;

public sealed class FECMappingToolbar : BoxContainer
{
    [Dependency] private readonly IClientConsoleHost _console = default!;
    [Dependency] private readonly IEntityManager _entities = default!;
    [Dependency] private readonly ILightManager _light = default!;
    [Dependency] private readonly IPlayerManager _player = default!;
    [Dependency] private readonly IStateManager _state = default!;

    public FECMappingToolbar()
    {
        IoCManager.InjectDependencies(this);
        Orientation = LayoutOrientation.Horizontal;
        SeparationOverride = 4;
        Margin = new Thickness(12, 0, 0, 0);

        AddButton("fec-mapping-ui-undo", "fec-mapping-ui-undo-tooltip", () => _entities.System<FECMappingHistorySystem>().Undo(false));
        AddButton("fec-mapping-ui-redo", "fec-mapping-ui-redo-tooltip", () => _entities.System<FECMappingHistorySystem>().Undo(true));
        AddButton("fec-mapping-ui-light", "fec-mapping-ui-light-tooltip", () => _light.Enabled = !_light.Enabled);
        AddButton("fec-mapping-ui-subfloor", "fec-mapping-ui-subfloor-tooltip", () => _entities.System<SubFloorHideSystem>().ShowAll ^= true);
        AddButton("fec-mapping-ui-save", "fec-mapping-ui-save-tooltip", OpenSave);
        AddButton("fec-mapping-ui-help", "fec-mapping-ui-help-tooltip", () => { });
        AddButton("fec-mapping-ui-exit", "fec-mapping-ui-exit-tooltip", Exit);
    }

    private void AddButton(string text, string tooltip, Action action)
    {
        var button = new Button
        {
            Text = Loc.GetString(text),
            ToolTip = Loc.GetString(tooltip),
            TooltipDelay = 0,
            StyleClasses = { "ButtonSquare" },
            MinHeight = 28,
        };
        button.OnPressed += _ => action();
        AddChild(button);
    }

    private void OpenSave()
    {
        var mapId = _player.LocalEntity is { } local && _entities.TryGetComponent(local, out TransformComponent? xform)
            ? xform.MapID.ToString()
            : string.Empty;

        new FECMappingSaveWindow(mapId).OpenCentered();
    }

    private void Exit()
    {
        _light.Enabled = true;
        _state.RequestStateChange<GameplayState>();
    }
}
