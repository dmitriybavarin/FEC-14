# Движок RobustToolbox в FEC-14

Движок подключен сабмодулем `RobustToolbox` и закреплен на конкретном коммите. Для работы с ним есть `Tools/_FEC14/engine.py` (нужен только Python 3.8+ и git) и задачи VS Code **FEC: Движок: ...**.

Инструмент ничего не коммитит и не пушит. Он переключает сабмодуль, переносит локальные правки движка через `git stash` и записывает новую версию в индекс (`git add RobustToolbox`). Коммит делает человек.

## Важно знать

- **Сборка сама откатывает движок.** `BuildChecker` при каждой сборке выполняет `git submodule update`. Он ставит движок на коммит из индекса FEC-14, поэтому без `git add RobustToolbox` первая же сборка вернет старую версию. `engine.py use` и `engine.py rmc` делают `git add` сами.
- **Игроки получают официальный движок.** Лаунчер скачивает движок с серверов SS14 по номеру версии, который сообщает сервер. Правки в коде движка работают только на сервере и у разработчиков. Клиентские правки движка до игроков не дойдут, а несовместимые с официальной сборкой правки сломают подключение.
- **.NET 10 начинается с движка 269.** У RMC-14 сейчас `264.0.2-fix-physics` (.NET 9). При переходе на 269+ в `global.json` нужен SDK `10.0.100`, csproj контента переводятся на `RobustToolbox/Imports/*.props` по образцу SS14, на сервере нужен .NET Runtime 10.

## Команды

```
python Tools/_FEC14/engine.py status            текущая версия, ветка, правки, версия у RMC-14
python Tools/_FEC14/engine.py fetch             скачать официальные версии и RMC-14
python Tools/_FEC14/engine.py versions -n 20    последние официальные версии
python Tools/_FEC14/engine.py rmc               поставить ту же версию, что у RMC-14
python Tools/_FEC14/engine.py use 292.0.0       поставить версию, коммит или ветку
python Tools/_FEC14/engine.py build             собрать сервер и клиент, отчет об ошибках в bin/engine_build_report.txt
python Tools/_FEC14/engine.py restore           вернуть версию из коммита FEC-14
python Tools/_FEC14/engine.py branch <имя>      создать или выбрать ветку форка движка
python Tools/_FEC14/engine.py merge <версия>    влить версию в ветку форка, без коммита
python Tools/_FEC14/engine.py conflicts         конфликты слияния
python Tools/_FEC14/engine.py resolve ours|theirs [файлы]   ours = версия FEC, theirs = официальная
python Tools/_FEC14/engine.py abort             отменить слияние
```

## Обычное обновление (после мерджа RMC-14)

1. `engine.py fetch`, затем `engine.py rmc`.
2. `engine.py build`. Если есть ошибки, отчет группирует их по кодам, типам и файлам.
3. Проверить игру, закоммитить `RobustToolbox` вместе с исправлениями контента.

Если что-то пошло не так: `engine.py restore`.

## Ветка движка в своем репозитории

Нужна, если в движке есть свои правки (например, строка ImageSharp в `Directory.Packages.props`): сейчас они живут только в рабочей копии и в git FEC-14 не попадают.

1. На GitHub открыть https://github.com/space-wizards/RobustToolbox и нажать **Fork**. Владелец: `dmitriybavarin`, снять галочку **Copy the master branch only**.
2. Создать ветку от текущей версии движка и подключить форк:

```
python Tools/_FEC14/engine.py branch fec/264.0.2 --remote https://github.com/dmitriybavarin/RobustToolbox.git
git -C RobustToolbox add -A
git -C RobustToolbox commit -m "FEC: правки движка"
git -C RobustToolbox push -u fec fec/264.0.2
```

То же без инструмента:

```
cd RobustToolbox
git remote add fec https://github.com/dmitriybavarin/RobustToolbox.git
git switch -c fec/264.0.2
git add -A
git commit -m "FEC: правки движка"
git push -u fec fec/264.0.2
cd ..
```

3. Переключить FEC-14 на форк движка:

```
git submodule set-url RobustToolbox https://github.com/dmitriybavarin/RobustToolbox.git
git submodule set-branch --branch fec/264.0.2 RobustToolbox
git add .gitmodules RobustToolbox
git commit -m "FEC: движок из форка"
git push
```

После этого остальные участники делают `git pull` и `python RUN_THIS.py`.

## Обновление ветки форка до новой версии

```
python Tools/_FEC14/engine.py fetch
python Tools/_FEC14/engine.py branch fec/292.0.0
python Tools/_FEC14/engine.py merge 292.0.0
python Tools/_FEC14/engine.py conflicts
python Tools/_FEC14/engine.py resolve theirs <файл>
python Tools/_FEC14/engine.py build
git -C RobustToolbox commit -m "FEC: слияние v292.0.0"
git -C RobustToolbox push -u fec fec/292.0.0
git add RobustToolbox
```

Как решать конфликты:

- `Directory.Packages.props` и другие файлы версий: обычно `theirs` (официальная версия), затем снова внести свои строки.
- Файлы, которые меняли только в FEC: `ours`.
- Файлы, где правки есть с обеих сторон: открыть в VS Code и соединить вручную, затем `git -C RobustToolbox add <файл>`.

## CI

Workflow `.github/workflows/no-submodule-update.yml` из upstream валит любой PR, который меняет `RobustToolbox`. Обновления движка пушить напрямую в ветку или временно отключить эту проверку.
