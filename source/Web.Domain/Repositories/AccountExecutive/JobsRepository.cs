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
        JobsSummarySet GetJobsSummary();
        JobDetails GetJobDetailsById(int id);
        IEnumerable<Job> GetJobs();
    }

    public class JobsRepository : IJobsRepository
    {
        public JobsSummarySet GetJobsSummary()
        {
            return new JobsSummarySet
            {
                All = 18,
                Proposed = 9,
                Callouts = 6
            };
        }

        public JobDetails GetJobDetailsById(int id)
        {
            //todo: connect to DB, querying on selected id
            return new JobDetails
            {
                Id = 1,
                ClientName = "Nexen",
                Title = "0123456 - Intermediate Project Manager (w/ Asset Management/Investment Planning/Analytics exp). - long term contract!!!",
                ClientContact = new ClientContact
                {
                    FirstName = "Lucy",
                    LastName = "Lu",
                    EmailAddress = "lucy.lu@email.com",
                    phoneNumber = "(555)555-1234"
                },
                DirectReport = new ClientContact
                {
                    FirstName = "Mary",
                    LastName = "Jo",
                    EmailAddress = "mary.jo@email.com",
                    phoneNumber = "(555)555-4321"
                },
                Shortlisted = new List<IM_Consultant>
                {
                    new IM_Consultant
                    {
                        FirstName = "Joe",
                        LastName = "Ferigno",
                    },
                    new IM_Consultant
                    {
                        FirstName = "Peter",
                        LastName = "Griffon",
                    },
                    new IM_Consultant
                    {
                        FirstName = "Spider",
                        LastName = "Man",
                    }
                }.AsEnumerable(),
                Proposed = new List<IM_Consultant>
                {
                    new IM_Consultant
                    {
                        FirstName = "Joe",
                        LastName = "Ferigno",
                    },
                    new IM_Consultant
                    {
                        FirstName = "Peter",
                        LastName = "Griffon",
                    }
                }.AsEnumerable(),
                Callouts = new List<IM_Consultant>
                {
                    new IM_Consultant
                    {
                        FirstName = "Joe",
                        LastName = "Ferigno",
                    }
                }.AsEnumerable()
            };
        }

        public IEnumerable<Job> GetJobs()
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
