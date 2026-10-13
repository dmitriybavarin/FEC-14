namespace Content.Shared._FEC14.Night;

[RegisterComponent]
public sealed partial class FECNightComponent : Component
{
    [ViewVariables]
    public FECNightPhase Phase = FECNightPhase.Pending;

    [ViewVariables]
    public TimeSpan NextPhaseAt;

    [ViewVariables]
    public Color DayColor;
}

public enum FECNightPhase : byte
{
    Pending,
    Waiting,
    Falling,
    Night,
    Rising,
    Done,
}
