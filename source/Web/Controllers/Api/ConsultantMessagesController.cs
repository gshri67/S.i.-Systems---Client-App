using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using SiSystems.ClientApp.Web.Domain;
using SiSystems.ClientApp.Web.Models;

namespace SiSystems.ClientApp.Web.Controllers.Api
{
    public class ConsultantMessagesController : ApiController
    {
        private readonly ConsultantMessageService _service;
        public ConsultantMessagesController(ConsultantMessageService service)
        {
            _service = service;
        }

        //To test email handling in dev environment,
        //Sign in to SendGrid
        //Start up ngrok (https://ngrok.com/) on your local machine and point it to the Web app port (50021)
        //Head to https://sendgrid.com/developer/reply and log in
        //Set it.infrastructure.bymail.in Host entry to point to your ngrok URL +/api/ConsultantMessages
        //Send an email to someuser@it.infrastructure.bymail.in
        //This method should be called.. Use a breakpoint.
        public async Task<HttpResponseMessage> Post()
        {
            var root = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new MultipartFormDataStreamProvider(root);
            await Request.Content.ReadAsMultipartAsync(provider);

            var email = new InboundEmail
            {
                Dkim = provider.FormData.GetValues("dkim").FirstOrDefault(),
                To = provider.FormData.GetValues("to").FirstOrDefault(),
                Html = provider.FormData.GetValues("html").FirstOrDefault(),
                From = provider.FormData.GetValues("from").FirstOrDefault(),
                Text = provider.FormData.GetValues("text").FirstOrDefault(),
                SenderIp = provider.FormData.GetValues("sender_ip").FirstOrDefault(),
                Envelope = provider.FormData.GetValues("envelope").FirstOrDefault(),
                Attachments = int.Parse(provider.FormData.GetValues("attachments").FirstOrDefault()),
                Subject = provider.FormData.GetValues("subject").FirstOrDefault(),
                Charsets = provider.FormData.GetValues("charsets").FirstOrDefault(),
                Spf = provider.FormData.GetValues("spf").FirstOrDefault()
            };

            //got email..
            //now send one...
            ForwardEmail(email);
            
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        private void ForwardEmail(InboundEmail email)
        {
            MailMessage mailMsg = new MailMessage();

            // To
            mailMsg.To.Add(new MailAddress(email.To.Replace("it.infrastructure.bymail.in", "devfacto.com")));

            // From
            mailMsg.From = new MailAddress("noreply@it.infrastructure.bymail.in", "S.i. Systems");

            // Subject and multipart/alternative Body
            mailMsg.Subject = email.Subject;
            
            string text = email.Text;
            string html = email.Html;

            mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
            mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));

            // Init SmtpClient and send
            SmtpClient smtpClient = new SmtpClient();

            smtpClient.Send(mailMsg);
      }

        //public HttpResponseMessage Post(ConsultantMessage message)
        //{
        //    _service.SendConsultantMessage(message);
        //    return Request.CreateResponse(HttpStatusCode.OK);
        //}
    }
}
