cmd-whitelistadd-desc = Добавляет игрока с указанным именем в вайтлист сервера.
cmd-whitelistadd-help = Использование: whitelistadd <имя пользователя или User ID>
cmd-whitelistadd-existing = { $username } уже в вайтлисте!
cmd-whitelistadd-added = { $username } добавлен в вайтлист
cmd-whitelistadd-not-found = Не удалось найти "{ $username }"
cmd-whitelistadd-arg-player = [игрок]

cmd-whitelistremove-desc = Удаляет игрока с указанным именем из вайтлиста сервера.
cmd-whitelistremove-help = Использование: whitelistremove <имя пользователя или User ID>
cmd-whitelistremove-existing = { $username } нет в вайтлисте!
cmd-whitelistremove-removed = { $username } удален из вайтлиста
cmd-whitelistremove-not-found = Не удалось найти "{ $username }"
cmd-whitelistremove-arg-player = [игрок]

cmd-kicknonwhitelisted-desc = Кикает с сервера всех игроков не из вайтлиста.
cmd-kicknonwhitelisted-help = Использование: kicknonwhitelisted

ban-banned-permanent = Этот бан снимается только через апелляцию.
ban-banned-permanent-appeal = Этот бан снимается только через апелляцию. Подать ее можно здесь: { $link }
ban-expires = Бан выдан на { $duration } мин. и истечет { $time } UTC.
ban-banned-1 = Вам или другому пользователю этого компьютера или подключения запрещено играть на этом сервере.
ban-banned-2 = Причина бана: "{ $reason }"
ban-banned-3 = Попытки обойти бан, например через новый аккаунт, фиксируются.

soft-player-cap-full = Сервер заполнен!
panic-bunker-account-denied = На сервере включен режим "бункер". Обычно его включают для защиты от рейдов. Пока что не пускаем аккаунты, которые не подходят под определенные требования. Попробуйте позже.
panic-bunker-account-denied-reason = На сервере включен режим "бункер". Обычно его включают для защиты от рейдов. Пока что не пускаем аккаунты, которые не подходят под определенные требования. Попробуйте позже. Причина: "{ $reason }"
panic-bunker-account-reason-account = Ваш аккаунт Space Station 14 слишком новый. Он должен существовать дольше { $minutes } мин.
panic-bunker-account-reason-overall = Ваше общее игровое время на сервере должно превышать { $minutes } мин.

whitelist-playtime = У вас недостаточно игрового времени для входа на этот сервер. Нужно наиграть не менее { $minutes } мин.
whitelist-player-count = Сейчас сервер не принимает игроков. Попробуйте позже.
whitelist-notes = У вас слишком много админских заметок, чтобы зайти на этот сервер. Посмотреть их можно командой /adminremarks в чате.
whitelist-manual = Вас нет в вайтлисте этого сервера.
whitelist-blacklisted = Вы в черном списке этого сервера.
whitelist-always-deny = Вам запрещено заходить на этот сервер.
whitelist-fail-prefix = Нет в вайтлисте: { $msg }

cmd-blacklistadd-desc = Добавляет игрока с указанным именем в черный список сервера.
cmd-blacklistadd-help = Использование: blacklistadd <имя пользователя>
cmd-blacklistadd-existing = { $username } уже в черном списке!
cmd-blacklistadd-added = { $username } добавлен в черный список
cmd-blacklistadd-not-found = Не удалось найти "{ $username }"
cmd-blacklistadd-arg-player = [игрок]

cmd-blacklistremove-desc = Удаляет игрока с указанным именем из черного списка сервера.
cmd-blacklistremove-help = Использование: blacklistremove <имя пользователя>
cmd-blacklistremove-existing = { $username } нет в черном списке!
cmd-blacklistremove-removed = { $username } удален из черного списка
cmd-blacklistremove-not-found = Не удалось найти "{ $username }"
cmd-blacklistremove-arg-player = [игрок]

baby-jail-account-denied = Это сервер для новичков и тех, кто хочет им помогать. Слишком старые аккаунты и аккаунты не из вайтлиста сюда не пускаем. Загляните на другие серверы, в Space Station 14 есть что посмотреть. Удачной игры!
baby-jail-account-denied-reason = Это сервер для новичков и тех, кто хочет им помогать. Слишком старые аккаунты и аккаунты не из вайтлиста сюда не пускаем. Загляните на другие серверы, в Space Station 14 есть что посмотреть. Удачной игры! Причина: "{ $reason }"
baby-jail-account-reason-account = Ваш аккаунт Space Station 14 слишком старый. Ему должно быть меньше { $minutes } мин.
baby-jail-account-reason-overall = Ваше общее игровое время на сервере должно быть меньше { $minutes } мин.

generic-misconfigured = Сервер настроен неправильно и не принимает игроков. Свяжитесь с владельцем сервера и попробуйте позже.

ipintel-server-ratelimited = Вы не забанены. Игра проверяет новые подключения через внешний сервис, и сейчас он исчерпал лимит проверок. Подождите минуту-другую и подключитесь снова. Апелляция не нужна. Если не поможет, зайдите в другой день или создайте тикет.
ipintel-unknown = Сервер проверяет подключения через внешнюю систему безопасности, но в ней произошла ошибка. Обратитесь к администрации сервера и попробуйте позже.
ipintel-suspicious = Вы подключаетесь через дата-центр или VPN. Это не бан аккаунта, достаточно отключить VPN. Если проблема остается или без VPN вы играть не можете, запросите исключение на appeal.rouny-ss14.com

hwid-required = Ваш клиент отказался передавать идентификатор оборудования. Обратитесь к администрации за помощью.
