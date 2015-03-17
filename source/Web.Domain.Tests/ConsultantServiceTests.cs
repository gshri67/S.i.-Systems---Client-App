using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using SiSystems.ClientApp.SharedModels;
using SiSystems.ClientApp.Web.Domain.Context;
using SiSystems.ClientApp.Web.Domain.Repositories;
using SiSystems.ClientApp.Web.Domain.Services;

namespace SiSystems.ClientApp.Web.Domain.Tests
{
    [TestFixture]
    public class ConsultantServiceTests
    {
        private Mock<ISessionContext> _sessionContextMock;
        private Mock<ICompanyRepository> _companyRepositoryMock;

        private const int CurrentUserClientId = 1;
        private const int AnotherClientId = 2;

        [SetUp]
        public void Setup()
        {
            _sessionContextMock = new Mock<ISessionContext>();
            _sessionContextMock.Setup(m => m.CurrentUser).Returns(() => new User
            {
                CompanyId = CurrentUserClientId,
                FirstName = "Test",
                LastName = "User",
                Login = "Test.User",
                PasswordHash = "#",
                ClientsMaxVisibleRate = 20,
                ClientPortalType = 833
            });

            _companyRepositoryMock = new Mock<ICompanyRepository>();
            _companyRepositoryMock.Setup(m => m.GetAllAssociatedCompanyIds(It.IsAny<int>()))
                .Returns<int>((id) => new List<int> { id });
        }

        [Test]
        public void FindAlumni_WhenNoMutualContract_ShouldThrowUnauthorized()
        {
            var repo = new Mock<IConsultantRepository>();
            repo.Setup(m => m.Find(It.IsAny<int>())).Returns(
                new Consultant
                {
                    Contracts = new List<Contract>()
                });

            var service = new ConsultantService(repo.Object, _companyRepositoryMock.Object, _sessionContextMock.Object);
            Assert.Throws<UnauthorizedAccessException>(()=> service.Find(10));
        }

        [Test]
        public void FindAlumni_WhenMutualFloThruContract_ShouldNotThrow()
        {
            var repo = new Mock<IConsultantRepository>();
            repo.Setup(m => m.Find(It.IsAny<int>())).Returns(
                new Consultant
                {
                    Contracts = new List<Contract>
                    {
                        new Contract{ClientId = CurrentUserClientId, AgreementSubType = MatchGuideConstants.AgreementSubTypes.FloThru}
                    }
                });

            var service = new ConsultantService(repo.Object, _companyRepositoryMock.Object, _sessionContextMock.Object);
            Assert.DoesNotThrow(() => service.Find(10));
        }

        [Test]
        public void FindAlumni_WhenMultipleContracts_ShouldFilterOutContractsWithOtherClients()
        {
            var repo = new Mock<IConsultantRepository>();
            repo.Setup(m => m.Find(It.IsAny<int>())).Returns(
                new Consultant
                {
                    Contracts = new List<Contract>
                    {
                        new Contract{ClientId = CurrentUserClientId, AgreementSubType = MatchGuideConstants.AgreementSubTypes.FloThru},
                        new Contract{ClientId = AnotherClientId, AgreementSubType = MatchGuideConstants.AgreementSubTypes.FloThru},
                        new Contract{ClientId = CurrentUserClientId, AgreementSubType = MatchGuideConstants.AgreementSubTypes.FloThru}
                    }
                });

            var service = new ConsultantService(repo.Object, _companyRepositoryMock.Object, _sessionContextMock.Object);

            var consultant = service.Find(10);

            Assert.AreEqual(2, consultant.Contracts.Count());
            Assert.IsTrue(consultant.Contracts.All(c=>c.ClientId==CurrentUserClientId));
        }

        [Test]
        public void FindAlumni_ResultsShouldBeOrderedBySpecializationGroupSize()
        {
            var repo = new Mock<IConsultantRepository>();
            repo.Setup(m => m.Find(It.IsAny<string>(), It.IsAny<IEnumerable<int>>(), false))
                .Returns(new List<ConsultantGroup>
                {
                    new ConsultantGroup
                    {
                        Specialization = "Specialization with One Consultant",
                        Consultants = new List<ConsultantSummary>
                        {
                            new ConsultantSummary{}
                        }
                    },
                    new ConsultantGroup
                    {
                        Specialization = "Specialization with Two Consultants",
                        Consultants = new List<ConsultantSummary>
                        {
                            new ConsultantSummary(),
                            new ConsultantSummary(),
                        }
                    }
                });

            var service = new ConsultantService(repo.Object, _companyRepositoryMock.Object, _sessionContextMock.Object);

            var results = service.FindAlumni("Java");

            //Expect order to be reversed from repository result..
            Assert.AreEqual("Specialization with Two Consultants", results.First().Specialization);
        }

        [Test]
        public void FindAlumni_ConsultantsShouldBeOrderedByRating()
        {
            var repo = new Mock<IConsultantRepository>();
            var expectedOrderedResult = new List<ConsultantGroup>
            {
                new ConsultantGroup
                {
                    Specialization = "Javaers",
                    Consultants = new List<ConsultantSummary>
                    {
                        new ConsultantSummary {Id = 5, Rating = 1234},
                        new ConsultantSummary {Id = 6, Rating = null},
                        new ConsultantSummary {Id = 1, Rating = MatchGuideConstants.ResumeRating.AboveStandard},
                        new ConsultantSummary {Id = 2, Rating = MatchGuideConstants.ResumeRating.Standard},
                        new ConsultantSummary {Id = 3, Rating = MatchGuideConstants.ResumeRating.NotChecked},
                        new ConsultantSummary {Id = 4, Rating = MatchGuideConstants.ResumeRating.BelowStandard},
                    }
                }
            };
            repo.Setup(m => m.Find(It.IsAny<string>(), It.IsAny<IEnumerable<int>>(), false))
                .Returns(expectedOrderedResult);

            var service = new ConsultantService(repo.Object, _companyRepositoryMock.Object, _sessionContextMock.Object);

            var results = service.FindAlumni("Java");

            Assert.AreEqual(MatchGuideConstants.ResumeRating.AboveStandard, (int)results.First().Consultants.Select(e => e.Rating).First());
            Assert.AreEqual(0, (int)results.First().Consultants.Select(e => e.Rating).Last().GetValueOrDefault());
        }

        [Test]
        public void FindAlumni_ConsultantsWithLowRatingsShouldNotBeFiltered()
        {
            var repo = new Mock<IConsultantRepository>();
            repo.Setup(m => m.Find(It.IsAny<string>(), It.IsAny<IEnumerable<int>>(), false))
                .Returns(new List<ConsultantGroup>
                {
                    new ConsultantGroup
                    {
                        Specialization = "Javaers",
                        Consultants = new List<ConsultantSummary>
                        {
                            new ConsultantSummary{Rating = MatchGuideConstants.ResumeRating.NotChecked},
                            new ConsultantSummary{Rating = MatchGuideConstants.ResumeRating.AboveStandard},
                            new ConsultantSummary{Rating = MatchGuideConstants.ResumeRating.BelowStandard},
                        }
                    }
                });

            var service = new ConsultantService(repo.Object, _companyRepositoryMock.Object, _sessionContextMock.Object);

            var results = service.FindAlumni("");

            var ratings = results.SelectMany(g => g.Consultants).Select(c => c.Rating).ToList();

            Assert.Contains((MatchGuideConstants.ResumeRating)MatchGuideConstants.ResumeRating.BelowStandard, ratings);
        }

        [Test]
        public void FindAlumni_ShouldHideRatesAboveMaxAllowedRate()
        {
            var repo = new Mock<IConsultantRepository>();
            repo.Setup(m => m.Find(It.IsAny<string>(), It.IsAny<IEnumerable<int>>(), false))
                .Returns(new List<ConsultantGroup>
                {
                    new ConsultantGroup
                    {
                        Specialization = "Javaers",
                        Consultants = new List<ConsultantSummary>
                        {
                            new ConsultantSummary{MostRecentContractRate = 10},
                            new ConsultantSummary{MostRecentContractRate = 20},
                            new ConsultantSummary{MostRecentContractRate = 30},
                        }
                    }
                });

            var service = new ConsultantService(repo.Object, _companyRepositoryMock.Object, _sessionContextMock.Object);

            var summaries = service.FindAlumni(string.Empty).SelectMany(s => s.Consultants).OrderBy(s => s.MostRecentContractRate).ToArray();

            Assert.IsTrue(summaries[0].RateWitheld);
            Assert.IsFalse(summaries[1].RateWitheld);
            Assert.IsFalse(summaries[2].RateWitheld);
            Assert.AreEqual(0, summaries[0].MostRecentContractRate);
            Assert.AreEqual(10, summaries[1].MostRecentContractRate);
            Assert.AreEqual(20, summaries[2].MostRecentContractRate);
        }
    }
}
