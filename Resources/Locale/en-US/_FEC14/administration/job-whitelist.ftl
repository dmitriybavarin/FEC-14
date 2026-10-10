cmd-jobwhitelistpanel-desc = Opens the job whitelist panel.
cmd-jobwhitelistpanel-help = Usage: jobwhitelistpanel
fec-job-whitelist-ui-button = Job Whitelists
fec-job-whitelist-ui-title = Job Whitelists
fec-job-whitelist-ui-search = Search by player or job
fec-job-whitelist-ui-refresh = Refresh
fec-job-whitelist-ui-group-player = By Player
fec-job-whitelist-ui-group-job = By Job
fec-job-whitelist-ui-summary = Entries: { $entries }, players: { $players }
fec-job-whitelist-ui-empty = No whitelists found.
fec-job-whitelist-ui-player-name = { $player }{ $online ->
    [1] {" "}(online)
   *[other] {""}
}
fec-job-whitelist-ui-player-header = { $player }, jobs: { $count }
fec-job-whitelist-ui-job-header = { $job } ({ $id }), players: { $count }{ $other ->
    [1] {" "}[Other]
   *[other] {""}
}
fec-job-whitelist-ui-job-row = { $job } ({ $id }){ $other ->
    [1] {" "}[Other]
   *[other] {""}
}
fec-job-whitelist-ui-job-option = { $job } ({ $id }){ $whitelisted ->
    [1] {" "}*
   *[other] {""}
}{ $other ->
    [1] {" "}[Other]
   *[other] {""}
}
fec-job-whitelist-ui-remove = Remove
fec-job-whitelist-ui-remove-confirm = Confirm
fec-job-whitelist-ui-add-heading = Grant Whitelist
fec-job-whitelist-ui-player = Player name or user ID
fec-job-whitelist-ui-add = Grant
fec-job-whitelist-ui-other-hint = * the job requires a whitelist. [Other] the job is not shown in the lobby, players will see it in the "Other" category.
fec-job-whitelist-ui-no-player = Enter a player name or user ID.
fec-job-whitelist-ui-already = { $player } already has a whitelist for { $job }.
fec-job-whitelist-ui-added = Granted a whitelist for { $job } to { $player }.
fec-job-whitelist-ui-removed = Removed the whitelist for { $job } from { $player }.
fec-lobby-other-jobs = Other
fec-lobby-other-job-no-preference = This job is not chosen through lobby priorities.
