using Robust.Shared.GameStates;

namespace Content.Shared._FEC14.Access;

[RegisterComponent, NetworkedComponent]
[Access(typeof(FECAllAccessSystem))]
public sealed partial class FECAllAccessComponent : Component;
