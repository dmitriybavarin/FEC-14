device-pda-slot-component-slot-name-cartridge = Картридж
default-program-name = Программа
notekeeper-program-name = Заметки
nano-task-program-name = NanoTask
news-read-program-name = Новости станции
crew-manifest-program-name = Манифест экипажа
crew-manifest-cartridge-loading = Загрузка...
net-probe-program-name = NetProbe
net-probe-scan = Устройство {$device} просканировано!
net-probe-label-name = Название
net-probe-label-address = Адрес
net-probe-label-frequency = Частота
net-probe-label-network = Сеть
log-probe-program-name = LogProbe
log-probe-scan = Журналы устройства {$device} загружены!
log-probe-label-time = Время
log-probe-label-accessor = Кто открывал
log-probe-label-number = №
log-probe-print-button = Распечатать журналы
log-probe-printout-device = Устройство: {$name}
log-probe-printout-header = Последние записи:
log-probe-printout-entry = №{$number} / {$time} / {$accessor}
astro-nav-program-name = AstroNav
med-tek-program-name = MedTek
nano-task-ui-heading-high-priority-tasks =
    { $amount ->
        [0] Нет задач высокого приоритета
        [one] {$amount} задача высокого приоритета
        [few] {$amount} задачи высокого приоритета
       *[other] {$amount} задач высокого приоритета
    }
nano-task-ui-heading-medium-priority-tasks =
    { $amount ->
        [0] Нет задач среднего приоритета
        [one] {$amount} задача среднего приоритета
        [few] {$amount} задачи среднего приоритета
       *[other] {$amount} задач среднего приоритета
    }
nano-task-ui-heading-low-priority-tasks =
    { $amount ->
        [0] Нет задач низкого приоритета
        [one] {$amount} задача низкого приоритета
        [few] {$amount} задачи низкого приоритета
       *[other] {$amount} задач низкого приоритета
    }
nano-task-ui-done = Готово
nano-task-ui-revert-done = Отменить
nano-task-ui-priority-low = Низкий
nano-task-ui-priority-medium = Средний
nano-task-ui-priority-high = Высокий
nano-task-ui-cancel = Отмена
nano-task-ui-print = Печать
nano-task-ui-delete = Удалить
nano-task-ui-save = Сохранить
nano-task-ui-new-task = Новая задача
nano-task-ui-description-label = Описание:
nano-task-ui-description-placeholder = Достать что-то важное
nano-task-ui-requester-label = Заказчик:
nano-task-ui-requester-placeholder = Иван Нанотрейзен
nano-task-ui-item-title = Изменить задачу
nano-task-printed-description = [bold]Описание[/bold]: {$description}
nano-task-printed-requester = [bold]Заказчик[/bold]: {$requester}
nano-task-printed-high-priority = [bold]Приоритет[/bold]: [color=red]Высокий[/color]
nano-task-printed-medium-priority = [bold]Приоритет[/bold]: Средний
nano-task-printed-low-priority = [bold]Приоритет[/bold]: Низкий
wanted-list-program-name = Список розыска
wanted-list-label-no-records = Все спокойно, ковбой
wanted-list-search-placeholder = Поиск по имени и статусу
wanted-list-age-label = [color=darkgray]Возраст:[/color] [color=white]{$age}[/color]
wanted-list-job-label = [color=darkgray]Должность:[/color] [color=white]{$job}[/color]
wanted-list-species-label = [color=darkgray]Раса:[/color] [color=white]{$species}[/color]
wanted-list-gender-label = [color=darkgray]Пол:[/color] [color=white]{$gender}[/color]
wanted-list-reason-label = [color=darkgray]Причина:[/color] [color=white]{$reason}[/color]
wanted-list-unknown-reason-label = Причина неизвестна
wanted-list-initiator-label = [color=darkgray]Инициатор:[/color] [color=white]{$initiator}[/color]
wanted-list-unknown-initiator-label = Инициатор неизвестен
wanted-list-status-label = [color=darkgray]Статус:[/color] {$status ->
        [suspected] [color=yellow]Подозревается[/color]
        [wanted] [color=red]В розыске[/color]
        [detained] [color=#b18644]Задержан[/color]
        [paroled] [color=green]Условно освобожден[/color]
        [discharged] [color=green]Освобожден[/color]
        *[other] Нет
    }
wanted-list-history-table-time-col = Время
wanted-list-history-table-reason-col = Преступление
wanted-list-history-table-initiator-col = Инициатор
