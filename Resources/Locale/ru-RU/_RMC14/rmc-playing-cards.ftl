rmc-playing-card-suit-spades = [color=SteelBlue]пик[/color]
rmc-playing-card-suit-hearts = [color=Crimson]червей[/color]
rmc-playing-card-suit-diamonds = [color=Crimson]бубен[/color]
rmc-playing-card-suit-clubs = [color=SteelBlue]треф[/color]

rmc-playing-card-rank-ace = [color=GoldenRod]Туз[/color]
rmc-playing-card-rank-jack = [color=GoldenRod]Валет[/color]
rmc-playing-card-rank-queen = [color=GoldenRod]Дама[/color]
rmc-playing-card-rank-king = [color=GoldenRod]Король[/color]
rmc-playing-card-rank-ace-short = [color=GoldenRod]Т[/color]
rmc-playing-card-rank-jack-short = [color=GoldenRod]В[/color]
rmc-playing-card-rank-queen-short = [color=GoldenRod]Д[/color]
rmc-playing-card-rank-king-short = [color=GoldenRod]К[/color]

rmc-playing-card-examine = [color=Tan][bold][italic]Это [color=SandyBrown]{ $rank }[/color] { $suit }.[/italic][/bold][/color]
rmc-playing-card-examine-face-down = [color=Tan][italic]Карта лежит рубашкой вверх.[/italic][/color]

rmc-playing-card-flip = { $direction ->
    [up] Вы переворачиваете карту лицом вверх.
   *[other] Вы переворачиваете карту рубашкой вверх.
}
rmc-playing-card-draw = Вы берете карту: { $rank } { $suit }.
rmc-playing-card-draw-hidden = Вы берете карту.
rmc-playing-card-draw-deck = Вы берете карту из колоды.
rmc-playing-card-add-to-hand = Вы добавляете карту в руку. (Всего карт: { $count })

rmc-playing-card-deck-examine = [color=Tan][italic]Похоже, в коробке [color=OliveDrab]карт: { $count }[/color].[/italic][/color]
rmc-playing-card-deck-examine-verb = Управление
rmc-playing-card-deck-examine-verb-message = Показать управление колодой.
rmc-playing-card-deck-examine-shuffle = [bold][color=SteelBlue]Alt[/color]+использование [color=PeachPuff](E) (Z)[/color][/bold]
    : перемешать колоду.
rmc-playing-card-deck-examine-draw = [bold]Использование [color=PeachPuff](E) (Z)[/color] или [color=PeachPuff](C)[/color] [color=SteelBlue]взаимодействие[/color] другой рукой[/bold]
    : взять карту.
rmc-playing-card-deck-examine-pickup = [bold][color=SteelBlue]Удерживая колоду, кликнуть по пустому полу[/color][/bold]
    : собрать карты поблизости.

rmc-playing-card-deck-empty = Колода пуста!
rmc-playing-card-deck-full = Колода полна!
rmc-playing-card-deck-shuffle = Вы перемешиваете колоду: { $deck }.
rmc-playing-card-added-to-deck = Вы добавляете карту в колоду.
rmc-playing-card-added-cards-to-deck = Вы добавляете карты в колоду: { $count }.
rmc-playing-card-deck-pickup = Вы вкладываете карты в колоду: { $count }.
rmc-playing-card-draw-multiple = Вы берете карты: { $count }.

rmc-playing-card-hand-name = Карты в руке
rmc-playing-card-stack-name = Стопка карт
rmc-playing-card-hand-examine = [color=Tan][italic]Всего [color=OliveDrab]карт: { $count }[/color].[/italic][/color]
rmc-playing-card-hand-examine-hidden = [color=Tan][italic]Похоже, [color=OliveDrab]карт: { $count }[/color], рубашкой вверх.[/italic][/color]
rmc-playing-card-hand-suit-group = - [bold][italic][color=SandyBrown]{ $ranks }[/color] { $suit }[/italic][/bold]

rmc-playing-card-hand-examine-verb = Управление
rmc-playing-card-hand-examine-verb-message = Показать управление картами в руке.
rmc-playing-card-hand-examine-face-down = [bold][color=SteelBlue]Рубашкой вверх[/color] [color=PeachPuff]E[/color] или [color=PeachPuff]Z[/color][/bold]
    : взять верхнюю карту.
rmc-playing-card-hand-examine-face-up = [bold][color=SteelBlue]Лицом вверх[/color] [color=PeachPuff]E[/color] или [color=PeachPuff]Z[/color][/bold]
    : открыть руку и выбрать нужную карту.
rmc-playing-card-hand-examine-flip = [bold][color=SteelBlue]Перевернуть[/color] [color=PeachPuff]ALT + использование[/color][/bold]
    : перевернуть карты.

rmc-playing-card-hand-flip = { $direction ->
    [up] Вы переворачиваете карты лицом вверх.
   *[other] Вы переворачиваете карты рубашкой вверх.
}
rmc-playing-card-hand-shuffle = Вы тасуете карты: { $hand }.
rmc-playing-card-hand-empty = В руке нет карт!
rmc-playing-card-merge-hands = Вы объединяете карты. (Всего карт: { $count })

rmc-playing-card-verb-category-draw = Взять
rmc-playing-card-verb-flip = Перевернуть
rmc-playing-card-verb-draw = Взять карту
rmc-playing-card-verb-draw-5 = Взять 5
rmc-playing-card-verb-draw-half = Взять половину
rmc-playing-card-verb-draw-all = Взять все
rmc-playing-card-verb-pick = Выбрать карту
rmc-playing-card-verb-shuffle = Перемешать
