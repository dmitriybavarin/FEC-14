cmd-fecweather-desc = Opens the weather forecast window for all maps.
cmd-fecweather-help = Usage: fecweather

fec-weather-ui-button = Weather Forecast
fec-weather-ui-title = Weather Forecast
fec-weather-ui-map = Map:
fec-weather-ui-map-entry = { $name } (ID { $id })
fec-weather-ui-map-unnamed = Map { $id }
fec-weather-ui-events = Possible Weather Events
fec-weather-ui-no-cycle = This map has no weather cycle.
fec-weather-ui-command-hint = This panel only shows the forecast. Force weather: rmcweather start <map> <index> [now], end it: rmcweather end <map>.
fec-weather-ui-event = #{ $index } { $name }, { $seconds ->
    [0] permanent
   *[other] { $seconds } s
} ({ $weather })
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

fec-meteo-station-forecast = [color=#7fb3ff][bold]Weather report: { $location }[/bold][/color]
    { $body }
fec-meteo-station-location-unknown = area of operations
fec-meteo-station-idle = Clear. Next forecast update in about { $minutes } min.
fec-meteo-station-cooldown = Calm after the storm. Stable weather for about { $minutes } more min.
fec-meteo-station-warning = [color=orange]STORM WARNING:[/color] { $event } in { $seconds } s. Get under a roof.
fec-meteo-station-running = [color=red]Now: { $event }.[/color] Expected to end in about { $minutes } min.
fec-meteo-station-running-permanent = [color=red]Now: { $event }.[/color] No end in sight.
fec-meteo-station-no-data = No sensor link: no weather cycle detected in the area of operations.
fec-meteo-station-no-power = The station has no power.
