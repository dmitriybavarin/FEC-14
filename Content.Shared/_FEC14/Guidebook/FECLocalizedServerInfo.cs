using Robust.Shared.ContentPack;
using Robust.Shared.Utility;

namespace Content.Shared._FEC14.Guidebook;

public static class FECLocalizedServerInfo
{
    private static readonly ResPath Root = new("/ServerInfo");
    private static readonly ResPath LocalizedRoot = new("/ServerInfo/_FEC14");

    public static ResPath Resolve(IResourceManager resources, ResPath path)
    {
        var culture = IoCManager.Resolve<ILocalizationManager>().DefaultCulture?.Name;
        if (culture == null || !path.TryRelativeTo(Root, out var relative))
            return path;

        var localized = LocalizedRoot / culture / relative.Value;
        return resources.ContentFileExists(localized) ? localized : path;
    }
}
