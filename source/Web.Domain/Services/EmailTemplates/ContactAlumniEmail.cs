
namespace SiSystems.ClientApp.Web.Domain.Services.EmailTemplates
{
    [Template(IdConfigPropertyName = "Email.ContactAlumniTemplateId", Categories="Contact Alumni")]
    public class ContactAlumniEmail : TemplatedEmail
    {
        [TemplateSubstitution]
        public string ClientContactFullName { get; set; }

        [TemplateSubstitution]
        public string ClientCompanyName { get; set; }
    }
}
