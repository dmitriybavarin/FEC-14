cmd-rmcgivecommendation-desc = Вручает игроку медаль или желе
cmd-rmcgivecommendation-help = Использование: rmcgivecommendation <вручающий> <получатель> <имя получателя> <тип> <вид награды> <формулировка> [ID раунда]
  Аргументы:
  вручающий: кто вручает награду в игре (если есть пробелы, ОБЯЗАТЕЛЬНО в кавычках)
  получатель: ник или UserId игрока
  имя получателя: имя персонажа (если есть пробелы, ОБЯЗАТЕЛЬНО в кавычках)
  тип: medal или jelly
  вид награды: число (доступные виды подскажет Tab)
  формулировка: за что награда (ОБЯЗАТЕЛЬНО в кавычках)
  ID раунда: номер раунда, по умолчанию текущий (необязательно)

  Примеры:
    rmcgivecommendation "Верховное командование КМП США" PlayerName "Иван Петров" medal 1 "За исключительную храбрость"
    rmcgivecommendation "Королева-Мать" XenoPlayer "XX-Alpha" jelly 2 "За защиту улья"
    rmcgivecommendation "Верховное командование КМП США" PlayerName "Иван Петров" medal 1 "За исключительную храбрость" 42

cmd-rmcgivecommendation-invalid-arguments = Неверное число аргументов!
cmd-rmcgivecommendation-invalid-type = Неверный тип! Нужно medal или jelly.
cmd-rmcgivecommendation-invalid-award-type = Неверный вид "{ $type }"! Нужно от 1 до { $max }.
cmd-rmcgivecommendation-empty-citation = Формулировка не может быть пустой!
cmd-rmcgivecommendation-player-not-found = Игрок "{ $player }" не найден.

cmd-rmcgivecommendation-success = Награда "{ $award }" вручена, получатель { $player }!
cmd-rmcgivecommendation-admin-announcement = { $admin } вручает награду ({ $type }) "{ $award }" игроку { $receiver } (персонаж { $character }) за раунд { $round }

cmd-rmcgivecommendation-hint-giver = Имя вручающего в игре (вводите внимательно)
cmd-rmcgivecommendation-hint-giver-highcommand = Стандартный вручающий для медалей морпехов
cmd-rmcgivecommendation-hint-giver-queen-mother = Стандартный вручающий для желе ксеноморфов
cmd-rmcgivecommendation-hint-receiver = Ник или UserId получателя
cmd-rmcgivecommendation-hint-receiver-name = Имя персонажа получателя (вводите внимательно)
cmd-rmcgivecommendation-hint-type = Тип (medal или jelly)
cmd-rmcgivecommendation-hint-type-medal = Вручить медаль морпеху
cmd-rmcgivecommendation-hint-type-jelly = Вручить королевское желе ксеноморфу
cmd-rmcgivecommendation-hint-medal-type = Вид медали (1-{ $count })
cmd-rmcgivecommendation-hint-jelly-type = Вид желе (1-{ $count })
cmd-rmcgivecommendation-hint-invalid-type = Тип должен быть medal или jelly
cmd-rmcgivecommendation-hint-citation = Формулировка награды (вводите внимательно)
cmd-rmcgivecommendation-hint-round = ID раунда (необязательно)
cmd-rmcgivecommendation-hint-round-current = Текущий раунд
