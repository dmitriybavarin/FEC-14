rmc-bioscan-ares-announcement = [color=white][font size=16][bold]ARES v3.2: Результаты биосканирования[/bold][/font][/color][color=red][font size=14][bold]
    {$message}[/bold][/font][/color]

rmc-bioscan-ares = Биосканирование завершено.

  На корабле { $shipUncontained ->
    [0] не обнаружено неизвестных форм жизни
    [one] обнаружена {$shipUncontained} неизвестная форма жизни
    [few] обнаружены {$shipUncontained} неизвестные формы жизни
    *[other] обнаружено {$shipUncontained} неизвестных форм жизни
  }{ $shipLocation ->
    [none] {""}
    *[other], в том числе одна в зоне "{$shipLocation}"
  }, а в других местах { $onPlanet ->
    [0] ни одной
    *[other] около {$onPlanet}
  }{ $planetLocation ->
    [none].
    *[other], в том числе одна в зоне "{$planetLocation}".
  }

rmc-bioscan-xeno-announcement = [color=#318850][font size=14][bold]Королева-Мать тянется к вашему разуму из далеких миров.
  {$message}[/bold][/font][/color]

rmc-bioscan-xeno = Моим детям и их Королеве. В металлическом улье я { $onShip ->
  [0] не чую ни одного носителя
  [one] чую около {$onShip} носителя
  *[other] чую около {$onShip} носителей
}{ $shipLocation ->
  [none] {""}
  *[other], один из них в зоне "{$shipLocation}"
}, а в других местах {$onPlanet ->
  [0] ни одного
  [one] около {$onPlanet} носителя
  *[other] около {$onPlanet} носителей
}{$planetLocation ->
  [none].
  *[other], один из них в зоне "{$planetLocation}".
}
