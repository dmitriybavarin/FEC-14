using System.Numerics;
using Content.Client.Administration.Managers;
using Content.Client.UserInterface.Systems.DecalPlacer;
using Content.Shared.Ghost;
using Robust.Client.GameObjects;
using Robust.Client.Placement;
using Robust.Client.Player;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controllers.Implementations;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.Timing;
using Robust.Shared.Utility;

namespace Content.Client._FEC14.Mapping;

public sealed class FECMappingToolsBar : BoxContainer
{
    [Dependency] private readonly IClientAdminManager _admin = default!;
    [Dependency] private readonly IEntityManager _entities = default!;
    [Dependency] private readonly IPlacementManager _placement = default!;
    [Dependency] private readonly IPlayerManager _player = default!;
    [Dependency] private readonly IUserInterfaceManager _ui = default!;

    private static readonly Vector2 SlotSize = new(64, 64);

    private readonly BoxContainer _box;
    private readonly Button _eraser;
    private readonly Button _pick;
    private readonly Button _select;
    private readonly Button _paste;

    public FECMappingToolsBar()
    {
        IoCManager.InjectDependencies(this);
        Orientation = LayoutOrientation.Horizontal;
        _box = new BoxContainer
        {
            Orientation = LayoutOrientation.Horizontal,
            SeparationOverride = 4,
            Margin = new Thickness(0, 4, 0, 4),
            Visible = false,
        };
        AddChild(_box);

        AddSlot(Texture("/Textures/Interface/pencil.png"), "fec-mapping-tool-brush-tooltip",
            () => _ui.GetUIController<EntitySpawningUIController>().ToggleWindow());
        AddSlot(new SpriteSpecifier.Rsi(new ResPath("/Textures/Objects/Tiles/tile.rsi"), "steel"), "fec-mapping-tool-tiles-tooltip",
            () => _ui.GetUIController<TileSpawningUIController>().ToggleWindow());
        AddSlot(new SpriteSpecifier.Rsi(new ResPath("/Textures/Objects/Fun/crayons.rsi"), "black"), "fec-mapping-tool-decals-tooltip",
            () => _ui.GetUIController<DecalPlacerUIController>().ToggleWindow());
        _pick = AddSlot(Texture("/Textures/Interface/eyedropper.svg.png"), "fec-mapping-tool-pick-tooltip",
            () => History.SetTool(FECMappingTool.Pick), true);
        _eraser = AddSlot(Texture("/Textures/Interface/eraser.svg.png"), "fec-mapping-tool-eraser-tooltip",
            () => _placement.ToggleEraser(), true);
        _select = AddSlot(Texture("/Textures/Interface/VerbIcons/group.svg.192dpi.png"), "fec-mapping-tool-select-tooltip",
            () => History.SetTool(FECMappingTool.Select), true);
        _paste = AddSlot(Texture("/Textures/Interface/VerbIcons/insert.svg.192dpi.png"), "fec-mapping-tool-paste-tooltip",
            () => History.SetTool(FECMappingTool.Paste), true);
        AddSlot(Texture("/Textures/Interface/VerbIcons/rotate_ccw.svg.192dpi.png"), "fec-mapping-tool-undo-tooltip",
            () => History.Undo(false));
        AddSlot(Texture("/Textures/Interface/VerbIcons/rotate_cw.svg.192dpi.png"), "fec-mapping-tool-redo-tooltip",
            () => History.Undo(true));
    }

    private FECMappingHistorySystem History => _entities.System<FECMappingHistorySystem>();

    private static SpriteSpecifier Texture(string path)
    {
        return new SpriteSpecifier.Texture(new ResPath(path));
    }

    private Button AddSlot(SpriteSpecifier icon, string tooltip, Action action, bool toggle = false)
    {
        var button = new Button
        {
            ToolTip = Loc.GetString(tooltip),
            TooltipDelay = 0.2f,
            ToggleMode = toggle,
            StyleClasses = { "ButtonSquare" },
            SetSize = SlotSize,
        };
        button.AddChild(new TextureRect
        {
            Texture = _entities.System<SpriteSystem>().Frame0(icon),
            Stretch = TextureRect.StretchMode.KeepAspectCentered,
            Margin = new Thickness(6),
        });
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
        _pick.Pressed = tool == FECMappingTool.Pick;
        _select.Pressed = tool == FECMappingTool.Select;
        _paste.Pressed = tool == FECMappingTool.Paste;
    }
}
