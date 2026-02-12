using RegionExtension.Commands.Parameters;
using RegionExtension.Database;
using RegionExtension.Database.EventsArgs;
using RegionExtension.RegionTriggers.Conditions;
using RegionExtension.RegionTriggers.RegionProperties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TerrariaApi.Server;
using TShockAPI;
using TShockAPI.DB;
using TShockAPI.Hooks;

namespace RegionExtension.RegionTriggers
{
    public class PropertyManager
    {
        IRegionProperty[] _regionProperties = Assembly.GetExecutingAssembly().GetTypes().Where(t => typeof(IRegionProperty).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
                                                                                        .Select(t => (IRegionProperty)t.GetConstructors().Where(c => c.GetParameters().Length == 0)
                                                                                                                                         .First().Invoke(null)).ToArray();
        DatabaseTable<RegionPropertyDBUnit> _database;

        public IRegionProperty[] RegionProperties { get { return _regionProperties; } }

        public PropertyManager(IDbConnection dbConnection, TerrariaPlugin plugin)
        {
            _database = new DatabaseTable<RegionPropertyDBUnit>("RegionProperties", dbConnection);
            Initialize(plugin);
        }

        public void Initialize(TerrariaPlugin plugin)
        {
            _database.InitializeTable();
            var triggers = new List<Trigger>();
            foreach (var prop in _regionProperties)
                prop.InitializeEventHandler(plugin);
            LoadProperties();
            RegionExtManager.OnRegionDeleted += OnRegionDeleted;
        }

        private void OnRegionDeleted(BaseRegionArgs args)
        {
            var region = args.Region;
            RemoveAllProperties(region);
        }

        public IRegionProperty GetProperty(string name) =>
            _regionProperties.FirstOrDefault(p => p.Names.Contains(name.ToLower()));

        public ICommandParam[] GetPropertyParams(string propertyName) =>
            _regionProperties.FirstOrDefault(p => p.Names.Contains(propertyName.ToLower()))?.CommandParams;

        public bool AddRegionProps(Region region, string propertyName, ICommandParam[] commandParams)
        {
            var prop = GetRequiredProperty(propertyName);
            EnsurePropertyRowExists(region, prop);
            prop.AddRegionProperties(region, commandParams);
            return SavePropertyState(region, prop);
        }

        public bool RemoveRegionProperties(Region region, string propertyName, ICommandParam[] commandParams)
        {
            var prop = GetRequiredProperty(propertyName);
            if (!prop.DefinedRegions.Contains(region))
                return false;
            prop.RemoveRegionProperties(region, commandParams);
            if (!prop.DefinedRegions.Contains(region))
                return RemovePropertyState(region, prop);
            return SavePropertyState(region, prop);
        }

        public bool AddRegionCondition(Region region, string propertyName, ICommandParam[] commandParams, IRegionCondition regionCondition)
        {
            var prop = GetRequiredProperty(propertyName);
            EnsurePropertyRowExists(region, prop);
            prop.AddCondition(region, commandParams, regionCondition);
            return SavePropertyState(region, prop);
        }

        public bool RemoveRegionCondition(Region region, string propertyName, ICommandParam[] commandParams, IRegionCondition regionCondition)
        {
            var prop = GetRequiredProperty(propertyName);
            EnsurePropertyRowExists(region, prop);
            prop.RemoveCondition(region, commandParams, regionCondition);
            return SavePropertyState(region, prop);
        }

        public void ClearProperty(Region region, string propertyName)
        {
            var prop = GetRequiredProperty(propertyName);
            if (!prop.DefinedRegions.Contains(region))
                return;
            prop.ClearProperties(region);
            RemovePropertyState(region, prop);
        }

        public bool RemoveAllProperties(Region region)
        {
            foreach (var item in _regionProperties.Where(p => p.DefinedRegions.Contains(region)))
                item.ClearProperties(region);
            return _database.RemoveByColumn(new[] { (nameof(RegionPropertyDBUnit.RegionId), (object)region.ID) });
        }

        public void Reload(ReloadEventArgs e)
        {
            foreach (var property in _regionProperties)
                foreach (var region in property.DefinedRegions)
                    property.ClearProperties(region);
            LoadProperties();
        }

        public void Dispose(Plugin plugin)
        {
            foreach (var property in _regionProperties)
                property.Dispose(plugin);
        }

        private void LoadProperties()
        {
            foreach (var region in TShock.Regions.Regions)
            {
                var list = _database.GetValues(RegionPropertyDBUnit.Reader, new[] { (nameof(RegionPropertyDBUnit.RegionId), (object)region.ID) }).Select(p => (p.PropertyName, p.Conditions, p.Args));
                foreach (var propInfo in list)
                {
                    if (string.IsNullOrEmpty(propInfo.Args))
                    {
                        _database.RemoveByColumn(new[] { (nameof(RegionPropertyDBUnit.RegionId), (object)region.ID), (nameof(RegionPropertyDBUnit.PropertyName), (object)propInfo.PropertyName) });
                        continue;
                    }
                    _regionProperties.FirstOrDefault(p => p.Names[0].Equals(propInfo.PropertyName)).SetFromString(region, new(propInfo.Conditions, propInfo.Args));
                }
            }
        }

        private IRegionProperty GetRequiredProperty(string propertyName) =>
            _regionProperties.First(p => p.Names.Contains(propertyName.ToLower()));

        private void EnsurePropertyRowExists(Region region, IRegionProperty property)
        {
            if (!property.DefinedRegions.Contains(region))
                _database.SaveValue(new RegionPropertyDBUnit(region.ID, property.Names[0], ""));
        }

        private bool SavePropertyState(Region region, IRegionProperty property)
        {
            var pair = property.GetStringArgs(region);
            var conditions = GetPropertyConditions(region, property);
            return _database.UpdateByColumn(nameof(RegionPropertyDBUnit.Args), pair.Args, conditions) &&
                   _database.UpdateByColumn(nameof(RegionPropertyDBUnit.Conditions), pair.Conditions, conditions);
        }

        private bool RemovePropertyState(Region region, IRegionProperty property) =>
            _database.RemoveByColumn(GetPropertyConditions(region, property));

        private static (string columnName, object value)[] GetPropertyConditions(Region region, IRegionProperty property) =>
            new[]
            {
                (nameof(RegionPropertyDBUnit.RegionId), (object)region.ID),
                (nameof(RegionPropertyDBUnit.PropertyName), (object)property.Names[0])
            };
    }
}

