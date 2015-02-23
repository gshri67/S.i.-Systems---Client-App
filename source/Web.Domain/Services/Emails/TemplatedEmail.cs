using System.Collections.Generic;
using System.Net.Mail;
using SendGrid.SmtpApi;
using System.Linq;

namespace SiSystems.ClientApp.Web.Domain.Services.Emails
{
    public abstract class TemplatedEmail
    {
        protected string TemplateId { get; set; }

        public string To { get; set; }
        public string From { get; set; }

        public string Body { get; set; }
        
        protected IList<string> Categories { get; set; }
        
        public MailMessage ToMailMessage()
        {
            //Configuration override for use in DEV & TEST environments 
            //to prevent emails being sent to actual users
            var recipient = Settings.EmailRecipientOverride ?? To;

            MailMessage mail = new MailMessage
            {
                IsBodyHtml = true, 
                Body = ReplaceNewLinesWithBreakTags(Body)
            };

            mail.To.Add(new MailAddress(recipient));

            //mail.From is handled by SMTP configuration

            mail.ReplyToList.Add(new MailAddress(From));

            //Add SMTP header required for SendGrid to parse and process
            //template substitutions
            SetSmtpApiHeader(mail);

            return mail;
        }

        private void SetSmtpApiHeader(MailMessage mail)
        {
            var header = new Header();
            SetTemplate(header, TemplateId, GetSubstitutions());
            header.SetCategories(Categories);
            mail.Headers.Add("X-SMTPAPI", header.JsonString());
        }
        
        protected Dictionary<string, string> GetSubstitutions()
        {
            var props = from p in GetType().GetProperties()
                let attr = p.GetCustomAttributes(typeof(TemplateSubstitutionAttribute), true)
                where attr.Length == 1
                select new { Property = p, Attribute = attr.First() as TemplateSubstitutionAttribute};

            return props.ToDictionary(prop => prop.Attribute.Name, prop => prop.Property.GetValue(this) as string);
        }

        private static string ReplaceNewLinesWithBreakTags(string content)
        {
            return (content ?? string.Empty)
                .Replace("\n", "<br/>")
                .Replace("\r\n", "<br/>");
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
