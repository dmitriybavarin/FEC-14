using System.Linq;
using Content.Client.Eui;
using Content.Shared._FEC14.Weather;
using Content.Shared.Eui;
using JetBrains.Annotations;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.Localization;
using Robust.Shared.Utility;
using static Robust.Client.UserInterface.Controls.BoxContainer;

namespace Content.Client._FEC14.Weather;

[UsedImplicitly]
public sealed class FECWeatherEui : BaseEui
{
    [Dependency] private readonly ILocalizationManager _loc = default!;

    private FECWeatherWindow _window = default!;
    private FECWeatherEuiState? _state;
    private int? _selectedMap;
    private string? _selectedWeather;
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
        _window.WeatherSelector.OnItemSelected += args =>
        {
            _window.WeatherSelector.SelectId(args.Id);
            _selectedWeather = _state?.Weathers.ElementAtOrDefault(args.Id);
        };
        _window.EndButton.OnPressed += _ =>
        {
            if (_selectedMap is { } map)
                SendMessage(new FECWeatherEndEventMsg(map));
        };
        _window.SetButton.OnPressed += _ =>
        {
            if (_selectedMap is not { } map || _selectedWeather == null)
                return;

            int.TryParse(_window.SecondsEdit.Text.Trim(), out var seconds);
            SendMessage(new FECWeatherSetMsg(map, _selectedWeather, Math.Max(0, seconds)));
        };
        _window.ClearButton.OnPressed += _ =>
        {
            if (_selectedMap is { } map)
                SendMessage(new FECWeatherSetMsg(map, null, 0));
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

        var firstState = _state == null;
        _state = s;

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

        if (firstState)
        {
            _window.WeatherSelector.Clear();
            for (var i = 0; i < s.Weathers.Count; i++)
            {
                _window.WeatherSelector.AddItem(s.Weathers[i], i);
            }

            _selectedWeather = s.Weathers.FirstOrDefault();
            if (s.Weathers.Count > 0)
                _window.WeatherSelector.SelectId(0);
        }

        Refresh();
    }

    private void Refresh()
    {
        var map = _state?.Maps.FirstOrDefault(m => m.MapId == _selectedMap);
        if (map == null)
        {
            _window.StatusLabel.SetMessage(string.Empty);
            _window.Events.DisposeAllChildren();
            _window.EndButton.Disabled = true;
            return;
        }

        var active = map.State is "Warning" or "Running";
        var status = Loc.GetString("fec-weather-ui-status",
            ("state", map.HasCycle ? map.State : "None"),
            ("event", map.CurrentEvent == null ? string.Empty : EventName(map.CurrentEvent)),
            ("seconds", map.RemainingSeconds),
            ("weather", map.CurrentWeather ?? "none"));
        _window.StatusLabel.SetMessage(FormattedMessage.FromMarkupOrThrow(status));

        _window.NoCycleLabel.Visible = !map.HasCycle;
        _window.EndButton.Disabled = !active;

        var key = $"{map.MapId}|{active}|{string.Join(';', map.Events.Select(e => $"{e.Index}:{e.Name}:{e.DurationSeconds}"))}";
        if (key == _eventsKey)
            return;

        _eventsKey = key;
        _window.Events.DisposeAllChildren();
        foreach (var ev in map.Events)
        {
            var row = new BoxContainer { Orientation = LayoutOrientation.Horizontal, SeparationOverride = 6 };
            row.AddChild(new Label
            {
                Text = Loc.GetString("fec-weather-ui-event",
                    ("name", EventName(ev.Name)),
                    ("seconds", ev.DurationSeconds),
                    ("weather", ev.Weather)),
                HorizontalExpand = true,
                ClipText = true,
            });

            var warn = new Button { Text = Loc.GetString("fec-weather-ui-start-warning"), Disabled = active };
            var index = ev.Index;
            var mapId = map.MapId;
            warn.OnPressed += _ => SendMessage(new FECWeatherStartEventMsg(mapId, index, false));
            row.AddChild(warn);

            var now = new Button { Text = Loc.GetString("fec-weather-ui-start-now"), Disabled = active };
            now.OnPressed += _ => SendMessage(new FECWeatherStartEventMsg(mapId, index, true));
            row.AddChild(now);

            _window.Events.AddChild(row);
        }
    }

    private string EventName(string name)
    {
        return _loc.TryGetString(name, out var localized) ? localized : name;
    }
}
