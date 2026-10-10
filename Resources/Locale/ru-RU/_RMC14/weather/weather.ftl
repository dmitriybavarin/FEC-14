rmc-weather-effect-dust = Пыль забивается во все щели. Очень неприятно.
rmc-weather-effect-sand = Песок обдирает вас, снимая верхний слой!

rmc-weather-name-light-rain = небольшой дождь
rmc-weather-name-heavy-rain = сильный дождь
rmc-weather-name-snow = снегопад
rmc-weather-name-snowstorm = метель
rmc-weather-name-duststorm = пыльная буря
rmc-weather-name-sandstorm = песчаная буря
rmc-weather-name-rainstorm = ливень с грозой
rmc-weather-name-tropical-storm = тропический шторм
rmc-weather-name-monsoon-warning = муссон
rmc-weather-name-very-light-rain = морось

rmc-weather-warning-marine = [bold]Штормовое предупреждение.[/bold] Приближается { $weather }. Укройтесь в помещении.
rmc-weather-warning-xeno = Вдали по улью разносится рев. Надвигается { $weather }.

rmc-weather-siren-message = Воет сирена. ВНИМАНИЕ. ОБНАРУЖЕНА ОПАСНАЯ ПОГОДНАЯ АНОМАЛИЯ. НЕМЕДЛЕННО УКРОЙТЕСЬ.
rmc-storm-siren-message = Воет сирена. ВНИМАНИЕ. ВНИМАНИЕ. ПРИБЛИЖАЕТСЯ ТРОПИЧЕСКИЙ ШТОРМ. НЕМЕДЛЕННО УКРОЙТЕСЬ.

rmc-weather-command-description = Управляет циклом погоды RMC.
rmc-weather-command-help = rmcweather status <ID карты> | start <ID карты> <номер события> [now] | end <ID карты>
rmc-weather-command-not-enough-arguments = Не хватает аргументов. Использование: rmcweather status <ID карты> | start <ID карты> <номер события> [now] | end <ID карты>
rmc-weather-command-unknown-action = Неизвестное действие "{ $action }".
rmc-weather-command-missing-event = Не указан номер события.
rmc-weather-command-map-id-integer = ID карты должен быть целым числом.
rmc-weather-command-map-does-not-exist = Карты { $map } не существует.
rmc-weather-command-no-cycle = На карте { $map } нет цикла погоды.
rmc-weather-command-already-active = Погода на карте { $map } уже идет. Сначала завершите ее.
rmc-weather-command-started = На карте { $map } начинается { $weather }.
rmc-weather-command-no-active = На карте { $map } нет активной погоды.
rmc-weather-command-ended = Погода на карте { $map } завершена.
rmc-weather-command-status = Погода на карте { $map }: { $state ->
    [Idle] ожидание
    [Warning] предупреждение
    [Running] идет
    [Cooldown] перерыв
   *[other] { $state }
}
rmc-weather-command-status-event = { $status }, { $weather }, осталось { $seconds } с
rmc-weather-command-status-first-drop-complete = { $status }; первая высадка уже была
rmc-weather-command-status-waiting-first-drop = { $status }; ждем первой высадки, следы пока не смываются
rmc-weather-command-event-index-integer = Номер события должен быть целым числом.
rmc-weather-command-index-out-of-range = Нет события с номером { $index }.
rmc-weather-command-hint-action = действие
rmc-weather-command-hint-map-id = ID карты
rmc-weather-command-hint-event = номер события
rmc-weather-command-hint-now = now (сразу, без предупреждения)

rmc-weather-admin-started = На карте { $map } начинается { $weather }, длительность { $seconds } с.
rmc-weather-admin-started-forced = По команде админа на карте { $map } начинается { $weather }, длительность { $seconds } с.
rmc-weather-admin-started-permanent = На карте { $map } начинается { $weather }, бессрочно.
rmc-weather-admin-started-forced-permanent = По команде админа на карте { $map } начинается { $weather }, бессрочно.
rmc-weather-admin-ended = Погодное событие на карте { $map } завершено ({ $weather }).
rmc-weather-admin-ended-forced = Погодное событие на карте { $map }, запущенное админом, завершено ({ $weather }).
rmc-weather-admin-ended-after = Погодное событие на карте { $map } завершено через { $seconds } с ({ $weather }).
rmc-weather-admin-ended-forced-after = Погодное событие на карте { $map }, запущенное админом, завершено через { $seconds } с ({ $weather }).
