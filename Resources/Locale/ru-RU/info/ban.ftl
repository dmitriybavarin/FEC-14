cmd-ban-desc = Банит игрока
cmd-ban-help = Использование: ban <ник или ID пользователя> <причина> [длительность в минутах, не указывать или 0 для перманентного бана]
cmd-ban-player = Игрок с таким ником не найден.
cmd-ban-invalid-minutes = {$minutes} не подходит как количество минут!
cmd-ban-invalid-severity = {$severity} не подходит как степень тяжести!
cmd-ban-invalid-arguments = Неверное количество аргументов
cmd-ban-hint = <ник/ID пользователя>
cmd-ban-hint-reason = <причина>
cmd-ban-hint-duration = [длительность]
cmd-ban-hint-severity = [тяжесть]

cmd-ban-hint-duration-1 = Навсегда
cmd-ban-hint-duration-2 = 1 день
cmd-ban-hint-duration-3 = 3 дня
cmd-ban-hint-duration-4 = 1 неделя
cmd-ban-hint-duration-5 = 2 недели
cmd-ban-hint-duration-6 = 1 месяц

cmd-banpanel-desc = Открывает панель банов
cmd-banpanel-help = Использование: banpanel [ник или GUID пользователя]
cmd-banpanel-server = Из консоли сервера это использовать нельзя
cmd-banpanel-player-err = Указанный игрок не найден

cmd-banlist-desc = Показывает активные баны пользователя.
cmd-banlist-help = Использование: banlist <ник или ID пользователя>
cmd-banlist-empty = Активных банов нет, пользователь { $user }
cmd-banlist-hint = <ник/ID пользователя>

cmd-ban_exemption_update-desc = Задает игроку исключение из определенного типа банов.
cmd-ban_exemption_update-help = Использование: ban_exemption_update <игрок> <флаг> [<флаг> [...]]
    Укажите несколько флагов, чтобы выдать игроку несколько исключений.
    Чтобы снять все исключения, укажите единственный флаг "None".

cmd-ban_exemption_update-nargs = Нужно минимум 2 аргумента
cmd-ban_exemption_update-locate = Игрок "{$player}" не найден.
cmd-ban_exemption_update-invalid-flag = Неверный флаг "{$flag}".
cmd-ban_exemption_update-success = Флаги исключений обновлены, игрок "{$player}" ({$uid}).
cmd-ban_exemption_update-arg-player = <игрок>
cmd-ban_exemption_update-arg-flag = <флаг>

cmd-ban_exemption_get-desc = Показывает исключения из банов для игрока.
cmd-ban_exemption_get-help = Использование: ban_exemption_get <игрок>

cmd-ban_exemption_get-nargs = Нужен ровно 1 аргумент
cmd-ban_exemption_get-none = У пользователя нет исключений из банов.
cmd-ban_exemption_get-show = У пользователя есть исключения из флагов банов: {$flags}.
cmd-ban_exemption_get-arg-player = <игрок>

ban-panel-title = Панель банов
ban-panel-player = Игрок
ban-panel-ip = IP
ban-panel-hwid = HWID
ban-panel-reason = Причина
ban-panel-last-conn = Взять IP и HWID из последнего подключения?
ban-panel-submit = Забанить
ban-panel-confirm = Точно?
ban-panel-tabs-basic = Основное
ban-panel-tabs-reason = Причина
ban-panel-tabs-players = Список игроков
ban-panel-tabs-role = Бан роли
ban-panel-no-data = Для бана нужно указать пользователя, IP или HWID
ban-panel-invalid-ip = Не удалось разобрать IP-адрес. Попробуйте еще раз
ban-panel-select = Выберите тип
ban-panel-server = Бан на сервере
ban-panel-role = Бан роли
ban-panel-minutes = Минуты
ban-panel-hours = Часы
ban-panel-days = Дни
ban-panel-weeks = Недели
ban-panel-months = Месяцы
ban-panel-years = Годы
ban-panel-permanent = Навсегда
ban-panel-ip-hwid-tooltip = Оставьте пустым и отметьте галочку ниже, чтобы взять данные последнего подключения
ban-panel-severity = Тяжесть:
ban-panel-erase = Стереть сообщения в чате и игрока из раунда

server-ban-string = {$admin} выдает бан на сервере, тяжесть {$severity}, истекает {$expires}, для [{$name}, {$ip}, {$hwid}]. Причина: {$reason}
server-ban-string-no-pii = {$admin} выдает бан на сервере, тяжесть {$severity}, истекает {$expires}, игрок {$name}. Причина: {$reason}
server-ban-string-never = никогда

ban-kick-reason = Вы забанены
