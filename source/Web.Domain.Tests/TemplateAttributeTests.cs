using Newtonsoft.Json;
using NUnit.Framework;
using SiSystems.ClientApp.Web.Domain.Services.EmailTemplates;

namespace SiSystems.ClientApp.Web.Domain.Tests
{
    [Template(IdConfigPropertyName = "TestConfigProperty")]
    public class TestEmailType : TemplatedEmail
    {
        [TemplateSubstitution(Name = "-OverriddenSubstitutionVariableName-")]
        public string MySubstitution { get; set; }
    }

    [TestFixture]
    public class TemplateAttributeTests
    {
        [Test]
        public void ToMailMessage_OnOverriddenSubstitution_ShouldRespectName()
        {
            var email = new TestEmailType
            {
                To = "someone@email.com",
                From = "someoneelse@email.com",
                Body = "HEY!",
                MySubstitution = "sub_value"
            };

            var mailMessage = email.ToMailMessage();
            var header = JsonConvert.DeserializeObject<dynamic>(mailMessage.Headers["X-SMTPAPI"]);

            Assert.AreEqual("sub_value", header.sub["-OverriddenSubstitutionVariableName-"][0].ToString());
        }

    }
}
