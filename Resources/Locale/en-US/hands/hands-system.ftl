# Examine text after when they're holding something (in-hand)
comp-hands-examine = { CAPITALIZE(SUBJECT($user)) } { CONJUGATE-BE($user) } тримає { $items }.
comp-hands-examine-empty = { CAPITALIZE(SUBJECT($user)) } { CONJUGATE-BE($user) } нічого не тримає в руках.
comp-hands-examine-wrapper = { INDEFINITE($item) } [color=paleturquoise]{$item}[/color]

hands-system-blocked-by = Заковано
