using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiSystems.ClientApp.Web.Domain.Services.Emails
{
    /// <summary>
    /// Used for building Contact Alumni email messages
    /// sent through SendGrid's SMTP API
    /// </summary>
    public class ContactAlumniEmail : TemplatedEmail
    {
        [TemplateSubstitution(Name = "-clientContactFullName-")]
        public string ClientContactFullName { get; set; }

        [TemplateSubstitution(Name = "-clientCompanyName-")]
        public string ClientCompanyName { get; set; }

        public ContactAlumniEmail()
        {
            TemplateId = Settings.ContactAlumniTemplateId;
            Categories = new[] { "Contact Alumni" };
        }
    }
}
