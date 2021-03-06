﻿using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SiSystems.ClientApp.Web.Domain.Repositories;
using SiSystems.SharedModels;

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
        private int _companyFourId = 4;

        [SetUp]
        public void Setup()
        {
            _repo = new ConsultantRepository();
        }

        [Test]
        public void FindAlumni_ByName_WhenExpectedHasMultipleSpecializations_ShouldBringBackMultipleSpecializationGroups()
        {
            const string searchText = "Tommy";

            var groups = _repo.Find(searchText, new List<int> { _companyOneId });

            Assert.AreEqual(2, groups.Count());
        }

        [Test]
        public void FindAlumni_ByName_ShouldNotBeAffectedByCase()
        {
            const string searchText = "tOmMy";
            var groups = _repo.Find(searchText, new List<int> { _companyOneId });

            Assert.IsTrue(groups.Any()
                && groups.All(c => GroupSpecializationMatchesText(c, searchText) || c.Consultants.All(con => ConsultantNameMatchesText(con, searchText))));
        }

        [Test]
        public void FindAlumni_ShouldMatchOnResumeTextPhrase()
        {
            const string searchText = "LOREM IPSUM";
            var groups = _repo.Find(searchText, new List<int> { _companyOneId });

            var consultants = groups.SelectMany(g => g.Consultants).Select(c => c.FirstName).Distinct().ToList();
            
            //\Database.MatchGuide\Scripts\014 - Add Candidate Resumes.sql
            Assert.AreEqual(2, consultants.Count);
            Assert.Contains("Tommy", consultants);
            Assert.Contains("Candice", consultants);
        }

        [Test]
        public void FindAlumni_ShouldMatch_WhenTargetHasContractWithClient()
        {
            const string searchText = "Bill";

            //Company Two has contract with bill
            var groups = _repo.Find(searchText, new List<int> { _companyTwoId });

            Assert.IsTrue(groups.Any());
        }

        [Test]
        public void FindAlumni_ShouldNotRelyOnWordOrder()
        {
            const string searchText = "IPSUM LOREM";
            var groups = _repo.Find(searchText, new List<int> { _companyOneId });

            var consultants = groups.SelectMany(g => g.Consultants).Select(c => c.FirstName).Distinct().ToList();

            //\Database.MatchGuide\Scripts\014 - Add Candidate Resumes.sql
            Assert.AreEqual(2, consultants.Count);
            Assert.Contains("Tommy", consultants);
            Assert.Contains("Candice", consultants);
        }

        [Test]
        public void FindAlumni_PassingMultipleCompanyIds_ShouldFetchForAll()
        {
            const string searchText = "";

            var groups = _repo.Find(searchText, new List<int> { _companyOneId, _companyFourId });

            var contractors = groups.SelectMany(g => g.Consultants);

            Assert.IsTrue(contractors.Any(c => c.FullName == "Tommy Contractor"));
            Assert.IsTrue(contractors.Any(c => c.FullName == "Candice Consulty"));

            Assert.IsTrue(groups.Any());
        }


        //Test Data Explanation
        //Candice has active contract with company three
        //and expired contract with company 1

        [Test]
        public void FindAlumni_ShouldNotIncludeActiveContracts()
        {
            const string searchText = "Candice";

            var groups = _repo.Find(searchText, new List<int> { _companyThreeId });

            Assert.IsTrue(!groups.Any());
        }

        [Test]
        public void FindAlumni_ShouldIncludeAlumniActiveForAnotherClient()
        {
            const string searchText = "Candice";

            var groups = _repo.Find(searchText, new List<int> { _companyOneId });

            Assert.IsTrue(groups.Any());
        }

        [Test]
        public void FindAlumni_ShouldBeNoMatch_WhenTargetHasNoContractWithClient()
        {
            const string searchText = "Candice";

            var groups = _repo.Find(searchText, new List<int> { _companyTwoId });

            Assert.IsFalse(groups.Any());
        }

        [Test]
        public void FindAlumni_WhenNoResumeInfo_RatingShouldBeNotChecked()
        {
            const string searchText = "Bill";

            var groups = _repo.Find(searchText, new List<int> { _companyTwoId });

            Assert.AreEqual((MatchGuideConstants.ResumeRating)MatchGuideConstants.ResumeRating.NotChecked, groups.First().Consultants.First().Rating);
        }

        [Test]
        public void Find_ShouldSortNoSpecializationToEndOfList()
        {
            var groups = _repo.Find(string.Empty, new[] { _companyTwoId });

            Assert.AreEqual(groups.Last().Specialization, string.Empty);
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

            var skills = consultant.Specializations.SelectMany(s => s.Skills);

            var skillNames = skills.Select(s => s.Name).ToList();

            Assert.AreEqual(4, skillNames.Count);
            Assert.Contains("Java", skillNames);
            Assert.Contains("C#", skillNames);
            Assert.Contains("7 Sigma", skillNames);
            Assert.Contains("ColdFusion", skillNames);

            //verify experience included and in range
            Assert.IsTrue(skills.Select(s => s.YearsOfExperience)
                .All(y => y >= MatchGuideConstants.YearsOfExperience.LessThanTwo && y <= MatchGuideConstants.YearsOfExperience.MoreThanTen));
        }

        [Test]
        public void Find_ShouldIncludePersonalDetails()
        {
            var consultant = _repo.Find(12);
            Assert.AreEqual(12, consultant.Id);
            Assert.AreEqual("Candice", consultant.FirstName);
            Assert.AreEqual("Consulty", consultant.LastName);
            Assert.AreEqual("Candice Consulty", consultant.FullName);
            Assert.AreEqual("candice.consulty@email.com", consultant.EmailAddress);
            Assert.AreEqual((MatchGuideConstants.ResumeRating)314, consultant.Rating);
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

        [Test]
        public void FindActive_ShouldReturnBillRate()
        {
            //sally.divisioner@email.com - Bill Rate: 201, Pay Rate: 191
            var consultantGroups = _repo.Find("", new List<int>{_companyFourId}, true);
            Assert.AreEqual(201, consultantGroups.First().Consultants.First().MostRecentContractRate);
        }

        [Test]
        public void FindAlumni_ShouldReturnBillRate()
        {
            //Tommy Contractor - Bill Rate: 98, Pay Rate: 88
            var consultantGroups = _repo.Find("", new List<int> { _companyOneId }, false);
            Assert.AreEqual(98, consultantGroups.First().Consultants.First().MostRecentContractRate);
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
