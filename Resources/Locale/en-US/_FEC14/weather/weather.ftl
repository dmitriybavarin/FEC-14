cmd-fecweather-desc = Opens the weather control window.
cmd-fecweather-help = Usage: fecweather

fec-weather-ui-button = Weather
fec-weather-ui-title = Weather Control
fec-weather-ui-map = Map:
fec-weather-ui-map-entry = { $name } (ID { $id })
fec-weather-ui-map-unnamed = Map { $id }
fec-weather-ui-events = Map Weather Events
fec-weather-ui-no-cycle = This map has no weather cycle. Only manual weather is available.
fec-weather-ui-event = { $name }, { $seconds ->
    [0] permanent
   *[other] { $seconds } s
} ({ $weather })
fec-weather-ui-start-warning = With Warning
fec-weather-ui-start-now = Now
fec-weather-ui-end = End Current Event
fec-weather-ui-manual = Manual Weather
fec-weather-ui-seconds = Seconds
fec-weather-ui-set = Set
fec-weather-ui-clear = Clear
fec-weather-ui-manual-hint = Leave seconds empty or 0 for permanent weather. Manual weather has no warnings and no gameplay effects.
fec-weather-ui-status =
    { $state ->
        [Idle] The weather cycle is waiting for the next event.
        [Warning] Warning in progress. { $event } starts in { $seconds } s.
        [Running] { $event } is active, { $seconds } s left.
        [Cooldown] Cooldown after a weather event.
       *[other] This map has no weather cycle.
    }
    { $weather ->
        [none] No weather on the map right now.
       *[other] Current map weather is { $weather }.
    }
