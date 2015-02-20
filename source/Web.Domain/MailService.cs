using SendGrid.SmtpApi;
using System.Collections.Generic;
using System.Net.Mail;

namespace SiSystems.ClientApp.Web.Domain
{
    public class MailService
    {
        public static void SendExampleEmail(string to, string message)
        {
            //should override in dev environments to prevent
            //emails being sent to actual users
            var recipient = Settings.EmailRecipientOverride ?? to;

            MailMessage mail = new MailMessage();
            mail.To.Add(new MailAddress(recipient));
            var header = CreateHeaderForContactEmail(message);
            mail.Headers.Add("X-SMTPAPI", header);

            SmtpClient client = new SmtpClient();

            client.Send(mail);
        }

        private static string CreateHeaderForContactEmail(string message)
        {
            var header = new Header();

            //header.EnableFilter(Settings.ContactAlumniTemplateId);
            header.AddFilterSetting("templates",
                new List<string> {"template_id"}, Settings.ContactAlumniTemplateId);
            header.AddFilterSetting("templates",
                new List<string> {"enabled"}, "1");

            header.AddSubstitution("-message-", new List<string>
            {
                message
            });
            var xmstpapiJson = header.JsonString();
            return xmstpapiJson;
        }

    }
}
