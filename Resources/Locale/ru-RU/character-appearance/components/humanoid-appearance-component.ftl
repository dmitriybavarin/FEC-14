humanoid-appearance-component-unknown-species = Человек
humanoid-appearance-component-examine =
    { $ageid ->
        [young] { GENDER($user) ->
            [female] Она молода.
            [epicene] Они молоды.
           *[other] Он молод.
        }
        [middle] { GENDER($user) ->
            [female] Она среднего возраста.
            [epicene] Они среднего возраста.
           *[other] Он среднего возраста.
        }
        [old] { GENDER($user) ->
            [female] Она в преклонном возрасте.
            [epicene] Они в преклонном возрасте.
           *[other] Он в преклонном возрасте.
        }
       *[other] Возраст определить трудно.
    }
    По расе { $species }.
