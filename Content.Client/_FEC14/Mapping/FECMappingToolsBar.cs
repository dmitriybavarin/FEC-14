using System.Numerics;
using Content.Client.Administration.Managers;
using Content.Client.UserInterface.Systems.DecalPlacer;
using Content.Shared.Ghost;
using Robust.Client.Placement;
using Robust.Client.Player;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controllers.Implementations;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.Timing;

namespace Content.Client._FEC14.Mapping;

public sealed class FECMappingToolsBar : BoxContainer
{
    [Dependency] private readonly IClientAdminManager _admin = default!;
    [Dependency] private readonly IEntityManager _entities = default!;
    [Dependency] private readonly IPlacementManager _placement = default!;
    [Dependency] private readonly IPlayerManager _player = default!;
    [Dependency] private readonly IUserInterfaceManager _ui = default!;

    private readonly BoxContainer _box;
    private readonly Button _eraser;
    private readonly Button _select;
    private readonly Button _paste;

    public FECMappingToolsBar()
    {
        IoCManager.InjectDependencies(this);
        Orientation = LayoutOrientation.Horizontal;
        _box = new BoxContainer
        {
            Orientation = LayoutOrientation.Horizontal,
            SeparationOverride = 2,
            Margin = new Thickness(0, 4, 0, 4),
            Visible = false,
        };
        AddChild(_box);

        AddButton("fec-mapping-tool-brush", "fec-mapping-tool-brush-tooltip", () => _ui.GetUIController<EntitySpawningUIController>().ToggleWindow());
        AddButton("fec-mapping-tool-tiles", "fec-mapping-tool-tiles-tooltip", () => _ui.GetUIController<TileSpawningUIController>().ToggleWindow());
        AddButton("fec-mapping-tool-decals", "fec-mapping-tool-decals-tooltip", () => _ui.GetUIController<DecalPlacerUIController>().ToggleWindow());
        _eraser = AddButton("fec-mapping-tool-eraser", "fec-mapping-tool-eraser-tooltip", () => _placement.ToggleEraser(), true);
        _select = AddButton("fec-mapping-tool-select", "fec-mapping-tool-select-tooltip", () => History.SetTool(FECMappingTool.Select), true);
        _paste = AddButton("fec-mapping-tool-paste", "fec-mapping-tool-paste-tooltip", () => History.SetTool(FECMappingTool.Paste), true);
        AddButton("fec-mapping-tool-undo", "fec-mapping-tool-undo-tooltip", () => History.Undo(false));
        AddButton("fec-mapping-tool-redo", "fec-mapping-tool-redo-tooltip", () => History.Undo(true));
    }

    private FECMappingHistorySystem History => _entities.System<FECMappingHistorySystem>();

    private Button AddButton(string text, string tooltip, Action action, bool toggle = false)
    {
        var button = new Button
        {
            Text = Loc.GetString(text),
            ToolTip = Loc.GetString(tooltip),
            TooltipDelay = 0.2f,
            ToggleMode = toggle,
            StyleClasses = { "ButtonSquare" },
            MinSize = new Vector2(0, 32),
        };
        button.OnPressed += _ => action();
        _box.AddChild(button);
        return button;
    }

    protected override void FrameUpdate(FrameEventArgs args)
    {
        base.FrameUpdate(args);

        var show = _admin.CanAdminPlace() &&
                   _player.LocalEntity is { } local &&
                   _entities.HasComponent<GhostComponent>(local);
        _box.Visible = show;
        if (!show)
            return;

        var tool = History.Tool;
        _eraser.Pressed = _placement.Eraser;
        _select.Pressed = tool == FECMappingTool.Select;
        _paste.Pressed = tool == FECMappingTool.Paste;
    }
}
