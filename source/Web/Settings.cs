using System.Configuration;

namespace SiSystems.ClientApp.Web
{
    public class Settings
    {
        public static bool AllowInsecureConnections
        {
            get { return bool.Parse(ConfigurationManager.AppSettings["AllowInsecureHttp"]); }
        }

        public static int TokenExpiryInDays
        {
            get { return int.Parse(ConfigurationManager.AppSettings["Auth.TokenExpiryInDays"]); }
        }

        public static bool IsApiTestPageEnabled
        {
            get { return bool.Parse(ConfigurationManager.AppSettings["EnableApiTestPage"]); }
        }

        public static string LoginTokenEndpoint = "/api/Login";
    }
}
