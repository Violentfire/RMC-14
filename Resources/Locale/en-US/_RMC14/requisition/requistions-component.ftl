# Комп'ютер Постачань
requisition-paperwork-receiver-name = Логістичний Відділ
requisition-paperwork-reward-message = Пітвердження отримано! переведення ${$amount} з профіциту бюджету

# Папіпці Постачань
requisition-paper-print-name = {$name} накладна
requisition-paper-print-manifest = [head=2]
    {$containerName}[/head][bold]{$content}[/bold][head=2]
    ВАГА: {$weight} ФУНТІВ
    ЛОТ: {$lot}
    С/Н: №{$serialNumber}[/head]
requisition-paper-print-content = - {$count} {$item}

# Консоль Відправлення Ящика
ui-supply-drop-consle-name = Консоль Відправлення Ящика
ui-supply-drop-console-name-bolded = [bold]СКИДАННЯ ПОСТАВОК[/bold] 
ui-supply-drop-console-longitude = Широта:
ui-supply-drop-console-latitude = Довгота:
ui-supply-drop-pad-status = [bold]Статус Платформи Постачань[/bold]
ui-supply-drop-console-update = Оновити
ui-supply-drop-console-ready = Готово до запуску!
ui-supply-drop-console-launch = ЗАПУСТИТИ ЯЩИК ПОСТАВОК
ui-supply-drop-console-cooldown = {$time} секунд до наступного відправлення
ui-supply-drop-crate-status =
    { $hasCrate ->
        [true] Ящик ЗАВАНТАЖЕНО.
       *[false] Ящик НЕ ЗАВАНТАЖЕНО.
    }