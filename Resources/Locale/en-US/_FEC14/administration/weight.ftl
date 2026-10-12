fec-admin-weight-rank = { $rank } (weight { $weight })
fec-admin-weight-value = Weight: { $weight }
fec-admin-weight-label = Weight:
fec-admin-weight-tooltip = Admins with a lower weight cannot ban, punish or edit admins with a higher weight. Requires the ADMINWEIGHT flag to change. You cannot set a weight above your own.
fec-admin-weight-blocked = You cannot do this to an admin with a higher weight than yours.
fec-admin-weight-pardon-blocked = You cannot pardon a ban issued by an admin with a higher weight than yours.
cmd-feckick-desc = Kicks a player from the server, respecting admin weight.
cmd-feckick-help = Usage: feckick <username> [reason]
cmd-feckick-not-found = Player { $player } was not found.
cmd-feckick-reason = Kicked by an admin: { $reason }
cmd-feckick-no-reason = Kicked by an admin.
cmd-feckick-success = Player { $player } was kicked.
cmd-feckick-hint-player = <username>
cmd-feckick-hint-reason = [reason]

fec-audit-rank-added = [Permissions audit] { $admin } created rank { $rank } (flags { $flags })
fec-audit-rank-updated = [Permissions audit] { $admin } updated rank { $rank } (flags { $flags })
fec-audit-rank-removed = [Permissions audit] { $admin } removed rank { $rank }
fec-audit-admin-added = [Permissions audit] { $admin } made { $target } an admin (title { $title }, rank { $rank }, flags { $flags })
fec-audit-admin-updated = [Permissions audit] { $admin } updated admin { $target } (title { $title }, rank { $rank }, flags { $flags })
fec-audit-admin-removed = [Permissions audit] { $admin } removed admin { $target }
fec-audit-weight-set = [Permissions audit] { $admin } set rank { $rank } weight to { $weight }
fec-audit-blocked-rank = [Permissions audit] Blocked attempt by { $admin } to change rank { $rank } without the required flags or weight
fec-audit-blocked-admin = [Permissions audit] Blocked attempt by { $admin } to change admin { $target } without the required flags or weight

fec-history-button = History
fec-history-title = History of { $player }
fec-history-loading = Loading history…
fec-history-load-error = Failed to load part of the history, see server logs.
fec-history-empty = No records.
fec-history-filter-all = All records
fec-history-kind-ban = Ban
fec-history-kind-roleban = Role ban
fec-history-kind-chatban = Chat ban
fec-history-kind-note = Note
fec-history-kind-watchlist = Watchlist
fec-history-kind-message = Message
fec-history-kind-whitelist = Whitelist
fec-history-system = system
fec-history-entry = { $admin } { $text }
fec-history-entry-extra = { $admin } { $text } ({ $extra })
fec-history-until = until { $date }
fec-history-permanent = permanent
fec-history-pardoned = pardoned
fec-history-whitelist = Job { $job }{ $temp ->
        [1] , temporary until { $date }
       *[other] , permanent
    }
fec-history-summary-online = { $online ->
        [1] Online now
       *[other] Offline
    }
fec-history-summary-playtime = Playtime { $hours } h { $minutes } min
fec-history-summary-whitelist = Server whitelist { $whitelisted ->
        [1] yes
       *[other] no
    }
fec-history-summary-seen = First seen { $first }, last seen { $last }
cmd-playerhistory-desc = Opens the unified player history window.
cmd-playerhistory-help = Usage: playerhistory <username or ID>
