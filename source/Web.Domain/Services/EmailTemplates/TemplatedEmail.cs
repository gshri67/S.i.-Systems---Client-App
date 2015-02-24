using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using SendGrid.SmtpApi;

namespace SiSystems.ClientApp.Web.Domain.Services.EmailTemplates
{
    /// <summary>
    /// Use to build templated emails
    /// sent through SendGrid's SMTP API
    /// </summary>
    public abstract class TemplatedEmail
    {
        public string To { get; set; }
        public string From { get; set; }

        public string Body { get; set; }
        
        public MailMessage ToMailMessage()
        {
            var mail = new MailMessage
            {
                IsBodyHtml = true,
                Body = FormatAsHtml(Body)
            };

            //Add SMTP header required for SendGrid to parse and process
            //template substitutions
            var header = BuildSendGridSmtpApiHeader();
            mail.Headers.Add("X-SMTPAPI", header.JsonString());

            //Configuration override for use in DEV & TEST environments 
            //to prevent emails being sent to actual users
            var recipient = Settings.EmailRecipientOverride ?? To;
            mail.To.Add(new MailAddress(recipient));

            //mail.From is handled by SMTP configuration

            mail.ReplyToList.Add(new MailAddress(From));


            return mail;
        }

        private Header BuildSendGridSmtpApiHeader()
        {
            var header = new Header();

            var templateId = GetTemplateId();
            var substitutions = GetSubstitutions();
            var categories = GetCategories();

            header.AddFilterSetting("templates", new List<string> { "template_id" }, templateId);
            header.AddFilterSetting("templates", new List<string> { "enabled" }, "1");

            foreach (var sub in substitutions.Keys)
            {
                header.AddSubstitution(sub, new List<string> { substitutions[sub] });
            }

            header.SetCategories(categories);
            return header;
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

        /// <summary>
        /// Extract template substitutions for SendGrid template engine.
        /// Substitution properties are marked with the TemplateSubstitutionAttribute.
        /// </summary>
        protected Dictionary<string, string> GetSubstitutions()
        {
            var props = from p in GetType().GetProperties()
                        let attr = p.GetCustomAttributes(typeof(TemplateSubstitutionAttribute), true)
                        where attr.Length == 1
                        select new { Property = p, Attribute = attr.First() as TemplateSubstitutionAttribute };

            var subs = new Dictionary<string, string>();
            foreach (var prop in props)
            {
                //Template substitution variable name
                //If TemplateAttribute.Name is set, we'll use that,
                //otherwise our default is [PropertyName]
                var propName = !string.IsNullOrEmpty(prop.Attribute.Name) ? prop.Attribute.Name :  string.Format("[{0}]", prop.Property.Name);

                var value = prop.Property.GetValue(this) as string;

                if(value==null)
                    throw new InvalidOperationException("Substitution value cannot be null: "+propName);
                
                subs[propName] = value;
            }
            return subs;
        }

        /// <summary>
        /// Extract categories from TemplateAttribute on child type.
        /// </summary>
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


        private static string FormatAsHtml(string content)
        {
            return (content ?? string.Empty)
                .Replace("\n", "<br/>")
                .Replace("\r\n", "<br/>");
        }
    }
}
