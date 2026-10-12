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
cmd-fecevents-help = Использование: fecevents list | block <ID> [срок|forever] | unblock <ID> | start <ID>

fec-event-antag-marshal-name = Выживший маршал КМП США
fec-event-antag-marshal-objective = Вы можете оказаться маршалом Колониального бюро, застрявшим на колонии. Продержитесь до эвакуации, АРЕС следит за вами.

fec-event-marshal-name = Выживший маршал КМП США
fec-event-marshal-desc = На колонии остался маршал, которого ждут на «Алмейере». Если он погибнет или попадёт в плен, Верховное командование тайно пришлёт MARSOC.
fec-event-patrol-name = Патруль Колониального бюро
fec-event-patrol-desc = На колонию высаживается патруль маршалов Колониального бюро.
fec-event-storm-name = Аномальный шторм
fec-event-storm-desc = На планете внезапно начинается непогода, синоптики не успели предупредить.

fec-marshal-brief = Вы маршал Колониального бюро, отрезанный от своих на колонии. Корабль КМП США принял ваш маяк. Выживите и доберитесь до «Алмейера», АРЕС раз в час проверяет ваше состояние.
fec-marshal-ares-beacon = Принят аварийный маяк Колониального бюро. На поверхности находится маршал { $name }. Приоритет — эвакуация на борт. Состояние маршала будет проверяться каждый час.
fec-marshal-ares-alive = Маяк маршала { $name } активен, показатели в норме. Следующая проверка через { $minutes } мин.
fec-marshal-ares-evacuated = Маршал { $name } на борту корабля. Доклад отправлен Верховному командованию.
fec-marshal-hc-thanks = Верховное командование подтверждает эвакуацию маршала. Отличная работа, морпехи.
fec-marshal-status-missing = маяк не отвечает
fec-marshal-status-dead = маршал погиб
fec-marshal-status-detained = маршал захвачен
fec-marshal-ares-report = Сигнал маяка маршала { $name }: { $status }. Автоматический доклад направлен в Генеральный штаб.
fec-marshal-hc-reply = Генеральный штаб получил доклад АРЕС. Для проверки сведений направлена группа. Экипажу оказать полное содействие.
fec-marshal-ares-unknown-vessel = Внимание. К кораблю приближается неопознанное судно без ответчика свой-чужой. Системы ПВО наводятся на цель.
fec-marshal-ares-vessel-lost = Неопознанное судно ушло с радаров.
fec-marshal-ares-air-defense-failed = Отказ системы ПВО. Неопознанное судно таранит корпус «Алмейера». Аварийным службам прибыть к месту столкновения.
fec-marshal-ares-boarding = Обшивка пробита. Зафиксирована высадка вооружённой группы на борт. Опознаны знаки отличия MARSOC, личного отряда Верховного командования.
fec-marshal-punitive-brief = Вы рейдер MARSOC, личного отряда Верховного командования КМП США. Вы лучшие из лучших и подчиняетесь только Верховному командованию. Экипаж «Алмейера» не уберёг маршала Колониального бюро, и приказ однозначен: зачистить корабль. Никаких компромиссов, только результат.
fec-marshal-ares-squad-destroyed = Вооружённая группа на борту уничтожена.
fec-marshal-hc-squad-destroyed = Верховное командование потеряло связь с отрядом MARSOC. Инцидент будет расследован.
fec-marshal-end-punished-xenos = Экипаж «Алмейера» не справился с задачей и был наказан MARSOC, но силы КМП на планете продолжили бой и взяли верх над ксеноморфами.
fec-marshal-end-punished = Экипаж «Алмейера» не справился с задачей и был наказан MARSOC.
fec-marshal-summary-saved = Маршал { $name } эвакуирован на корабль
fec-marshal-summary-lost = Маршал { $name } потерян, { $status }
fec-marshal-summary-no-squad = Отряд MARSOC так и не вышел на связь
fec-marshal-summary-repelled = Экипаж отбил штурм MARSOC
fec-marshal-summary-punished = MARSOC зачистил «Алмейер»

fec-patrol-ares = Зафиксирована посадка патрульного челнока Колониального бюро на поверхности. Маршалы запрашивают связь с командованием.
fec-patrol-summary = На колонию высадился патруль Колониального бюро

fec-storm-hc-warning = Метеоспутник фиксирует аномальный фронт над зоной операции. Погода резко ухудшается, будьте готовы.
fec-storm-summary = Над колонией прошёл аномальный шторм
