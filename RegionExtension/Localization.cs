﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TShockAPI;

namespace RegionExtension
{
    public static class Localization
    {
        public static string[] PlayersLocalization = new string[256];

        public static Dictionary<string, Dictionary<string, string>> Languages = new Dictionary<string, Dictionary<string, string>>()
        {
            { "EN", new Dictionary<string, string>
            {
                #region Subcommands Description
                {"AddPropertyDesc", "Adds property to the region."},
                {"AddConditionPropDesc", "Adds condition to the property." },
                {"AddTriggerDesc", "Adds trigger to the region." },
                {"AddConditionTriggerDesc", "Adds conditions to the trigger." },
                {"AllowGroupDesc", "Allows a user group to a region." },
                {"AllowUserDesc", "Allows a user to a region." },
                {"ClearMembersDesc", "Remove all members from region." },
                {"ClearPointDesc", "Clears the temporary region points." },
                {"ClearPropertiesDesc", "Clears properties from the region." },
                {"ClearTriggersDesc", "Clears triggers from the region." },
                {"ConditionListDesc" , "List available conditions." },
                {"DefineRequestDesc", "Defines the region with the given name and send request." },
                {"DefineRegionDesc", "Defines the region with the given name." },
                {"ListDeletedRegionsDesc", "Get list of regions." },
                {"DeleteRegionDesc", "Deletes the given region." },
                {"DeleteTriggerDesc","Deletes trigger from the region." },
                {"ListEventsDesc","List all available events." },
                {"BreakFRRequestDesc", "Breaks fast region request." },
                {"FastRegionRequestDesc", "Create new region with two given point and params. Also send request." },
                {"FastRegionDesc", "Create new region with two given point and params." },
                {"GetHistoryDesc", "Gets history about region." },
                {"GetRegionNameDesc", "Shows the name of the region at the given point. Additional params -u, -z, -p" },
                {"LastActiveDesc", "Get list of last active regions." },
                {"MoveRegionDesc", "Move region with given name." },
                {"OwnerListDesc", "Get list of regions which the given player is owner" },
                {"InfoPropertyDesc", "Info about property of the region" },
                {"PropertyListDesc", "List available properties." },
                {"RedoHistoryDesc", "Redo actions on region." },
                {"RegionInfoDesc", "Displays several information about the given region." },
                {"RegionListDesc", "Lists all regions." },
                {"RegionRemoveGroupDesc", "Removes a user group from a region." },
                {"RemovePropertyDesc", "Remove property from the region" },
                {"RemovePropertyConditionDesc", "Removes condition from the property" },
                {"RemoveTriggerConditionDesc", "Removes condition from the trigger" },
                {"RegionRemoveDesc", "Removes a user from a region." },
                {"RenameRegionDesc", "Renames the given region." },
                {"RequestAcceptDesc", "Accept region request." },
                {"RequestDenyDesc", "Deny region request." },
                {"RequestInfoDesc", "Displays several information about the given request." },
                {"RequestListDesc", "Lists all region requests." },
                {"RegionResizeDesc", "Resizes a region." },
                {"RegionRestoreByUserDesc", "Restore region from deleted regions with user." },
                {"RegionRestoreDesc", "Restore region from deleted regions." },
                {"SelfOwnerDesc", "Get list of regions in which you is owner." },
                {"SetOwnerDesc", "Set region owner"},
                {"SetProtectDesc", "Sets whether the tiles inside the region are protected or not." },
                {"SetRegionPointDesc", "Sets the temporary region points." },
                {"SetRegionZDesc", "Sets the z-order of the region." },
                {"TeleportToRegionDesc", "Teleports you to the given region's center." },
                {"TriggerInfoDesc", "Info about triggers of the region." },
                {"TriggerListDesc", "List all available triggers." },
                {"UndoHistoryDesc", "Undo actions on region." },
                #endregion
                #region Triggers Description
                {"BuffTriggerDesc", "Buffs player." },
                {"CommandTriggerDesc", "Command trigger." },
                {"GiveItemTriggerDesc", "Gives items to the player" },
                {"KillTriggerDesc", "Kills player." },
                {"ProjectileTriggerDesc", "Spawns projectile." },
                {"PushTriggerDesc", "Pushes player from region" },
                {"MessageTriggerDesc", "Send message to the player." },
                {"SendPacketTriggerDesc", "Send packet to the player." },
                {"SpawnNpcTriggerDesc", "Spawns npc." },
                {"TeleportPosTriggerDesc", "Teleports player to the position." },
                {"TeleportWartTriggerDesc", "Teleports player to the warp." },
                #endregion
                #region Condition Description
                {"AllowCondDesc", "If player is allowed in the region." },
                {"ExactCondDesc", "If players count in the region." },
                {"LessCondDesc", "If less players in the region than count." },
                {"MoreCondDesc", "If more players in the region than count." },
                {"OwnerCondDesc", "if player is owner of the region." },
                {"PauseCondDesc", "pauses trigger in given time after activation." },
                {"PausePlayerCondDesc", "pauses trigger for player in given time after activation." },
                #endregion
                #region Property Description
                {"AlwaysPvpPropDesc", "Activates player pvp and prevents trying to change it." },
                {"BanHostilePropDesc", "Removes any hostile NPCs and projectiles from region, and prevents bosses from entering the region." },
                {"MaxSpawnPropDesc", "Rewrite near NPCs count for players." },
                {"NoPvpPropDesc", "Deactivates player pvp and prevents trying to change it." },
                {"NPCRewritePropDesc", "Rewrites npc spawn in the region." },
                {"ProjBanPropDesc", "Prevents projectile creation from player." },
                {"ItemBanPropDesc", "Ban items in the region." },
                #endregion
                #region Events Description
                {"OnEnterEventDesc", "activates when player enters in region" },
                {"OnLeaveEventDesc", "activates when player leaves from region" },
                {"OnInEventDesc", "activates while player in the region" }
                #endregion
            } },
            { "RU", new Dictionary<string, string>
            {
                #region Subcommands Description
                {"AddPropertyDesc", "Добавляет свойство региону."},
                {"AddConditionPropDesc", "Добавляет условие к свойству региона." },
                {"AddTriggerDesc", "Добавляет триггер к региону." },
                {"AddConditionTriggerDesc", "Добавляет условие к триггеру региона" },
                {"AllowGroupDesc", "Добавляет группу в регион." },
                {"AllowUserDesc", "Добавляет игрока в регион." },
                {"ClearMembersDesc", "Удаляет всех игроков из региона." },
                {"ClearPointDesc", "Сбрасывает установленные точки для задания региона." },
                {"ClearPropertiesDesc", "Удаляет все свойства из региона." },
                {"ClearTriggersDesc", "Удаляет все триггеры из региона." },
                {"ConditionListDesc" , "Перечисляет все доступные условия." },
                {"DefineRequestDesc", "Создает регион с установленными точками и заданным именем, и отправляет запрос." },
                {"DefineRegionDesc", "Создает регион с установленными точками и заданным именем." },
                {"ListDeletedRegionsDesc", "Перечисляет все удаленные регионы." },
                {"DeleteRegionDesc", "Удаляет заданный регион." },
                {"DeleteTriggerDesc","Удаляет триггер из региона." },
                {"ListEventsDesc","Перечисляет все доступные события." },
                {"BreakFRRequestDesc", "Удаляет запрос на быстрый регион." },
                {"FastRegionRequestDesc", "Создает регион и запрашивает установление точек. Также отправляет запрос." },
                {"FastRegionDesc", "Создает регион и запрашивает установление точек." },
                {"GetHistoryDesc", "Получает всю историю действий на регион." },
                {"GetRegionNameDesc", "Отображает имя региона в заданой точке. Дополнительные флаги -u, -z, -p" },
                {"LastActiveDesc", "Перечисляет все регионы в порядке активности." },
                {"MoveRegionDesc", "Перемещает регион в заданном направлении." },
                {"OwnerListDesc", "Перечисляет все регионы с заданным владельцем." },
                {"InfoPropertyDesc", "Отображает информацию о свойствах региона." },
                {"PropertyListDesc", "Перечисляет все доступные свойства." },
                {"RedoHistoryDesc", "Восстанавливает действие над регионом." },
                {"RegionInfoDesc", "Отображает информацию о регионе." },
                {"RegionListDesc", "Перечисляет все доступные регионы." },
                {"RegionRemoveGroupDesc", "Удаляет группу из региона." },
                {"RemovePropertyDesc", "Удаляет свойство из региона." },
                {"RemovePropertyConditionDesc", "Удаляет условие из свойства" },
                {"RegionRemoveDesc", "Удаляет пользователя из региона." },
                {"RenameRegionDesc", "Переименовывает заданный регион." },
                {"RequestAcceptDesc", "Принимает запрос." },
                {"RequestDenyDesc", "Отклоняет запрос." },
                {"RequestInfoDesc", "Отображает информацию о запросе." },
                {"RequestListDesc", "Отображает все активные запросы." },
                {"RegionResizeDesc", "Изменяет размер региона." },
                {"RegionRestoreByUserDesc", "Восстанавливает регионы из удаленных по пользователю." },
                {"RegionRestoreDesc", "Восстанавливает регион из удаленных." },
                {"SelfOwnerDesc", "Отображает все регионы, в которых вы владелец." },
                {"SetOwnerDesc", "Устанавливает владельца региона."},
                {"SetProtectDesc", "Устанавливает защищенность региона. true - нельзя менять блоки false - можно" },
                {"SetRegionPointDesc", "Устанавливает точку для создания региона." },
                {"SetRegionZDesc", "Устанавливает z-приоритет региона." },
                {"TeleportToRegionDesc", "Телепортирует в центр заданного региона." },
                {"TriggerInfoDesc", "Информация о триггерах в регионе." },
                {"TriggerListDesc", "Отображает все доступные триггеры." },
                {"UndoHistoryDesc", "Отменяет действие над регионом." },
                #endregion
                #region Triggers Description
                {"BuffTriggerDesc", "Баффает игрока." },
                {"CommandTriggerDesc", "Вызывает комманду." },
                {"GiveItemTriggerDesc", "Дает предмет игроку." },
                {"KillTriggerDesc", "Убивает игрока." },
                {"ProjectileTriggerDesc", "Спавнит снаряд." },
                {"PushTriggerDesc", "Выталкивает игрока из региона." },
                {"MessageTriggerDesc", "Отправляет сообщение игроку." },
                {"SendPacketTriggerDesc", "Отправляет пакет игроку." },
                {"SpawnNpcTriggerDesc", "Призывает НИПа." },
                {"TeleportPosTriggerDesc", "Телепортирует игрока на позицию." },
                {"TeleportWartTriggerDesc", "Телепортирует игрока на варп." },
                #endregion
                #region Condition Description
                {"AllowCondDesc", "Если игрок добавлен в регион." },
                {"ExactCondDesc", "Если игроков в регионе - заданное количество." },
                {"LessCondDesc", "Если игроков в регионе больше заданного количества." },
                {"MoreCondDesc", "Если игроков в регионе меньше заданного количества." },
                {"OwnerCondDesc", "Если игрок - владелец региона." },
                {"PauseCondDesc", "Останавливает триггер на заданное время. Формат: 0d0h0m0s" },
                {"PausePlayerCondDesc", "Останавливает триггер на заданное время для игрока. Формат: 0d0h0m0s" },
                #endregion
                #region Property Description
                {"AlwaysPvpPropDesc", "Включает PvP игрока и предотвращает его изменение." },
                {"BanHostilePropDesc", "Удаляет всех враждебных НИПов и снаряды, также не позволяет боссу зайти в регион." },
                {"MaxSpawnPropDesc", "Переписывает количество НИПов возле игрока. Число меньше 1 - увеличит появление НИПов возле игрока, число выше - уменьшит" },
                {"NoPvpPropDesc", "Выключает PvP игрока и предотвращает его изменение." },
                {"NPCRewritePropDesc", "Изменяет естественное появление НИПов в регионе." },
                {"ProjBanPropDesc", "Предотвращает использование снаряда игроком." },
                {"ItemBanPropDesc", "Предотвращает использование предмета игроком." },
                #endregion
                #region Events Description
                {"OnEnterEventDesc", "активируется когда игрок заходит в регион." },
                {"OnLeaveEventDesc", "активируется когда игрок выходит из региона." },
                {"OnInEventDesc", "активируется пока игрок находится в регионе." }
                #endregion
            } },
        };

        public static string GetStringForPlayer(string name, TSPlayer player = null)
        {
            var localization = "EN";
            if (player == null || player.TPlayer.whoAmI == -1 || string.IsNullOrEmpty(PlayersLocalization[player.TPlayer.whoAmI]))
                localization = Plugin.Config.DefaultLocalization;
            else
                localization = PlayersLocalization[player.TPlayer.whoAmI];
            if (!Languages[localization].ContainsKey(name))
            {
                if (localization != "EN" && Languages["EN"].ContainsKey(name))
                    return Languages["EN"][name];
                return name;
            }
            else
                return Languages[localization][name];
        }
    }
}
