using System;
using System.Reflection;
using RegionExtension.Infrastructure;
using Terraria;
using TerrariaApi.Server;

namespace RegionExtension
{
    [ApiVersion(2, 1)]
    public class Plugin : TerrariaPlugin
    {
        public override string Author => "Rekman";
        public override string Description => "More region command & functionality";
        public override string Name => "Region Extension";
        public override Version Version => Assembly.GetExecutingAssembly().GetName().Version;

        private readonly PluginEventDispatcher _eventDispatcher;

        public Plugin(Main game) : base(game)
        {
            _eventDispatcher = new PluginEventDispatcher(this);
        }

        public override void Initialize()
        {
            _eventDispatcher.Register();
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposing)
                return;

            PluginCommands.Dispose();
            PluginState.RegionExtensionManager?.PropertyManager?.Dispose(this);
            PluginState.RegionExtensionManager?.Dispose();
            RegionTriggers.Conditions.DelayManager.Dispose(this);
            _eventDispatcher.Deregister();
        }
    }
}

