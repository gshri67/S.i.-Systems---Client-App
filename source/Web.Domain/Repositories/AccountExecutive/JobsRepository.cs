using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        IEnumerable<Contractor> GetShortlistedContractorsByJobId(int id);
        IEnumerable<Contractor> GetProposedContractorsByJobId(int id);
        IEnumerable<Contractor> GetCalloutContractorsByJobId(int id);
        IEnumerable<JobSummary> GetJobSummariesByAccountExecutiveId(int id);
    }

    public class JobsRepository : IJobsRepository {
        public JobsSummarySet GetJobsSummaryByAccountExecutiveId(int id)
        {
            throw new NotImplementedException();
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

        public IEnumerable<Contractor> GetShortlistedContractorsByJobId(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Contractor> GetProposedContractorsByJobId(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Contractor> GetCalloutContractorsByJobId(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<JobSummary> GetJobSummariesByAccountExecutiveId(int id)
        {
            throw new NotImplementedException();
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

        public IEnumerable<Contractor> GetShortlistedContractorsByJobId(int id)
        {
            IEnumerable<Contractor> shortlisted = new List<Contractor>
            {
                LouFerigno,
                PeterGriffin,
                SpiderMan
            }.AsEnumerable();

            return shortlisted;
        }

        public IEnumerable<Contractor> GetProposedContractorsByJobId(int id)
        {
            IEnumerable<Contractor> proposed = new List<Contractor>
            {
                LouFerigno,
                PeterGriffin
            }.AsEnumerable();

            return proposed;
        }

        public IEnumerable<Contractor> GetCalloutContractorsByJobId(int id)
        {
            IEnumerable<Contractor> callouts = new List<Contractor>
            {
                LouFerigno
            }.AsEnumerable();

            return callouts;
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
            EmailAddress = "lucy.lu@email.com",
            PhoneNumber = "(555)555-1234"
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
