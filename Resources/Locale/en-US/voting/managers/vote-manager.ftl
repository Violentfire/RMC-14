# Displayed as initiator of vote when no user creates the vote
ui-vote-initiator-server = Сервер

## Default.Votes

ui-vote-restart-title = Перезапустимо раунд?
ui-vote-restart-succeeded = Голосування за рестарт успішно.
ui-vote-restart-failed = Голосування за рестарт провалено (потрібно { TOSTRING($ratio, "P0") } голосів за).
ui-vote-restart-fail-not-enough-ghost-players = Голосування за рестарт провалено. Треба мінімум { $ghostPlayerRequirement }% гравців на привидах, щоб розпочати це голосуванння.
ui-vote-restart-yes = Так
ui-vote-restart-no = Ні
ui-vote-restart-abstain = Утримаюсь

ui-vote-gamemode-title = Наступний режим:
ui-vote-gamemode-tie = Нічия в голосуванні за режим! Обираю... { $picked }
ui-vote-gamemode-win = { $winner } переміг у голосуванні за режим!

ui-vote-map-title = Наступна мапа:
ui-vote-map-tie = Нічия в голосуванні за мапу! Обираю... { $picked }
ui-vote-map-win = { $winner } перемогла у голосуванні за мапу!
ui-vote-map-notlobby = Голосування за мапу має сенс тільки у лобі!
ui-vote-map-notlobby-time = Голосування за мапу має сенс тільки у лобі, коли залишилось щонайменше { $time } секунд!


# Votekick votes
ui-vote-votekick-unknown-initiator = Гравець
ui-vote-votekick-unknown-target = Невідомий гравець
ui-vote-votekick-title = { $initiator } почав голосування за кік гравця: { $targetEntity }. Причина: { $reason }
ui-vote-votekick-yes = За
ui-vote-votekick-no = Проти
ui-vote-votekick-abstain = Утримаюсь
ui-vote-votekick-success = Голосування за вигнання { $target } успішно. Причина: { $reason }
ui-vote-votekick-failure = Голосування за вигнання { $target } провалено. Причина: { $reason }
ui-vote-votekick-not-enough-eligible = Недостатньо гравців, які можуть прийняти участь в голосуванні: { $voters }/{ $requirement }
ui-vote-votekick-server-cancelled = Голосування за вигнання { $target } скасовано сервером.
