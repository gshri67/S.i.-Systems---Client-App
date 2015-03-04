﻿using System;
using ExpectedObjects;
using Moq;
using NUnit.Framework;
using SiSystems.ClientApp.SharedModels;
using SiSystems.ClientApp.Web.Domain.Context;
using SiSystems.ClientApp.Web.Domain.Repositories;
using SiSystems.ClientApp.Web.Domain.Services;
using SiSystems.ClientApp.Web.Domain.Services.EmailTemplates;

namespace SiSystems.ClientApp.Web.Domain.Tests
{
    [TestFixture]
    public class ContractProposalServiceTests
    {
        [Test]
        public void SendProposal_ShouldSendTemplatedEmail()
        {
            var consultantRepoMock = new Mock<IConsultantRepository>();
            var sessionContextMock = new Mock<ISessionContext>();
            var mailServiceMock = new Mock<SendGridMailService>();

            consultantRepoMock.Setup(m => m.Find(It.Is<int>(id => id == 0xBEEF)))
                .Returns(new Consultant
                {
                    Id = 0xBEEF,
                    EmailAddress = "someone@email.com"
                });

            sessionContextMock.Setup(s => s.CurrentUser)
                .Returns(new User
                {
                    ClientId = 1,
                    Login = "henry.bees@email.com",
                    FirstName = "Henry",
                    LastName = "Bees",
                    CompanyName = "Bees Systems"
                });

            var inputProposal = new ContractProposal
            {
                ClientId = 1337,
                ConsultantId = 0xBEEF,
                StartDate = new DateTime(2015, 01, 01),
                EndDate = new DateTime(2015, 12, 12),
                Fee = 123,
                RateToConsultant = 111,
                TimesheetApproverEmailAddress = "aguy@email.com",
                ContractApproverEmailAddress = "aguy@email.com"
            };

            var expectedEmail = new ContractProposalEmail
            {
                To = "someone@email.com",
                From = "henry.bees@email.com",
                Body = null,
                Fee = "$123.00",
                RateToConsultant = "$111.00",
                TotalRate = "$234.00",
                StartDate = "1/1/2015",
                EndDate = "12/12/2015",
                InvoiceFormat = string.Empty,
                TimesheetApproverEmailAddress = "aguy@email.com",
                ContractApproverEmailAddress = "aguy@email.com",
                ClientCompanyName = "Bees Systems",
                ClientContactFullName = "Henry Bees",
                ClientContactEmailAddress = "henry.bees@email.com"
            }.ToExpectedObject();

            var service = new ContractProposalService(consultantRepoMock.Object, sessionContextMock.Object, mailServiceMock.Object);

            service.SendProposal(inputProposal);

            // ReSharper disable once SuspiciousTypeConversion.Global
            mailServiceMock.Verify(m => m.SendTemplatedEmail(It.Is<ContractProposalEmail>(email => expectedEmail.Equals(email))), Times.Once);
        }
    }
}