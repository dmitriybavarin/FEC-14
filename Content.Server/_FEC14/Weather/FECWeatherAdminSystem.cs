namespace Content.Server._FEC14.Weather;

public sealed class FECWeatherAdminSystem : EntitySystem
{
    private static readonly TimeSpan RefreshInterval = TimeSpan.FromSeconds(1);

    private readonly HashSet<FECWeatherEui> _open = new();
    private TimeSpan _accumulator;

    public void Track(FECWeatherEui eui)
    {
        _open.Add(eui);
    }

    public void Untrack(FECWeatherEui eui)
    {
        _open.Remove(eui);
    }

    public override void Update(float frameTime)
    {
        if (_open.Count == 0)
            return;

        _accumulator += TimeSpan.FromSeconds(frameTime);
        if (_accumulator < RefreshInterval)
            return;

        _accumulator = TimeSpan.Zero;
        foreach (var eui in _open)
        {
            eui.StateDirty();
        }
    }
}
