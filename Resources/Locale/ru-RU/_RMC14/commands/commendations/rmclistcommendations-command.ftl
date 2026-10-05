cmd-rmclistcommendations-desc = Показывает награды по раунду, игроку, ID или последние выданные.
cmd-rmclistcommendations-help = Использование:
  rmclistcommendations last <количество> [тип]
    - Последние выданные награды
    - количество: сколько последних наград показать
    - тип: фильтр по типу награды (по умолчанию все)

  rmclistcommendations round <ID раунда> [тип]
    - Все награды за раунд
    - тип: фильтр по типу награды (по умолчанию все)

  rmclistcommendations id <ID награды>
    - Одна награда по ID

  rmclistcommendations player giver <ник или ID> <количество> [тип]
    - Награды, выданные игроком
    - количество: сколько последних наград показать
    - тип: фильтр по типу награды (по умолчанию все)

  rmclistcommendations player receiver <ник или ID> <количество> [тип]
    - Награды, полученные игроком
    - количество: сколько последних наград показать
    - тип: фильтр по типу награды (по умолчанию все)

  Примеры:
    rmclistcommendations last 10
    rmclistcommendations last 5 jelly
    rmclistcommendations round 42
    rmclistcommendations round 42 medal
    rmclistcommendations id 128
    rmclistcommendations player giver PlayerName 10
    rmclistcommendations player receiver PlayerName 5 jelly

cmd-rmclistcommendations-invalid-arguments = Неверные аргументы!
cmd-rmclistcommendations-invalid-round-id = Неверный ID раунда!
cmd-rmclistcommendations-invalid-id = Неверный ID награды!
cmd-rmclistcommendations-invalid-type = Неверный тип "{ $type }"!
cmd-rmclistcommendations-invalid-player-mode = Неверный режим игрока! Нужно giver или receiver.
cmd-rmclistcommendations-invalid-count = Неверное количество! Нужно положительное число.
cmd-rmclistcommendations-player-not-found = Игрок "{ $player }" не найден.
cmd-rmclistcommendations-no-results = Наград не найдено.

cmd-rmclistcommendations-last-header = Последние награды, показано { $count } (запрошено { $total }):
cmd-rmclistcommendations-round-header = Награды за раунд { $round } (всего { $count }):
cmd-rmclistcommendations-id-header = Награда { $id }:
cmd-rmclistcommendations-giver-header = Последние выданные награды, показано { $count } (запрошено { $total }):
cmd-rmclistcommendations-receiver-header = Последние полученные награды, показано { $count } (запрошено { $total }):

cmd-rmclistcommendations-format = id [{ $id }] { $type }: { $name }, { $giverUserName } ({ $giver }) → { $receiverUserName } ({ $receiver }), раунд { $round }: { $text }

cmd-rmclistcommendations-hint-mode = Режим (last, round, id или player)
cmd-rmclistcommendations-hint-mode-last = Последние награды
cmd-rmclistcommendations-hint-mode-round = Награды за раунд
cmd-rmclistcommendations-hint-mode-id = Награда по ID
cmd-rmclistcommendations-hint-mode-player = Награды игрока
cmd-rmclistcommendations-hint-round-id = ID раунда
cmd-rmclistcommendations-hint-commendation-id = ID награды
cmd-rmclistcommendations-hint-player-mode = Режим игрока (giver или receiver)
cmd-rmclistcommendations-hint-player-giver = Награды, выданные игроком
cmd-rmclistcommendations-hint-player-receiver = Награды, полученные игроком
cmd-rmclistcommendations-hint-player = Ник или UserId игрока
cmd-rmclistcommendations-hint-count = Сколько наград показать
cmd-rmclistcommendations-hint-type = Фильтр по типу награды
