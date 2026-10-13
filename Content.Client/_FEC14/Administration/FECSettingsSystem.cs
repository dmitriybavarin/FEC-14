using Content.Shared._FEC14.Administration;

namespace Content.Client._FEC14.Administration;

public sealed class FECSettingsSystem : EntitySystem
{
    public event Action<FECSettingsStateEvent>? StateReceived;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeNetworkEvent<FECSettingsStateEvent>(ev => StateReceived?.Invoke(ev));
    }

    public void Request()
    {
        RaiseNetworkEvent(new FECSettingsRequestEvent());
    }

    public void Set(string key, string value)
    {
        RaiseNetworkEvent(new FECSettingsSetEvent(key, value));
    }

    public void Reset(string key)
    {
        RaiseNetworkEvent(new FECSettingsResetEvent(key));
    }
}
