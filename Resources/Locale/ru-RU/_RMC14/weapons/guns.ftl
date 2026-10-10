cm-gun-unskilled = Похоже, вы не умеете пользоваться этим оружием
cm-gun-no-ammo-message = Патроны кончились!
cm-gun-use-delay = Подождите {$seconds} сек., прежде чем стрелять снова!
cm-gun-pump-examine = [bold]Перед выстрелом передерните цевье клавишей [color=cyan]особого действия[/color] (по умолчанию пробел).[/bold]
cm-gun-pump-first-with = Сначала передерните цевье клавишей {$key}!
cm-gun-pump-first = Сначала передерните цевье!

rmc-breech-loaded-open-shoot-attempt = Сначала закройте казенник!
rmc-breech-loaded-not-ready-to-shoot = Сначала откройте и закройте казенник!
rmc-breech-loaded-closed-load-attempt = Сначала откройте казенник!
rmc-breech-loaded-closed-extract-attempt = Сначала откройте казенник!
rmc-breech-loaded-toggle-attempt-cooldown = Подождите, прежде чем снова {$action} казенник!
rmc-breech-loaded-open = открывать
rmc-breech-loaded-close = закрывать

rmc-wield-use-delay = Подождите {$seconds} сек., прежде чем браться за оружие двумя руками!
rmc-shoot-use-delay = Подождите {$seconds} сек., прежде чем стрелять из этого оружия!

rmc-shoot-harness-required = Нужна подвесная система
rmc-wear-smart-gun-required = Чтобы это надеть, нужен экипированный смартган.
rmc-gun-arc-blocked = Нельзя стрелять за пределами сектора обстрела оружия.

rmc-shoot-id-lock-unauthorized = Спуск заблокирован. Пользователь не авторизован.
rmc-id-lock-unauthorized = Действие запрещено. Пользователь не авторизован.
rmc-id-lock-authorization = Вы поднимаете оружие, и оно регистрирует вас как владельца.
rmc-id-lock-authorization-combat = Оружие пищит и регистрирует вас как владельца.
rmc-id-lock-toggle-lock = Вы {$action} ID-блокировку оружия.

rmc-id-lock-color-unauthorized = red
rmc-id-lock-color-authorized = chartreuse
rmc-id-lock-toggle-on = включаете
rmc-id-lock-toggle-off = выключаете

rmc-iff-toggle = Вы {$action} систему "свой-чужой" оружия.
rmc-iff-toggle-off = выключаете
rmc-iff-toggle-on = включаете

rmc-revolver-spin = Вы раскручиваете барабан.

rmc-examine-text-weapon-accuracy = Множитель точности сейчас [color={$colour}]{TOSTRING($accuracy, "F2")}[/color].

rmc-examine-text-scatter-max = Текущий максимальный разброс: [color={$colour}]{TOSTRING($scatter, "F1")}[/color] град.
rmc-examine-text-scatter-min = Текущий минимальный разброс: [color={$colour}]{TOSTRING($scatter, "F1")}[/color] град.
rmc-examine-text-shots-to-max-scatter = Разброс достигает максимума за [color={$colour}]{$shots}[/color] выстр.
rmc-examine-text-iff = [color=cyan]Это оружие не стреляет по своим, пули пролетают мимо них![/color]
rmc-examine-text-id-lock-no-user = [color=chartreuse]Оружие не зарегистрировано. Поднимите его, чтобы стать владельцем.[/color]
rmc-examine-text-id-lock = [color=chartreuse]Владелец: [/color][color={$color}]{$name}[/color][color=chartreuse].[/color]
rmc-examine-text-id-lock-unlocked = [color=chartreuse]Владелец: [/color][color={$color}]{$name}[/color][color=chartreuse], ограничения стрельбы сняты.[/color]
rmc-examine-text-execute = [color=red]При наличии навыка из этого оружия можно казнить![/color]

rmc-gun-rack-examine = [bold]Перед выстрелом передерните затвор клавишей [color=cyan]особого действия[/color] (по умолчанию пробел).[/bold]
rmc-gun-rack-first-with = Сначала передерните затвор клавишей {$key}!
rmc-gun-rack-first = Сначала передерните затвор!

rmc-assisted-reload-fail-angle = Чтобы перезарядить чужое оружие, встаньте за спиной стрелка!
rmc-assisted-reload-fail-full = Оружие уже заряжено.
rmc-assisted-reload-fail-mismatch = Этот боеприпас сюда не подходит!
rmc-assisted-reload-start-user = Вы начинаете перезаряжать оружие напарника! Не двигайтесь...
rmc-assisted-reload-start-target = { CAPITALIZE($reloader) } начинает перезаряжать ваше оружие! Не двигайтесь...

rmc-gun-stacks-hit-single = В яблочко!
rmc-gun-stacks-hit-multiple = В яблочко! Попаданий подряд: {$hits}!
rmc-gun-stacks-reset = Оружие пищит: данные наведения потеряны, обычный режим стрельбы восстановлен.

rmc-gun-shoot-air-self = ВЫ СТРЕЛЯЕТЕ В ВОЗДУХ!
rmc-gun-shoot-air-other = { CAPITALIZE($user) } СТРЕЛЯЕТ В ВОЗДУХ!
rmc-gun-shoot-air-blocked = Над вами слишком плотный потолок.
rmc-gun-shoot-air-examine = [bold]Чтобы выстрелить в воздух, нажмите клавишу [color=cyan]особого действия[/color] (по умолчанию пробел){$harm ->
    [true] {" в боевом режиме"}
    *[false] {""}
    }.[/bold]

rmc-flare-gun-examine = Последняя ракета-маркер выпущена с обозначением [color=#ad3b98][bold]{$id}[/bold][/color].

expendable-light-starshell-ash-empty-name = потухший пепел осветительного снаряда
expendable-light-starshell-ash-empty-desc = Выгоревшие остатки осветительного снаряда
