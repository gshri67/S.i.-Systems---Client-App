
namespace SiSystems.ClientApp.Web.Domain.Services.EmailTemplates
{
    /// <summary>
    /// Describes Contact Alumni email messages
    /// sent through SendGrid's SMTP API
    /// </summary>
    [Template(IdConfigPropertyName = "Email.ContactAlumniTemplateId", Categories="Contact Alumni")]
    public class ContactAlumniEmail : TemplatedEmail
    {
        [TemplateSubstitution]
        public string ClientContactFullName { get; set; }

        [TemplateSubstitution]
        public string ClientCompanyName { get; set; }
    }
}
