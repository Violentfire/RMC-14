### Інтерфейс

chat-manager-max-message-length = Ваше повідомлення перевищує {$maxMessageLength} ліміт по символах
chat-manager-ooc-chat-enabled-message = Чат ООС було увімкнено.
chat-manager-ooc-chat-disabled-message = Чат ООС було вимкнено.
chat-manager-looc-chat-enabled-message = Чат ЛООС було увімкнено.
chat-manager-looc-chat-disabled-message = Чат ЛООС було вимкнено.
chat-manager-dead-looc-chat-enabled-message = Мертві гравці тепер можуть використовувати ЛООС.
chat-manager-dead-looc-chat-disabled-message = Мертві гравці більше не можуть використовувати ЛООС.
chat-manager-crit-looc-chat-enabled-message = Персонажи у крит. стані тепер можуть використовувати ЛООС.
chat-manager-crit-looc-chat-disabled-message = Персонажи у крит. стані більше не можуть використовувати ЛООС.
chat-manager-admin-ooc-chat-enabled-message = Чат Адмін ООС було увімкнено.
chat-manager-admin-ooc-chat-disabled-message = Чат Адмін ООС було вимкнено.

chat-manager-max-message-length-exceeded-message = Ваше повідомлення перевищує {$limit} ліміт по символах
chat-manager-no-headset-on-message = Ви не маєте гарнітуру на собі!
chat-manager-no-radio-key = Не вказано радіоключ!
chat-manager-no-such-channel = Не існує каналу з ключем '{$key}'!
chat-manager-whisper-headset-on-message = Ви не можете шепотіти у радіо!

chat-manager-server-wrap-message = [bold]{$message}[/bold]
chat-manager-sender-announcement = Центральне Командування
chat-manager-sender-announcement-wrap-message = [font size=14][bold]{$sender} Оголошення:[/font][font size=12]
                                                {$message}[/bold][/font]
chat-manager-entity-say-wrap-message = [BubbleHeader][bold][Name]{$entityName}[/Name][/bold][/BubbleHeader] {$verb}, [font={$fontType} size={$fontSize}]"[BubbleContent]{$message}[/BubbleContent]"[/font]
chat-manager-entity-say-bold-wrap-message = [BubbleHeader][bold][Name]{$entityName}[/Name][/bold][/BubbleHeader] {$verb}, [font={$fontType} size={$fontSize}]"[BubbleContent][bold]{$message}[/bold][/BubbleContent]"[/font]

chat-manager-entity-whisper-wrap-message = [font size=11][italic][BubbleHeader][Name]{$entityName}[/Name][/BubbleHeader] whispers,"[BubbleContent]{$message}[/BubbleContent]"[/italic][/font]
chat-manager-entity-whisper-unknown-wrap-message = [font size=11][italic][BubbleHeader]Someone[/BubbleHeader] whispers, "[BubbleContent]{$message}[/BubbleContent]"[/italic][/font]

# THE() is not used here because the entity and its name can technically be disconnected if a nameOverride is passed...
chat-manager-entity-me-wrap-message = [italic]{ PROPER($entity) ->
    *[false] The {$entityName} {$message}[/italic]
     [true] {CAPITALIZE($entityName)} {$message}[/italic]
    }

chat-manager-entity-looc-wrap-message = ЛООС: [bold]{$entityName}:[/bold] {$message}
chat-manager-send-ooc-wrap-message = ООС: [bold]{$playerName}:[/bold] {$message}
chat-manager-send-ooc-patron-wrap-message = ООС: [bold][color={$patronColor}]{$playerName}[/color]:[/bold] {$message}

chat-manager-send-dead-chat-wrap-message = {$deadChannelName}: [bold][BubbleHeader]{$playerName}[/BubbleHeader]:[/bold] [BubbleContent]{$message}[/BubbleContent]
chat-manager-send-admin-dead-chat-wrap-message = {$adminChannelName}: [bold]([BubbleHeader]{$userName}[/BubbleHeader]):[/bold] [BubbleContent]{$message}[/BubbleContent]
chat-manager-send-admin-chat-wrap-message = {$adminChannelName}: [bold]{$playerName}:[/bold] {$message}
chat-manager-send-admin-announcement-wrap-message = [bold]{$adminChannelName}: {$message}[/bold]

chat-manager-send-hook-ooc-wrap-message = ООС: [bold](D){$senderName}:[/bold] {$message}
chat-manager-send-hook-admin-wrap-message = АДМІН: [bold](D){$senderName}:[/bold] {$message}

chat-manager-dead-channel-name = МЕРТВІ
chat-manager-admin-channel-name = АДМІН

chat-manager-rate-limited = Ви відправляєте повідомлення занадто швидко!
chat-manager-rate-limit-admin-announcement = Попередження про привищення ліміту повідомлень: { $player }

## Дієслова для чату

chat-speech-verb-suffix-exclamation = !
chat-speech-verb-suffix-exclamation-strong = !!
chat-speech-verb-suffix-question = ?
chat-speech-verb-suffix-stutter = -
chat-speech-verb-suffix-mumble = ..

chat-speech-verb-name-none = Відсутній
chat-speech-verb-name-default = Звичайний
chat-speech-verb-default = каже
chat-speech-verb-name-exclamation = Вигук
chat-speech-verb-exclamation = вигукує
chat-speech-verb-name-exclamation-strong = Крик
chat-speech-verb-exclamation-strong = кричить
chat-speech-verb-name-question = Питання
chat-speech-verb-question = питає
chat-speech-verb-name-stutter = Заїкання
chat-speech-verb-stutter = заїкається
chat-speech-verb-name-mumble = Бурмотіння
chat-speech-verb-mumble = бурмоче

chat-speech-verb-name-arachnid = Арахніди
chat-speech-verb-insect-1 = стрикоче
chat-speech-verb-insect-2 = цвірінькає
chat-speech-verb-insect-3 = клацає

chat-speech-verb-name-moth = Молі
chat-speech-verb-winged-1 = тріпоче
chat-speech-verb-winged-2 = пурхтить
chat-speech-verb-winged-3 = дзижчить

chat-speech-verb-name-slime = Слайми
chat-speech-verb-slime-1 = хлюпає
chat-speech-verb-slime-2 = булькає
chat-speech-verb-slime-3 = сочиться

chat-speech-verb-name-plant = Діони
chat-speech-verb-plant-1 = шелестить
chat-speech-verb-plant-2 = колишеться
chat-speech-verb-plant-3 = скрипить

chat-speech-verb-name-robotic = Роботи
chat-speech-verb-robotic-1 = стверджує
chat-speech-verb-robotic-2 = біпає
chat-speech-verb-robotic-3 = бупає

chat-speech-verb-name-reptilian = Рептилії
chat-speech-verb-reptilian-1 = шипить
chat-speech-verb-reptilian-2 = фиркає
chat-speech-verb-reptilian-3 = гарчить

chat-speech-verb-name-skeleton = Скелети
chat-speech-verb-skeleton-1 = гримить
chat-speech-verb-skeleton-2 = клацає
chat-speech-verb-skeleton-3 = скрегоче

chat-speech-verb-name-vox = Вокси
chat-speech-verb-vox-1 = верещить
chat-speech-verb-vox-2 = волає
chat-speech-verb-vox-3 = хрипить

chat-speech-verb-name-canine = Собаки
chat-speech-verb-canine-1 = гавкає
chat-speech-verb-canine-2 = ричить
chat-speech-verb-canine-3 = виє

chat-speech-verb-name-goat = Кози
chat-speech-verb-goat-1 = бекає
chat-speech-verb-goat-2 = бурчить
chat-speech-verb-goat-3 = реве

chat-speech-verb-name-small-mob = Миші
chat-speech-verb-small-mob-1 = пищить
chat-speech-verb-small-mob-2 = пискає

chat-speech-verb-name-large-mob = Карпи
chat-speech-verb-large-mob-1 = ричить
chat-speech-verb-large-mob-2 = гарчить

chat-speech-verb-name-monkey = Мавпи
chat-speech-verb-monkey-1 = щебече
chat-speech-verb-monkey-2 = верещить

chat-speech-verb-name-cluwne = Клувні

chat-speech-verb-name-parrot = Папуги
chat-speech-verb-parrot-1 = пищить
chat-speech-verb-parrot-2 = цвірінькає
chat-speech-verb-parrot-3 = щебече

chat-speech-verb-cluwne-1 = хіхікає
chat-speech-verb-cluwne-2 = регоче
chat-speech-verb-cluwne-3 = сміється

chat-speech-verb-name-ghost = Привиди
chat-speech-verb-ghost-1 = скаржиться
chat-speech-verb-ghost-2 = дихає
chat-speech-verb-ghost-3 = наспівує
chat-speech-verb-ghost-4 = бурмоче

chat-speech-verb-name-electricity = Електричні
chat-speech-verb-electricity-1 = потріскує
chat-speech-verb-electricity-2 = дзижчить
chat-speech-verb-electricity-3 = верещить

chat-speech-verb-name-wawa = Вава
chat-speech-verb-wawa-1 = інтонує
chat-speech-verb-wawa-2 = стверджує
chat-speech-verb-wawa-3 = проголошує
chat-speech-verb-wawa-4 = розмірковує
