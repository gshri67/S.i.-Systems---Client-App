using System.Linq;
using NUnit.Framework;
using SiSystems.ClientApp.Web.Domain.Repositories;
using SiSystems.ClientApp.SharedModels;

namespace SiSystems.ClientApp.Web.Domain.Tests
{
    [TestFixture]
    [Category("Database")]
    public class ConsultantRepositoryTests
    {
        private ConsultantRepository _repo;
        private int _companyOneId = 1;
        private int _companyTwoId = 2;
        private int _companyThreeId = 3;

        [SetUp]
        public void Setup()
        {
            _repo = new ConsultantRepository();
        }

        [Test]
        public void FindAlumni_ByName_WhenExpectedHasMultipleSpecializations_ShouldBringBackMultipleSpecializationGroups()
        {
            const string searchText = "Tommy";

            var groups = _repo.FindAlumni(searchText, _companyOneId);

            Assert.AreEqual(2, groups.Count());
        }

        [Test]
        public void FindAlumni_ByName_ShouldNotBeAffectedByCase()
        {
            const string searchText = "tOmMy";
            var groups = _repo.FindAlumni(searchText, _companyOneId);

            Assert.IsTrue(groups.Any()
                && groups.All(c => GroupSpecializationMatchesText(c, searchText) || c.Consultants.All(con => ConsultantNameMatchesText(con, searchText))));
        }

        [Test]
        public void FindAlumni_ShouldMatch_WhenTargetHasContractWithClient()
        {
            const string searchText = "Bill";

            //Company Two has contract with bill
            var groups = _repo.FindAlumni(searchText, _companyTwoId);

            Assert.IsTrue(groups.Any());
        }


        //Test Data Explanation
        //Candice has active contract with company three
        //and expired contract with company 1

        [Test]
        public void FindAlumni_ShouldNotIncludeActiveContracts()
        {
            const string searchText = "Candice";

            var groups = _repo.FindAlumni(searchText, _companyThreeId);

            Assert.IsTrue(!groups.Any());
        }

        [Test]
        public void FindAlumni_ShouldIncludeAlumniActiveForAnotherClient()
        {
            const string searchText = "Candice";

            var groups = _repo.FindAlumni(searchText, _companyOneId);

            Assert.IsTrue(groups.Any());
        }

        [Test]
        public void FindAlumni_ShouldBeNoMatch_WhenTargetHasNoContractWithClient()
        {
            const string searchText = "Candice";
            
            var groups = _repo.FindAlumni(searchText, _companyTwoId);

            Assert.IsFalse(groups.Any());
        }

        [Test]
        public void FindAlumni_WhenNoResumeInfo_RatingShouldBeNotChecked()
        {
            const string searchText = "Bill"; 

            var groups = _repo.FindAlumni(searchText, _companyTwoId);

            Assert.AreEqual(MatchGuideConstants.ResumeRating.NotChecked, groups.First().Consultants.First().Rating);
        }

        [Test]
        public void Find_ShouldIncludeSpecializationsAndSkills()
        {
            //Tommy Consultant
            //Database.MatchGuide\Scripts\013 - Add Candidate Specializations.sql
            var consultant = _repo.Find(10);

            var specializationNames = consultant.Specializations.Select(s => s.Name).ToList();

            Assert.Contains("Project Management", specializationNames);
            Assert.Contains("Software Development", specializationNames);

            var skillNames = consultant.Specializations.Select(s => s.Skills.Select(sk=>sk.Name)).SelectMany(s=>s).ToList();
            Assert.AreEqual(4, skillNames.Count);
            Assert.Contains("Java", skillNames);
            Assert.Contains("C#", skillNames);
            Assert.Contains("7 Sigma", skillNames);
            Assert.Contains("ColdFusion", skillNames);
        }

        [Test]
        public void Find_ShouldIncludeResumeText()
        {
            var consultant = _repo.Find(12);
            var expectedResumeText = "Candice Consulty Resume text lorem ipsum and such..";
            Assert.AreEqual(expectedResumeText, consultant.ResumeText);
        }

        [Test]
        public void Find_WhenNoResume_ResumeTextShouldBeNull()
        {
            var consultant = _repo.Find(11);
            Assert.IsNull(consultant.ResumeText);
        }


        private bool GroupSpecializationMatchesText(ConsultantGroup group, string text)
        {
            return group.Specialization.ToUpperInvariant().Contains(text.ToUpperInvariant());
        }

        private bool ConsultantNameMatchesText(ConsultantSummary consultant, string text)
        {
            return consultant.FullName.ToUpperInvariant().Contains(text.ToUpperInvariant());
        }

    }
}
