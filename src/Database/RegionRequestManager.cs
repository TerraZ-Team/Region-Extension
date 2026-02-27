using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TShockAPI.DB;
using TShockAPI;
using Terraria;

namespace RegionExtension.Database
{
    public class RegionRequestManager
    {
        private IDbConnection _database;

        private SqlTable _table =
            new SqlTable("RequestDatabase",
                         new SqlColumn(TableInfo.RegionId.ToString(), MySqlDbType.Int32),
                         new SqlColumn(TableInfo.WorldID.ToString(), MySqlDbType.Text),
                         new SqlColumn(TableInfo.UserID.ToString(), MySqlDbType.Int32),
                         new SqlColumn(TableInfo.DateCreation.ToString(), MySqlDbType.Text)
                         );

        private List<Request> _requests = new List<Request>();
        public List<Request> Requests { get { return _requests; } }

        public RegionRequestManager(IDbConnection db)
        {
            _database = db;
            InitializeTable();
        }

        public void InitializeTable()
        {
            var creator = new SqlTableCreator(_database, QueryBuilderFactory.Create(_database));
            creator.EnsureTableStructure(_table);
            LoadInfo();
        }

        public bool AddRequest(Region region, UserAccount user)
        {
            try
            {
                var variablesString = string.Join(", ", _table.Columns.Select(c => c.Name));
                _database.Query($"INSERT INTO {_table.Name} ({variablesString}) VALUES (@0, @1, @2, @3);",
                    region.ID,
                    Main.worldID.ToString(),
                    user.ID,
                    DateTime.UtcNow.ToString());
                Requests.Add(new Request(region, user, DateTime.UtcNow));
                return true;
            }
            catch (Exception ex)
            {
                TShock.Log.Error(ex.Message);
                return false;
            }
        }

        private bool UpdateQuery(IDbConnection db, string table, string column, string value, int RegionId)
        {
            try
            {
                db.Query($"UPDATE {table} SET {column}=@0 WHERE RegionId=@1", value, RegionId);
                return true;
            }
            catch (Exception ex)
            {
                TShock.Log.Error(ex.Message);
                return false;
            }
        }

        public void LoadInfo()
        {
            try
            {
                using (var reader = _database.QueryReader($"SELECT * FROM {_table.Name} WHERE WorldID=@0", Main.worldID.ToString()))
                {
                    while (reader.Read())
                    {
                        Region region = TShock.Regions.GetRegionByID(reader.Get<int>(TableInfo.RegionId.ToString()));
                        UserAccount user = TShock.UserAccounts.GetUserAccountByID(reader.Get<int>(TableInfo.UserID.ToString()));
                        if (region == null || user == null)
                            continue;
                        DateTime date = DateTime.Parse(reader.Get<string>(TableInfo.DateCreation.ToString()));
                        _requests.Add(new Request(region, user, date));
                    }
                }
            }
            catch (Exception ex)
            {
                TShock.Log.Error(ex.Message);
            }
        }

        public bool DeleteRequest(Region region)
        {
            try
            {
                _database.Query($"DELETE FROM {_table.Name} WHERE RegionId=@0", region.ID);
                _requests.RemoveAll(r => r.Region.ID == region.ID);
                return true;
            }
            catch (Exception ex)
            {
                TShock.Log.Error(ex.Message);
                return false;
            }
        }

        public IEnumerable<string> GetSortedRegionRequestsNames() =>
            Requests.Select(r =>
                            {
                                var time = StringTime.FromString(Utils.GetSettingsByUserAccount(r.User).RequestTime);
                                var endTime = r.DateCreation + time;
                                var str = time.IsZero() ? $"[c/fffffff:{r.Region.Name}]" :
                                                        Utils.GetGradientByDateTime(r.Region.Name, r.DateCreation, endTime);
                                return (str: str, endTime: endTime);
                            }).OrderBy(r => r.endTime).Select(r => r.str);

        enum TableInfo
        {
            RegionId,
            WorldID,
            UserID,
            DateCreation,
            Denied,
            Denier,
            Type
        }
    }

    public class Request
    {
        public Request(Region region, UserAccount user, DateTime dateCreation)
        {
            Region = region;
            User = user;
            DateCreation = dateCreation;
        }

        public Region Region { get; set; }
        public UserAccount User { get; set; }
        public DateTime DateCreation { get; set; }

        public IEnumerable<string> GetInfoStrings()
        {
            var settings = Utils.GetSettingsByUserAccount(User);
            var requestTime = (DateCreation + StringTime.FromString(settings.RequestTime)).ToString(Utils.DateFormat);
            return new string[]
            {
                "Region: " + Region.Name,
                "User: " +  User.Name,
                "DateCreation: " + DateCreation.ToString(Utils.DateFormat),
                settings.AutoApproveRequest ? "DateApprove: " + requestTime :
                                              "DateDeletion: " + requestTime
            };
        }
    }
}

