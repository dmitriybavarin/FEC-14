rmc-mortar-deploy-start = Вы начинаете устанавливать миномет: { $mortar }.
rmc-mortar-deploy-end-not-planet = Вы разворачиваете переносной миномет M402. Плохая идея.
rmc-mortar-shell-busy = Миномет уже кто-то использует: { $mortar }
rmc-mortar-not-aimed = Сначала нужно навести миномет: { $mortar }.
rmc-mortar-covered = Пожалуй, не стоит ставить миномет в помещении: { $mortar }.
rmc-mortar-target-invalid = По этой цели миномет не выстрелит: { $mortar }.
rmc-mortar-target-not-area = Эта зона вне границ!
rmc-mortar-target-covered = По цели не попасть. Вероятно, она под землей.
rmc-mortar-target-is-lz = Нельзя обстреливать зону высадки!
rmc-mortar-bad-idea = Вы понимаете, насколько это плохая идея, и быстро останавливаетесь.
rmc-mortar-cant-insert = Нельзя зарядить { $shell } в миномет: { $mortar }!
rmc-mortar-not-deployed = Сначала установите миномет: { $mortar }!
rmc-mortar-fire-cooldown = Ствол миномета ({ $mortar }) еще раскален. Подождите несколько секунд и прекратите огонь.
rmc-mortar-less-accurate-with-range = [color=red]Чем дальше цель, тем ниже точность![/color]
rmc-mortar-target-start-self = Вы начинаете выставлять угол и дальность миномета ({ $mortar }) под новые координаты.
rmc-mortar-target-start-others = { CAPITALIZE($user) } начинает выставлять угол и дальность миномета: { $mortar }.
rmc-mortar-target-finish-self = Вы выставили угол и дальность миномета ({ $mortar }) под новые координаты.
rmc-mortar-target-finish-others = { CAPITALIZE($user) } выставляет угол и дальность миномета: { $mortar }.
rmc-mortar-dial-start-self = Вы начинаете вносить поправку в угол и дальность миномета ({ $mortar }) под новые координаты.
rmc-mortar-dial-start-others = { CAPITALIZE($user) } начинает вносить поправку в угол и дальность миномета: { $mortar }.
rmc-mortar-dial-finish-self = Вы внесли поправку в угол и дальность миномета ({ $mortar }) под новые координаты.
rmc-mortar-dial-finish-others = { CAPITALIZE($user) } вносит поправку в угол и дальность миномета: { $mortar }.

rmc-mortar-shell-load-start-self = Вы начинаете заряжать снаряд ({ $shell }) в миномет: { $mortar }.
rmc-mortar-shell-load-start-others = { CAPITALIZE($user) } начинает заряжать снаряд ({ $shell }) в миномет: { $mortar }.
rmc-mortar-shell-load-finish-self = Вы заряжаете снаряд ({ $shell }) в миномет: { $mortar }.
rmc-mortar-shell-load-finish-others = { CAPITALIZE($user) } заряжает снаряд ({ $shell }) в миномет: { $mortar }
rmc-mortar-shell-fire = { CAPITALIZE($mortar) } стреляет!
rmc-mortar-shell-warning = СНАРЯД ПАДАЕТ { $direction ->
        [NORTH] К СЕВЕРУ
        [NORTHEAST] К СЕВЕРО-ВОСТОКУ
        [EAST] К ВОСТОКУ
        [SOUTHEAST] К ЮГО-ВОСТОКУ
        [SOUTH] К ЮГУ
        [SOUTHWEST] К ЮГО-ЗАПАДУ
        [WEST] К ЗАПАДУ
        [NORTHWEST] К СЕВЕРО-ЗАПАДУ
       *[other] К СЕВЕРУ
    } ОТ ВАС
rmc-mortar-shell-warning-above = СНАРЯД ПАДАЕТ ПРЯМО НА ВАС
rmc-mortar-shell-impact-warning = СНАРЯД ВОТ-ВОТ УДАРИТ { $direction ->
        [NORTH] К СЕВЕРУ
        [NORTHEAST] К СЕВЕРО-ВОСТОКУ
        [EAST] К ВОСТОКУ
        [SOUTHEAST] К ЮГО-ВОСТОКУ
        [SOUTH] К ЮГУ
        [SOUTHWEST] К ЮГО-ЗАПАДУ
        [WEST] К ЗАПАДУ
        [NORTHWEST] К СЕВЕРО-ЗАПАДУ
       *[other] К СЕВЕРУ
    } ОТ ВАС
rmc-mortar-shell-impact-warning-above = СНАРЯД ВОТ-ВОТ УДАРИТ ПРЯМО ПО ВАМ

rmc-mortar-interface = Интерфейс миномета
rmc-mortar-target-title = Координаты цели
rmc-mortar-offset-title = Поправка

rmc-mortar-target-x = Цель X:
rmc-mortar-target-y = Цель Y:
rmc-mortar-target-set = Задать цель
rmc-mortar-target-too-close = Сюда навести нельзя: точка слишком близко к миномету.
rmc-mortar-target-too-far = Сюда навести нельзя: точка слишком далеко от миномета.

rmc-mortar-offset-x = Поправка X:
rmc-mortar-offset-y = Поправка Y:
rmc-mortar-offset-set = Внести поправку
rmc-mortar-offset-too-far = Такую поправку не внести: точка слишком далеко от исходной цели.
rmc-mortar-offset-too-close = Такую поправку не внести: точка слишком близко к миномету.
rmc-mortar-offset-max = Макс.
  поправка: { $max }

rmc-mortar-view-camera = Открыть
  камеру

rmc-mortar-camera-title = Камера миномета
rmc-mortar-camera-name = Пара-камера ({ $x }):({ $y })

rmc-mortar-toggle-mode = Режим наведения
rmc-mortar-toggle-mode-message = Переключиться между наведением по координатам и по лазеру

rmc-mortar-coordinates-mode-switched-self = Вы переключаете миномет ({ $mortar }) на наведение по координатам.
rmc-mortar-laser-mode-switched-self = Вы переключаете миномет ({ $mortar }) на лазерное наведение.

rmc-mortar-coordinates-mode-switched-others = { CAPITALIZE($user) } переключает миномет ({ $mortar }) на наведение по координатам.
rmc-mortar-laser-mode-switched-others = { CAPITALIZE($user) } переключает миномет ({ $mortar }) на лазерное наведение.

rmc-mortar-linking-start = Вы начинаете связывать целеуказатель ({ $laserDesignator }) с минометом: { $mortar }.
rmc-mortar-laser-linked-self = Вы связали целеуказатель ({ $laserDesignator }) с минометом: { $mortar }.
rmc-mortar-laser-linked-others = { CAPITALIZE($user) } связывает целеуказатель ({ $laserDesignator }) с минометом: { $mortar }.
rmc-mortar-already-linking = Миномет уже связывают с лазерным целеуказателем: { $mortar }.

rmc-mortar-no-laser-target = У миномета нет лазерной цели: { $mortar }!
rmc-mortar-no-laser-designator = К миномету не привязан лазерный целеуказатель: { $mortar }!

rmc-mortar-in-coordinates-mode = Миномет в режиме наведения по координатам: { $mortar }.
rmc-mortar-in-laser-mode = Миномет в режиме лазерного наведения: { $mortar }.

rmc-mortar-laser-aimed = Миномет наведен на цель и готов к стрельбе: { $mortar }!

rmc-mortar-toggle-mode-hint = [color=cyan]Alt + клик по миномету переключает режим наведения.[/color]

rmc-mortar-dial-coordinates = Миномет ({ $mortar }) в режиме лазерного наведения. Чтобы вносить поправки, переключите его на наведение по координатам!

rmc-mortar-beeping = пищит!
rmc-mortar-beeping-warning = тревожно пищит!
rmc-mortar-targeting = Миномет еще наводится: { $mortar }.
