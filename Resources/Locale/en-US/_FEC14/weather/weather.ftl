cmd-fecweather-desc = Opens the weather control panel for all maps.
cmd-fecweather-help = Usage: fecweather

fec-weather-ui-button = Weather Control
fec-weather-ui-title = Weather Control
fec-weather-ui-map = Map:
fec-weather-ui-map-entry = { $name } (ID { $id })
fec-weather-ui-map-unnamed = Map { $id }
fec-weather-ui-events = Possible Weather Events
fec-weather-ui-no-cycle = This map has no weather cycle.
fec-weather-ui-command-hint = Start begins the weather with a warning, Now skips the warning. End Weather stops the current one.
fec-weather-ui-start = Start
fec-weather-ui-start-now = Now
fec-weather-ui-end = End Weather
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
fec-meteo-station-idle-clear = Clear. No precipitation expected in the near future.
fec-meteo-station-idle = Clear. Next forecast update in about { $minutes } min.
fec-meteo-station-cooldown = Calm after the storm. Stable weather for about { $minutes } more min.
fec-meteo-station-warning = [color=orange]STORM WARNING:[/color] { $event } in { $seconds } s. Get under a roof.
fec-meteo-station-running = [color=red]Now: { $event }.[/color] Expected to end in about { $minutes } min.
fec-meteo-station-running-permanent = [color=red]Now: { $event }.[/color] No end in sight.
fec-meteo-station-no-data = No sensor link: no weather cycle detected in the area of operations.
fec-meteo-station-no-power = The station has no power.
fec-meteo-station-no-tower = No link to the weather tower. Repair and turn on the meteorological tower in the area of operations.

fec-weather-tower-examine-on = The tower is [color=green]online[/color] and transmitting weather data.
fec-weather-tower-examine-off = The tower is [color=yellow]offline[/color]. Meteorological stations receive no data.
fec-weather-tower-examine-broken = The tower is [color=red]damaged[/color]. It needs to be repaired with a welder.
fec-weather-tower-needs-repairs = The tower is damaged and needs repairs first.
fec-weather-tower-no-power = The tower has no power.
fec-weather-tower-turned-on = You turn the weather tower on.
fec-weather-tower-turned-off = You turn the weather tower off.

fec-weather-cas-blocked = No-fly weather over the target. Close air support is impossible.
fec-weather-flight-blocked = No-fly weather. Dropship launch is forbidden until conditions improve.
fec-meteo-station-no-fly = [color=orange]No-fly weather. CAS and dropship flights are impossible.[/color]

fec-weather-tower-examine-xeno = The tower is [color=purple]entangled in hive resin[/color]. It cannot be repaired while the resin is there.
fec-weather-tower-entangled = The tower is entangled in resin, clear it first.
fec-weather-tower-captured-marine = Link to the meteorological tower lost. The tower is entangled in xenomorph resin, ARES storm warnings are unavailable.
fec-weather-tower-captured-xeno = The tall ones lost their weather tower. Now only we can see the sky.
fec-weather-tower-freed-marine = Resin has been cleared from the meteorological tower. Repair and turn on the tower to restore weather reports.
fec-meteo-station-xeno-tower = No link to the weather tower. The tower has been captured by xenomorphs.

fec-meteo-station-night-soon = Night is coming: sunset in about { $minutes } min. Visibility will drop.
fec-meteo-station-night-falling = The sun is setting, visibility is dropping fast.
fec-meteo-station-night = [color=#8aa4ff]It is night.[/color] Dawn in about { $minutes } min.
fec-meteo-station-night-rising = Dawn is breaking, visibility is returning to normal.
