using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Configuration;

namespace SiSystems.ClientApp.Web.Domain
{
    public class Settings
    {
        public static string EmailRecipientOverride
        {
            get
            {
                var recipient = ConfigurationManager.AppSettings["Email.RecipientOverride"];
                return string.IsNullOrWhiteSpace(recipient) ? null : recipient;
            }
        }

        public static string MatchGuideAccountServiceUrl    
        {
            get { return ConfigurationManager.AppSettings["MatchGuideAccountService.Url"]; }
        }

        public static string MatchGuideAccountServiceGatewayId
        {
            get { return ConfigurationManager.AppSettings["MatchGuideAccountService.GatewayId"]; }
        }

        public static string MatchGuideAccountServiceGatewayPwd
        {
            get { return ConfigurationManager.AppSettings["MatchGuideAccountService.GatewayPwd"]; }
        }

        public static string MatchGuideMyAccountServiceUrl
        {
            get { return ConfigurationManager.AppSettings["MatchGuideMyAccountService.Url"]; }
        }

        public static Dictionary<string, int> ParticipatingCompaniesList
        {
            get
            {
                if (!ShouldUseConfiguredParticipatingCompaniesList)
                {
                    throw new ConfigurationErrorsException("Attempted to use hard coded list of participating companies from configuration file, but the application is expecting to use the database.");
                }
                var jsonString = ConfigurationManager.AppSettings["ParticipatingCompanies.List"];
                if (string.IsNullOrWhiteSpace(jsonString)) return new Dictionary<string, int>();

                return JsonConvert.DeserializeObject<Dictionary<string, int>>(jsonString, new KeyValuePairConverter());
            }
        }

        public static bool ShouldUseConfiguredParticipatingCompaniesList
        {
            get
            {
                bool shouldUse = false;
                var configValue = ConfigurationManager.AppSettings["ParticipatingCompanies.InUse"];
                bool.TryParse(configValue, out shouldUse);

                return shouldUse;
            }
        }
    }
}
