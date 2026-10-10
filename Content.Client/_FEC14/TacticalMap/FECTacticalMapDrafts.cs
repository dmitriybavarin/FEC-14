using Content.Client._RMC14.TacticalMap;
using Content.Shared._RMC14.TacticalMap;
using Robust.Shared.Timing;

namespace Content.Client._FEC14.TacticalMap;

public static class FECTacticalMapDrafts
{
    private static readonly Dictionary<EntityUid, (GameTick Created, List<TacticalMapLine> Lines)> Drafts = new();

    public static void Save(IEntityManager entities, EntityUid owner, TacticalMapControl canvas)
    {
        if (!canvas.FECDirty || !entities.TryGetComponent(owner, out MetaDataComponent? meta))
        {
            Drafts.Remove(owner);
            return;
        }

        Drafts[owner] = (meta.CreationTick, new List<TacticalMapLine>(canvas.Lines));
    }

    public static void Restore(IEntityManager entities, EntityUid owner, TacticalMapControl canvas)
    {
        if (Drafts.TryGetValue(owner, out var draft) &&
            entities.TryGetComponent(owner, out MetaDataComponent? meta) &&
            meta.CreationTick == draft.Created)
        {
            canvas.FECLoadLines(draft.Lines, true);
            return;
        }

        Drafts.Remove(owner);
        canvas.FECLoadLines(new List<TacticalMapLine>(canvas.Lines), false);
    }
}
