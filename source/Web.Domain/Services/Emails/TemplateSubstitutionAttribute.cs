using System;

namespace SiSystems.ClientApp.Web.Domain.Services.Emails
{
    public class TemplateSubstitutionAttribute : Attribute
    {
        public string Name { get; set; }
    }
}
