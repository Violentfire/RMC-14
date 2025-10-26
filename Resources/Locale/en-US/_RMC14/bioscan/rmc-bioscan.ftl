rmc-bioscan-ares-announcement = [color=white][font size=16][bold]АРЕС v3.2 Статус Біоскану[/bold][/font][/color][color=red][font size=14][bold]
    {$message}[/bold][/font][/color]

rmc-bioscan-ares = Біосканування завершено.

  Датчики фіксують { $shipUncontained ->
    [0] відсутність
    *[other] {$shipUncontained}
  } невизначених { $shipUncontained ->
    [0] сигнатур
    [1] сигнатура
    *[other] сигнатур
  } на борту корабля{ $shipLocation ->
    [none] {""}
    *[other], зокрема в {$shipLocation},
  } і { $onPlanet ->
    [0] відсутність
    *[other] приблизно {$onPlanet}
  } { $onPlanet ->
    [0] сигнатур
    [1] сигнатура
    *[other] сигнатур
  } в інших місцях{ $planetLocation ->
    [none].
    *[other], зокрема в {$planetLocation}
  }

rmc-bioscan-xeno-announcement = [color=#318850][font size=14][bold]Королева-мати проникає в ваш розум з далеких світів.
    {$message}[/bold][/font][/color]

rmc-bioscan-xeno = До моїх дітей і їх Королеви: я відчуваю { $onShip ->
  [0] що інкубаторів більше не залишилося
  [1] приблизно одного інкубатора
  *[other] близька {$onShip} інкубаторів
} в металевому вулику{ $shipLocation ->
  [none] {""}
  *[other], зокрема в {$shipLocation},
} і {$onPlanet ->
  [0] більше немає
  *[other] {$onPlanet}
} деінде{$planetLocation ->
  [none].
  *[other], зокрема в {$planetLocation}
}
