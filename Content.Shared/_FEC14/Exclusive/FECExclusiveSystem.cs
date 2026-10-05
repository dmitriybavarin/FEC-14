using Content.Shared.Examine;
using Content.Shared.Verbs;

namespace Content.Shared._FEC14.Exclusive;

public sealed class FECExclusiveSystem : EntitySystem
{
    [Dependency] private readonly ExamineSystemShared _examine = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<FECExclusiveComponent, GetVerbsEvent<ExamineVerb>>(OnGetExamineVerbs);
    }

    private void OnGetExamineVerbs(Entity<FECExclusiveComponent> ent, ref GetVerbsEvent<ExamineVerb> args)
    {
        if (!args.CanInteract || !args.CanAccess)
            return;

        _examine.AddHoverExamineVerb(args,
            ent.Comp,
            Loc.GetString(ent.Comp.VerbText),
            Loc.GetString(ent.Comp.HoverMessage),
            ent.Comp.Icon);
    }
}
