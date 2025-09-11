cm-gun-unskilled = Ви не знаєте як використовувати {THE($gun)}
cm-gun-no-ammo-message = Скінчилися набої!
cm-gun-use-delay = Зачекайте {$seconds} секунд перед тим як знову вистрілити!
cm-gun-pump-examine = [bold]Натисніть вашу [color=cyan]унікальну[/color] клавішу (Spacebar за замовчуванням) щоб прокачати перед пострілом.[/bold]
cm-gun-pump-first-with = Вам спершу потрібно прокачати зброю за допомогою {$key}!
cm-gun-pump-first = Спочатку потрібно прокачати зброю!

rmc-breech-loaded-open-shoot-attempt = Спершу потрібно закрити затвор!
rmc-breech-loaded-not-ready-to-shoot = Спершу треба передьорнуть затвор!
rmc-breech-loaded-closed-load-attempt = Спершу треба відкрити затвор!
rmc-breech-loaded-closed-extract-attempt = You need to open the breech first!

rmc-wield-use-delay = Спочатку зачекайте {$seconds} секунд перш ніж використовувати {THE($wieldable)}!
rmc-shoot-use-delay = Спочатку зачекайте {$seconds} секунд перед тим як стріляти з {THE($wieldable)}!

rmc-shoot-harness-required = Необхідне спорядження
rmc-wear-smart-gun-required = Вам треба мати Смартган, щоб носити їх.

rmc-shoot-id-lock-unauthorized = Гачок заблоковано. НЕАВТОРИЗОВАНИЙ КОРИСТУВАЧ.
rmc-id-lock-unauthorized = ВІДХИЛЕНО. НЕАВТОРИЗОВАНИЙ КОРИСТУВАЧ.
rmc-id-lock-authorization = Ви взяли {$gun}, авторизуючись як власник.
rmc-id-lock-authorization-combat = {$gun} пікає, авторизуючи вас як власника.
rmc-id-lock-toggle-lock = Ви {$action} індентифікаціний замок на {$gun}.

rmc-id-lock-color-unauthorized = червоним
rmc-id-lock-color-authorized = шартрезовим
rmc-id-lock-toggle-on = заблокували
rmc-id-lock-toggle-off = розблокували

rmc-iff-toggle = Ви {$action} УДВ для {$gun}.
rmc-iff-toggle-off = вимкнули
rmc-iff-toggle-on = увімкнули

rmc-revolver-spin = Ви крутите барабан.

rmc-examine-text-weapon-accuracy = The current accuracy multiplier is [color={$colour}]{TOSTRING($accuracy, "F2")}[/color].

rmc-examine-text-scatter-max = Поточний максимальний розкид [color={$colour}]{TOSTRING($scatter, "F1")}[/color] градусів.
rmc-examine-text-scatter-min = Поточний мінімальний розкид [color={$colour}]{TOSTRING($scatter, "F1")}[/color] градусів.
rmc-examine-text-shots-to-max-scatter = Треба ще [color={$colour}]{$shots}[/color] пострілів до максимального розкиду.
rmc-examine-text-iff = [color=cyan]Ця зброя стріляє повз союзників, ігноруючи їх![/color]
rmc-examine-text-id-lock-no-user = [color=chartreuse]It's unregistered. Pick it up to register yourself as its owner.[/color]
rmc-examine-text-id-lock = [color=chartreuse]It is registered to [/color][color={$color}]{$name}[/color][color=chartreuse].[/color]
rmc-examine-text-id-lock-unlocked = [color=chartreuse]It is registered to [/color][color={$color}]{$name}[/color][color=chartreuse], but has its fire restrictions unlocked.[/color]

rmc-gun-rack-examine = [bold]Натисніть свою [color=cyan]унікальну[/color] клавішу (Пробіл за замовченням) щоб поставити перед стрільбою.[/bold]
rmc-gun-rack-first-with = Вам спочатку потрібно поставити зброю за допомогою {$key}!
rmc-gun-rack-first = Спочатку треба поставити зброю!

rmc-assisted-reload-fail-angle = You must be standing behind {$target} in order to reload {POSS-ADJ($target)} weapon!
rmc-assisted-reload-fail-full = {CAPITALIZE(POSS-ADJ($target))} {$weapon} is already loaded.
rmc-assisted-reload-fail-mismatch = The {$ammo} can't be loaded into a {$weapon}!
rmc-assisted-reload-start-user = You begin reloading {$target}'s {$weapon}! Hold still...
rmc-assisted-reload-start-target = {$reloader} begins reloading your {$weapon} with the {$ammo}! Hold still...

rmc-gun-stacks-hit-single = Вцілив!
rmc-gun-stacks-hit-multiple = Вцілив! {$hits} влучань поспіль!
rmc-gun-stacks-reset = {$weapon} втрачає дані про ціль, і перемикається на стандарний режим стрільби.

rmc-gun-shoot-air-self = YOU FIRE YOUR { CAPITALIZE($weapon) } INTO THE AIR!
rmc-gun-shoot-air-other = { CAPITALIZE(THE($user)) } FIRES { CAPITALIZE(THE($weapon)) } INTO THE AIR!
rmc-gun-shoot-air-blocked = The roof above you is too dense.
rmc-gun-shoot-air-examine = [bold]Press your [color=cyan]unique action[/color] keybind (Spacebar by default){$harm ->
    [true] {" while in harm mode"}
    *[false] {""}
    } to fire into the air.[/bold]

rmc-flare-gun-examine = The last signal flare fired has the designation: [color=#ad3b98][bold]{$id}[/bold][/color]

expendable-light-starshell-ash-empty-name = extinguished star shell ash
expendable-light-starshell-ash-empty-desc = Burnt out remains of a star shell
