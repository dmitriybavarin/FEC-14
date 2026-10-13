using Content.Server._RMC14.Rules.DistressSignal;
using Content.Server.GameTicking;
using Content.Shared._FEC14.Weather;
using Content.Shared._RMC14.Marines;
using Content.Shared._RMC14.Weather;
using Content.Shared._RMC14.Xenonids;
using Content.Shared.GameTicking;
using Content.Shared.Mobs;

namespace Content.Server._FEC14.GameTicking;

public sealed class FECRoundSummarySystem : EntitySystem
{
    [Dependency] private readonly GameTicker _ticker = default!;

    private int _marineDeaths;
    private int _xenoDeaths;
    private int _weatherEvents;
    private TimeSpan _weatherTime;
    private string? _firstBlood;
    private TimeSpan? _firstBloodAt;
    private readonly List<string> _extraLines = new();

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<MobStateChangedEvent>(OnMobStateChanged);
        SubscribeLocalEvent<RMCWeatherStartedEvent>(OnWeatherStarted);
        SubscribeLocalEvent<RMCWeatherEndedEvent>(OnWeatherEnded);
        SubscribeLocalEvent<RoundEndTextAppendEvent>(OnRoundEndText, after: [typeof(CMDistressSignalRuleSystem)]);
        SubscribeLocalEvent<RoundRestartCleanupEvent>(OnCleanup);
    }

    public void AddLine(string line)
    {
        _extraLines.Add(line);
    }

    private void OnMobStateChanged(MobStateChangedEvent ev)
    {
        if (ev.NewMobState != MobState.Dead || ev.OldMobState == MobState.Dead)
            return;

        if (HasComp<MarineComponent>(ev.Target))
            _marineDeaths++;
        else if (HasComp<XenoComponent>(ev.Target))
            _xenoDeaths++;
        else
            return;

        if (_firstBlood != null)
            return;

        _firstBlood = Name(ev.Target);
        _firstBloodAt = _ticker.RoundDuration();
    }

    private void OnWeatherStarted(ref RMCWeatherStartedEvent ev)
    {
        _weatherEvents++;
    }

    private void OnWeatherEnded(ref RMCWeatherEndedEvent ev)
    {
        if (ev.Elapsed is { } elapsed)
            _weatherTime += elapsed;
    }

    private void OnRoundEndText(RoundEndTextAppendEvent ev)
    {
        var duration = _ticker.RoundDuration();
        ev.AddLine(string.Empty);
        ev.AddLine(Loc.GetString("fec-summary-header"));
        ev.AddLine(Loc.GetString("fec-summary-duration", ("hours", (int) duration.TotalHours), ("minutes", duration.Minutes)));
        ev.AddLine(Loc.GetString("fec-summary-deaths", ("marines", _marineDeaths), ("xenos", _xenoDeaths)));

        if (_firstBlood != null && _firstBloodAt is { } at)
            ev.AddLine(Loc.GetString("fec-summary-first-blood", ("name", _firstBlood), ("minutes", (int) at.TotalMinutes)));

        ev.AddLine(_weatherEvents == 0
            ? Loc.GetString("fec-summary-weather-clear")
            : Loc.GetString("fec-summary-weather", ("count", _weatherEvents), ("minutes", (int) _weatherTime.TotalMinutes)));

        var towerQuery = EntityQueryEnumerator<FECWeatherTowerComponent>();
        while (towerQuery.MoveNext(out var tower))
        {
            ev.AddLine(Loc.GetString(tower.XenoControlled
                ? "fec-summary-tower-xeno"
                : tower.State == FECWeatherTowerState.On ? "fec-summary-tower-on" : "fec-summary-tower-off"));
            break;
        }

        foreach (var line in _extraLines)
        {
            ev.AddLine(line);
        }
    }

    private void OnCleanup(RoundRestartCleanupEvent ev)
    {
        _marineDeaths = 0;
        _xenoDeaths = 0;
        _weatherEvents = 0;
        _weatherTime = TimeSpan.Zero;
        _firstBlood = null;
        _firstBloodAt = null;
        _extraLines.Clear();
    }
}
