using Content.Shared.Examine;

namespace Content.Shared._FEC14.Wet;

public sealed class FECWetExamineSystem : EntitySystem
{
    public override void Initialize()
    {
        SubscribeLocalEvent<FECWetComponent, ExaminedEvent>(OnExamined);
    }

    private void OnExamined(Entity<FECWetComponent> ent, ref ExaminedEvent args)
    {
        args.PushMarkup(Loc.GetString("fec-wet-examine", ("target", ent.Owner)));
    }
}
