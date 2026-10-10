using Content.Shared.Chemistry.Reagent;
using Content.Shared.EntityEffects;
using Content.Shared.FixedPoint;
using Robust.Shared.Prototypes;

namespace Content.Shared._RMC14.Chemistry.Effects;

public sealed partial class ConvertToReagent : EntityEffect
{
    [DataField(required: true)]
    public ProtoId<ReagentPrototype> TargetReagent;

    [DataField]
    public FixedPoint2 PercentRate = 0.1;

    [DataField]
    public FixedPoint2 MinimumRate = 5;

    protected override string ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys)
    {
        return Loc.GetString("fec-code-chem-effect-convert-to-reagent", ("v1", TargetReagent), ("v2", PercentRate * 100), ("v3", MinimumRate)); // FEC14
    }

    public override void Effect(EntityEffectBaseArgs args)
    {
        if (args is not EntityEffectReagentArgs reagentArgs)
            return;

        if (reagentArgs.Source == null)
            return;

        if (reagentArgs.Reagent == null)
            return;

        if (reagentArgs.Reagent.ID == TargetReagent.Id)
            return;

        if (reagentArgs.Quantity <= FixedPoint2.Zero)
            return;

        var convertAmount = FixedPoint2.Min(FixedPoint2.Max(reagentArgs.Quantity * PercentRate, MinimumRate) * reagentArgs.Scale, reagentArgs.Quantity);
        if (convertAmount <= FixedPoint2.Zero)
            return;

        reagentArgs.Source.RemoveReagent(reagentArgs.Reagent.ID, convertAmount);
        reagentArgs.Source.AddReagent(TargetReagent, convertAmount);
    }
}
