fec-profile-events-tab = События

fec-events-unknown = Событие { $id } не найдено
fec-events-disabled = События отключены настройкой сервера
fec-events-blocked = Событие { $id } заблокировано
fec-events-not-enough-players = Недостаточно игроков, нужно { $min }, сейчас { $current }
fec-events-no-planet = На этот раунд нет планеты для высадки
fec-events-rule-failed = Не удалось запустить правило события
fec-events-summary-started = Произошло событие «{ $name }»
fec-events-status-started = уже запущено
fec-events-status-blocked = заблокировано навсегда
fec-events-status-blocked-until = заблокировано до { $date }
fec-events-status-planned = запланировано на { $minutes } минуте
fec-events-status-available = доступно
fec-events-list-entry = { $id } — { $name } (от { $players } игроков) — { $status }
fec-events-none = Событий нет
fec-events-blocked-done = Событие { $id } заблокировано
fec-events-unblocked = Событие { $id } разблокировано
fec-events-not-blocked = Событие { $id } не было заблокировано
fec-events-started = Событие { $id } запущено
fec-events-hint-id = ID события
fec-events-hint-duration = Срок блокировки (30m, 12h, 7d) или forever

cmd-fecevents-desc = Управление событиями раунда.
cmd-fecevents-help = Использование: fecevents list | block <ID> [срок|forever] | unblock <ID> | start <ID> | cancel <ID> | rotation

fec-event-antag-marshal-name = Выживший маршал КМП США
fec-event-antag-marshal-objective = Вы можете оказаться маршалом Колониального бюро, застрявшим на колонии. Продержитесь до эвакуации, ARES следит за вами.

fec-event-marshal-name = Выживший маршал КМП США
fec-event-marshal-desc = На колонии остался маршал, которого ждут на «Алмайере». Если он погибнет или попадёт в плен, Верховное командование тайно пришлёт MARSOC.
fec-event-patrol-name = Патруль Колониального бюро
fec-event-patrol-desc = На колонию высаживается патруль маршалов Колониального бюро.
fec-event-storm-name = Аномальный шторм
fec-event-storm-desc = На планете внезапно начинается непогода, синоптики не успели предупредить.

fec-marshal-brief = Вы маршал Колониального бюро, отрезанный от своих на колонии. Корабль КМП США принял ваш маяк. Выживите и доберитесь до «Алмайера», ARES раз в час проверяет ваше состояние.
fec-marshal-ares-beacon = Принят аварийный маяк Колониального бюро. На поверхности находится маршал { $name }. Приоритет — эвакуация на борт. Состояние маршала будет проверяться каждый час.
fec-marshal-ares-alive = Маяк маршала { $name } активен, показатели в норме. Следующая проверка через { $minutes } мин.
fec-marshal-ares-evacuated = Маршал { $name } на борту корабля. Доклад отправлен Верховному командованию.
fec-marshal-hc-thanks = Верховное командование подтверждает эвакуацию маршала. Отличная работа, морпехи.
fec-marshal-status-missing = маяк не отвечает
fec-marshal-status-dead = маршал погиб
fec-marshal-status-detained = маршал захвачен
fec-marshal-ares-report = Сигнал маяка маршала { $name }: { $status }. Автоматический доклад направлен в Генеральный штаб.
fec-marshal-hc-reply = Генеральный штаб получил доклад ARES. Для проверки сведений направлена группа. Экипажу оказать полное содействие.
fec-marshal-ares-unknown-vessel = Внимание. К кораблю приближается неопознанное судно без ответчика свой-чужой. Системы ПВО наводятся на цель.
fec-marshal-ares-vessel-lost = Неопознанное судно ушло с радаров.
fec-marshal-ares-air-defense-failed = Отказ системы ПВО. Неопознанное судно таранит корпус «Алмайера». Аварийным службам прибыть к месту столкновения.
fec-marshal-ares-boarding = Обшивка пробита. Зафиксирована высадка вооружённой группы на борт. Опознаны знаки отличия MARSOC, личного отряда Верховного командования.
fec-marshal-punitive-brief = Вы рейдер MARSOC, личного отряда Верховного командования КМП США. Вы лучшие из лучших и подчиняетесь только Верховному командованию. Экипаж «Алмайера» не уберёг маршала Колониального бюро, и приказ однозначен: зачистить корабль. Никаких компромиссов, только результат.
fec-marshal-ares-squad-destroyed = Вооружённая группа на борту уничтожена.
fec-marshal-hc-squad-destroyed = Верховное командование потеряло связь с отрядом MARSOC. Инцидент будет расследован.
fec-marshal-end-punished-xenos = Экипаж «Алмайера» не справился с задачей и был наказан MARSOC, но силы КМП на планете продолжили бой и взяли верх над ксеноморфами.
fec-marshal-end-punished = Экипаж «Алмайера» не справился с задачей и был наказан MARSOC.
fec-marshal-summary-saved = Маршал { $name } эвакуирован на корабль
fec-marshal-summary-lost = Маршал { $name } потерян, { $status }
fec-marshal-summary-no-squad = Отряд MARSOC так и не вышел на связь
fec-marshal-summary-repelled = Экипаж отбил штурм MARSOC
fec-marshal-summary-punished = MARSOC зачистил «Алмайер»

fec-patrol-ares = Зафиксирована посадка патрульного челнока Колониального бюро на поверхности. Маршалы запрашивают связь с командованием.
fec-patrol-summary = На колонию высадился патруль Колониального бюро

fec-storm-hc-warning = Метеоспутник фиксирует аномальный фронт над зоной операции. Погода резко ухудшается, будьте готовы.
fec-storm-summary = Над колонией прошёл аномальный шторм

fec-events-status-rotation = уже было в этой ротации
fec-events-rotation-reset = Ротация событий сброшена, все события снова доступны

fec-events-vote-title = Какое событие произойдёт в раунде?
fec-events-vote-initiator = Система событий

fec-events-log-slot = Ролл на событие прошёл, слот на { $minutes } минуте
fec-events-log-no-roll = Ролл на событие не прошёл, в этом раунде событий не будет
fec-events-log-no-candidates = Подходящих событий нет, слот пропущен
fec-events-log-vote = Голосование призраков: { $names }
fec-events-log-vote-result = Призраки выбрали «{ $name }»
fec-events-log-vote-cancelled = Голосование отменено, выбрано «{ $name }»
fec-events-log-skipped = «{ $name }» не запущено, { $reason }
fec-events-log-started = Запущено «{ $name }»
fec-events-log-started-admin = Админ запустил «{ $name }»
fec-events-log-started-chain = По цепочке запущено «{ $name }»
fec-events-log-outcome = Исход «{ $name }», { $outcome }
fec-events-log-chain = Цепочка, «{ $name }» на { $minutes } минуте
fec-events-log-chain-blocked = Цепочка не продолжена, «{ $name }» заблокировано
fec-events-log-chain-skipped = Цепочка, «{ $name }» не запущено, { $reason }
fec-events-log-blocked = { $admin } заблокировал «{ $name }»
fec-events-log-unblocked = { $admin } разблокировал «{ $name }»
fec-events-log-rotation-reset = { $admin } сбросил ротацию событий
fec-events-log-rotation-done = Все события ротации прошли, ротация началась заново

fec-events-outcome-failed = не удалось начать
fec-events-outcome-saved = маршал спасён
fec-events-outcome-lost = маршал потерян
fec-events-outcome-no-squad = отряд не вышел на связь
fec-events-outcome-repelled = экипаж отбил штурм
fec-events-outcome-punished = экипаж наказан
fec-events-outcome-landed = патруль высадился
fec-events-outcome-started = шторм начался

fec-events-ui-button = События
fec-events-ui-title = События раунда
fec-events-ui-info = Игроков { $players }. { $slotCount ->
    [0] Слотов нет
   *[other] Слоты на минутах { $slots }
}. { $vote ->
    [1] Идёт голосование
   *[other] Голосования нет
}
fec-events-ui-duration = Срок блокировки
fec-events-ui-reset-rotation = Сбросить ротацию
fec-events-ui-log = Журнал раунда
fec-events-ui-log-empty = Журнал пуст
fec-events-ui-log-line = { $time }  { $text }
fec-events-ui-row = { $name } ({ $id }), от { $players } игр., вес { $weight }, { $status }
fec-events-ui-start = Запустить
fec-events-ui-start-confirm = Точно?
fec-events-ui-block = Блок
fec-events-ui-unblock = Разблок

cmd-feceventspanel-desc = Открывает окно событий раунда.
cmd-feceventspanel-help = Использование: feceventspanel

fec-event-pizza-name = Доставка пиццы
fec-event-pizza-desc = На «Алмайер» прибывает курьер с пиццей. Кто её заказал, никто не помнит.
fec-pizza-ares = На борт принят гражданский челнок службы доставки. Груз, пицца. Получатель не установлен.
fec-pizza-summary = На корабль доставили пиццу

fec-event-freelancers-name = Наёмники
fec-event-freelancers-desc = На колонии высаживается враждебный отряд наёмников-фрилансеров.
fec-freelancers-ares = Зафиксирована посадка неопознанного челнока на поверхности. Ответчик свой-чужой не отвечает, будьте осторожны.
fec-freelancers-xeno = На нашу землю спустились новые носители. Они вооружены и не принадлежат к тем, кто на корабле.
fec-freelancers-summary = На колонию высадились наёмники

fec-events-status-awaiting = ждёт подтверждения, старт в { $time }
fec-events-cancelled = Запуск события { $id } отменён
fec-events-not-awaiting = Событие { $id } не ждёт запуска
fec-events-admin-awaiting = Событие «{ $name }» запустится через { $seconds } сек. Отменить можно в окне «События» или командой fecevents cancel.
fec-events-admin-low-online = Слот события на { $minutes } минуте пропадёт, игроков { $players }, а самому доступному событию нужно { $needed }.
fec-events-admin-no-candidates = Слот события на { $minutes } минуте пропадёт, подходящих событий нет.
fec-events-admin-low-online-chain = Цепочка «{ $name }» может не запуститься, игроков { $players }, нужно { $needed }.
fec-events-log-cancelled = { $admin } отменил запуск «{ $name }»
fec-events-ui-awaiting-row = { $chained ->
    [1] Цепочка
   *[other] Ожидает
} «{ $name }», старт через { $seconds } сек
fec-events-ui-confirm = Сейчас
fec-events-ui-cancel = Отменить
