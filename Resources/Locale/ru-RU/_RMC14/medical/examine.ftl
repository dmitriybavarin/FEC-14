rmc-medical-examine-unrevivable = [color=purple][italic]Взгляд остекленел, признаков жизни нет.[/italic][/color]

rmc-medical-examine-headless = [color=purple][italic]Однозначно { GENDER($victim) ->
    [female] мертва
    [epicene] мертвы
   *[other] мертв
  }.[/italic][/color]

rmc-medical-examine-unconscious = [color=lightblue]Похоже, без сознания.[/color]

rmc-medical-examine-dead = [color=red]Не дышит.[/color]

rmc-medical-examine-dead-simple-mob = [color=red]{ GENDER($victim) ->
    [female] МЕРТВА. Откинула
    [epicene] МЕРТВЫ. Откинули
   *[other] МЕРТВ. Откинул
  } копыта.[/color]

rmc-medical-examine-dead-xeno = [color=red]{ GENDER($victim) ->
    [female] МЕРТВА. Откинула копыта. Отправилась
    [epicene] МЕРТВЫ. Откинули копыта. Отправились
   *[other] МЕРТВ. Откинул копыта. Отправился
  } в великий улей на небесах.[/color]

rmc-medical-examine-alive = [color=green]{ GENDER($victim) ->
    [female] Жива и дышит
    [epicene] Живы и дышат
   *[other] Жив и дышит
  }.[/color]

rmc-medical-examine-bleeding = [color=#d10a0a]На теле кровоточащие раны.[/color]

rmc-medical-examine-verb = Медицинские действия
