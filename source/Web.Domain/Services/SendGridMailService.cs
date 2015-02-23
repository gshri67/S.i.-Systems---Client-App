using System.Net.Mail;
using SiSystems.ClientApp.Web.Domain.Services.Emails;

namespace SiSystems.ClientApp.Web.Domain.Services
{
    class SendGridMailService
    {
        public void SendTemplatedEmail(TemplatedEmail email)
        {
            SmtpClient client = new SmtpClient();
            client.Send(email.ToMailMessage());
        }
    }
}
