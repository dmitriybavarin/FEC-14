requisition-paperwork-receiver-name = Отдел логистики
requisition-paperwork-reward-message = Подтверждение получено! Из излишков бюджета переведено ${$amount}

requisition-paper-print-name = накладная: {$name}
requisition-paper-print-manifest = [head=2]
    {$containerName}[/head][bold]{$content}[/bold][head=2]
    ВЕС {$weight} ФУНТ.
    ПАРТИЯ {$lot}
    С/Н {$serialNumber}[/head]
requisition-paper-print-content = - {$count} {$item}

ui-supply-drop-consle-name = Консоль сброса поставок
ui-supply-drop-console-name-bolded = [bold]СБРОС СНАБЖЕНИЯ[/bold]
ui-supply-drop-console-longitude = Долгота:
ui-supply-drop-console-latitude = Широта:
ui-supply-drop-pad-status = [bold]Состояние площадки[/bold]
ui-supply-drop-console-update = Обновить
ui-supply-drop-console-ready = Готово к запуску!
ui-supply-drop-console-launch = ЗАПУСТИТЬ СБРОС СНАБЖЕНИЯ
ui-supply-drop-console-launch-confirmation = Подтвердить сброс поставок?
ui-supply-drop-console-cooldown = До следующего запуска: {$time} сек.
ui-supply-drop-crate-status =
    { $hasCrate ->
        [true] Состояние площадки: ящик загружен.
       *[false] Ящик не загружен.
    }
