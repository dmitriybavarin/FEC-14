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
