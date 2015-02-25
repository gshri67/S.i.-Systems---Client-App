using System.Net.Mail;
using SiSystems.ClientApp.Web.Domain.Services.EmailTemplates;

namespace SiSystems.ClientApp.Web.Domain.Services
{
    public class SendGridMailService
    {
        public virtual void SendTemplatedEmail(TemplatedEmail email)
        {
            SmtpClient client = new SmtpClient();
            client.Send(email.ToMailMessage());
        }
    }
}
