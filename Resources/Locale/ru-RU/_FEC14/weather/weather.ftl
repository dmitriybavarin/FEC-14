cmd-fecweather-desc = Открывает окно прогноза погоды по картам.
cmd-fecweather-help = Использование: fecweather

fec-weather-ui-button = Прогноз погоды
fec-weather-ui-title = Прогноз погоды
fec-weather-ui-map = Карта:
fec-weather-ui-map-entry = { $name } (ID { $id })
fec-weather-ui-map-unnamed = Карта { $id }
fec-weather-ui-events = Возможные погодные события
fec-weather-ui-no-cycle = На этой карте нет цикла погоды.
fec-weather-ui-command-hint = Панель только показывает прогноз. Форсировать погоду: rmcweather start <карта> <номер> [now], закончить: rmcweather end <карта>.
fec-weather-ui-event = #{ $index } { CAPITALIZE($name) }, { $seconds ->
    [0] бессрочно
   *[other] { $seconds } с
} ({ $weather })
fec-weather-ui-status =
    { $state ->
        [Idle] Цикл погоды ждет следующего события.
        [Warning] Идет предупреждение. Скоро начнется { $event }, осталось { $seconds } с.
        [Running] Идет { $event }, осталось { $seconds } с.
        [Cooldown] Перерыв после погодного события.
       *[other] На этой карте нет цикла погоды.
    }
    { $weather ->
        [none] Сейчас на карте нет погоды.
       *[other] Сейчас на карте { $weather }.
    }

fec-meteo-station-forecast = [color=#7fb3ff][bold]Метеосводка: { $location }[/bold][/color]
    { $body }
fec-meteo-station-location-unknown = зона операции
fec-meteo-station-idle = Ясно. Следующее обновление прогноза примерно через { $minutes } мин.
fec-meteo-station-cooldown = Затишье после непогоды. Стабильная погода ещё примерно { $minutes } мин.
fec-meteo-station-warning = [color=orange]ШТОРМОВОЕ ПРЕДУПРЕЖДЕНИЕ:[/color] { $event } через { $seconds } с. Укройтесь под крышей.
fec-meteo-station-running = [color=red]Сейчас: { $event }.[/color] Ожидаемое окончание примерно через { $minutes } мин.
fec-meteo-station-running-permanent = [color=red]Сейчас: { $event }.[/color] Окончание не прогнозируется.
fec-meteo-station-no-data = Нет связи с датчиками: погодный цикл в зоне операции не обнаружен.
fec-meteo-station-no-power = Станция обесточена.
