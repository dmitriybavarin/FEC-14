# Как сделать событие без кода

Событие состоит из двух прототипов в любом `.yml` внутри `Resources/Prototypes/_FEC14/Events/`: правила (что происходит) и записи `fecEvent` (когда и как часто).

## 1. Правило: высадка группы

```yaml
- type: entity
  parent: BaseGameRule
  id: FECMyEventRule
  components:
  - type: FECTeamDropRule
    location: Planet          # Planet — точка спавна на планете, Ship — на «Алмейере»
    scatter: 1.5              # разброс вокруг точки, в тайлах
    spawners:                 # спавнеры ролей призраков или любые сущности; повтор = ещё одна копия
    - RMCRandomHumanoidFreelancerStandardHostile
    - RMCRandomHumanoidFreelancerStandardHostile
    aresAnnouncement: fec-my-event-ares        # объявление АРЕС (необязательно)
    highCommandAnnouncement: fec-my-event-hc   # объявление Верховного командования (необязательно)
    xenoAnnouncement: fec-my-event-xeno        # сообщение Королевы-Матери всем ксено (необязательно)
    summary: fec-my-event-summary              # строка в итогах раунда (необязательно)
    explosionType: Default                     # взрыв в точке высадки (необязательно)
    explosionIntensity: 100
    outcome: landed                            # исход для цепочек
```

## 2. Запись события

```yaml
- type: fecEvent
  id: MyEvent
  name: fec-event-my-name
  description: fec-event-my-desc
  rule: FECMyEventRule
  minPlayers: 30          # меньше игроков — событие не выпадет
  weight: 1               # вес в ролле и голосовании, 0 — только вручную или по цепочке
  earliestStart: 1200     # окно запуска в секундах от начала раунда
  latestStart: 5400
  requiresPlanet: true    # false — событие возможно и без планеты
  inRotation: true        # false — только вручную или по цепочке
  chains:                 # что запустить после исхода этого события
  - outcome: landed
    event: CMBPatrol
    delay: 600
    chance: 0.5
```

## 3. Локализация

Все ключи (`name`, `description`, объявления, `summary`) добавить сразу в
`Resources/Locale/ru-RU/_FEC14/events/` и `Resources/Locale/en-US/_FEC14/events/`.
Для нового исхода нужен ключ `fec-events-outcome-<исход>`.

## 4. Проверка

`FECEventsTest` в CI проверит правило, переводы, окно запуска и цепочки.
В игре событие запускается из окна «События» в админ-меню или командой `fecevents start <ID>`.

Готовые исходы: `failed`, `landed`, `started`, `saved`, `lost`, `no-squad`, `repelled`, `punished`.
