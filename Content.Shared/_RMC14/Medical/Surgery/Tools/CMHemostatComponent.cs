using Robust.Shared.GameStates;

namespace Content.Shared._RMC14.Medical.Surgery.Tools;

[RegisterComponent, NetworkedComponent]
[Access(typeof(SharedCMSurgerySystem))]
public sealed partial class CMHemostatComponent : Component, ICMSurgeryToolComponent
{
    public string ToolName => Loc.GetString("fec-code-surgery-tool-hemostat"); // FEC14
}
