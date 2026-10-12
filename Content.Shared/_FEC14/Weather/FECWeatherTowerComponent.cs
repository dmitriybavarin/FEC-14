using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared._FEC14.Weather;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
[Access(typeof(FECWeatherTowerSystem))]
public sealed partial class FECWeatherTowerComponent : Component
{
    [DataField, AutoNetworkedField]
    public FECWeatherTowerState State = FECWeatherTowerState.Off;
}

[Serializable, NetSerializable]
public enum FECWeatherTowerState : byte
{
    Broken,
    Off,
    On,
}

[Serializable, NetSerializable]
public enum FECWeatherTowerLayers : byte
{
    Layer,
}
