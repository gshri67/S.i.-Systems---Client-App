using System;
using Newtonsoft.Json;
using NUnit.Framework;
using SiSystems.ClientApp.Web.Domain.Services.EmailTemplates;

namespace SiSystems.ClientApp.Web.Domain.Tests
{
    [TestFixture]
    public class TemplatedEmailTests
    {
        private ContactAlumniEmail CreateSimpleContactAlumniEmail()
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

        private ContractProposalEmail CreateSimpleContractProposalEmail()
        {
            return new ContractProposalEmail
            {
                To = "someone@email.com",
                From = "person@email.com",
                Body = "HELLO",
                Fee = "123.00",
                RateToConsultant = "111.00",
                TotalRate = "134.00",
                StartDate = "1/1/2015",
                EndDate = "12/12/2015",
                TimesheetApproverEmailAddress = "aguy@email.com",
                ContractApproverEmailAddress = "aguy@email.com",
                InvoiceFormat = "bits and bytes",
                ClientCompanyName = "Bees Systems",
                ClientContactFullName = "Henry Bees",
                ClientContactEmailAddress = "henry.bees@email.com",
                Specialization = "Testing"
            };
        }

        [Test]
        public void ContactAlumni_ToMailMessage_ShouldContainSmtpApiHeader()
        {
            var mailMessage = CreateSimpleContactAlumniEmail().ToMailMessage();

            Assert.IsNotNull(mailMessage.Headers["X-SMTPAPI"]);
        }

        [Test]
        public void ContactAlumni_ToMailMessage_HeaderShouldEnableExpectedTemplate()
        {
            var mailMessage = CreateSimpleContactAlumniEmail().ToMailMessage();

            var header = JsonConvert.DeserializeObject<dynamic>(mailMessage.Headers["X-SMTPAPI"]);
            var templateId = header.filters.templates.settings.template_id.ToString();
            var isEnabled = header.filters.templates.settings.enabled.ToString();

            //id set in config file..
            Assert.AreEqual("2b727004-d8aa-4cea-a01e-f91e4be38dfa", templateId);
            Assert.AreEqual("1", isEnabled);
        }

        [Test]
        public void ContactAlumni_ToMailMessage_HeaderShouldContainExpectedSubstitutions()
        {
            var mailMessage = CreateSimpleContactAlumniEmail().ToMailMessage();
            var header = JsonConvert.DeserializeObject<dynamic>(mailMessage.Headers["X-SMTPAPI"]);

            Assert.AreEqual("Bees Systems", header.sub["[ClientCompanyName]"][0].ToString());
            Assert.AreEqual("Henry Bees", header.sub["[ClientContactFullName]"][0].ToString());
        }

        [Test]
        public void ContactAlumni_ToMailMessage_HeaderShouldContainExpectedCategories()
        {
            var mailMessage = CreateSimpleContactAlumniEmail().ToMailMessage();
            var header = JsonConvert.DeserializeObject<dynamic>(mailMessage.Headers["X-SMTPAPI"]);

            var category = header.category[0];

            Assert.AreEqual("Contact Alumni", category.ToString());

        }

        [Test]
        public void ContractProposal_ToMailMessage_HeaderShouldContainExpectedSubstitutions()
        {
            var mailMessage = CreateSimpleContractProposalEmail().ToMailMessage();
            var header = JsonConvert.DeserializeObject<dynamic>(mailMessage.Headers["X-SMTPAPI"]);
            
            Assert.AreEqual("bits and bytes", header.sub["[InvoiceFormat]"][0].ToString());
        }

        [Test]
        public void ContractProposal_ToMailMessage_WhenSubstitutionValueMissing_ShouldThrow()
        {
            var templatedEmail = new ContractProposalEmail{To="someone@email.com", From="someoneelse@email.com"};
            Assert.Throws<InvalidOperationException>(() => templatedEmail.ToMailMessage());
        }

    }
}
