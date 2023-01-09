using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace DatabaseConnect
{
    public static class ReaderSetting
    {
        private const string Path = "DatabaseConnect.DBSetting.json";

        public static async Task<DatabaseUI.Database.Models.DBSetting> GetSetting()
        {
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(ReaderSetting)).Assembly;
            using (StreamReader reader = new StreamReader(assembly.GetManifestResourceStream(Path)))
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<DatabaseUI.Database.Models.DBSetting>(await reader.ReadToEndAsync());
            }
        }
    }
}
