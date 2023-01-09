using DatabaseUI.Database.Models;
using System.Threading.Tasks;

namespace DatabaseConnect
{
    public static class AppDBConnection
    {
        public static DBConnection Connection { get; private set; }
        private static DBSetting _dbSetting;

        public static async Task ConnectAsync(User user)
        {
            if (_dbSetting == null)
                _dbSetting = await ReaderSetting.GetSetting();
            var connection = new DBConnection(_dbSetting);
            await connection.ConnectAsync(user);
            Connection = connection;
        }
    }
}
