using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RegionExtension.Commands.Parameters;
using RegionExtension.Database;
using RegionExtension.RegionTriggers.Conditions;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using TerrariaApi.Server;
using TShockAPI;
using TShockAPI.DB;
using TShockAPI.Hooks;

namespace RegionExtension.Infrastructure
{
    internal sealed class PluginEventDispatcher
    {
        private readonly Plugin _plugin;
        private readonly object _lastActiveLock = new object();
        private bool _checkingHasBuild;
        private bool _handlingItemDrop;
        private List<Point16> _lastActive = new List<Point16>();
        private DateTime _lastActiveCheck = DateTime.UtcNow;

        public PluginEventDispatcher(Plugin plugin)
        {
            _plugin = plugin;
        }

        public void Register()
        {
            ServerApi.Hooks.GameInitialize.Register(_plugin, OnInitialize);
            ServerApi.Hooks.NetGetData.Register(_plugin, OnGetData);
            ServerApi.Hooks.GamePostInitialize.Register(_plugin, OnPostInitialize, int.MinValue);
            ServerApi.Hooks.GamePostUpdate.Register(_plugin, OnPostUpdate);
            ServerApi.Hooks.NetGreetPlayer.Register(_plugin, OnGreetPlayer);
            ServerApi.Hooks.NetSendData.Register(_plugin, OnSendData);
            GeneralHooks.ReloadEvent += OnReload;
            PlayerHooks.PlayerLogout += OnPlayerLogout;
            PlayerHooks.PlayerPostLogin += OnPlayerLogin;
            PlayerHooks.PlayerCommand += OnPlayerCommand;
            PlayerHooks.PlayerHasBuildPermission += OnHasPlayerPermission;
            PluginState.RegionExtensionManager = new RegionExtManager(TShock.DB);
        }

        public void Deregister()
        {
            ServerApi.Hooks.GameInitialize.Deregister(_plugin, OnInitialize);
            ServerApi.Hooks.NetGetData.Deregister(_plugin, OnGetData);
            ServerApi.Hooks.GamePostInitialize.Deregister(_plugin, OnPostInitialize);
            ServerApi.Hooks.GamePostUpdate.Deregister(_plugin, OnPostUpdate);
            ServerApi.Hooks.NetGreetPlayer.Deregister(_plugin, OnGreetPlayer);
            ServerApi.Hooks.NetSendData.Deregister(_plugin, OnSendData);
            GeneralHooks.ReloadEvent -= OnReload;
            PlayerHooks.PlayerLogout -= OnPlayerLogout;
            PlayerHooks.PlayerPostLogin -= OnPlayerLogin;
            PlayerHooks.PlayerCommand -= OnPlayerCommand;
            PlayerHooks.PlayerHasBuildPermission -= OnHasPlayerPermission;
        }

        private void OnReload(ReloadEventArgs e)
        {
            try
            {
                PluginState.Config = ConfigFile.Read();
                PluginState.RegionExtensionManager?.Reload(e);
                DelayManager.Reload(_plugin);
                e.Player?.SendInfoMessage("[RegionExt] Config and triggers reloaded.");
            }
            catch (Exception ex)
            {
                TShock.Log.Error($"[RegionExt] Reload failed: {ex}");
                e.Player?.SendErrorMessage("[RegionExt] Reload failed. Check server logs.");
            }
        }

        private void OnSendItemDrop(SendDataEventArgs args)
        {
            var id = args.number;
            var rewrites = PluginState.ItemRewrites;
            if (id >= Main.maxItems || rewrites[id] == null || !rewrites[id].Active)
                return;

            args.Handled = true;
            var bits = new BitsByte(b2: rewrites[id].Damage != -1, b5: rewrites[id].UseTime != -1, b6: rewrites[id].Projectile != -1, b7: rewrites[id].Projectile != -1, b8: rewrites[id].Projectile != -1);
            var bits2 = new BitsByte(b5: true);
            if (rewrites[id].Damage != -1)
                Main.item[id].inner.damage = rewrites[id].Damage;
            if (rewrites[id].UseTime != -1)
                Main.item[id].inner.useTime = rewrites[id].UseTime;
            if (rewrites[id].Projectile != -1)
            {
                Main.item[id].inner.shoot = rewrites[id].Projectile;
                Main.item[id].inner.useAmmo = AmmoID.None;
                if (Main.item[id].inner.shootSpeed == 0)
                    Main.item[id].inner.shootSpeed = 10;
            }
            NetMessage.SendData((int)args.MsgId, args.remoteClient, args.ignoreClient, args.text, args.number, args.number2, args.number3, args.number4, args.number5, args.number6, args.number7);
            NetMessage.SendData((int)PacketTypes.TweakItem, -1, -1, null, id, bits.value, bits2.value);
        }

        private void OnSendData(SendDataEventArgs args)
        {
            switch ((int)args.MsgId)
            {
                case (int)PacketTypes.ItemDrop:
                case (int)PacketTypes.UpdateItemDrop:
                case (int)PacketTypes.SyncItemsWithShimmer:
                case (int)PacketTypes.SyncItemCannotBeTakenByEnemies:
                    if (_handlingItemDrop)
                        return;
                    _handlingItemDrop = true;
                    try
                    {
                        OnSendItemDrop(args);
                    }
                    finally
                    {
                        _handlingItemDrop = false;
                    }
                    break;
            }
        }

        private void OnGreetPlayer(GreetPlayerEventArgs args)
        {
            PluginState.RegionExtensionManager?.TriggerManager?.OnPlayerEnter(args);
            PluginState.TriggerIgnores[args.Who] = false;
        }

        private void OnPostUpdate(EventArgs args)
        {
            PluginState.RegionExtensionManager?.Update();
            UpdateLastActive();
        }

        private void OnPlayerLogin(PlayerPostLoginEventArgs e)
        {
            if (StringTime.FromString(PluginState.Config.NotificationPeriod).IsZero() || !e.Player.HasPermission(Permissions.RegionExtCmd))
                return;
            PluginState.RegionExtensionManager.SendRequestNotify(e.Player, PluginState.RegionExtensionManager.RegionRequestManager.GetSortedRegionRequestsNames());
        }

        private void OnPostInitialize(EventArgs args)
        {
            InitializePlugin();
        }

        private void InitializePlugin()
        {
            PluginState.RegionExtensionManager.PostInitialize(_plugin);
            DelayManager.Initialize(_plugin);
            TShock.Log.ConsoleInfo("Region extension loaded!");
        }

        private void OnHasPlayerPermission(PlayerHasBuildPermissionEventArgs e)
        {
            if (_checkingHasBuild)
            {
                e.Result = PermissionHookResult.Unhandled;
                return;
            }
            _checkingHasBuild = true;
            if (e.Player.HasBuildPermission(e.X, e.Y, true) && TShock.Regions.InArea(e.X, e.Y))
            {
                lock (_lastActiveLock)
                    _lastActive.Add(new Point16(e.X, e.Y));
            }
            _checkingHasBuild = false;
        }

        private void UpdateLastActive()
        {
            if (DateTime.UtcNow < _lastActiveCheck.AddSeconds(90))
                return;
            List<Point16> points;
            lock (_lastActiveLock)
            {
                points = _lastActive;
                _lastActive = new List<Point16>();
            }
            var regionsToUpdate = new HashSet<int>();
            foreach (var point in points)
                foreach (var id in TShock.Regions.InAreaRegionID(point.X, point.Y))
                    regionsToUpdate.Add(id);
            foreach (var id in regionsToUpdate)
                PluginState.RegionExtensionManager.InfoManager.UpdateLastActivity(id, DateTime.UtcNow);
            points.Clear();
            _lastActiveCheck = DateTime.UtcNow;
        }

        private void OnInitialize(EventArgs args)
        {
            PluginCommands.Initialize(_plugin);
            PluginState.Contexts = new ContextManager();
            PluginState.Contexts.Initialize();
            PluginState.FastRegions = new List<FastRegion>();
            PluginState.Config = ConfigFile.Read();
        }

        private void OnPlayerLogout(PlayerLogoutEventArgs e)
        {
            int id = FastRegionLookup.FindByUser(e.Player.Account, PluginState.FastRegions);
            if (id != -1)
                PluginState.FastRegions.RemoveAt(id);
        }

        private void OnGetData(GetDataEventArgs args)
        {
            switch (args.MsgID)
            {
                case PacketTypes.MassWireOperation:
                    HandleMassWireOperation(args);
                    break;
                case PacketTypes.Tile:
                    HandleTileOperation(args);
                    break;
                case PacketTypes.ItemDrop:
                case PacketTypes.UpdateItemDrop:
                    HandleItemDropOperation(args);
                    break;
            }
        }

        private void HandleMassWireOperation(GetDataEventArgs args)
        {
            if (!TryGetFastRegionIndex(args.Msg.whoAmI, out var id))
                return;

            using (var reader = new BinaryReader(new MemoryStream(args.Msg.readBuffer, args.Index, args.Length)))
            {
                int startX = reader.ReadInt16();
                int startY = reader.ReadInt16();
                int endX = reader.ReadInt16();
                int endY = reader.ReadInt16();
                if (!IsInWorldBounds(startX, startY) || !IsInWorldBounds(endX, endY))
                    return;
                if (PluginState.FastRegions[id].SetPoints(startX, startY, endX, endY))
                    PluginState.FastRegions.RemoveAt(id);
            }

            args.Handled = true;
        }

        private void HandleTileOperation(GetDataEventArgs args)
        {
            if (!TryGetFastRegionIndex(args.Msg.whoAmI, out var id))
                return;

            using (var reader = new BinaryReader(new MemoryStream(args.Msg.readBuffer, args.Index, args.Length)))
            {
                reader.ReadByte();
                int x = reader.ReadInt16();
                int y = reader.ReadInt16();
                if (!IsInWorldBounds(x, y))
                    return;
                if (PluginState.FastRegions[id].SetPoint(x, y))
                    PluginState.FastRegions.RemoveAt(id);
            }

            args.Handled = true;
        }

        private static void HandleItemDropOperation(GetDataEventArgs args)
        {
            using var reader = new BinaryReader(new MemoryStream(args.Msg.readBuffer, args.Index, args.Length));
            int id = reader.ReadInt16();
            var rewrites = PluginState.ItemRewrites;
            if (id >= Main.maxItems || rewrites[id] == null || !rewrites[id].Active)
                return;
            reader.BaseStream.Seek(13, SeekOrigin.Begin);
            int stack = reader.ReadInt16();
            if (stack == 0)
                rewrites[id].Active = false;
        }

        private static bool IsInWorldBounds(int x, int y) =>
            x >= 0 && y >= 0 && x < Main.maxTilesX && y < Main.maxTilesY;

        private static bool TryGetFastRegionIndex(int whoAmI, out int id)
        {
            id = FastRegionLookup.FindByUser(TShock.Players[whoAmI]?.Account, PluginState.FastRegions);
            return id != -1;
        }

        private void OnPlayerCommand(PlayerCommandEventArgs args)
        {
            if (!args.Player.HasPermission(Permissions.RegionExtCmd) && !args.Player.HasPermission("regionext.own"))
                return;
            switch (args.CommandName)
            {
                case "re":
                case "regionext":
                case "rt":
                case "regiontrigger":
                case "rp":
                case "regionproperty":
                case "ro":
                    case "regionown":
                case "region":
                    for (int i = 1; i < args.Parameters.Count; i++)
                        if (args.Parameters[i].StartsWith(PluginState.Config.ContextSpecifier))
                            PluginState.Contexts.InitializeContext(i, args);
                    if (PluginState.Config.AutoCompleteSameName && args.Parameters.Count > 1 && "define" == args.Parameters[0])
                        args.Parameters[1] = Utils.AutoCompleteSameName(args.Parameters[1], PluginState.Config.AutoCompleteSameNameFormat);
                    break;
            }
        }
    }
}



