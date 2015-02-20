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

        public static string ContactAlumniTemplateId
        {
            get { return ConfigurationManager.AppSettings["Email.ContactAlumniTemplateId"]; }
        }
    }
}
