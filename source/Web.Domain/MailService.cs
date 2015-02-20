using System.Collections.Generic;
using System.Net.Mail;
using SendGrid.SmtpApi;

namespace SiSystems.ClientApp.Web.Domain
{
    class SendGridMailService
    {
        public void SendTemplatedEmail(string templateId, string to, string from, Dictionary<string, string> substitutions)
        {
            //Configuration override in DEV & TEST environments 
            //to prevent emails being sent to actual users
            var recipient = Settings.EmailRecipientOverride ?? to;

            MailMessage mail = new MailMessage();

            mail.To.Add(new MailAddress(recipient));
            //mail.From is handled by SMTP configuration

            mail.ReplyToList.Add(new MailAddress(from));

            var header = new Header();

            SetTemplate(header, templateId, substitutions);

            mail.Headers.Add("X-SMTPAPI", header.JsonString());

            SmtpClient client = new SmtpClient();
            client.Send(mail);
        }

        /// <summary>
        /// Adds required header voodoo to associate 
        /// the email with a specific SendGrid Template
        /// </summary>
        private static void SetTemplate(Header header, string templateId, Dictionary<string, string> substitutions)
        {
            header.AddFilterSetting("templates",
                new List<string> { "template_id" }, templateId);
            header.AddFilterSetting("templates",
                new List<string> { "enabled" }, "1");

            AddTemplateSubstitutions(header, substitutions);
        }

        private static void AddTemplateSubstitutions(Header header, Dictionary<string, string> substitutions)
        {
            foreach (var sub in substitutions.Keys)
            {
                header.AddSubstitution(sub, new List<string> { substitutions[sub] });
            }
        }
    }
}
