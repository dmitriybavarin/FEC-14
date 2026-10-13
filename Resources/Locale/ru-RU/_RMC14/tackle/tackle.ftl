cm-tackle-try-self = Вы пытаетесь сбить { CASE($target, "acc") } с ног
cm-tackle-try-target = { CAPITALIZE($user) } пытается сбить вас с ног
cm-tackle-try-observer = { CAPITALIZE($user) } пытается сбить { CASE($target, "acc") } с ног

cm-tackle-success-self = Вы сбиваете { CASE($target, "acc") } с ног!
cm-tackle-success-target = { CAPITALIZE($user) } сбивает вас с ног!
cm-tackle-success-observer = { CAPITALIZE($user) } сбивает { CASE($target, "acc") } с ног!

rmc-disarm-shove-others = { CAPITALIZE($performerName) } {$shoveText} { CASE($targetName, "acc") }!
rmc-disarm-shove-target = { CAPITALIZE($performerName) } {$shoveText} вас!
rmc-disarm-shove-self = Вы толкаете { CASE($targetName, "acc") }!

rmc-disarm-text-skilled = сбивает с ног
rmc-disarm-text-1 = толкает
rmc-disarm-text-2 = пихает

rmc-disarm-break-pulls-others = { CAPITALIZE($performerName) } разрывает хватку { CASE($targetName, "gen") }!
rmc-disarm-break-pulls-self = Вы разрываете хватку { CASE($targetName, "gen") }!
rmc-disarm-break-pulls-target = { CAPITALIZE($performerName) } разрывает вашу хватку!

rmc-disarm-attempt-others = { CAPITALIZE($performerName) } пытается обезоружить { CASE($targetName, "acc") }!
rmc-disarm-attempt-self = Вы пытаетесь обезоружить { CASE($targetName, "acc") }!
rmc-disarm-attempt-target = { CAPITALIZE($performerName) } пытается вас обезоружить!

rmc-disarm-success-others = { CAPITALIZE($performerName) } обезоруживает { CASE($targetName, "acc") }!
rmc-disarm-success-self = Вы обезоруживаете { CASE($targetName, "acc") }!
rmc-disarm-success-target = { CAPITALIZE($performerName) } обезоруживает вас!

rmc-disarm-discharge-others = В борьбе { $performerName } случайно стреляет из оружия { CASE($targetName, "gen") }!
rmc-disarm-discharge-self = В борьбе вы случайно стреляете из оружия { CASE($targetName, "gen") }!
rmc-disarm-discharge-target = В борьбе { $performerName } случайно стреляет из вашего оружия!
