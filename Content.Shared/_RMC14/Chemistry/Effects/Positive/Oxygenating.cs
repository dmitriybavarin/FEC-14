using Content.Shared._RMC14.Body;
using Content.Shared._RMC14.Damage;
using Content.Shared.Chemistry.Reagent;
using Content.Shared.Damage;
using Content.Shared.Damage.Prototypes;
using Content.Shared.EntityEffects;
using Content.Shared.FixedPoint;
using Robust.Shared.Prototypes;

namespace Content.Shared._RMC14.Chemistry.Effects.Positive;

public sealed partial class Oxygenating : RMCChemicalEffect
{
    private static readonly ProtoId<DamageGroupPrototype> AirlossGroup = "Airloss";

    private static readonly ProtoId<DamageTypePrototype> BluntType = "Blunt";
    private static readonly ProtoId<DamageTypePrototype> PoisonType = "Poison";

    private static readonly ProtoId<ReagentPrototype> Lexorin = "RMCLexorin";

    protected override string ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys)
    {
        var healing = ActualPotency >= 3
            ? Loc.GetString("fec-code-chem-effect-oxygenating-all", ("v1", PotencyPerSecond)) // FEC14
            : Loc.GetString("fec-code-chem-effect-oxygenating-partial", ("v1", PotencyPerSecond)); // FEC14

        return Loc.GetString("fec-code-chem-effect-oxygenating", ("v1", healing), ("v2", PotencyPerSecond * 0.5), ("v3", PotencyPerSecond), ("v4", PotencyPerSecond * 2)); // FEC14
    }

    protected override void Tick(DamageableSystem damageable, FixedPoint2 potency, EntityEffectReagentArgs args)
    {
        var rmcDamageable = args.EntityManager.System<SharedRMCDamageableSystem>();
        var amount = ActualPotency >= 3 ? 99999 : potency;
        var healing = rmcDamageable.DistributeHealingCached(args.TargetEntity, AirlossGroup, amount);
        damageable.TryChangeDamage(args.TargetEntity, healing, true, interruptsDoAfters: false);

        var bloodstream = args.EntityManager.System<SharedRMCBloodstreamSystem>();
        bloodstream.RemoveBloodstreamChemical(args.TargetEntity, Lexorin, potency);
    }

    protected override void TickOverdose(DamageableSystem damageable, FixedPoint2 potency, EntityEffectReagentArgs args)
    {
        var damage = new DamageSpecifier();
        damage.DamageDict[PoisonType] = potency * 0.5f;
        damageable.TryChangeDamage(args.TargetEntity, damage, true, interruptsDoAfters: false);
    }

    protected override void TickCriticalOverdose(DamageableSystem damageable, FixedPoint2 potency, EntityEffectReagentArgs args)
    {
        var damage = new DamageSpecifier();
        damage.DamageDict[BluntType] = potency;
        damage.DamageDict[PoisonType] = potency * 2f;
        damageable.TryChangeDamage(args.TargetEntity, damage, true, interruptsDoAfters: false);
    }
}
