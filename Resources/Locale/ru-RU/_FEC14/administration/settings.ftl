fec-admin-category-round = Раунд
fec-admin-category-players = Игроки
fec-admin-category-server = Сервер
fec-admin-weather-button = Метеосводка
fec-history-prompt-button = История игрока
fec-history-prompt-title = История игрока
fec-history-prompt-open = Открыть

fec-settings-button = Параметры FEC
fec-settings-title = Параметры FEC
fec-settings-hint = Изменения применяются сразу и действуют до перезапуска сервера. Постоянные значения задаются в конфиге сервера.
fec-settings-apply = Применить
fec-settings-reset = Сброс
fec-settings-changed = { $key } изменён с { $old } на { $value }
fec-settings-bad-value = Неверное значение «{ $value }» для { $key }
fec-settings-tooltip = { $key }, по умолчанию { $default }{ $range ->
    [1] , от { $min } до { $max }
   *[other] {""}
}

fec-settings-category-events = События
fec-settings-category-weather = Погода
fec-settings-category-lobby = Лобби

fec-settings-fec-events-enabled = События включены
fec-settings-fec-events-chance = Шанс ролла на слот (0–1)
fec-settings-fec-events-max-per-round = Слотов событий за раунд
fec-settings-fec-events-vote-enabled = Голосование призраков
fec-settings-fec-events-vote-options = Вариантов в голосовании
fec-settings-fec-events-vote-seconds = Длительность голосования, сек
fec-settings-fec-events-confirm-seconds = Ожидание подтверждения админом, сек
fec-settings-fec-events-low-online-warn-minutes = Предупреждать о малом онлайне за, мин
fec-settings-fec-events-role-cooldown-rounds = Перерыв для роли события, раундов
fec-settings-fec-weather-clear-round-chance = Шанс ясного раунда (0–1)
fec-settings-fec-weather-max-start-chance = Макс. шанс начала непогоды (0–1)
fec-settings-fec-weather-min-check-minutes = Мин. интервал проверки погоды, мин
fec-settings-fec-weather-min-clear-minutes = Мин. ясная пауза между непогодой, мин
fec-settings-fec-species-whitelist = Разрешённые расы (через запятую)

cmd-fecsettings-desc = Открывает окно параметров FEC.
cmd-fecsettings-help = Использование: fecsettings

cmd-fectp-desc = Телепорт к карте, гриду, сущности или игроку. Понимает номера из lsmap и lsgrid.
cmd-fectp-help = Использование: fectp <map:N | номер грида или сущности | ник> [кого телепортировать]
fec-tp-done = Телепорт к { $target }, карта { $map }, { $x } { $y }
fec-tp-not-found = Не найдено «{ $target }». Карта — map:N из lsmap, грид — номер из lsgrid, сущность — её номер, игрок — ник.
fec-tp-no-player = Игрок «{ $player }» не найден или без тела
fec-tp-hint-target = map:N, номер грида или сущности, ник
fec-tp-hint-player = Кого телепортировать (по умолчанию вас)

fec-admin-tab = FEC14
fec-admin-category-settings = Настройки
fec-settings-no-access = Для изменения настроек нужен флаг SERVER.
fec-tp-button = Телепорт
fec-tp-go = Переместиться
fec-tp-prompt-hint = map:N — карта из lsmap, число — грид или сущность, или ник игрока.
