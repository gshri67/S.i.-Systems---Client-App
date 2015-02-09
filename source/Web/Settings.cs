using System.Configuration;

namespace SiSystems.ClientApp.Web
{
    public class Settings
    {
        public static bool AllowInsecureConnections
        {
            get { return bool.Parse(ConfigurationManager.AppSettings["AllowInsecureHttp"]); }
        }
    }
}
