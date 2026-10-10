using Content.Shared.CCVar;
using Robust.Shared.Configuration;

namespace Content.Server._FEC14.Viewport;

public sealed class FECViewportSystem : EntitySystem
{
    public const int MaximumWidth = 35;

    [Dependency] private readonly IConfigurationManager _config = default!;

    public override void Initialize()
    {
        base.Initialize();

        _config.OverrideDefault(CCVars.ViewportMaximumWidth, MaximumWidth);
    }
}
