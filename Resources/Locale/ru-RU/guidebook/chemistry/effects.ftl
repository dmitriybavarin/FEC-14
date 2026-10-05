-create-3rd-person =
    { $chance ->
        [1] Создает
        *[other] создает
    }
-cause-3rd-person =
    { $chance ->
        [1] Вызывает
        *[other] вызывает
    }
-satiate-3rd-person =
    { $chance ->
        [1] Утоляет
        *[other] утоляет
    }
reagent-effect-guidebook-create-entity-reaction-effect =
    { $chance ->
        [1] Создает
        *[other] создает
    } объект {$entname}{ $amount ->
        [1] {""}
        *[other] {" "}({$amount} шт.)
    }
reagent-effect-guidebook-explosion-reaction-effect =
    { $chance ->
        [1] Вызывает
        *[other] вызывает
    } взрыв
reagent-effect-guidebook-emp-reaction-effect =
    { $chance ->
        [1] Вызывает
        *[other] вызывает
    } электромагнитный импульс
reagent-effect-guidebook-flash-reaction-effect =
    { $chance ->
        [1] Вызывает
        *[other] вызывает
    } ослепляющую вспышку
reagent-effect-guidebook-foam-area-reaction-effect =
    { $chance ->
        [1] Создает
        *[other] создает
    } много пены
reagent-effect-guidebook-smoke-area-reaction-effect =
    { $chance ->
        [1] Создает
        *[other] создает
    } много дыма
reagent-effect-guidebook-satiate-thirst =
    { $chance ->
        [1] Утоляет
        *[other] утоляет
    } жажду { $relative ->
        [1] со средней скоростью
        *[other] в {NATURALFIXED($relative, 3)} раза быстрее среднего
    }
reagent-effect-guidebook-satiate-hunger =
    { $chance ->
        [1] Утоляет
        *[other] утоляет
    } голод { $relative ->
        [1] со средней скоростью
        *[other] в {NATURALFIXED($relative, 3)} раза быстрее среднего
    }
reagent-effect-guidebook-health-change =
    { $chance ->
        [1] { $healsordeals ->
                [heals] Лечит урон:
                [deals] Наносит урон:
                *[both] Меняет здоровье:
             }
        *[other] { $healsordeals ->
                    [heals] лечит урон:
                    [deals] наносит урон:
                    *[both] меняет здоровье:
                 }
    } { $changes }
reagent-effect-guidebook-even-health-change =
    { $chance ->
        [1] { $healsordeals ->
            [heals] Равномерно лечит урон:
            [deals] Равномерно наносит урон:
            *[both] Равномерно меняет здоровье:
        }
        *[other] { $healsordeals ->
            [heals] равномерно лечит урон:
            [deals] равномерно наносит урон:
            *[both] равномерно меняет здоровье:
        }
    } { $changes }
reagent-effect-guidebook-status-effect =
    { $type ->
        [add]   { $chance ->
                    [1] Вызывает
                    *[other] вызывает
                } {LOC($key)} минимум на {NATURALFIXED($time, 3)} с с накоплением
        *[set]  { $chance ->
                    [1] Вызывает
                    *[other] вызывает
                } {LOC($key)} минимум на {NATURALFIXED($time, 3)} с без накопления
        [remove]{ $chance ->
                    [1] Сокращает
                    *[other] сокращает
                } {LOC($key)} на {NATURALFIXED($time, 3)} с
    }
reagent-effect-guidebook-set-solution-temperature-effect =
    { $chance ->
        [1] Устанавливает
        *[other] устанавливает
    } температуру раствора ровно на {NATURALFIXED($temperature, 2)} K
reagent-effect-guidebook-adjust-solution-temperature-effect =
    { $chance ->
        [1] { $deltasign ->
                [1] Нагревает
                *[-1] Охлаждает
            }
        *[other]
            { $deltasign ->
                [1] нагревает
                *[-1] охлаждает
            }
    } раствор, пока температура не станет { $deltasign ->
                [1] не выше {NATURALFIXED($maxtemp, 2)} K
                *[-1] не ниже {NATURALFIXED($mintemp, 2)} K
            }
reagent-effect-guidebook-adjust-reagent-reagent =
    { $chance ->
        [1] { $deltasign ->
                [1] Добавляет
                *[-1] Удаляет
            }
        *[other]
            { $deltasign ->
                [1] добавляет
                *[-1] удаляет
            }
    } {NATURALFIXED($amount, 2)} ед. реагента {$reagent} { $deltasign ->
        [1] в раствор
        *[-1] из раствора
    }
reagent-effect-guidebook-adjust-reagent-group =
    { $chance ->
        [1] { $deltasign ->
                [1] Добавляет
                *[-1] Удаляет
            }
        *[other]
            { $deltasign ->
                [1] добавляет
                *[-1] удаляет
            }
    } {NATURALFIXED($amount, 2)} ед. реагентов группы {$group} { $deltasign ->
            [1] в раствор
            *[-1] из раствора
        }
reagent-effect-guidebook-adjust-temperature =
    { $chance ->
        [1] { $deltasign ->
                [1] Добавляет
                *[-1] Отнимает
            }
        *[other]
            { $deltasign ->
                [1] добавляет
                *[-1] отнимает
            }
    } {POWERJOULES($amount)} тепла { $deltasign ->
            [1] телу, в котором находится
            *[-1] у тела, в котором находится
        }
reagent-effect-guidebook-chem-cause-disease =
    { $chance ->
        [1] Вызывает
        *[other] вызывает
    } болезнь { $disease }
reagent-effect-guidebook-chem-cause-random-disease =
    { $chance ->
        [1] Вызывает
        *[other] вызывает
    } одну из болезней: { $diseases }
reagent-effect-guidebook-jittering =
    { $chance ->
        [1] Вызывает
        *[other] вызывает
    } дрожь
reagent-effect-guidebook-chem-clean-bloodstream =
    { $chance ->
        [1] Очищает
        *[other] очищает
    } кровь от других химикатов
reagent-effect-guidebook-cure-disease =
    { $chance ->
        [1] Лечит
        *[other] лечит
    } болезни
reagent-effect-guidebook-cure-eye-damage =
    { $chance ->
        [1] { $deltasign ->
                [1] Повреждает
                *[-1] Лечит
            }
        *[other]
            { $deltasign ->
                [1] повреждает
                *[-1] лечит
            }
    } глаза
reagent-effect-guidebook-chem-vomit =
    { $chance ->
        [1] Вызывает
        *[other] вызывает
    } рвоту
reagent-effect-guidebook-create-gas =
    { $chance ->
        [1] Создает
        *[other] создает
    } газ { $gas } ({ $moles } моль)
reagent-effect-guidebook-drunk =
    { $chance ->
        [1] Вызывает
        *[other] вызывает
    } опьянение
reagent-effect-guidebook-electrocute =
    { $chance ->
        [1] Бьет
        *[other] бьет
    } употребившего током на {NATURALFIXED($time, 3)} с
reagent-effect-guidebook-emote =
    { $chance ->
        [1] Вызывает
        *[other] вызывает
    } у употребившего эмоцию [bold][color=white]{$emote}[/color][/bold]
reagent-effect-guidebook-extinguish-reaction =
    { $chance ->
        [1] Тушит
        *[other] тушит
    } огонь
reagent-effect-guidebook-flammable-reaction =
    { $chance ->
        [1] Повышает
        *[other] повышает
    } горючесть
reagent-effect-guidebook-ignite =
    { $chance ->
        [1] Поджигает
        *[other] поджигает
    } употребившего
reagent-effect-guidebook-make-sentient =
    { $chance ->
        [1] Делает
        *[other] делает
    } употребившего разумным
reagent-effect-guidebook-make-polymorph =
    { $chance ->
        [1] Превращает
        *[other] превращает
    } употребившего в существо { $entityname }
reagent-effect-guidebook-modify-bleed-amount =
    { $chance ->
        [1] { $deltasign ->
                [1] Усиливает
                *[-1] Ослабляет
            }
        *[other] { $deltasign ->
                    [1] усиливает
                    *[-1] ослабляет
                 }
    } кровотечение
reagent-effect-guidebook-modify-blood-level =
    { $chance ->
        [1] { $deltasign ->
                [1] Повышает
                *[-1] Понижает
            }
        *[other] { $deltasign ->
                    [1] повышает
                    *[-1] понижает
                 }
    } уровень крови
reagent-effect-guidebook-paralyze =
    { $chance ->
        [1] Парализует
        *[other] парализует
    } употребившего минимум на {NATURALFIXED($time, 3)} с
reagent-effect-guidebook-movespeed-modifier =
    { $chance ->
        [1] Меняет
        *[other] меняет
    } скорость передвижения в {NATURALFIXED($walkspeed, 3)} раза минимум на {NATURALFIXED($time, 3)} с
reagent-effect-guidebook-reset-narcolepsy =
    { $chance ->
        [1] Временно подавляет
        *[other] временно подавляет
    } нарколепсию
reagent-effect-guidebook-wash-cream-pie-reaction =
    { $chance ->
        [1] Смывает
        *[other] смывает
    } кремовый торт с лица
reagent-effect-guidebook-cure-zombie-infection =
    { $chance ->
        [1] Лечит
        *[other] лечит
    } текущую зомби-инфекцию
reagent-effect-guidebook-cause-zombie-infection =
    { $chance ->
        [1] Заражает
        *[other] заражает
    } зомби-инфекцией
reagent-effect-guidebook-innoculate-zombie-infection =
    { $chance ->
        [1] Лечит
        *[other] лечит
    } текущую зомби-инфекцию и дает иммунитет к будущим заражениям
reagent-effect-guidebook-reduce-rotting =
    { $chance ->
        [1] Отменяет
        *[other] отменяет
    } {NATURALFIXED($time, 3)} с гниения
reagent-effect-guidebook-area-reaction =
    { $chance ->
        [1] Вызывает
        *[other] вызывает
    } реакцию дыма или пены на {NATURALFIXED($duration, 3)} с
reagent-effect-guidebook-add-to-solution-reaction =
    { $chance ->
        [1] Заставляет
        *[other] заставляет
    } химикаты, нанесенные на объект, попадать в его внутренний контейнер для растворов
reagent-effect-guidebook-artifact-unlock =
    { $chance ->
        [1] Помогает
        *[other] помогает
        } разблокировать инопланетный артефакт.
reagent-effect-guidebook-artifact-durability-restore =
    Восстанавливает прочность активных узлов инопланетного артефакта на {$restored}.
reagent-effect-guidebook-plant-attribute =
    { $chance ->
        [1] Меняет
        *[other] меняет
    } {$attribute} на [color={$colorName}]{$amount}[/color]
reagent-effect-guidebook-plant-cryoxadone =
    { $chance ->
        [1] Омолаживает
        *[other] омолаживает
    } растение в зависимости от его возраста и времени роста
reagent-effect-guidebook-plant-phalanximine =
    { $chance ->
        [1] Возвращает
        *[other] возвращает
    } жизнеспособность растению, потерявшему ее из-за мутации
reagent-effect-guidebook-plant-diethylamine =
    { $chance ->
        [1] Повышает
        *[other] повышает
    } продолжительность жизни и/или базовое здоровье растения, с шансом 10% для каждого
reagent-effect-guidebook-plant-robust-harvest =
    { $chance ->
        [1] Повышает
        *[other] повышает
    } силу растения на {$increase}, но не выше {$limit}. Когда сила достигает {$seedlesstreshold}, растение теряет семена. Попытка поднять силу выше {$limit} может с шансом 10% снизить урожай
reagent-effect-guidebook-plant-seeds-add =
    { $chance ->
        [1] Возвращает
        *[other] возвращает
    } растению семена
reagent-effect-guidebook-plant-seeds-remove =
    { $chance ->
        [1] Удаляет
        *[other] удаляет
    } семена растения
