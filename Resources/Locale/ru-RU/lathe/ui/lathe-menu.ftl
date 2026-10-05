lathe-menu-title = Меню станка
lathe-menu-queue = Очередь
lathe-menu-server-list = Список серверов
lathe-menu-sync = Синхронизировать
lathe-menu-search-designs = Поиск чертежей
lathe-menu-category-all = Все
lathe-menu-search-filter = Фильтр:
lathe-menu-amount = Количество:
lathe-menu-recipe-count = { $count ->
    [one] {$count} рецепт
    [few] {$count} рецепта
    *[other] {$count} рецептов
}
lathe-menu-reagent-slot-examine = Сбоку есть слот для мензурки.
lathe-reagent-dispense-no-container = Жидкость выливается на пол!
lathe-menu-result-reagent-display = {$reagent} ({$amount} ед.)
lathe-menu-material-display = {$material} ({$amount})
lathe-menu-tooltip-display = {$material}: {$amount}
lathe-menu-description-display = [italic]{$description}[/italic]
lathe-menu-material-amount =
    {NATURALFIXED($amount, 2)} { $unit ->
        [sheet] { $amount ->
            [one] лист
            [few] листа
            [many] листов
           *[other] листа
        }
        [bar] { $amount ->
            [one] слиток
            [few] слитка
            [many] слитков
           *[other] слитка
        }
        [plank] { $amount ->
            [one] доска
            [few] доски
            [many] досок
           *[other] доски
        }
        [roll] { $amount ->
            [one] рулон
            [few] рулона
            [many] рулонов
           *[other] рулона
        }
        [bunch] { $amount ->
            [one] гроздь
            [few] грозди
            [many] гроздей
           *[other] грозди
        }
        [slab] { $amount ->
            [one] пласт
            [few] пласта
            [many] пластов
           *[other] пласта
        }
        [web] { $amount ->
            [one] моток
            [few] мотка
            [many] мотков
           *[other] мотка
        }
        [boll] { $amount ->
            [one] коробочка
            [few] коробочки
            [many] коробочек
           *[other] коробочки
        }
        [bill] { $amount ->
            [one] купюра
            [few] купюры
            [many] купюр
           *[other] купюры
        }
       *[other] { $amount ->
            [one] кусок
            [few] куска
            [many] кусков
           *[other] куска
        }
    }
lathe-menu-material-amount-missing =
    {$material}: {NATURALFIXED($amount, 2)} { $unit ->
        [sheet] { $amount ->
            [one] лист
            [few] листа
            [many] листов
           *[other] листа
        }
        [bar] { $amount ->
            [one] слиток
            [few] слитка
            [many] слитков
           *[other] слитка
        }
        [plank] { $amount ->
            [one] доска
            [few] доски
            [many] досок
           *[other] доски
        }
        [roll] { $amount ->
            [one] рулон
            [few] рулона
            [many] рулонов
           *[other] рулона
        }
        [bunch] { $amount ->
            [one] гроздь
            [few] грозди
            [many] гроздей
           *[other] грозди
        }
        [slab] { $amount ->
            [one] пласт
            [few] пласта
            [many] пластов
           *[other] пласта
        }
        [web] { $amount ->
            [one] моток
            [few] мотка
            [many] мотков
           *[other] мотка
        }
        [boll] { $amount ->
            [one] коробочка
            [few] коробочки
            [many] коробочек
           *[other] коробочки
        }
        [bill] { $amount ->
            [one] купюра
            [few] купюры
            [many] купюр
           *[other] купюры
        }
       *[other] { $amount ->
            [one] кусок
            [few] куска
            [many] кусков
           *[other] куска
        }
    } ([color=red]не хватает {NATURALFIXED($missingAmount, 2)}[/color])
lathe-menu-no-materials-message = Материалы не загружены.
lathe-menu-silo-linked-message = Хранилище подключено
lathe-menu-fabricating-message = Производство...
lathe-menu-materials-title = Материалы
lathe-menu-queue-title = Очередь производства
