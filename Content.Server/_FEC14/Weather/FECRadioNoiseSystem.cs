using System.Text;
using Content.Shared._FEC14.CCVar;
using Content.Shared._FEC14.Weather;
using Robust.Shared.Configuration;
using Robust.Shared.Map;
using Robust.Shared.Random;

namespace Content.Server._FEC14.Weather;

public sealed class FECRadioNoiseSystem : EntitySystem
{
    private const string NoiseSymbols = "#-~%*";

    [Dependency] private readonly IConfigurationManager _config = default!;
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly FECWeatherTacticsSystem _tactics = default!;

    public bool TryDistort(MapId receiverMap, MapId sourceMap, string message, out string noisy)
    {
        noisy = message;
        if (receiverMap == sourceMap || !_config.GetCVar(FECCVars.RadioNoiseEnabled))
            return false;

        var noise = Math.Max(_tactics.GetRadioNoise(receiverMap), _tactics.GetRadioNoise(sourceMap));
        if (noise <= 0f)
            return false;

        var builder = new StringBuilder(message.Length);
        foreach (var symbol in message)
        {
            if (char.IsLetterOrDigit(symbol) && _random.Prob(noise))
                builder.Append(NoiseSymbols[_random.Next(NoiseSymbols.Length)]);
            else
                builder.Append(symbol);
        }

        noisy = builder.ToString();
        return true;
    }
}
