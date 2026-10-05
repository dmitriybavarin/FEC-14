using Robust.Shared.GameStates;

namespace Content.Shared._FEC14.Exclusive;

[RegisterComponent, NetworkedComponent]
[Access(typeof(FECExclusiveSystem))]
public sealed partial class FECExclusiveComponent : Component
{
    [DataField]
    public string Icon = "/Textures/_FEC14/Logo/icon/icon-32x32.png";

    [DataField]
    public LocId VerbText = "fec-exclusive-verb";

    [DataField]
    public LocId HoverMessage = "fec-exclusive-hover";
}
