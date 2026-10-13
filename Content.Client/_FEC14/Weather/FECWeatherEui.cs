using System.Linq;
using Content.Client.Eui;
using Content.Shared._FEC14.Weather;
using Content.Shared.Eui;
using JetBrains.Annotations;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.Localization;
using Robust.Shared.Utility;

namespace Content.Client._FEC14.Weather;

[UsedImplicitly]
public sealed class FECWeatherEui : BaseEui
{
    [Dependency] private readonly ILocalizationManager _loc = default!;

    private FECWeatherWindow _window = default!;
    private FECWeatherEuiState? _state;
    private int? _selectedMap;
    private string _eventsKey = string.Empty;
    private string _mapsKey = string.Empty;

    public override void Opened()
    {
        _window = new FECWeatherWindow();
        _window.OnClose += () => SendMessage(new CloseEuiMessage());
        _window.MapSelector.OnItemSelected += args =>
        {
            _window.MapSelector.SelectId(args.Id);
            _selectedMap = args.Id;
            _eventsKey = string.Empty;
            Refresh();
        };
        _window.EndButton.OnPressed += _ =>
        {
            if (_selectedMap is { } mapId)
                SendMessage(new FECWeatherEndMessage(mapId));
        };
        _window.OpenCentered();
    }

    public override void Closed()
    {
        _window.Close();
    }

    public override void HandleState(EuiStateBase state)
    {
        if (state is not FECWeatherEuiState s)
            return;

        _state = s;
        _window.ResultLabel.Text = s.Message ?? string.Empty;
        _window.ResultLabel.Visible = !string.IsNullOrEmpty(s.Message);

        var mapsKey = string.Join(';', s.Maps.Select(m => $"{m.MapId}:{m.Name}"));
        var mapsChanged = mapsKey != _mapsKey;
        _mapsKey = mapsKey;

        if (mapsChanged)
        {
            _window.MapSelector.Clear();
            foreach (var map in s.Maps)
            {
                var name = string.IsNullOrWhiteSpace(map.Name)
                    ? Loc.GetString("fec-weather-ui-map-unnamed", ("id", map.MapId))
                    : Loc.GetString("fec-weather-ui-map-entry", ("name", map.Name), ("id", map.MapId));
                _window.MapSelector.AddItem(name, map.MapId);
            }
        }

        if (_selectedMap == null || s.Maps.All(m => m.MapId != _selectedMap))
        {
            _selectedMap = s.Maps.FirstOrDefault(m => m.HasCycle)?.MapId ?? s.Maps.FirstOrDefault()?.MapId;
            mapsChanged = true;
        }

        if (mapsChanged && _selectedMap is { } selected)
            _window.MapSelector.SelectId(selected);

        Refresh();
    }

    private void Refresh()
    {
        var map = _state?.Maps.FirstOrDefault(m => m.MapId == _selectedMap);
        if (map == null)
        {
            _window.StatusLabel.SetMessage(string.Empty);
            _window.Events.DisposeAllChildren();
            return;
        }

        var status = Loc.GetString("fec-weather-ui-status",
            ("state", map.HasCycle ? map.State : "None"),
            ("event", map.CurrentEvent == null ? string.Empty : EventName(map.CurrentEvent)),
            ("seconds", map.RemainingSeconds),
            ("weather", map.CurrentWeather ?? "none"));
        _window.StatusLabel.SetMessage(FormattedMessage.FromMarkupOrThrow(status));

        _window.NoCycleLabel.Visible = !map.HasCycle;

        var key = $"{map.MapId}|{string.Join(';', map.Events.Select(e => $"{e.Index}:{e.Name}:{e.DurationSeconds}"))}";
        if (key == _eventsKey)
            return;

        _eventsKey = key;
        _window.Events.DisposeAllChildren();
        foreach (var ev in map.Events)
        {
            var row = new BoxContainer { Orientation = BoxContainer.LayoutOrientation.Horizontal, SeparationOverride = 4 };
            row.AddChild(new Label
            {
                Text = Loc.GetString("fec-weather-ui-event",
                    ("index", ev.Index),
                    ("name", EventName(ev.Name)),
                    ("seconds", ev.DurationSeconds),
                    ("weather", ev.Weather)),
                HorizontalExpand = true,
                ClipText = true,
            });

            var mapId = map.MapId;
            var index = ev.Index;
            var start = new Button { Text = Loc.GetString("fec-weather-ui-start") };
            start.OnPressed += _ => SendMessage(new FECWeatherStartMessage(mapId, index, false));
            var now = new Button { Text = Loc.GetString("fec-weather-ui-start-now") };
            now.OnPressed += _ => SendMessage(new FECWeatherStartMessage(mapId, index, true));
            row.AddChild(start);
            row.AddChild(now);
            _window.Events.AddChild(row);
        }
    }

    private string EventName(string name)
    {
        return _loc.TryGetString(name, out var localized) ? localized : name;
    }
}
