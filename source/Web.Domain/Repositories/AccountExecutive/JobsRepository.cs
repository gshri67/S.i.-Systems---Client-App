using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using SiSystems.SharedModels;

namespace SiSystems.ClientApp.Web.Domain.Repositories.AccountExecutive
{
    public interface IJobsRepository
    {
        JobsSummarySet GetJobsSummaryByAccountExecutiveId(int id);
        JobDetails GetJobDetailsByJobId(int id);
        IEnumerable<Job> GetJobsByAccountExecutiveId(int id);
        IEnumerable<Job> GetJobsByClientId(int id);
        Job GetJobWithJobId(int id);
        IEnumerable<JobSummary> GetJobSummariesByAccountExecutiveId(int id);
    }

    public class JobsRepository : IJobsRepository {
        public JobsSummarySet GetJobsSummaryByAccountExecutiveId(int id)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                var numJobs = db.Connection.Query<int>(AccountExecutiveJobsQueries.NumberOfJobsQuery, new { Id = id }).FirstOrDefault();

                var numProposed = db.Connection.Query<int>(AccountExecutiveJobsQueries.NumberOfJobsWithProposedCandidatesQuery, new { Id = id }).FirstOrDefault();

                var numCallouts = db.Connection.Query<int>(AccountExecutiveJobsQueries.NumberOfJobsWithCalloutsQuery, new { Id = id }).FirstOrDefault();

                return new JobsSummarySet
                {
                    All = numJobs,
                    Proposed = numProposed,
                    Callouts = numCallouts
                };
            }
        }

        public JobDetails GetJobDetailsByJobId(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Job> GetJobsByAccountExecutiveId(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Job> GetJobsByClientId(int id)
        {
            throw new NotImplementedException();
        }

        public Job GetJobWithJobId(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<JobSummary> GetJobSummariesByAccountExecutiveId(int id)
        {
            throw new NotImplementedException();
        }
    }

    internal static class AccountExecutiveJobsQueries
    {
        private const string NumberOfJobsForAccountExecutiveQuery =
            @"SELECT COUNT(DISTINCT(Agreement.AgreementID))
            FROM Agreement
            JOIN PickList ON PickList.PickListID = Agreement.StatusType
            JOIN ActivityTransaction ON ActivityTransaction.AgreementID = Agreement.AgreementID
            WHERE Agreement.AgreementType IN(
	            SELECT PickListId from dbo.udf_GetPickListIds('agreementtype', 'opportunity', -1)
            )
            AND PickList.PickListID IN (
	            SELECT PickListId from dbo.udf_GetPickListIds('OpportunityStatusType', 'Open,On Hold,Submissions Complete', -1)
            )
            AND Agreement.AccountExecID = @Id";

        private const string JobsWithCalloutsFilter =
            @"AND ActivityTransaction.ActivityTypeID IN (
                Select ActivityTypeID FROM ActivityType WHERE ActivityTypeName = 'OpportunityCallout' AND Inactive = 0
            )";

        private const string JobsWithProposedFilter =
            @"AND ActivityTransaction.ActivityTypeID IN (
                Select ActivityTypeID FROM ActivityType WHERE ActivityTypeName = 'CandidatePropose' AND Inactive = 0
            )";

        public static string NumberOfJobsWithCalloutsQuery
        {
            get
            {
                return string.Format("{1}{0}{2}", 
                    Environment.NewLine,
                    NumberOfJobsForAccountExecutiveQuery,
                    JobsWithCalloutsFilter);
            }
        }

        public static string NumberOfJobsWithProposedCandidatesQuery
        {
            get
            {
                return string.Format("{1}{0}{2}",
                    Environment.NewLine, 
                    NumberOfJobsForAccountExecutiveQuery,
                    JobsWithProposedFilter);
            }
        }

        public static string NumberOfJobsQuery
        {
            get
            {
                return string.Format("{0}", NumberOfJobsForAccountExecutiveQuery);
            }
        }
    }

    public class MockJobsRepository : IJobsRepository
    {
        public JobsSummarySet GetJobsSummaryByAccountExecutiveId(int id)
        {
            return new JobsSummarySet
            {
                All = 4,
                Proposed = 2,
                Callouts = 1
            };
        }

        public JobDetails GetJobDetailsByJobId(int jobId)
        {
            //todo: connect to DB, querying on selected id
            return new JobDetails
            {
                Id = ProjectManager.Id,
                ClientName = ProjectManager.ClientName,
                Title = ProjectManager.JobTitle,
                ClientContact = LucyLu,
                Shortlisted = new List<Contractor>
                {
                    LouFerigno,PeterGriffin,SpiderMan
                }.AsEnumerable(),
                Proposed = new List<Contractor>
                {
                    LouFerigno,PeterGriffin
                }.AsEnumerable(),
                Callouts = new List<Contractor>
                {
                    LouFerigno
                }.AsEnumerable()
            };
        }

        public IEnumerable<Job> GetJobsByAccountExecutiveId(int id)
        {
            return new List<Job>
            {
                BusinessAnalyst,
                ProjectManager,
                SolutionsDeveloper,
                EnterpriseArchitect
            };
        }

        public IEnumerable<Job> GetJobsByClientId(int id)
        {
            return new List<Job>
            {
                BusinessAnalyst,
                ProjectManager,
                SolutionsDeveloper,
                EnterpriseArchitect
            };
        }

        public Job GetJobWithJobId(int id)
        {
            return BusinessAnalyst;
        }

        public IEnumerable<JobSummary> GetJobSummariesByAccountExecutiveId(int id)
        {
            return new List<JobSummary>
            {
                CenovusSummary,
                NexenSummary
            };
        }

        private JobSummary CenovusSummary = new JobSummary
        {
            ClientId = 1,
            ClientName = "Cenovus",
            NumJobs = 8,
            NumProposed = 3,
            NumCallouts = 1
        };
        private JobSummary NexenSummary = new JobSummary
        {
            ClientId = 2,
            ClientName = "Nexen",
            NumJobs = 12,
            NumProposed = 6,
            NumCallouts = 0
        };

        private UserContact LucyLu = new UserContact
        {
            FirstName = "Lucy",
            LastName = "Lu",
            EmailAddresses = new List<string>(){ "lucy.lu@email.com" }.AsEnumerable(),
            PhoneNumbers = new List<string>(){ "(555)555-1234" }.AsEnumerable()
        };

        private Contractor LouFerigno = new Contractor
        {
            FirstName = "Lou",
            LastName = "Ferigno",
        };

        private Contractor PeterGriffin = new Contractor
        {
            FirstName = "Peter",
            LastName = "Griffin",
        };

        private Contractor SpiderMan = new Contractor
        {
            FirstName = "Spider",
            LastName = "Man",
        };

        private Job BusinessAnalyst = new Job
        {
            Id = 1,
            JobTitle = "60142 - Senior Financial Systems Business Analyst",
            ClientName = "Cenovus",
            IssueDate = DateTime.UtcNow.AddDays(-14),
            HasCallout = true,
            IsProposed = true
        };

        private Job ProjectManager = new Job
        {
            Id = 2,
            JobTitle = "60141 - Intermediate Project Manager (w/ Asset Management/Investment Planning/Analytics exp). - long term contract!!!",
            ClientName = "Cenovus",
            IssueDate = DateTime.UtcNow.AddDays(-7),
            HasCallout = false,
            IsProposed = true
        };
        private Job SolutionsDeveloper = new Job
        {
            Id = 3,
            JobTitle = "60139 - ASP.NET MVC Developer - Bilingual",
            ClientName = "Cenovus",
            IssueDate = DateTime.UtcNow.AddDays(-1),
            HasCallout = false,
            IsProposed = false
        };
        private Job EnterpriseArchitect = new Job
        {
            Id = 4,
            JobTitle = "60138 - Information Architect",
            ClientName = "Nexen",
            IssueDate = new DateTime(2015, 11, 14),
            HasCallout = false,
            IsProposed = true
        };


    }
}
