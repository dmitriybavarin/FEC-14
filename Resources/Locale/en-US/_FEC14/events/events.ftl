fec-profile-events-tab = Events

fec-events-unknown = Event { $id } not found
fec-events-disabled = Events are disabled by server config
fec-events-blocked = Event { $id } is blocked
fec-events-not-enough-players = Not enough players, need { $min }, currently { $current }
fec-events-no-planet = There is no planet this round
fec-events-rule-failed = Failed to start the event rule
fec-events-summary-started = Event "{ $name }" happened
fec-events-status-started = already started
fec-events-status-blocked = blocked forever
fec-events-status-blocked-until = blocked until { $date }
fec-events-status-planned = planned at minute { $minutes }
fec-events-status-available = available
fec-events-list-entry = { $id } — { $name } ({ $players }+ players) — { $status }
fec-events-none = No events
fec-events-blocked-done = Event { $id } blocked
fec-events-unblocked = Event { $id } unblocked
fec-events-not-blocked = Event { $id } was not blocked
fec-events-started = Event { $id } started
fec-events-hint-id = Event ID
fec-events-hint-duration = Block duration (30m, 12h, 7d) or forever

cmd-fecevents-desc = Manages round events.
cmd-fecevents-help = Usage: fecevents list | block <ID> [duration|forever] | unblock <ID> | start <ID> | rotation

fec-event-antag-marshal-name = Surviving CMB Marshal
fec-event-antag-marshal-objective = You may become a Colonial Marshal Bureau marshal stranded on the colony. Survive until evacuation, ARES is watching over you.

fec-event-marshal-name = Surviving CMB Marshal
fec-event-marshal-desc = A marshal is stranded on the colony and expected aboard the Almayer. If they die or are captured, High Command covertly sends MARSOC.
fec-event-patrol-name = Colonial Marshal patrol
fec-event-patrol-desc = A Colonial Marshal Bureau patrol lands on the colony.
fec-event-storm-name = Anomalous storm
fec-event-storm-desc = Bad weather suddenly hits the planet before forecasters can warn anyone.

fec-marshal-brief = You are a Colonial Marshal Bureau marshal cut off on the colony. The USCM ship received your beacon. Survive and reach the Almayer, ARES checks on you every hour.
fec-marshal-ares-beacon = Colonial Marshal Bureau distress beacon received. Marshal { $name } is on the surface. Evacuation to the ship is a priority. The marshal's status will be checked every hour.
fec-marshal-ares-alive = Marshal { $name }'s beacon is active, vitals nominal. Next check in { $minutes } min.
fec-marshal-ares-evacuated = Marshal { $name } is aboard the ship. Report sent to High Command.
fec-marshal-hc-thanks = High Command confirms the marshal's evacuation. Good work, marines.
fec-marshal-status-missing = beacon not responding
fec-marshal-status-dead = marshal is dead
fec-marshal-status-detained = marshal captured
fec-marshal-ares-report = Marshal { $name }'s beacon: { $status }. An automatic report has been sent to General Staff.
fec-marshal-hc-reply = General Staff received the ARES report. A team is dispatched to verify it. The crew is to cooperate fully.
fec-marshal-ares-unknown-vessel = Warning. An unidentified vessel without IFF transponder is approaching the ship. Air defense is acquiring the target.
fec-marshal-ares-vessel-lost = The unidentified vessel has left radar range.
fec-marshal-ares-air-defense-failed = Air defense failure. The unidentified vessel is ramming the Almayer's hull. Damage control teams to the impact site.
fec-marshal-ares-boarding = Hull breached. Armed boarding party detected aboard. MARSOC insignia identified, the personal unit of High Command.
fec-marshal-punitive-brief = You are a MARSOC raider, the personal unit of USCM High Command. You are the best of the best and answer only to High Command. The Almayer's crew failed to protect a Colonial Marshal, and your orders are clear: purge the ship. No compromises, only results.
fec-marshal-ares-squad-destroyed = The armed boarding party has been eliminated.
fec-marshal-hc-squad-destroyed = High Command has lost contact with the MARSOC team. The incident will be investigated.
fec-marshal-end-punished-xenos = The Almayer's crew failed its task and was punished by MARSOC, but USCM forces on the planet kept fighting and defeated the xenomorphs.
fec-marshal-end-punished = The Almayer's crew failed its task and was punished by MARSOC.
fec-marshal-summary-saved = Marshal { $name } was evacuated to the ship
fec-marshal-summary-lost = Marshal { $name } was lost, { $status }
fec-marshal-summary-no-squad = The MARSOC team never made contact
fec-marshal-summary-repelled = The crew repelled the MARSOC assault
fec-marshal-summary-punished = MARSOC purged the Almayer

fec-patrol-ares = A Colonial Marshal Bureau patrol shuttle has landed on the surface. The marshals request contact with command.
fec-patrol-summary = A Colonial Marshal Bureau patrol landed on the colony

fec-storm-hc-warning = Weather satellites detect an anomalous front over the area of operations. Conditions are worsening fast, be ready.
fec-storm-summary = An anomalous storm swept over the colony

fec-events-status-rotation = already used this rotation
fec-events-rotation-reset = Event rotation reset, all events are available again

fec-events-vote-title = Which event will happen this round?
fec-events-vote-initiator = Event system

fec-events-log-slot = Event roll passed, slot at minute { $minutes }
fec-events-log-no-roll = Event roll failed, no events this round
fec-events-log-no-candidates = No suitable events, slot skipped
fec-events-log-vote = Ghost vote: { $names }
fec-events-log-vote-result = Ghosts picked "{ $name }"
fec-events-log-vote-cancelled = Vote cancelled, picked "{ $name }"
fec-events-log-skipped = "{ $name }" not started, { $reason }
fec-events-log-started = Started "{ $name }"
fec-events-log-started-admin = Admin started "{ $name }"
fec-events-log-started-chain = Chain started "{ $name }"
fec-events-log-outcome = "{ $name }" outcome, { $outcome }
fec-events-log-chain = Chain, "{ $name }" at minute { $minutes }
fec-events-log-chain-blocked = Chain stopped, "{ $name }" is blocked
fec-events-log-chain-skipped = Chain, "{ $name }" not started, { $reason }
fec-events-log-blocked = { $admin } blocked "{ $name }"
fec-events-log-unblocked = { $admin } unblocked "{ $name }"
fec-events-log-rotation-reset = { $admin } reset the event rotation
fec-events-log-rotation-done = All rotation events happened, rotation restarted

fec-events-outcome-failed = failed to start
fec-events-outcome-saved = marshal saved
fec-events-outcome-lost = marshal lost
fec-events-outcome-no-squad = team never made contact
fec-events-outcome-repelled = crew repelled the assault
fec-events-outcome-punished = crew punished
fec-events-outcome-landed = patrol landed
fec-events-outcome-started = storm started

fec-events-ui-title = Round events
fec-events-ui-info = { $players } players. { $slotCount ->
    [0] No slots
   *[other] Slots at minutes { $slots }
}. { $vote ->
    [1] Vote running
   *[other] No vote
}
fec-events-ui-duration = Block duration
fec-events-ui-reset-rotation = Reset rotation
fec-events-ui-log = Round log
fec-events-ui-log-empty = Log is empty
fec-events-ui-log-line = { $time }  { $text }
fec-events-ui-row = { $name } ({ $id }), { $players }+ players, weight { $weight }, { $status }
fec-events-ui-start = Start
fec-events-ui-start-confirm = Sure?
fec-events-ui-block = Block
fec-events-ui-unblock = Unblock

cmd-feceventspanel-desc = Opens the round events window.
cmd-feceventspanel-help = Usage: feceventspanel
