cmd-rmcdeletecommendations-desc = Удаляет награды по раунду, вручившему, получателю или ID.
cmd-rmcdeletecommendations-help = Использование:
  rmcdeletecommendations id <ID награды>
    - Удалить одну награду по ID

  rmcdeletecommendations round <ID раунда> <тип>
    - Удалить все награды этого типа за раунд
    - тип: фильтр по типу награды

  rmcdeletecommendations round <ID раунда> <тип> giver <ник или ID>
    - Удалить награды этого типа за раунд, выданные игроком
    - тип: фильтр по типу награды

  rmcdeletecommendations round <ID раунда> <тип> receiver <ник или ID>
    - Удалить награды этого типа за раунд, полученные игроком
    - тип: фильтр по типу награды

  Примеры:
    rmcdeletecommendations id 128
    rmcdeletecommendations round 42 medal
    rmcdeletecommendations round 42 jelly giver PlayerName
    rmcdeletecommendations round 42 medal receiver PlayerName

cmd-rmcdeletecommendations-invalid-arguments = Неверные аргументы!
cmd-rmcdeletecommendations-invalid-round-id = Неверный ID раунда!
cmd-rmcdeletecommendations-invalid-id = Неверный ID награды!
cmd-rmcdeletecommendations-invalid-type = Неверный тип "{ $type }"!
cmd-rmcdeletecommendations-invalid-player-mode = Неверный режим игрока! Нужно giver или receiver.
cmd-rmcdeletecommendations-player-not-found = Игрок "{ $player }" не найден.
cmd-rmcdeletecommendations-no-results = Наград не найдено.

cmd-rmcdeletecommendations-id-header = Удалена награда { $id }:
cmd-rmcdeletecommendations-round-header = Удалены награды за раунд { $round } (всего { $count }):
cmd-rmcdeletecommendations-format = id [{ $id }] { $type }: { $name }, { $giverUserName } ({ $giver }) → { $receiverUserName } ({ $receiver }), раунд { $round }: { $text }
cmd-rmcdeletecommendations-admin-announcement = { $admin } удаляет награды с ID { $ids }
cmd-rmcdeletecommendations-admin-announcement-round = { $admin } удаляет награды за раунд { $round } с ID { $ids }

cmd-rmcdeletecommendations-hint-mode = Режим (id или round)
cmd-rmcdeletecommendations-hint-mode-id = Удалить награду по ID
cmd-rmcdeletecommendations-hint-mode-round = Удалить награды за раунд
cmd-rmcdeletecommendations-hint-round-id = ID раунда
cmd-rmcdeletecommendations-hint-commendation-id = ID награды
cmd-rmcdeletecommendations-hint-type = Тип награды
cmd-rmcdeletecommendations-hint-player-mode = Режим игрока (giver или receiver)
cmd-rmcdeletecommendations-hint-player-giver = Награды, выданные игроком
cmd-rmcdeletecommendations-hint-player-receiver = Награды, полученные игроком
cmd-rmcdeletecommendations-hint-player = Ник или UserId игрока
