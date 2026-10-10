using System.Linq;
using Content.Shared.Access;
using Content.Shared.Access.Components;
using Content.Shared.Access.Systems;
using Robust.Shared.Prototypes;

namespace Content.Shared._FEC14.Access;

public sealed class FECAllAccessSystem : EntitySystem
{
    [Dependency] private readonly SharedAccessSystem _access = default!;
    [Dependency] private readonly IPrototypeManager _prototypes = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<FECAllAccessComponent, MapInitEvent>(OnMapInit, after: [typeof(SharedAccessSystem)]);
    }

    private void OnMapInit(Entity<FECAllAccessComponent> ent, ref MapInitEvent args)
    {
        if (!TryComp(ent, out AccessComponent? access))
            return;

        var tags = access.Tags
            .Concat(_prototypes.EnumeratePrototypes<AccessLevelPrototype>().Select(p => new ProtoId<AccessLevelPrototype>(p.ID)))
            .ToHashSet();

        _access.TrySetTags(ent, tags, access);
    }
}
