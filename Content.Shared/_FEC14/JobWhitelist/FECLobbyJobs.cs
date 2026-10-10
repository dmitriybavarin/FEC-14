using Content.Shared.Roles;
using Robust.Shared.Prototypes;

namespace Content.Shared._FEC14.JobWhitelist;

public static class FECLobbyJobs
{
    public static HashSet<string> Visible(IPrototypeManager prototypes)
    {
        var visible = new HashSet<string>();
        foreach (var department in prototypes.EnumeratePrototypes<DepartmentPrototype>())
        {
            if (department.EditorHidden || !department.IsCM || department.Hidden)
                continue;

            foreach (var role in department.Roles)
            {
                if (prototypes.TryIndex(role, out var job) && job.SetPreference && !job.Hidden)
                    visible.Add(job.ID);
            }
        }

        return visible;
    }

    public static List<JobPrototype> Other(IPrototypeManager prototypes, IEnumerable<string> whitelisted)
    {
        var visible = Visible(prototypes);
        var result = new List<JobPrototype>();
        foreach (var id in whitelisted)
        {
            if (visible.Contains(id) || !prototypes.TryIndex<JobPrototype>(id, out var job))
                continue;

            if (!result.Contains(job))
                result.Add(job);
        }

        result.Sort((a, b) => string.Compare(a.LocalizedName, b.LocalizedName, StringComparison.CurrentCultureIgnoreCase));
        return result;
    }
}
