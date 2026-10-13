using System.Numerics;
using Robust.Shared.GameStates;

namespace Content.Shared._FEC14.Wet;

[RegisterComponent, NetworkedComponent]
public sealed partial class FECWetComponent : Component
{
    [ViewVariables]
    public TimeSpan ExpiresAt;

    [ViewVariables]
    public Vector2? LastPrint;

    [ViewVariables]
    public bool LeftFoot;
}
