using Content.Shared.Eui;
using Robust.Shared.Serialization;

namespace Content.Shared._FEC14.Weather;

[Serializable, NetSerializable]
public sealed class FECWeatherEuiState(List<FECWeatherMapEntry> maps, string? message = null) : EuiStateBase
{
    public readonly List<FECWeatherMapEntry> Maps = maps;
    public readonly string? Message = message;
}

[Serializable, NetSerializable]
public sealed class FECWeatherStartMessage(int mapId, int eventIndex, bool instant) : EuiMessageBase
{
    public readonly int MapId = mapId;
    public readonly int EventIndex = eventIndex;
    public readonly bool Instant = instant;
}

[Serializable, NetSerializable]
public sealed class FECWeatherEndMessage(int mapId) : EuiMessageBase
{
    public readonly int MapId = mapId;
}

[Serializable, NetSerializable]
public sealed class FECWeatherMapEntry(
    int mapId,
    string name,
    bool hasCycle,
    string state,
    string? currentEvent,
    int remainingSeconds,
    string? currentWeather,
    List<FECWeatherEventEntry> events)
{
    public readonly int MapId = mapId;
    public readonly string Name = name;
    public readonly bool HasCycle = hasCycle;
    public readonly string State = state;
    public readonly string? CurrentEvent = currentEvent;
    public readonly int RemainingSeconds = remainingSeconds;
    public readonly string? CurrentWeather = currentWeather;
    public readonly List<FECWeatherEventEntry> Events = events;
}

[Serializable, NetSerializable]
public sealed class FECWeatherEventEntry(int index, string name, int durationSeconds, string weather)
{
    public readonly int Index = index;
    public readonly string Name = name;
    public readonly int DurationSeconds = durationSeconds;
    public readonly string Weather = weather;
}
