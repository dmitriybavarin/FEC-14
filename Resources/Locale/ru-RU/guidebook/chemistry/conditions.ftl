reagent-effect-condition-guidebook-total-damage =
    { $max ->
        [2147483648] общий урон не меньше {NATURALFIXED($min, 2)}
        *[other] { $min ->
                    [0] общий урон не больше {NATURALFIXED($max, 2)}
                    *[other] общий урон от {NATURALFIXED($min, 2)} до {NATURALFIXED($max, 2)}
                 }
    }
reagent-effect-condition-guidebook-total-hunger =
    { $max ->
        [2147483648] общий голод цели не меньше {NATURALFIXED($min, 2)}
        *[other] { $min ->
                    [0] общий голод цели не больше {NATURALFIXED($max, 2)}
                    *[other] общий голод цели от {NATURALFIXED($min, 2)} до {NATURALFIXED($max, 2)}
                 }
    }
reagent-effect-condition-guidebook-reagent-threshold =
    { $max ->
        [2147483648] реагента {$reagent} не меньше {NATURALFIXED($min, 2)} ед.
        *[other] { $min ->
                    [0] реагента {$reagent} не больше {NATURALFIXED($max, 2)} ед.
                    *[other] реагента {$reagent} от {NATURALFIXED($min, 2)} до {NATURALFIXED($max, 2)} ед.
                 }
    }
reagent-effect-condition-guidebook-mob-state-condition =
    существо { $state ->
        [Alive] живо
        [Critical] в критическом состоянии
        [Dead] мертво
       *[other] в состоянии { $state }
    }
reagent-effect-condition-guidebook-job-condition =
    должность цели { $job }
reagent-effect-condition-guidebook-solution-temperature =
    температура раствора { $max ->
            [2147483648] не меньше {NATURALFIXED($min, 2)} K
            *[other] { $min ->
                        [0] не больше {NATURALFIXED($max, 2)} K
                        *[other] от {NATURALFIXED($min, 2)} K до {NATURALFIXED($max, 2)} K
                     }
    }
reagent-effect-condition-guidebook-body-temperature =
    температура тела { $max ->
            [2147483648] не меньше {NATURALFIXED($min, 2)} K
            *[other] { $min ->
                        [0] не больше {NATURALFIXED($max, 2)} K
                        *[other] от {NATURALFIXED($min, 2)} K до {NATURALFIXED($max, 2)} K
                     }
    }
reagent-effect-condition-guidebook-organ-type =
    усваивающий орган { $shouldhave ->
                                [true] относится
                                *[false] не относится
                           } к типу {$name}
reagent-effect-condition-guidebook-has-tag =
    у цели { $invert ->
                 [true] нет
                 *[false] есть
                } тега {$tag}
reagent-effect-condition-guidebook-this-reagent = этого реагента
reagent-effect-condition-guidebook-breathing =
    употребивший { $isBreathing ->
                [true] нормально дышит
                *[false] задыхается
               }
reagent-effect-condition-guidebook-internals =
    употребивший { $usingInternals ->
                [true] дышит через баллон
                *[false] дышит окружающим воздухом
               }
