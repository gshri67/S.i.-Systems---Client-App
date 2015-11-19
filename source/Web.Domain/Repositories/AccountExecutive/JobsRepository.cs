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
    }

    public class JobsRepository : IJobsRepository
    {
        public JobsSummarySet GetJobsSummaryByAccountExecutiveId(int id)
        {
            return new JobsSummarySet
            {
                All = 18,
                Proposed = 9,
                Callouts = 6
            };
        }

        public JobDetails GetJobDetailsByJobId(int jobId)
        {
            //todo: connect to DB, querying on selected id
            return new JobDetails
            {
                Id = 1,
                ClientName = "Nexen",
                Title = "0123456 - Intermediate Project Manager (w/ Asset Management/Investment Planning/Analytics exp). - long term contract!!!",
                ClientContact = new UserContact
                {
                    FirstName = "Lucy",
                    LastName = "Lu",
                    EmailAddress = "lucy.lu@email.com",
                    PhoneNumber = "(555)555-1234"
                },
                DirectReport = new UserContact
                {
                    FirstName = "Mary",
                    LastName = "Jo",
                    EmailAddress = "mary.jo@email.com",
                    PhoneNumber = "(555)555-4321"
                },
                Shortlisted = new List<Contractor>
                {
                    new Contractor
                    {
                        FirstName = "Joe",
                        LastName = "Ferigno",
                    },
                    new Contractor
                    {
                        FirstName = "Peter",
                        LastName = "Griffon",
                    },
                    new Contractor
                    {
                        FirstName = "Spider",
                        LastName = "Man",
                    }
                }.AsEnumerable(),
                Proposed = new List<Contractor>
                {
                    new Contractor
                    {
                        FirstName = "Joe",
                        LastName = "Ferigno",
                    },
                    new Contractor
                    {
                        FirstName = "Peter",
                        LastName = "Griffon",
                    }
                }.AsEnumerable(),
                Callouts = new List<Contractor>
                {
                    new Contractor
                    {
                        FirstName = "Joe",
                        LastName = "Ferigno",
                    }
                }.AsEnumerable()
            };
        }

        public IEnumerable<Job> GetJobsByAccountExecutiveId(int id)
        {
            var jobList = new List<Job>();

            for (var i = 0; i < 28; i++)
            {
                var job = new Job
                {
                    ClientName = i < 10 ? "Nexen" : "Cenovus", 
                    Id = i, 
                    IsProposed = (i%3) == 0
                };


                job.HasCallout = job.IsProposed && ((i % 2) == 0);

                job.JobTitle = string.Format("Developer {0}", i);

                job.IssueDate = new DateTime(2015, 10, i + 1 + (i % 5) - (i % 3));

                jobList.Add(job);
            }

            return jobList.AsEnumerable();
        }
    }
}
