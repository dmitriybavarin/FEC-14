chat-manager-max-message-length = Сообщение длиннее {$maxMessageLength} символов
chat-manager-ooc-chat-enabled-message = OOC-чат включен.
chat-manager-ooc-chat-disabled-message = OOC-чат выключен.
chat-manager-looc-chat-enabled-message = LOOC-чат включен.
chat-manager-looc-chat-disabled-message = LOOC-чат выключен.
chat-manager-dead-looc-chat-enabled-message = Мертвые игроки теперь могут писать в LOOC.
chat-manager-dead-looc-chat-disabled-message = Мертвые игроки больше не могут писать в LOOC.
chat-manager-crit-looc-chat-enabled-message = Игроки в критическом состоянии теперь могут писать в LOOC.
chat-manager-crit-looc-chat-disabled-message = Игроки в критическом состоянии больше не могут писать в LOOC.
chat-manager-admin-ooc-chat-enabled-message = Админский OOC-чат включен.
chat-manager-admin-ooc-chat-disabled-message = Админский OOC-чат выключен.

chat-manager-max-message-length-exceeded-message = Сообщение длиннее {$limit} символов
chat-manager-no-headset-on-message = На вас нет гарнитуры!
chat-manager-no-radio-key = Не указан ключ канала!
chat-manager-no-such-channel = Канала с ключом "{$key}" нет!
chat-manager-whisper-headset-on-message = Шептать в рацию нельзя!

chat-manager-server-wrap-message = [bold]{$message}[/bold]
chat-manager-sender-announcement = Центральное командование
chat-manager-sender-announcement-wrap-message = [font size=14][bold]Объявляет {$sender}[/font][font size=12]
                                                {$message}[/bold][/font]
chat-manager-entity-say-wrap-message = [BubbleHeader][bold][Name]{$entityName}[/Name][/bold][/BubbleHeader] {$verb}: [font={$fontType} size={$fontSize}]"[BubbleContent]{$message}[/BubbleContent]"[/font]
chat-manager-entity-say-bold-wrap-message = [BubbleHeader][bold][Name]{$entityName}[/Name][/bold][/BubbleHeader] {$verb}: [font={$fontType} size={$fontSize}]"[BubbleContent][bold]{$message}[/bold][/BubbleContent]"[/font]

chat-manager-entity-whisper-wrap-message = [font size=11][italic][BubbleHeader][Name]{$entityName}[/Name][/BubbleHeader] шепчет: "[BubbleContent]{$message}[/BubbleContent]"[/italic][/font]
chat-manager-entity-whisper-unknown-wrap-message = [font size=11][italic][BubbleHeader]Кто-то[/BubbleHeader] шепчет: "[BubbleContent]{$message}[/BubbleContent]"[/italic][/font]

chat-manager-entity-me-wrap-message = [italic]{ PROPER($entity) ->
    *[false] {CAPITALIZE($entityName)} {$message}[/italic]
     [true] {CAPITALIZE($entityName)} {$message}[/italic]
    }

chat-manager-entity-looc-wrap-message = LOOC: [bold]{$entityName}:[/bold] {$message}
chat-manager-send-ooc-wrap-message = OOC: [bold]{$playerName}:[/bold] {$message}
chat-manager-send-ooc-patron-wrap-message = OOC: [bold][color={$patronColor}]{$playerName}[/color]:[/bold] {$message}

chat-manager-send-dead-chat-wrap-message = {$deadChannelName}: [bold][BubbleHeader]{$playerName}[/BubbleHeader]:[/bold] [BubbleContent]{$message}[/BubbleContent]
chat-manager-send-admin-dead-chat-wrap-message = {$adminChannelName}: [bold]([BubbleHeader]{$userName}[/BubbleHeader]):[/bold] [BubbleContent]{$message}[/BubbleContent]
chat-manager-send-admin-chat-wrap-message = {$adminChannelName}: [bold]{$playerName}:[/bold] {$message}
chat-manager-send-admin-announcement-wrap-message = [bold]{$adminChannelName}: {$message}[/bold]

chat-manager-send-hook-ooc-wrap-message = OOC: [bold](D){$senderName}:[/bold] {$message}
chat-manager-send-hook-admin-wrap-message = АДМИН: [bold](D){$senderName}:[/bold] {$message}

chat-manager-dead-channel-name = МЕРТВЫЕ
chat-manager-admin-channel-name = АДМИН

chat-manager-rate-limited = Вы отправляете сообщения слишком часто!
chat-manager-rate-limit-admin-announcement = Превышен лимит сообщений: { $player }

chat-speech-verb-suffix-exclamation = !
chat-speech-verb-suffix-exclamation-strong = !!
chat-speech-verb-suffix-question = ?
chat-speech-verb-suffix-stutter = -
chat-speech-verb-suffix-mumble = ..

chat-speech-verb-name-none = Нет
chat-speech-verb-name-default = По умолчанию
chat-speech-verb-default = говорит
chat-speech-verb-name-exclamation = Восклицание
chat-speech-verb-exclamation = восклицает
chat-speech-verb-name-exclamation-strong = Крик
chat-speech-verb-exclamation-strong = кричит
chat-speech-verb-name-question = Вопрос
chat-speech-verb-question = спрашивает
chat-speech-verb-name-stutter = Заикание
chat-speech-verb-stutter = запинается
chat-speech-verb-name-mumble = Бормотание
chat-speech-verb-mumble = бормочет

chat-speech-verb-name-arachnid = Арахнид
chat-speech-verb-insect-1 = стрекочет
chat-speech-verb-insect-2 = щебечет
chat-speech-verb-insect-3 = щелкает

chat-speech-verb-name-moth = Ниан
chat-speech-verb-winged-1 = порхает
chat-speech-verb-winged-2 = хлопает крыльями
chat-speech-verb-winged-3 = жужжит

chat-speech-verb-name-slime = Слаймолюд
chat-speech-verb-slime-1 = хлюпает
chat-speech-verb-slime-2 = булькает
chat-speech-verb-slime-3 = сочится

chat-speech-verb-name-plant = Диона
chat-speech-verb-plant-1 = шелестит
chat-speech-verb-plant-2 = покачивается
chat-speech-verb-plant-3 = скрипит

chat-speech-verb-name-robotic = Робот
chat-speech-verb-robotic-1 = сообщает
chat-speech-verb-robotic-2 = пищит
chat-speech-verb-robotic-3 = бипает

chat-speech-verb-name-reptilian = Унатх
chat-speech-verb-reptilian-1 = шипит
chat-speech-verb-reptilian-2 = фыркает
chat-speech-verb-reptilian-3 = пыхтит

chat-speech-verb-name-skeleton = Скелет
chat-speech-verb-skeleton-1 = гремит костями
chat-speech-verb-skeleton-2 = клацает
chat-speech-verb-skeleton-3 = скрежещет зубами

chat-speech-verb-name-vox = Вокс
chat-speech-verb-vox-1 = скрипит
chat-speech-verb-vox-2 = визжит
chat-speech-verb-vox-3 = каркает

chat-speech-verb-name-canine = Собака
chat-speech-verb-canine-1 = лает
chat-speech-verb-canine-2 = гавкает
chat-speech-verb-canine-3 = воет

chat-speech-verb-name-goat = Коза
chat-speech-verb-goat-1 = блеет
chat-speech-verb-goat-2 = хрюкает
chat-speech-verb-goat-3 = кричит

chat-speech-verb-name-small-mob = Мышь
chat-speech-verb-small-mob-1 = пищит
chat-speech-verb-small-mob-2 = попискивает

chat-speech-verb-name-large-mob = Карп
chat-speech-verb-large-mob-1 = ревет
chat-speech-verb-large-mob-2 = рычит

chat-speech-verb-name-monkey = Обезьяна
chat-speech-verb-monkey-1 = верещит
chat-speech-verb-monkey-2 = визжит

chat-speech-verb-name-cluwne = Клувн

chat-speech-verb-name-parrot = Попугай
chat-speech-verb-parrot-1 = кричит
chat-speech-verb-parrot-2 = чирикает
chat-speech-verb-parrot-3 = щебечет

chat-speech-verb-cluwne-1 = хихикает
chat-speech-verb-cluwne-2 = гогочет
chat-speech-verb-cluwne-3 = смеется

chat-speech-verb-name-ghost = Призрак
chat-speech-verb-ghost-1 = жалуется
chat-speech-verb-ghost-2 = выдыхает
chat-speech-verb-ghost-3 = напевает
chat-speech-verb-ghost-4 = бормочет

chat-speech-verb-name-electricity = Электричество
chat-speech-verb-electricity-1 = трещит
chat-speech-verb-electricity-2 = жужжит
chat-speech-verb-electricity-3 = визжит

chat-speech-verb-name-wawa = Вава
chat-speech-verb-wawa-1 = нараспев произносит
chat-speech-verb-wawa-2 = сообщает
chat-speech-verb-wawa-3 = заявляет
chat-speech-verb-wawa-4 = размышляет
