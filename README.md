# Region Extension

Плагин добавляет расширенные команды и механику для работы с регионами в TShock, а также поддерживает контекстные параметры команд.

## Команды

### RegionExt (`/re`, `/regionext`, `/region`)
Все команды интегрированы с `/region`, стандартные регион-команды заменяются обработкой плагина.

- `move/mv <regionname> <amount> <u/d/r/l>` - Перемещает регион с указанным именем в заданном направлении.
- `setowner/so [useraccount] [region]` - Назначает владельца региона.
- `clearmembers/cm [regionname]` - Удаляет всех участников региона.
- `fastregion/fr <regionname> [ownername] [z] [protect]` - Создаёт регион с параметрами и запрашивает две точки.
- `frbreak` - Отменяет активный запрос `fastregion`.
- `ownerlist/ol [username] [page]` - Показывает регионы, где указанный игрок владелец.
- `allowedlist/al [username] [page]` - Показывает регионы, где указанный игрок добавлен.
- `listact/la [page]` - Список последних активных регионов.
- `listrequest/lr [page]` - Список всех заявок на регионы.
- `requestinfo/ri [region] [page]` - Информация о заявке на регион.
- `requestaccept/ra [region]` - Подтвердить заявку.
- `requestdeny/rd [region]` - Отклонить заявку.

Примечание: заявки отправляются игроком через `/regionown`. Игрок с доступом к основным командам должен подтвердить заявку, иначе регион будет удалён по настройкам.

### RegionOwn (`/ro`, `/regionown`)
Команды для владельцев регионов. По смыслу похожи на основные, но в каждой есть проверка, что игрок действительно владелец.

- `setowner/so [useraccount] [region]` - Назначить владельца региона.
- `clearmembers/cm [regionname]` - Удалить всех участников региона.
- `ownerlist/ol [page]` - Список ваших регионов.
- `allow/a <useraccount> [region]` - Добавить игрока в регион.
- `remove/r <useraccount> [region]` - Удалить игрока из региона.
- `info/i [region] [page]` - Информация о регионе.
- `set <1/2>` - Установить временную точку региона.
- `define/d <name>` - Создать регион с заданным именем и отправить заявку.
- `delete/del [region]` - Удалить регион.
- `fastregion/fr <region>` - Быстрое создание региона по двум точкам с отправкой заявки.
- `fastregionbreak/frb` - Отменить активный fastregion-запрос.

### RegionHistory (`/rh`, `/regionhistory`)
История изменений регионов.

- `undo/u <count> [region]` - Отменить действия над регионом.
- `redo/r <count> [region]` - Повторить отменённые действия.
- `restore/res <regionname>` - Восстановить удалённый регион.
- `restoreuser/resu <user> [count]` - Восстановить удалённые регионы указанного пользователя.
- `history/h [page] [region]` - Показать историю региона.
- `dellist/dl [page]` - Список удалённых регионов.

Примечание: максимум записей удалённых регионов в буфере - 64.

### RegionTrigger (`/rt`, `/regiontrigger`)
Команды триггеров регионов.

- `add/a <region> <event> <trigger>` - Добавить триггер в регион.
- `delete/d <region> <id>` - Удалить триггер из региона.
- `info/i [region] [page]` - Информация о триггерах региона.
- `list/l [page]` - Список доступных триггеров.
- `helptrigger/ht <trigger> [page]` - Подробная справка по триггеру.
- `eventlist/el [page]` - Список доступных событий.
- `conditionlist/cl [page]` - Список условий.
- `addcond/ac <region> <condition> [ids...]` - Добавить условие триггерам.
- `removecond/rc <region> <condition> [ids...]` - Удалить условие у триггеров.
- `clear/c [region]` - Удалить все триггеры региона.

Триггер - это действие, выполняемое при событии.

Пример:
- `/rt a $t e msg Hello world!`
- Игроку, который вошёл в регион (`e/enter`), отправится сообщение `Hello world!`.

Условия определяют, когда триггер может сработать.

Пример:
- `/rt ac $t !a 0`
- Триггер с id `0` сработает только для игрока, который не добавлен в регион (`!a`).

Если у триггера несколько условий, все они должны быть истинны.

### RegionProperty (`/rp`, `/regionproperty`)
Команды свойств региона.

- `add/a <region> <property>` - Добавить свойство в регион.
- `remove/r <region> <property>` - Удалить свойство из региона.
- `list/l [page]` - Список доступных свойств.
- `info/i [page] [region]` - Информация о свойствах региона.
- `helpproperty/hp <property> [page]` - Подробная справка по свойству.
- `addcond/ac <region> <condition> <property>` - Добавить условие к свойству.
- `removecond/rc <region> <condition> <property>` - Удалить условие у свойства.
- `clear/c [region]` - Удалить все свойства региона.
- `blockdoortoggle/bdt` - Блокировать переключение дверей на стороне сервера.

Свойства задают постоянные правила поведения региона.

Пример:
- `/rp a $t ap`
- У игроков в регионе автоматически включается PvP и его нельзя выключить (`ap/alwayspvp`).

Условия также работают и для свойств (с ограничениями по совместимости).

Пример:
- `/rp ac $t !a ap`
- Свойство будет действовать только на игроков, не добавленных в регион.

## Доступные триггеры

- `command/cmd <command>` - Выполнить команду.
  - `@r` - будет заменено на имя региона.
  - `@p` - будет заменено на имя игрока, активировавшего триггер.
- `push/p` - Вытолкнуть игрока из региона.
- `packet/pa <int> [text] [data...]` - Отправить пакет игроку.
- `message/msg <text...>` - Отправить сообщение игроку.
- `spawnnpc/spawnmob/sn/sm <npc> [count] [x] [y] [health] [strength]` - Заспавнить NPC.
- `spawnproj/sp <projectile> [count] [damage] [knockback] [x] [y] [speedX] [speedY]` - Заспавнить снаряд.
- `giveitem/spawnitem/g/si <item> [stack] [prefix] [x] [y] [damage] [usetime] [projectile]` - Выдать предмет игроку.
- `tppos <x> <y>` - Телепортировать игрока в позицию.
- `warp <warp>` - Телепортировать игрока на варп.
- `kill/k` - Убить игрока.
- `buff/b <buff> [time]` - Выдать бафф (время в секундах).
- `pvp` - Переключить PvP игрока.

В координатах можно использовать функции, вычисляемые в момент срабатывания.

Пример:
- `/rt a $t e g 1 1 0 px+1 py+1`
- Выдаст предмет в координатах относительно игрока.

Доступные функции:
- `px, py` - координаты игрока, активировавшего триггер.
- `cx, cy` - координаты левого верхнего угла региона.
- `w, h` - ширина и высота региона.
- `ri` - случайное число от `0` до `int.MaxValue`.
- `rd` - случайное число от `0` до `1`.
- `lx, ly` - локальные координаты игрока в регионе, где задан триггер.
- `gx, gy` - координаты игрока, который задал триггер.

## Доступные события

- `onenter/enter/e` - Игрок входит в регион.
- `onleave/leave/l` - Игрок выходит из региона.
- `onin/in/i` - Игрок находится в регионе.
- `onpvpon/pvpon` - Игрок включил PvP.
- `onpvpoff/pvpoff` - Игрок выключил PvP.

Обновление проверок: раз в 0.5 секунды.

## Доступные условия

- `allowed/a` - Игрок добавлен в регион.
- `exact/e <count>` - Точное число игроков в регионе.
- `less/l <count>` - Игроков меньше, чем `<count>`.
- `more/m <count>` - Игроков больше, чем `<count>`.
- `owner/o` - Игрок владелец региона.
- `pause/p [time]` - Пауза между срабатываниями. Формат: `0d0h0m0s`.
- `playerpause/pp [time]` - Пауза между срабатываниями для конкретного игрока.
- `delay/d [time] [flag]` - Отложенное срабатывание.
- `playerdelay/pd [time] [flag]` - Отложенное срабатывание для игрока.
- `recheck/rc` - Перепроверка фактического нахождения игрока в регионе.
- `hasitem/hi <item>` - У игрока есть указанный предмет.

Флаги `delay`:
- `-f` - срабатывание по окончании задержки независимо от игрока (по умолчанию);
- `-i` - срабатывание, если игрок в регионе в конце задержки;
- `-a` - срабатывание, если игрок находился в регионе в течение всей задержки.

## Доступные свойства

- `alwayspvp/ap` - Включает PvP и запрещает его изменение.
- `banhostile/bh` - Удаляет враждебных NPC/снаряды и не пускает боссов в регион.
- `clearitems/ci` - Удаляет предметы из региона.
- `maxspawn/ms <ratio>` - Меняет расчёт nearby NPC для игрока.
  - Пример: если рядом 10 NPC и `ratio=0.5`, игра учитывает их как 5 (спавна будет больше). При `1.5` учитывает как 15 (спавна будет меньше). При `0` спавн рядом отключается.
- `nopvp/np` - Выключает PvP и запрещает его изменение.
- `spawnrewrite/sr <npcs...>` - Переписывает естественный спавн NPC в регионе.
  - Формат: `{NameOrId}:{Weight}`. Вес задаёт вероятность относительно остальных NPC в списке.
- `projban/pb <projs...>` - Запрещает использование снарядов игроком.
- `itemban/ib <items...>` - Запрещает использование предметов игроком.

## Вспомогательные команды

- `context` - Список контекстных команд.
- `reperm` - Список всех permissions плагина.
- `reloc` - Смена локализации (`EN`/`RU`).
- `triggerignore/ti` - Игнорирование триггеров и части свойств.

## Контекст

Контекстные параметры ускоряют ввод команд.

- `$this/$t` - имя текущего региона игрока.
- `$myname/$mn` - имя аккаунта игрока.
- `$near/$n` - имя ближайшего игрока.

Пример:
- `/region info $this`

## Permissions

- `tshock.admin.region` - `/regionext /re /region`
- `regionext.own` - `/regionown /ro`
- `regionext.history` - `/regionhistory /rh`
- `regionext.trigger` - `/regiontrigger /rt`
- `regionext.property` - `/regionproperty /rp`
- `regionext.trigger.ignore` - `/triggerignore /ti`
- `regionext.trigger.sendpacket` - `sendpacket/sp`
- `regionext.trigger.message` - `message/msg`
- `regionext.trigger.push` - `push/p`
- `regionext.trigger.command` - `command/cmd`
- `regionext.trigger.warp` - `warp`
- `regionext.trigger.spawnnpc` - `spawnnpc/spawnmob/sn/sm`
- `regionext.trigger.giveitem` - `giveitem/spawnitem/g/si`
- `regionext.trigger.tppos` - `tppos`
- `regionext.trigger.spawnproj` - `spawnproj/sp`
- `regionext.trigger.kill` - `kill/k`
- `regionext.trigger.buff` - `buff/b`
- `regionext.trigger.pvp` - `pvp`
- `regionext.property.pvp` - `alwayspvp/ap`, `nopvp/np`
- `regionext.property.banhostile` - `banhostile/bh`
- `regionext.property.spawnrewrite` - `spawnrewrite/sr`
- `regionext.property.projban` - `projban/pb`
- `regionext.property.itemban` - `itemban/ib`
- `regionext.property.maxspawn` - `maxspawn/ms`
- `regionext.property.blocktileframe` - `blocktileframe/btf`
- `regionext.property.blockdoortoggle` - `blockdoortoggle/bdt`

## Config

```json
{
  "ContextSpecifier": "$",                // Начальный символ контекста
  "ContextAllow": true,                     // Разрешить использование контекста
  "AutoCompleteSameName": true,             // Автодобавление суффикса к одинаковым именам
  "AutoCompleteSameNameFormat": "{0}:{1}",// Формат имени: {0}=имя, {1}=номер
  "NotificationPeriod": "10m",             // Период уведомлений о заявках
  "DefaultLocalization": "EN",             // Локализация по умолчанию
  "BannedTriggerCommands": [
    "group",
    "user"
  ],
  "RequestSettings": [
    {
      "GroupName": "default",
      "MaxRequestCount": 3,
      "RequestTime": "3d",
      "AutoApproveRequest": false,
      "MaxRequestArea": 10000,
      "MaxRequestHeight": 100,
      "MaxRequestWidth": 100,
      "ProtectRequestedRegion": true,
      "DefaultRequestZ": 0
    },
    {
      "GroupName": "superadmin",
      "MaxRequestCount": 0,
      "RequestTime": "0s",
      "AutoApproveRequest": true,
      "MaxRequestArea": 0,
      "MaxRequestHeight": 0,
      "MaxRequestWidth": 0,
      "ProtectRequestedRegion": true,
      "DefaultRequestZ": 0
    }
  ]
}
```
