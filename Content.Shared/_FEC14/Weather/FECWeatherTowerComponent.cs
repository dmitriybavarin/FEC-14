using Content.Shared.Weather;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared._FEC14.Weather;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
[Access(typeof(FECWeatherTowerSystem))]
public sealed partial class FECWeatherTowerComponent : Component
{
    [DataField, AutoNetworkedField]
    public FECWeatherTowerState State = FECWeatherTowerState.Off;

    [DataField, AutoNetworkedField]
    public bool XenoControlled;

    [DataField]
    public TimeSpan CaptureTime = TimeSpan.FromSeconds(60);

    [ViewVariables]
    public TimeSpan? WeededSince;

    [DataField]
    public Dictionary<ProtoId<WeatherPrototype>, FECWeatherTowerScreen> Screens = new();

    [DataField]
    public FECWeatherTowerScreen UnknownWeatherScreen = FECWeatherTowerScreen.Rain;

    [DataField]
    public TimeSpan ScreenUpdateInterval = TimeSpan.FromSeconds(2);

    [ViewVariables]
    public TimeSpan NextScreenUpdate;

    [ViewVariables]
    public FECWeatherTowerScreen Screen = FECWeatherTowerScreen.Off;
}

[Serializable, NetSerializable]
public enum FECWeatherTowerState : byte
{
    Broken,
    Off,
    On,
}

[Serializable, NetSerializable]
public enum FECWeatherTowerScreen : byte
{
    Off,
    Clear,
    Rain,
    Storm,
    Snow,
    Dust,
    Warning,
}

[Serializable, NetSerializable]
public enum FECWeatherTowerLayers : byte
{
    Layer,
    Screen,
}
