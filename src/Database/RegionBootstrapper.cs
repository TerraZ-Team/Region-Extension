using System;
using TerrariaApi.Server;
using TShockAPI;

namespace RegionExtension.Database
{
    internal sealed class RegionBootstrapper
    {
        private readonly DatabaseRepositoryFactory _databaseRepositoryFactory;
        private readonly PluginContext _context;

        public RegionBootstrapper(DatabaseRepositoryFactory databaseRepositoryFactory, PluginContext context)
        {
            _databaseRepositoryFactory = databaseRepositoryFactory;
            _context = context;
        }

        public RegionServices Initialize(TerrariaPlugin plugin)
        {
            var connection = _databaseRepositoryFactory.CreateConnection(TShock.Config.Settings, TShock.SavePath);
            return new RegionServices
            {
                Connection = connection,
                InfoManager = new RegionInfoManager(connection),
                HistoryManager = new RegionHistoryManager(connection),
                DeletedRegions = new DeletedRegionsDB(connection),
                RequestManager = new RegionRequestManager(connection),
                TriggerManager = new RegionTriggers.TriggerManager(connection, _context.TriggerIgnores),
                PropertyManager = new RegionTriggers.PropertyManager(connection, plugin, _context)
            };
        }
    }
}
