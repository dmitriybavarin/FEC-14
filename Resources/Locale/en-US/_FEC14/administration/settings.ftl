fec-admin-category-round = Round
fec-admin-category-players = Players
fec-admin-category-server = Server
fec-admin-weather-button = Weather report
fec-history-prompt-button = Player history
fec-history-prompt-title = Player history
fec-history-prompt-open = Open

fec-settings-button = FEC settings
fec-settings-title = FEC settings
fec-settings-hint = Changes apply immediately and last until the server restarts. Permanent values belong in the server config.
fec-settings-apply = Apply
fec-settings-reset = Reset
fec-settings-changed = { $key } changed from { $old } to { $value }
fec-settings-bad-value = Invalid value "{ $value }" for { $key }
fec-settings-tooltip = { $key }, default { $default }{ $range ->
    [1] , from { $min } to { $max }
   *[other] {""}
}

fec-settings-category-events = Events
fec-settings-category-weather = Weather
fec-settings-category-lobby = Lobby

fec-settings-fec-events-enabled = Events enabled
fec-settings-fec-events-chance = Roll chance per slot (0–1)
fec-settings-fec-events-max-per-round = Event slots per round
fec-settings-fec-events-vote-enabled = Ghost vote
fec-settings-fec-events-vote-options = Vote options
fec-settings-fec-events-vote-seconds = Vote duration, s
fec-settings-fec-events-confirm-seconds = Admin confirmation wait, s
fec-settings-fec-events-low-online-warn-minutes = Warn about low online, min before
fec-settings-fec-events-role-cooldown-rounds = Event role cooldown, rounds
fec-settings-fec-weather-clear-round-chance = Clear round chance (0–1)
fec-settings-fec-weather-max-start-chance = Max bad weather start chance (0–1)
fec-settings-fec-weather-min-check-minutes = Min weather check interval, min
fec-settings-fec-weather-min-clear-minutes = Min clear pause between bad weather, min
fec-settings-fec-species-whitelist = Allowed species (comma separated)

cmd-fecsettings-desc = Opens the FEC settings window.
cmd-fecsettings-help = Usage: fecsettings

cmd-fectp-desc = Teleports to a map, grid, entity or player. Understands ids from lsmap and lsgrid.
cmd-fectp-help = Usage: fectp <map:N | grid or entity id | username> [who to teleport]
fec-tp-done = Teleported to { $target }, map { $map }, { $x } { $y }
fec-tp-not-found = "{ $target }" not found. Map is map:N from lsmap, grid is an id from lsgrid, entity is its id, player is a username.
fec-tp-no-player = Player "{ $player }" not found or has no body
fec-tp-hint-target = map:N, grid or entity id, username
fec-tp-hint-player = Who to teleport (you by default)

fec-admin-tab = FEC14
fec-admin-category-fun = Fun
fec-admin-category-settings = Settings
fec-settings-no-access = Changing settings requires the SERVER flag.
fec-settings-category-fun = Fun
fec-settings-fec-fun-enabled = Fun features enabled
fec-settings-fec-fun-death-quote-chance = Epitaph chance on death (0–1)
fec-settings-fec-fun-round-start-quote = ARES tip of the day at round start
fec-tp-button = Teleport
fec-tp-go = Go
fec-tp-prompt-hint = map:N is a map from lsmap, a number is a grid or entity, or a username.
