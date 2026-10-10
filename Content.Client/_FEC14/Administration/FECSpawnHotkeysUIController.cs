using Content.Client.Administration.Managers;
using Content.Client.Gameplay;
using Content.Client.UserInterface.Systems.DecalPlacer;
using Content.Shared.Input;
using Robust.Client.Input;
using Robust.Client.UserInterface.Controllers;
using Robust.Client.UserInterface.Controllers.Implementations;
using Robust.Shared.Input;

namespace Content.Client._FEC14.Administration;

public sealed class FECSpawnHotkeysUIController : UIController, IOnStateEntered<GameplayState>, IOnStateExited<GameplayState>
{
    [Dependency] private readonly IClientAdminManager _admin = default!;
    [Dependency] private readonly IInputManager _input = default!;

    public void OnStateEntered(GameplayState state)
    {
        _input.UIKeyBindStateChanged += OnUIKeyBindStateChanged;
    }

    public void OnStateExited(GameplayState state)
    {
        _input.UIKeyBindStateChanged -= OnUIKeyBindStateChanged;
    }

    private bool OnUIKeyBindStateChanged(BoundKeyEventArgs args)
    {
        if (args.State != BoundKeyState.Down || UIManager.KeyboardFocused == null || !_admin.CanAdminPlace())
            return false;

        if (args.Function == ContentKeyFunctions.OpenEntitySpawnWindow)
            UIManager.GetUIController<EntitySpawningUIController>().ToggleWindow();
        else if (args.Function == ContentKeyFunctions.OpenTileSpawnWindow)
            UIManager.GetUIController<TileSpawningUIController>().ToggleWindow();
        else if (args.Function == ContentKeyFunctions.OpenDecalSpawnWindow)
            UIManager.GetUIController<DecalPlacerUIController>().ToggleWindow();
        else
            return false;

        args.Handle();
        return true;
    }
}
