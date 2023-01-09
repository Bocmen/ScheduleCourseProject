using Npgsql;
using System.Data.Common;
using System.Threading.Tasks;

namespace DatabaseUI.Database.Models
{
    public class DBConnection : Abstract.DBSession
    {
        public virtual DBSetting DBSetting { get; private set; }
        protected NpgsqlConnection Connection = null;

        public DBConnection(DBSetting setting) => DBSetting = setting;

        public async Task ConnectAsync(User user)
        {
            await Disconnect();
            Connection = new NpgsqlConnection($"Host={DBSetting.Host};Username={user.Login};Password={user.Password};Database={DBSetting.DBName}");
            await Connection.OpenAsync();
        }

        public override Task<int> ExecuteNonQueryAsync(string command)
        {
            using (NpgsqlCommand npgc = new NpgsqlCommand(command, Connection))
                return npgc.ExecuteNonQueryAsync();
        }
        public override async Task<DbDataReader> ExecuteReaderAsync(string command)
        {
            using (NpgsqlCommand npgc = new NpgsqlCommand(command, Connection))
                return await npgc.ExecuteReaderAsync();
        }
        public override Task<object> ExecuteScalarAsync(string command)
        {
            using (NpgsqlCommand npgc = new NpgsqlCommand(command, Connection))
                return npgc.ExecuteScalarAsync();
        }
        public virtual async Task Disconnect()
        {
            if (Connection != null)
            {
                await Connection.CloseAsync();
                await Connection.DisposeAsync();
            }
        }
        public override void Dispose() => Connection?.Dispose();
    }
}
