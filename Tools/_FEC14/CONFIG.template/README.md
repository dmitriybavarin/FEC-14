# .CONFIG: настройки хоста FEC14

Папка `.CONFIG/` в корне репозитория в `.gitignore`: сюда можно класть токены и пароли, в git они не попадут.
Шаблон лежит в `Tools/_FEC14/CONFIG.template/`. Создать или дополнить `.CONFIG/` (существующие файлы не перезаписываются):

```
python Tools/_FEC14/init_cfg.py
```

или задачей VS Code «FEC: Создать папку .CONFIG».

## Файлы

| Файл | Кто читает | Что внутри |
|---|---|---|
| `server_config.toml` | сервер (`--config-file .CONFIG/server_config.toml`, так запускают задачи VS Code) | порт, название, хаб, база данных, вебхуки Discord, ссылки, настройки `[fec]` |
| `ooc_colors.json` | сервер, бот из `.BOT` | цвета ников в OOC по GUID |

## Настройки FEC14

- `fec.species_whitelist`: какие расы можно выбрать в редакторе персонажа, через запятую ID прототипов.
  По умолчанию `Human`. Пустая строка разрешает все расы с `roundStart: true`.

- `fec.ooc_colors`: свой цвет ника в OOC по GUID аккаунта, формат `"GUID=#RRGGBB, GUID=#RRGGBB"`.
  Работает для любых игроков, без Patreon. GUID лежит в базе сервера: таблица `player`, колонка `user_id`.
- `fec.ooc_colors_file` (по умолчанию `.CONFIG/ooc_colors.json`): те же цвета в JSON `{"GUID": {"color": "#RRGGBB", "name": "ник"}}`.
  Его правит бот из `.BOT` (`/ooccolor`), сервер перечитывает файл сам раз в 5 секунд. Файл важнее `fec.ooc_colors`.

## Вайтлисты

В игре два независимых вайтлиста. Обе команды выполняются в консоли сервера или админом с флагом `Ban`.

**Вход на сервер** (`[whitelist]`):
- `enabled = true` пускает на сервер только тех, кто в списке. По умолчанию выключено.
- `prototype_list = "basicWhitelist"`: правила из `Resources/Prototypes/whitelists.yml` (пускать вайтлистнутых, остальным отказ).
- Команды: `whitelistadd <ник>`, `whitelistremove <ник>`, `kicknonwhitelisted` (выгнать тех, кого нет в списке).

**Должности** (`game.role_whitelist`, по умолчанию `true`):
- Закрыты без вайтлиста: Командующий офицер, синтетик (в том числе синтетики выживших), старший унтер-офицер-советник,
  главный инспектор и инспектор военной прокуратуры, командующий офицер выживших. В редакторе они показаны как "Закрыто".
- Команды: `jobwhitelistadd <ник> <ID должности>`, `jobwhitelistremove <ник> <ID должности>`, `jobwhitelistget <ник>`.
- ID должностей: `CMCommandingOfficer`, `RMCJobSynthetic`, `CMSeniorEnlistedAdvisor`, `CMProvostChiefInspector`, `CMProvostInspector`.
- `role_whitelist = false` открывает эти должности всем.

## Сервер вне VS Code

```
dotnet run --project Content.Server -- --config-file .CONFIG/server_config.toml
```

Для собранного сервера путь задается относительно рабочей папки, либо положи файл рядом с exe под именем `server_config.toml`.
