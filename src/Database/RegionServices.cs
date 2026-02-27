using System.Data;
using RegionExtension.RegionTriggers;

namespace RegionExtension.Database
{
    internal sealed class RegionServices
    {
        public IDbConnection Connection { get; init; }
        public RegionInfoManager InfoManager { get; init; }
        public RegionHistoryManager HistoryManager { get; init; }
        public DeletedRegionsDB DeletedRegions { get; init; }
        public RegionRequestManager RequestManager { get; init; }
        public TriggerManager TriggerManager { get; init; }
        public PropertyManager PropertyManager { get; init; }
    }
}
