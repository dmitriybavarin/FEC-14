using Robust.Shared.GameStates;

namespace Content.Shared._FEC14.Wet;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class FECWetFootprintComponent : Component
{
    [DataField, AutoNetworkedField]
    public TimeSpan Lifetime = TimeSpan.FromSeconds(60);

    [AutoNetworkedField]
    public TimeSpan ExpiresAt;
}
