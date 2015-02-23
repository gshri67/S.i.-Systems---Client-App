using Newtonsoft.Json;
using NUnit.Framework;
using SiSystems.ClientApp.Web.Domain.Services.Emails;

namespace SiSystems.ClientApp.Web.Domain.Tests
{
    [TestFixture]
    public class TemplatedEmailTests
    {
        private ContactAlumniEmail CreateSimpleMessage()
        {
            return new ContactAlumniEmail
            {
                To = "someone@email.com",
                From = "person@email.com",
                Body = "HELLO",
                ClientCompanyName = "Bees Systems",
                ClientContactFullName = "Henry Bees"
            };
        }

        [Test]
        public void ToMailMessage_ShouldContainSmtpApiHeader()
        {
            var mailMessage = CreateSimpleMessage().ToMailMessage();

            Assert.IsNotNull(mailMessage.Headers["X-SMTPAPI"]);
        }

        [Test]
        public void ToMailMessage_HeaderShouldEnableExpectedTemplate()
        {
            var mailMessage = CreateSimpleMessage().ToMailMessage();

            var header = JsonConvert.DeserializeObject<dynamic>(mailMessage.Headers["X-SMTPAPI"]);
            var templateId = header.filters.templates.settings.template_id.ToString();
            var isEnabled = header.filters.templates.settings.enabled.ToString();

            Assert.AreEqual(Settings.ContactAlumniTemplateId, templateId);
            Assert.AreEqual("1", isEnabled);
        }

        [Test]
        public void ToMailMessage_HeaderShouldContainExpectedSubstitutions()
        {
            var mailMessage = CreateSimpleMessage().ToMailMessage();
            var header = JsonConvert.DeserializeObject<dynamic>(mailMessage.Headers["X-SMTPAPI"]);

            Assert.AreEqual("Bees Systems", header.sub["-clientCompanyName-"][0].ToString());
            Assert.AreEqual("Henry Bees", header.sub["-clientContactFullName-"][0].ToString());
        }

        [Test]
        public void ToMailMessage_HeaderShouldContainExpectedCategories()
        {
            var mailMessage = CreateSimpleMessage().ToMailMessage();
            var header = JsonConvert.DeserializeObject<dynamic>(mailMessage.Headers["X-SMTPAPI"]);

            var category = header.category[0];

            Assert.AreEqual("Contact Alumni", category.ToString());

        }

    }
}
