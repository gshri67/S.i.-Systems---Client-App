using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using SendGrid.SmtpApi;

namespace SiSystems.ClientApp.Web.Domain.Services.EmailTemplates
{
    public abstract class TemplatedEmail
    {
        public string To { get; set; }
        public string From { get; set; }

        public string Body { get; set; }
        
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

            SetTemplate(header, GetTemplateId(), GetSubstitutions());
            header.SetCategories(GetCategories());

            mail.Headers.Add("X-SMTPAPI", header.JsonString());
        }

        private string GetTemplateId()
        {
            var templateAttr = GetType().GetCustomAttributes(typeof(TemplateAttribute), true).FirstOrDefault();
            if (templateAttr != null)
            {
                return ((TemplateAttribute)templateAttr).Id;
            }
            throw new Exception("Missing Template attribute on TemplatedEmail instance.");
        }

        protected Dictionary<string, string> GetSubstitutions()
        {
            var props = from p in GetType().GetProperties()
                        let attr = p.GetCustomAttributes(typeof(TemplateSubstitutionAttribute), true)
                        where attr.Length == 1
                        select new { Property = p, Attribute = attr.First() as TemplateSubstitutionAttribute };

            var subs = new Dictionary<string, string>();
            foreach (var prop in props)
            {
                var propName = GetSubstitutionVariableName(prop.Attribute, prop.Property);
                var value = prop.Property.GetValue(this) as string;

                if(value==null)
                    throw new InvalidOperationException("Substitution value cannot be null: "+propName);
                
                subs[propName] = value;
            }
            return subs;
        }

        private IEnumerable<string> GetCategories()
        {
            var templateAttr = GetType().GetCustomAttributes(typeof(TemplateAttribute), true).FirstOrDefault();
            if (templateAttr != null)
            {
                var categories = ((TemplateAttribute) templateAttr).Categories;
                return (categories??string.Empty).Split(',').Select(s=>s.Trim());
            }
            throw new Exception("Missing Template attribute on TemplatedEmail instance.");
        } 

        /// <summary>
        /// If Name is set on TemplateSubstitutionAttribute, we'll use that,
        /// otherwise we'll assume default and wrap the property name in []
        /// </summary>
        private string GetSubstitutionVariableName(TemplateSubstitutionAttribute attr, PropertyInfo propertyInfo)
        {
            if (!string.IsNullOrEmpty(attr.Name))
                return attr.Name;

            return string.Format("[{0}]", propertyInfo.Name);
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
