discord-watchlist-connection-header =
    { $players ->
        [one] К серверу {$serverName} подключился {$players} игрок из списка наблюдения
        [few] К серверу {$serverName} подключились {$players} игрока из списка наблюдения
        *[other] К серверу {$serverName} подключились {$players} игроков из списка наблюдения
    }
discord-watchlist-connection-entry = - {$playerName}, сообщение "{$message}"{ $expiry ->
        [0] {""}
        *[other] {" "}(истекает <t:{$expiry}:R>)
    }{ $otherWatchlists ->
        [0] {""}
        [one] {" "}и еще {$otherWatchlists} список наблюдения
        [few] {" "}и еще {$otherWatchlists} списка наблюдения
        *[other] {" "}и еще {$otherWatchlists} списков наблюдения
    }
