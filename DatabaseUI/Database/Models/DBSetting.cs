namespace DatabaseUI.Database.Models
{
    public class DBSetting
    {
        public string Host { get; private set; }
        public int Port { get; private set; }
        public string DBName { get; private set; }

        public DBSetting(string host, int port, string dbName)
        {
            Host = host;
            Port = port;
            DBName = dbName;
        }
    }
}
