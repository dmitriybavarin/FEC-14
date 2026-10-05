using System.Reflection;
using Robust.Shared;
using Robust.Shared.Configuration;

namespace Content.Server._FEC14.Build;

public sealed class FECEngineVersionSystem : EntitySystem
{
    [Dependency] private readonly IConfigurationManager _config = default!;

    public override void Initialize()
    {
        base.Initialize();

        var version = typeof(CVars).Assembly
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
            .InformationalVersion;

        if (string.IsNullOrEmpty(version))
            return;

        var metadataIndex = version.IndexOf('+');
        if (metadataIndex >= 0)
            version = version[..metadataIndex];

        _config.OverrideDefault(CVars.BuildEngineVersion, version);
    }
}
