guidebook-reagent-effect-description =
    {$chance ->
        [1] { $effect }
        *[other] С вероятностью { NATURALPERCENT($chance, 2) } { $effect }
    }{ $conditionCount ->
        [0] .
        *[other] , если { $conditions }.
    }
guidebook-reagent-name = [bold][color={$color}]{CAPITALIZE($name)}[/color][/bold]
guidebook-reagent-recipes-header = Рецепт
guidebook-reagent-recipes-reagent-display = [bold]{$reagent}[/bold] \[{$ratio}\]
guidebook-reagent-sources-header = Источники
guidebook-reagent-sources-ent-wrapper = [bold]{$name}[/bold] \[1\]
guidebook-reagent-sources-gas-wrapper = [bold]{$name} (газ)[/bold] \[1\]
guidebook-reagent-effects-header = Эффекты
guidebook-reagent-effects-metabolism-group-rate = [bold]{$group}[/bold] [color=gray]({$rate} ед. в секунду) (передоз: {$overdose}, крит. передоз: {$critOverdose})[/color]
guidebook-reagent-plant-metabolisms-header = Метаболизм растений
guidebook-reagent-plant-metabolisms-rate = [bold]Метаболизм растений[/bold] [color=gray](базово 1 ед. каждые 3 секунды)[/color]
guidebook-reagent-physical-description = [italic]На вид {$description}.[/italic]
guidebook-reagent-recipes-mix-info = {$minTemp ->
    [0] {$hasMax ->
            [true] {CAPITALIZE($verb)} ниже {NATURALFIXED($maxTemp, 2)} K
            *[false] {CAPITALIZE($verb)}
        }
    *[other] {CAPITALIZE($verb)} {$hasMax ->
            [true] от {NATURALFIXED($minTemp, 2)} K до {NATURALFIXED($maxTemp, 2)} K
            *[false] выше {NATURALFIXED($minTemp, 2)} K
        }
}
