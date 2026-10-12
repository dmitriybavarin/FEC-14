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

fec-meteo-station-forecast = [color=#7fb3ff][bold]Метеосводка по { $location }[/bold][/color]
    { $body }
fec-meteo-station-location-unknown = зоне операции
fec-meteo-station-idle-clear = Ясно. Осадков в ближайшее время не ожидается.
fec-meteo-station-idle = Ясно. Следующее обновление прогноза примерно через { $minutes } мин.
fec-meteo-station-cooldown = Затишье после непогоды. Стабильная погода ещё примерно { $minutes } мин.
fec-meteo-station-warning = [color=orange]ШТОРМОВОЕ ПРЕДУПРЕЖДЕНИЕ:[/color] { $event } через { $seconds } с. Укройтесь под крышей.
fec-meteo-station-running = [color=red]Сейчас { $event }.[/color] Ожидаемое окончание примерно через { $minutes } мин.
fec-meteo-station-running-permanent = [color=red]Сейчас { $event }.[/color] Окончание не прогнозируется.
fec-meteo-station-no-data = Нет связи с датчиками: погодный цикл в зоне операции не обнаружен.
fec-meteo-station-no-power = Станция обесточена.
fec-meteo-station-no-tower = Нет связи с метеовышкой. Почините и включите метеорологическую вышку в зоне операции.

fec-weather-tower-examine-on = Вышка [color=green]работает[/color] и передаёт метеоданные.
fec-weather-tower-examine-off = Вышка [color=yellow]выключена[/color]. Метеостанции не получают данные.
fec-weather-tower-examine-broken = Вышка [color=red]повреждена[/color]. Нужен ремонт сварочным аппаратом.
fec-weather-tower-needs-repairs = Вышка повреждена, сначала её нужно починить.
fec-weather-tower-no-power = У вышки нет питания.
fec-weather-tower-turned-on = Вы включаете метеовышку.
fec-weather-tower-turned-off = Вы выключаете метеовышку.

fec-weather-cas-blocked = Нелётная погода над целью. Огневая поддержка с воздуха невозможна.
fec-weather-flight-blocked = Нелётная погода. Вылет дропшипа запрещён до улучшения обстановки.
fec-meteo-station-no-fly = [color=orange]Нелётная погода. CAS и вылеты дропшипов невозможны.[/color]

fec-weather-tower-examine-xeno = Вышка [color=purple]опутана смолой улья[/color]. Пока смола на месте, вышку не починить.
fec-weather-tower-entangled = Вышка опутана смолой, сначала очистите её.
fec-weather-tower-captured-marine = Связь с метеорологической вышкой потеряна. Вышка опутана смолой ксеноморфов, штормовые предупреждения ARES недоступны.
fec-weather-tower-captured-xeno = Высокие потеряли свою башню погоды. Теперь небо видим только мы.
fec-weather-tower-freed-marine = Смола с метеорологической вышки очищена. Почините и включите вышку, чтобы восстановить метеосводки.
fec-meteo-station-xeno-tower = Нет связи с метеовышкой. Вышка захвачена ксеноморфами.
