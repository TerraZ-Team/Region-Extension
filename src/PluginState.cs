using System.Collections.Generic;
using RegionExtension.Database;
using Terraria;

namespace RegionExtension
{
    public static class PluginState
    {
        public static ContextManager Contexts { get; set; }
        public static List<FastRegion> FastRegions { get; set; } = new List<FastRegion>();
        public static ConfigFile Config { get; set; }
        public static RegionExtManager RegionExtensionManager { get; set; }
        public static bool[] TriggerIgnores { get; } = new bool[Main.maxPlayers];
        public static ItemRewrite[] ItemRewrites { get; } = new ItemRewrite[Main.maxItems];
    }
}

