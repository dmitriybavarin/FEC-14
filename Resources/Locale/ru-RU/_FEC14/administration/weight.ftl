fec-admin-weight-rank = { $rank } (вес { $weight })
fec-admin-weight-value = Вес: { $weight }
fec-admin-weight-label = Вес:
fec-admin-weight-tooltip = Админ с меньшим весом не может банить, наказывать и редактировать админа с большим весом. Менять вес можно только с флагом ADMINWEIGHT и не выше своего.
fec-admin-weight-blocked = Нельзя применить это к админу, у которого вес больше вашего.
fec-admin-weight-pardon-blocked = Нельзя снять бан, выданный админом с большим весом, чем ваш.
cmd-feckick-desc = Кикает игрока с сервера с учётом веса админов.
cmd-feckick-help = Использование: feckick <ник> [причина]
cmd-feckick-not-found = Игрок { $player } не найден.
cmd-feckick-reason = Кикнут администрацией: { $reason }
cmd-feckick-no-reason = Кикнут администрацией.
cmd-feckick-success = Игрок { $player } кикнут.
cmd-feckick-hint-player = <ник>
cmd-feckick-hint-reason = [причина]

fec-audit-rank-added = [Аудит прав] { $admin } создал ранг { $rank } (флаги { $flags })
fec-audit-rank-updated = [Аудит прав] { $admin } изменил ранг { $rank } (флаги { $flags })
fec-audit-rank-removed = [Аудит прав] { $admin } удалил ранг { $rank }
fec-audit-admin-added = [Аудит прав] { $admin } выдал админку { $target } (титул { $title }, ранг { $rank }, флаги { $flags })
fec-audit-admin-updated = [Аудит прав] { $admin } изменил админку { $target } (титул { $title }, ранг { $rank }, флаги { $flags })
fec-audit-admin-removed = [Аудит прав] { $admin } снял админку с { $target }
fec-audit-weight-set = [Аудит прав] { $admin } установил вес ранга { $rank } на { $weight }
fec-audit-blocked-rank = [Аудит прав] Заблокирована попытка { $admin } изменить ранг { $rank } без нужных флагов или веса
fec-audit-blocked-admin = [Аудит прав] Заблокирована попытка { $admin } изменить права админа { $target } без нужных флагов или веса

fec-history-button = История
fec-history-title = История игрока { $player }
fec-history-loading = Загрузка истории…
fec-history-load-error = Не удалось загрузить часть истории, подробности в логах сервера.
fec-history-empty = Записей нет.
fec-history-filter-all = Все записи
fec-history-kind-ban = Бан
fec-history-kind-roleban = Бан роли
fec-history-kind-chatban = Чат-бан
fec-history-kind-note = Заметка
fec-history-kind-watchlist = Наблюдение
fec-history-kind-message = Сообщение
fec-history-kind-whitelist = Вайтлист
fec-history-system = система
fec-history-entry = { $admin } { $text }
fec-history-entry-extra = { $admin } { $text } ({ $extra })
fec-history-until = до { $date }
fec-history-permanent = навсегда
fec-history-pardoned = снят
fec-history-whitelist = Должность { $job }{ $temp ->
        [1] , временно до { $date }
       *[other] , навсегда
    }
fec-history-summary-online = Сейчас { $online ->
        [1] на сервере
       *[other] не в сети
    }
fec-history-summary-playtime = Наиграно { $hours } ч { $minutes } мин
fec-history-summary-whitelist = Вайтлист сервера { $whitelisted ->
        [1] есть
       *[other] нет
    }
fec-history-summary-seen = Впервые { $first }, последний вход { $last }
cmd-playerhistory-desc = Открывает единое окно истории игрока.
cmd-playerhistory-help = Использование: playerhistory <ник или ID>
