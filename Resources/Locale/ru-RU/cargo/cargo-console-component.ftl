cargo-console-menu-title = Консоль заказов поставок
cargo-console-menu-account-name-label = Счет:{" "}
cargo-console-menu-account-name-none-text = Нет
cargo-console-menu-account-name-format = [bold][color={$color}]{$name}[/color][/bold] [font="Monospace"]\[{$code}\][/font]
cargo-console-menu-shuttle-name-label = Название шаттла:{" "}
cargo-console-menu-shuttle-name-none-text = Нет
cargo-console-menu-points-label = Баланс:{" "}
cargo-console-menu-points-amount = ${$amount}
cargo-console-menu-shuttle-status-label = Статус шаттла:{" "}
cargo-console-menu-shuttle-status-away-text = В пути
cargo-console-menu-order-capacity-label = Вместимость заказа:{" "}
cargo-console-menu-call-shuttle-button = Активировать телепад
cargo-console-menu-permissions-button = Доступы
cargo-console-menu-categories-label = Категории:{" "}
cargo-console-menu-search-bar-placeholder = Поиск
cargo-console-menu-requests-label = Запросы
cargo-console-menu-orders-label = Заказы
cargo-console-menu-order-reason-description = Причина: {$reason}
cargo-console-menu-populate-categories-all-text = Все
cargo-console-menu-populate-orders-cargo-order-row-product-name-text = {$productName} (x{$orderAmount}), заказчик {$orderRequester}, счет [color={$accountColor}]{$account}[/color]
cargo-console-menu-cargo-order-row-approve-button = Одобрить
cargo-console-menu-cargo-order-row-cancel-button = Отменить
cargo-console-menu-tab-title-orders = Заказы
cargo-console-menu-tab-title-funds = Переводы
cargo-console-menu-account-action-transfer-limit = [bold]Лимит перевода:[/bold] ${$limit}
cargo-console-menu-account-action-transfer-limit-unlimited-notifier = [color=gold](Без лимита)[/color]
cargo-console-menu-account-action-select = [bold]Действие со счетом:[/bold]
cargo-console-menu-account-action-amount = [bold]Сумма:[/bold] $
cargo-console-menu-account-action-button = Перевести
cargo-console-menu-toggle-account-lock-button = Переключить лимит перевода
cargo-console-menu-account-action-option-withdraw = Снять наличные
cargo-console-menu-account-action-option-transfer = Перевести средства на {$code}
cargo-console-order-not-allowed = Доступ запрещен
cargo-console-station-not-found = Нет доступной станции
cargo-console-invalid-product = Неверный ID товара
cargo-console-too-many = Слишком много одобренных заказов
cargo-console-snip-snip = Заказ урезан до вместимости
cargo-console-insufficient-funds = Недостаточно средств (нужно {$cost})
cargo-console-unfulfilled = Нет места для выполнения заказа
cargo-console-trade-station = Отправлено: {$destination}
cargo-console-unlock-approved-order-broadcast = [bold]{$productName} x{$orderAmount}[/bold] стоимостью [bold]{$cost}[/bold] одобряет [bold]{$approver}[/bold]
cargo-console-fund-withdraw-broadcast = [bold]{$name} снимает {$amount} кредитов со счета {$name1} \[{$code1}\]
cargo-console-fund-transfer-broadcast = [bold]{$name} переводит {$amount} кредитов со счета {$name1} \[{$code1}\] на счет {$name2} \[{$code2}\][/bold]
cargo-console-fund-transfer-user-unknown = Неизвестно
cargo-console-paper-reason-default = Нет
cargo-console-paper-approver-default = Сам заказчик
cargo-console-paper-print-name = Заказ №{$orderNumber}
cargo-console-paper-print-text = [head=2]Заказ №{$orderNumber}[/head]
    {"[bold]Товар:[/bold]"} {$itemName} (x{$orderQuantity})
    {"[bold]Заказчик:[/bold]"} {$requester}

    {"[head=3]Информация о заказе[/head]"}
    {"[bold]Плательщик[/bold]:"} {$account} [font="Monospace"]\[{$accountcode}\][/font]
    {"[bold]Одобрено:[/bold]"} {$approver}
    {"[bold]Причина:[/bold]"} {$reason}
cargo-shuttle-console-menu-title = Консоль грузового шаттла
cargo-shuttle-console-station-unknown = Неизвестно
cargo-shuttle-console-shuttle-not-found = Не найден
cargo-shuttle-console-organics = На шаттле обнаружены органические формы жизни
cargo-no-shuttle = Грузовой шаттл не найден!
cargo-funding-alloc-console-menu-title = Консоль распределения средств
cargo-funding-alloc-console-label-account = [bold]Счет[/bold]
cargo-funding-alloc-console-label-code = [bold] Код [/bold]
cargo-funding-alloc-console-label-balance = [bold] Баланс [/bold]
cargo-funding-alloc-console-label-cut = [bold] Доля дохода (%) [/bold]
cargo-funding-alloc-console-label-primary-cut = Доля отдела поставок с продаж вне сейфов (%):
cargo-funding-alloc-console-label-lockbox-cut = Доля отдела поставок с продаж из сейфов (%):
cargo-funding-alloc-console-label-help-non-adjustible = Отдел поставок получает {$percent}% прибыли с продаж вне сейфов. Остальное делится так, как указано ниже:
cargo-funding-alloc-console-label-help-adjustible = Остаток средств с продаж вне сейфов делится так, как указано ниже:
cargo-funding-alloc-console-button-save = Сохранить изменения
cargo-funding-alloc-console-label-save-fail = [bold]Неверное распределение дохода![/bold] [color=red]({$pos ->
    [1] +
    *[-1] -
}{$val}%)[/color]
cargo-acquisition-slip-body = [head=3]Сведения о товаре[/head]
    {"[bold]Товар:[/bold]"} {$product}
    {"[bold]Описание:[/bold]"} {$description}
    {"[bold]Цена за штуку:[/bold"}] ${$unit}
    {"[bold]Количество:[/bold]"} {$amount}
    {"[bold]Стоимость:[/bold]"} ${$cost}

    {"[head=3]Сведения о покупке[/head]"}
    {"[bold]Заказчик:[/bold]"} {$orderer}
    {"[bold]Причина:[/bold]"} {$reason}
