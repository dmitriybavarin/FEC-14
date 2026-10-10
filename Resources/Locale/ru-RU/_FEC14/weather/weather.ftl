cmd-fecweather-desc = Открывает окно управления погодой.
cmd-fecweather-help = Использование: fecweather

fec-weather-ui-button = Погода
fec-weather-ui-title = Управление погодой
fec-weather-ui-map = Карта:
fec-weather-ui-map-entry = { $name } (ID { $id })
fec-weather-ui-map-unnamed = Карта { $id }
fec-weather-ui-events = Погодные события карты
fec-weather-ui-no-cycle = На этой карте нет цикла погоды. Доступна только ручная погода.
fec-weather-ui-event = { CAPITALIZE($name) }, { $seconds ->
    [0] бессрочно
   *[other] { $seconds } с
} ({ $weather })
fec-weather-ui-start-warning = С предупреждением
fec-weather-ui-start-now = Сразу
fec-weather-ui-end = Закончить текущее событие
fec-weather-ui-manual = Ручная погода
fec-weather-ui-seconds = Секунды
fec-weather-ui-set = Включить
fec-weather-ui-clear = Убрать
fec-weather-ui-manual-hint = Если секунд нет или 0, погода бессрочная. Ручная погода идет без предупреждений и не влияет на игру.
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
