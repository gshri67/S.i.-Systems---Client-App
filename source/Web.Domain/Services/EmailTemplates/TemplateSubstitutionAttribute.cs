using System;
using System.Configuration;

namespace SiSystems.ClientApp.Web.Domain.Services.EmailTemplates
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TemplateAttribute : Attribute
    {
        public string IdConfigPropertyName { get; set; }

        public string Id
        {
            get { return ConfigurationManager.AppSettings[IdConfigPropertyName]; }
        }

        public string Categories { get; set; }
    }
    
    /// <summary>
    /// Marks a property as a variable that is used for replacement in an email template
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class TemplateSubstitutionAttribute : Attribute
    {
        public string Name { get; set; }
    }
}
