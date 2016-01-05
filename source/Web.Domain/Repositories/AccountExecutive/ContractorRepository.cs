using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using SiSystems.SharedModels;

namespace SiSystems.ClientApp.Web.Domain.Repositories.AccountExecutive
{
    public interface IContractorRepository
    {
        Contractor GetContractorById(int id);
        IEnumerable<int> GetShortlistedContractorsByJobId(int id);
        IEnumerable<int> GetProposedContractorsByJobId(int id);
        IEnumerable<int> GetCalloutContractorsByJobId(int id);
        int GetContractorIdByContractId(int contractId);
        //ContractorRateSummary GetContractorRateSummaryByContractId(int contractId);
        //ContractorRateSummary GetContractorRateSummaryByJobId(int contractId);
        IEnumerable<Contractor> GetContractors();
    }
    
    public class ContractorRepository : IContractorRepository
    {

        public IEnumerable<Specialization> GetSpecializationsByUserId(int id)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string specializationQuery = @"set nocount on
                                                        DECLARE @t TABLE(Id int, SpecializationId int, Name varchar(50), YearsOfExperience int)
                                                        insert @t
	                                                        SELECT s.SkillId, cs.SpecID, SkillName, cs.ExpID
	                                                        FROM Skill s
	                                                        JOIN Candidate_SkillsMatrix cs on cs.SkillID = s.SkillID
	                                                        WHERE UserId = @UserId
                                                        set nocount off 
                                                        SELECT * FROM @t
                                                        SELECT SpecializationId Id, Name, Description FROM Specialization WHERE SpecializationID in (select t.SpecializationId from @t t)
                                                        ORDER BY Name";

                var multi = db.Connection.QueryMultiple(specializationQuery, new { UserId = id });
                var skills = multi.Read<Skill>();
                var specializations = multi.Read<Specialization>();
                foreach (var specialization in specializations)
                {
                    specialization.Skills = skills.Where(sk => sk.SpecializationId == specialization.Id);
                }

                // Move the "No Specialization" to the end
                var noSpecialization = specializations.SingleOrDefault(s => string.IsNullOrWhiteSpace(s.Name));
                var sortedSpecializations = specializations.ToList();
                if (noSpecialization != null && sortedSpecializations.Remove(noSpecialization))
                {
                    sortedSpecializations.Add(noSpecialization);
                }

                return specializations;
            }
        }

        public Contractor GetContractorById(int id)
        {
            const string resumeAndRatingQuery =
                @"SELECT ResumeInfo.ResumeText, ResumeInfo.ReferenceValue
                FROM Users Candidate 
                LEFT JOIN Candidate_ResumeInfo ResumeInfo ON Candidate.UserID = ResumeInfo.UserID
                WHERE Candidate.UserId = @Id";
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {

                var contractor = db.Connection.Query<Contractor>(resumeAndRatingQuery, new { Id = id }).FirstOrDefault() ??
                                 new Contractor();

                contractor.Specializations = GetSpecializationsByUserId(id);

                return contractor;
            }
        }

        public IEnumerable<int> GetShortlistedContractorsByJobId(int id)
        {
            const string contractorsQuery =
                @"SELECT DISTINCT(Matrix.CandidateUserID)
                FROM Agreement
                JOIN PickList ON PickList.PickListID = Agreement.StatusType
                JOIN Agreement_OpportunityCandidateMatrix Matrix on Agreement.AgreementID = Matrix.AgreementID
                WHERE Agreement.AgreementType IN(
	                SELECT PickListId from dbo.udf_GetPickListIds('agreementtype', 'opportunity', -1)
                )
                AND PickList.PickListID IN (
	                SELECT PickListId from dbo.udf_GetPickListIds('OpportunityStatusType', 'Open,On Hold,Submissions Complete', -1)
                )
                AND Agreement.AgreementID = @Id
                AND Matrix.StatusType IN (
	                SELECT PickListId from dbo.udf_GetPickListIds('CandidateOpportunityStatusType', 'Short List', -1)
                )
                AND Matrix.StatusSubType IN (
	                SELECT PickListId from dbo.udf_GetPickListIds('CandidateOpportunitySubStatusType', 'Pending,Placed,Proposed', -1)
                )";

            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {

                var contractors = db.Connection.Query<int>(contractorsQuery, new { Id = id });
                
                return contractors;
            }
        }

        public IEnumerable<int> GetProposedContractorsByJobId(int id)
        {
            const string contractorsQuery =
                @"SELECT DISTINCT(ActivityTransaction.CandidateUserID)
                FROM Agreement
                JOIN PickList ON PickList.PickListID = Agreement.StatusType
                JOIN ActivityTransaction ON ActivityTransaction.AgreementID = Agreement.AgreementID
                JOIN ActivityType ON ActivityType.ActivityTypeID = ActivityTransaction.ActivityTypeID
                WHERE Agreement.AgreementType IN(
	                SELECT PickListId from dbo.udf_GetPickListIds('agreementtype', 'opportunity', -1)
                )
                AND PickList.PickListID IN (
	                SELECT PickListId from dbo.udf_GetPickListIds('OpportunityStatusType', 'Open,On Hold,Submissions Complete', -1)
                )
                AND Agreement.AgreementID = @Id
                AND ActivityTransaction.ActivityTypeID IN (
                    Select ActivityTypeID FROM ActivityType WHERE ActivityTypeName = 'CandidatePropose' AND Inactive = 0
                )";

            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {

                var contractors = db.Connection.Query<int>(contractorsQuery, new { Id = id });

                return contractors;
            }
        }

        public IEnumerable<int> GetCalloutContractorsByJobId(int id)
        {
            const string contractorsQuery =
                @"SELECT DISTINCT(ActivityTransaction.CandidateUserID)
                FROM Agreement
                JOIN PickList ON PickList.PickListID = Agreement.StatusType
                JOIN ActivityTransaction ON ActivityTransaction.AgreementID = Agreement.AgreementID
                JOIN ActivityType ON ActivityType.ActivityTypeID = ActivityTransaction.ActivityTypeID
                WHERE Agreement.AgreementType IN(
	                SELECT PickListId from dbo.udf_GetPickListIds('agreementtype', 'opportunity', -1)
                )
                AND PickList.PickListID IN (
	                SELECT PickListId from dbo.udf_GetPickListIds('OpportunityStatusType', 'Open,On Hold,Submissions Complete', -1)
                )
                AND Agreement.AgreementID = @Id
                AND ActivityTransaction.ActivityTypeID IN (
                    Select ActivityTypeID FROM ActivityType WHERE ActivityTypeName = 'OpportunityCallout' AND Inactive = 0
                )";

            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {

                var contractors = db.Connection.Query<int>(contractorsQuery, new { Id = id });

                return contractors;
            }
        }

        public int GetContractorIdByContractId(int contractId)
        {
            const string contractorsQuery =
                @"SELECT Agreement.CandidateID
                FROM Agreement
                WHERE Agreement.AgreementID = @Id";

            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {

                var id = db.Connection.Query<int>(contractorsQuery, new { Id = contractId }).FirstOrDefault();

                return id;
            }
        }

        public IEnumerable<Contractor> GetContractors()
        {
            throw new NotImplementedException();
        }
    }

    public class MockContractorRepository : IContractorRepository
    {
        public Contractor GetContractorById(int id)
        {
            return new Contractor
            {
                ContactInformation = new UserContact
                {
                    Id = 1,
                    FirstName = "Robert",
                    LastName = "Paulson",
                    EmailAddresses = new List<EmailAddress>() { new EmailAddress { Email = "rp.clientcontact@email.com", Title = "Primary" } }.AsEnumerable(),
                    PhoneNumbers = new List<PhoneNumber> 
                { 
                    new PhoneNumber
                    {
                        Title = "Work",
                        AreaCode = 555,
                        Prefix = 555,
                        LineNumber = 1231
                    }, new PhoneNumber
                    {
                        Title = "Cell",
                        AreaCode = 555,
                        Prefix = 222,
                        LineNumber = 2212
                    }
                }.AsEnumerable(),
                },
                Rating = MatchGuideConstants.ResumeRating.Standard,
                ResumeText = string.Empty,
                Specializations = new List<Specialization>
                {
                    DeveloperSpecialization,
                    ProjectManagerSpecialization
                },
                Contracts = new List<ConsultantContract>
                {
                    new ConsultantContract
                    {
                        ClientId = 1,
                        ContractId = 2,
                        ClientName = "Cenovus",
                        ContractorName = "Robert Paulson",
                        Title = string.Format("{0} - Project Manager", 59326),
                        AgreementSubType = MatchGuideConstants.AgreementSubTypes.FloThru,
                        StartDate = DateTime.UtcNow.AddDays(-15),
                        EndDate = DateTime.UtcNow.AddMonths(3).AddDays(5)
                    }
                }
            };
        }

        public IEnumerable<int> GetShortlistedContractorsByJobId(int id)
        {
            IEnumerable<Contractor> shortlisted = new List<Contractor>
            {
                LouFerigno,
                PeterGriffin,
                SpiderMan
            }.AsEnumerable();

            return Enumerable.Empty<int>();
        }

        public IEnumerable<int> GetProposedContractorsByJobId(int id)
        {
            IEnumerable<Contractor> proposed = new List<Contractor>
            {
                LouFerigno,
                PeterGriffin
            }.AsEnumerable();

            return Enumerable.Empty<int>();
        }

        public IEnumerable<int> GetCalloutContractorsByJobId(int id)
        {
            IEnumerable<Contractor> callouts = new List<Contractor>
            {
                LouFerigno
            }.AsEnumerable();

            return Enumerable.Empty<int>();
        }

        public int GetContractorIdByContractId(int contractId)
        {
            return 10;
        }

        public IEnumerable<Contractor> GetContractors()
        {
            IEnumerable<Contractor> allContractors = new List<Contractor>
            {
                LouFerigno,
                PeterGriffin,
                SpiderMan
            }.AsEnumerable();

            return allContractors;
        }

        private Contractor LouFerigno = new Contractor
        {
            ContactInformation = new UserContact
            {
                FirstName = "Lou",
                LastName = "Ferigno"
            },
            BillRate = 176.22f,
            PayRate = 180.00f,
            GrossMargin = 30.00f,
            Markup = 100.22f
        };

        private Contractor PeterGriffin = new Contractor
        {
            ContactInformation = new UserContact
            {
                FirstName = "Peter",
                LastName = "Griffin"
            },
            
            BillRate = 176.22f,
            PayRate = 180.00f,
            GrossMargin = 30.00f,
            Markup = 100.22f
        };

        private Contractor SpiderMan = new Contractor
        {
            ContactInformation = new UserContact
            {
                FirstName = "Spider",
                LastName = "Man"
            },
            BillRate = 176.22f,
            PayRate = 180.00f,
            GrossMargin = 30.00f,
            Markup = 100.22f
        };

        private Specialization DeveloperSpecialization = new Specialization
        {
            Id = 1,
            Name = "SW Developer",
            Description = "Developer",
            Skills = new List<Skill>
            {
                new Skill
                {
                    Id = 1,
                    Name = ".NET",
                    SpecializationId = 1,
                    YearsOfExperience = MatchGuideConstants.YearsOfExperience.TwoToFour
                },
                new Skill
                {
                    Id = 2,
                    Name = "C++",
                    SpecializationId = 1,
                    YearsOfExperience = MatchGuideConstants.YearsOfExperience.FiveToSeven
                },
                new Skill
                {
                    Id = 3,
                    Name = "MVC",
                    SpecializationId = 1,
                    YearsOfExperience = MatchGuideConstants.YearsOfExperience.LessThanTwo
                },
            }
        };

        private Specialization ProjectManagerSpecialization = new Specialization
        {
            Id = 2,
            Name = "Project Manager",
            Description = "Manger",
            Skills = new List<Skill>
            {
                new Skill
                {
                    Id = 4,
                    Name = "7 Sigma",
                    SpecializationId = 2,
                    YearsOfExperience = MatchGuideConstants.YearsOfExperience.TwoToFour
                },
                new Skill
                {
                    Id = 5,
                    Name = "Agile",
                    SpecializationId = 2,
                    YearsOfExperience = MatchGuideConstants.YearsOfExperience.LessThanTwo
                }
            }
        };
    }
}
