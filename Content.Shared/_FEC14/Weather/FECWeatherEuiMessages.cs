using Content.Shared.Eui;
using Robust.Shared.Serialization;

namespace Content.Shared._FEC14.Weather;

[Serializable, NetSerializable]
public sealed class FECWeatherEuiState(List<FECWeatherMapEntry> maps, List<string> weathers) : EuiStateBase
{
    public readonly List<FECWeatherMapEntry> Maps = maps;
    public readonly List<string> Weathers = weathers;
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

[Serializable, NetSerializable]
public sealed class FECWeatherStartEventMsg(int mapId, int index, bool now) : EuiMessageBase
{
    public readonly int MapId = mapId;
    public readonly int Index = index;
    public readonly bool Now = now;
}

[Serializable, NetSerializable]
public sealed class FECWeatherEndEventMsg(int mapId) : EuiMessageBase
{
    public readonly int MapId = mapId;
}

[Serializable, NetSerializable]
public sealed class FECWeatherSetMsg(int mapId, string? weather, int seconds) : EuiMessageBase
{
    public readonly int MapId = mapId;
    public readonly string? Weather = weather;
    public readonly int Seconds = seconds;
}

[Serializable, NetSerializable]
public sealed class FECWeatherRefreshMsg : EuiMessageBase;
