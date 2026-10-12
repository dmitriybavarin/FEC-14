cmd-jobwhitelistpanel-desc = Открывает панель вайтлистов должностей.
cmd-jobwhitelistpanel-help = Использование: jobwhitelistpanel
fec-job-whitelist-ui-button = Вайтлисты должностей
fec-job-whitelist-ui-title = Вайтлисты должностей
fec-job-whitelist-ui-search = Поиск по игроку или должности
fec-job-whitelist-ui-refresh = Обновить
fec-job-whitelist-ui-group-player = По игрокам
fec-job-whitelist-ui-group-job = По должностям
fec-job-whitelist-ui-summary = Записей: { $entries }, игроков: { $players }
fec-job-whitelist-ui-empty = Вайтлистов нет.
fec-job-whitelist-ui-player-name = { $player }{ $online ->
    [1] {" "}(в сети)
   *[other] {""}
}
fec-job-whitelist-ui-player-header = { $player }, должностей: { $count }
fec-job-whitelist-ui-job-header = { $job } ({ $id }), игроков: { $count }{ $other ->
    [1] {" "}[Остальное]
   *[other] {""}
}
fec-job-whitelist-ui-job-row = { $job } ({ $id }){ $other ->
    [1] {" "}[Остальное]
   *[other] {""}
}
fec-job-whitelist-ui-job-option = { $job } ({ $id }){ $whitelisted ->
    [1] {" "}*
   *[other] {""}
}{ $other ->
    [1] {" "}[Остальное]
   *[other] {""}
}
fec-job-whitelist-ui-remove = Снять
fec-job-whitelist-ui-remove-confirm = Точно?
fec-job-whitelist-ui-add-heading = Выдать вайтлист
fec-job-whitelist-ui-player = Ник или ID игрока
fec-job-whitelist-ui-add = Выдать
fec-job-whitelist-ui-other-hint = * должность требует вайтлист. [Остальное] должности нет в лобби, игрок увидит ее в категории "Остальное".
fec-job-whitelist-ui-no-player = Введите ник или ID игрока.
fec-job-whitelist-ui-already = { $player } уже имеет вайтлист на должность { $job }.
fec-job-whitelist-ui-added = { $player } получает вайтлист на должность { $job }.
fec-job-whitelist-ui-removed = { $player } больше не имеет вайтлиста на должность { $job }.
fec-lobby-other-jobs = Остальное
fec-lobby-other-job-no-preference = Эта должность выбирается не через приоритеты в лобби.

fec-temp-whitelist-duration = Срок
fec-temp-whitelist-duration-tooltip = Пусто — навсегда. Примеры 30m, 12h, 7d, 2w (минуты, часы, дни, недели).
fec-temp-whitelist-row = { $text } [до { $date }]
fec-temp-whitelist-added = { $player } получает временный вайтлист на должность { $job } до { $date }.
fec-temp-whitelist-expired = Временный вайтлист { $player } на должность { $job } истёк и снят.
fec-temp-whitelist-bad-duration = Неверный срок «{ $duration }». Примеры 30m, 12h, 7d, 2w.
cmd-jobwhitelisttemp-desc = Выдаёт временный вайтлист на должность.
cmd-jobwhitelisttemp-help = Использование: jobwhitelisttemp <ник> <должность> <срок>, например jobwhitelisttemp Ivan CMSquadLeader 7d
cmd-jobwhitelisttemp-hint-player = <ник или ID>
cmd-jobwhitelisttemp-hint-job = <должность>
cmd-jobwhitelisttemp-hint-duration = <срок, например 30m, 12h, 7d>
