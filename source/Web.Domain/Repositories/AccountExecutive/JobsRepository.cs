﻿using System;
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
        JobsSummarySet GetSummaryCountsByAccountExecutiveId(int id);
        IEnumerable<Job> GetJobsByClientIdAndAccountExecutiveId(int id, int aeId);
        Job GetJobWithJobId(int id);
        IEnumerable<JobSummary> GetJobSummariesByAccountExecutiveId(int id);
        int GetCompanyIdWithJobId(int id);
    }

    public class JobsRepository : IJobsRepository {
        public JobsSummarySet GetSummaryCountsByAccountExecutiveId(int id)
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

        public IEnumerable<Job> GetJobsByClientIdAndAccountExecutiveId(int id, int aeId)
        {
            const string jobsByClientIdQuery =
                @"SELECT Agreement.AgreementID AS Id, Company.CompanyName AS ClientName, Detail.JobTitle AS Title, Agreement.CreateDate AS IssueDate
                FROM Agreement
                JOIN Agreement_OpportunityDetail AS Detail ON Agreement.AgreementID = Detail.AgreementID
                JOIN PickList ON PickList.PickListID = Agreement.StatusType
                JOIN Company ON Agreement.CompanyID = Company.CompanyID
                WHERE Agreement.AgreementType IN(
	                SELECT PickListId from dbo.udf_GetPickListIds('agreementtype', 'opportunity', -1)
                )
                AND PickList.PickListID IN (
	                SELECT PickListId from dbo.udf_GetPickListIds('OpportunityStatusType', 'Open,On Hold,Submissions Complete', -1)
                )
                AND Agreement.CompanyID = @Id
                AND Agreement.AccountExecId = @AEID";

            const string numberOfShortlistedCandidadtesForJob =
                @"SELECT COUNT(DISTINCT(Matrix.CandidateUserID))
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
                var jobs = db.Connection.Query<Job>(jobsByClientIdQuery, new { Id = id, AEID = aeId });

                foreach (var job in jobs)
                {
                    job.NumShortlisted = db.Connection.Query<int>(numberOfShortlistedCandidadtesForJob, new { Id = job.Id }).FirstOrDefault();

                    job.NumProposed = db.Connection.Query<int>(AccountExecutiveJobsQueries.NumberOfProposedCandidatesForJob, new { Id = job.Id }).FirstOrDefault();

                    job.NumCallouts = db.Connection.Query<int>(AccountExecutiveJobsQueries.NumberOfCandidatesWithCalloutsForJob, new { Id = job.Id }).FirstOrDefault();
                }

                return jobs;
            }
        }

        public Job GetJobWithJobId(int id)
        {
            const string jobByAgreementIdQuery =
                @"SELECT Agreement.AgreementID AS Id, Company.CompanyName AS ClientName, Detail.JobTitle AS Title, Agreement.CreateDate AS IssueDate, Agreement.StartDate AS StartDate, Agreement.EndDate AS EndDate
                FROM Agreement
                JOIN Agreement_OpportunityDetail AS Detail ON Agreement.AgreementID = Detail.AgreementID
                JOIN PickList ON PickList.PickListID = Agreement.StatusType
                JOIN Company ON Agreement.CompanyID = Company.CompanyID
                WHERE Agreement.AgreementID = @Id";

            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                var job = db.Connection.Query<Job>(jobByAgreementIdQuery, new { Id = id }).FirstOrDefault();

                if(job == null)
                    return new Job();

                job.NumShortlisted = db.Connection.Query<int>(AccountExecutiveJobsQueries.NumberOfShortlistedCandidadtesForJob, new { Id = job.Id }).FirstOrDefault();

                job.NumProposed = db.Connection.Query<int>(AccountExecutiveJobsQueries.NumberOfProposedCandidatesForJob, new { Id = job.Id }).FirstOrDefault();

                job.NumCallouts = db.Connection.Query<int>(AccountExecutiveJobsQueries.NumberOfCandidatesWithCalloutsForJob, new { Id = job.Id }).FirstOrDefault();

                return job;
            }
        }

        public int GetCompanyIdWithJobId(int id)
        {
            const string companyByJobIdQuery =
                @"SELECT Agreement.CompanyID AS companyId
                FROM Agreement
                JOIN Agreement_OpportunityDetail AS Detail ON Agreement.AgreementID = Detail.AgreementID
                JOIN PickList ON PickList.PickListID = Agreement.StatusType
                JOIN Company ON Agreement.CompanyID = Company.CompanyID
                WHERE Agreement.AgreementID = @Id";

            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                var companyId = db.Connection.Query<int>(companyByJobIdQuery, new { Id = id }).FirstOrDefault();
                return companyId;
            }
        }

        public IEnumerable<JobSummary> GetJobSummariesByAccountExecutiveId(int id)
        {
            const string listOfClientsWithJobsQuery =
            @"SELECT Distinct(Company.CompanyID) AS ClientID, Company.CompanyName AS ClientName
            FROM Agreement
            JOIN PickList ON PickList.PickListID = Agreement.StatusType
            JOIN Company ON Agreement.CompanyID = Company.CompanyID
            WHERE Agreement.AgreementType IN(
	            SELECT PickListId from dbo.udf_GetPickListIds('agreementtype', 'opportunity', -1)
            )
            AND PickList.PickListID IN (
	            SELECT PickListId from dbo.udf_GetPickListIds('OpportunityStatusType', 'Open,On Hold,Submissions Complete', -1)
            )
            AND Agreement.AccountExecID = @Id";

            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                var clientsWithJobs = db.Connection.Query<JobSummary>(listOfClientsWithJobsQuery, new { Id = id });

                foreach (var client in clientsWithJobs)
                {
                    client.NumJobs = db.Connection.Query<int>(AccountExecutiveJobsQueries.NumberOfJobsForClient, new { Id = client.ClientId, UserId = id }).FirstOrDefault();

                    client.NumProposed = db.Connection.Query<int>(AccountExecutiveJobsQueries.NumberOfJobsWithProposedForClient, new { Id = client.ClientId, UserId = id }).FirstOrDefault();

                    client.NumCallouts = db.Connection.Query<int>(AccountExecutiveJobsQueries.NumberOfJobsWithCalloutsForClient, new { Id = client.ClientId, UserId = id }).FirstOrDefault();
                }

                return clientsWithJobs;
            }
        }
    }

    internal static class AccountExecutiveJobsQueries
    {
        private const string JobCountForClientQuery =
            @"SELECT COUNT(DISTINCT(Agreement.AgreementID))
            FROM Agreement
            JOIN PickList ON PickList.PickListID = Agreement.StatusType
            WHERE Agreement.AgreementType IN(
	            SELECT PickListId from dbo.udf_GetPickListIds('agreementtype', 'opportunity', -1)
            )
            AND PickList.PickListID IN (
	            SELECT PickListId from dbo.udf_GetPickListIds('OpportunityStatusType', 'Open,On Hold,Submissions Complete', -1)
            )
            AND Agreement.CompanyID = @Id
            AND Agreement.AccountExecID = @UserId";

        private const string JobsSubsetCountForClientBaseQuery =
            @"SELECT COUNT(DISTINCT(ActivityTransaction.AgreementID))
            FROM Agreement
            JOIN PickList ON PickList.PickListID = Agreement.StatusType
            LEFT JOIN ActivityTransaction ON ActivityTransaction.AgreementID = Agreement.AgreementID
            LEFT JOIN ActivityType ON ActivityType.ActivityTypeID = ActivityTransaction.ActivityTypeID
            WHERE Agreement.AgreementType IN(
	            SELECT PickListId from dbo.udf_GetPickListIds('agreementtype', 'opportunity', -1)
            )
            AND PickList.PickListID IN (
	            SELECT PickListId from dbo.udf_GetPickListIds('OpportunityStatusType', 'Open,On Hold,Submissions Complete', -1)
            )
            AND Agreement.CompanyID = @Id
            AND Agreement.AccountExecID = @UserId";

        private const string NumberOfJobsForAccountExecutiveQuery =
            @"SELECT COUNT(DISTINCT(Agreement.AgreementID))
            FROM Agreement
            JOIN PickList ON PickList.PickListID = Agreement.StatusType
            LEFT JOIN ActivityTransaction ON ActivityTransaction.AgreementID = Agreement.AgreementID
            WHERE Agreement.AgreementType IN(
	            SELECT PickListId from dbo.udf_GetPickListIds('agreementtype', 'opportunity', -1)
            )
            AND PickList.PickListID IN (
	            SELECT PickListId from dbo.udf_GetPickListIds('OpportunityStatusType', 'Open,On Hold,Submissions Complete', -1)
            )
            AND Agreement.AccountExecID = @Id";

        private const string NumberOfCandidatesForJob =
            @"SELECT COUNT(DISTINCT(ActivityTransaction.CandidateUserID))
            FROM Agreement
            JOIN PickList ON PickList.PickListID = Agreement.StatusType
            LEFT JOIN ActivityTransaction ON ActivityTransaction.AgreementID = Agreement.AgreementID
            LEFT JOIN ActivityType ON ActivityType.ActivityTypeID = ActivityTransaction.ActivityTypeID
            WHERE Agreement.AgreementType IN(
	            SELECT PickListId from dbo.udf_GetPickListIds('agreementtype', 'opportunity', -1)
            )
            AND PickList.PickListID IN (
	            SELECT PickListId from dbo.udf_GetPickListIds('OpportunityStatusType', 'Open,On Hold,Submissions Complete', -1)
            )
            AND Agreement.AgreementID = @Id";

        private const string JobsWithCalloutsFilter =
            @"AND ActivityTransaction.ActivityTypeID IN (
                Select ActivityTypeID FROM ActivityType WHERE ActivityTypeName = 'OpportunityCallout' AND Inactive = 0
            )";

        private const string JobsWithProposedFilter =
            @"AND ActivityTransaction.ActivityTypeID IN (
                Select ActivityTypeID FROM ActivityType WHERE ActivityTypeName = 'CandidatePropose' AND Inactive = 0
            )";

        private const string  NumberOfShortlistedCandidadtesForJobQuery =
                @"SELECT COUNT(DISTINCT(Matrix.CandidateUserID))
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

        public static string NumberOfJobsForClient
        {
            get { return string.Format("{0}", JobCountForClientQuery); }
        }

        public static string NumberOfJobsWithProposedForClient
        {
            get
            {
                return string.Format("{1}{0}{2}", 
                    Environment.NewLine, 
                    JobsSubsetCountForClientBaseQuery, 
                    JobsWithProposedFilter);
            }
        }

        public static string NumberOfJobsWithCalloutsForClient
        {
            get
            {
                return string.Format("{1}{0}{2}",
                    Environment.NewLine,
                    JobsSubsetCountForClientBaseQuery,
                    JobsWithCalloutsFilter);
            }
        }

        public static string NumberOfShortlistedCandidatesForJob
        {
            get
            {
                return string.Format("{1}{0}{2}", 
                    Environment.NewLine,
                    NumberOfCandidatesForJob, 
                    JobsWithProposedFilter);
            }
        }

        public static string NumberOfProposedCandidatesForJob
        {
            get
            {
                return string.Format("{1}{0}{2}",
                    Environment.NewLine,
                    NumberOfCandidatesForJob,
                    JobsWithProposedFilter);
            }
        }
        public static string NumberOfCandidatesWithCalloutsForJob
        {
            get
            {
                return string.Format("{1}{0}{2}",
                    Environment.NewLine,
                    NumberOfCandidatesForJob,
                    JobsWithCalloutsFilter);
            }
        }

        public static string NumberOfShortlistedCandidadtesForJob
        {
            get { return NumberOfShortlistedCandidadtesForJobQuery; }
        }
    }

    public class MockJobsRepository : IJobsRepository
    {
        public JobsSummarySet GetSummaryCountsByAccountExecutiveId(int id)
        {
            return new JobsSummarySet
            {
                All = 4,
                Proposed = 2,
                Callouts = 1
            };
        }

        public IEnumerable<Job> GetJobsByClientIdAndAccountExecutiveId(int id, int aeId)
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
            if( id == 1 )
                return BusinessAnalyst;
            else if( id == 2 )
                return ProjectManager;
            else if( id == 3 )
                return SolutionsDeveloper;
            else
                return EnterpriseArchitect;
        }

        public IEnumerable<JobSummary> GetJobSummariesByAccountExecutiveId(int id)
        {
            return new List<JobSummary>
            {
                CenovusSummary,
                NexenSummary
            };
        }

        public int GetCompanyIdWithJobId(int id)
        {
            return 1;
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

        private Job BusinessAnalyst = new Job
        {
            Id = 1,
            Title = "60142 - Senior Financial Systems Business Analyst",
            ClientName = "Cenovus",
            IssueDate = DateTime.UtcNow.AddDays(-14),
            NumProposed = 1,
            NumCallouts = 1
        };

        private Job ProjectManager = new Job
        {
            Id = 2,
            Title = "60141 - Intermediate Project Manager (w/ Asset Management/Investment Planning/Analytics exp). - long term contract!!!",
            ClientName = "Cenovus",
            IssueDate = DateTime.UtcNow.AddDays(-7),
            NumProposed = 1,
            NumCallouts = 0
        };
        private Job SolutionsDeveloper = new Job
        {
            Id = 3,
            Title = "60139 - ASP.NET MVC Developer - Bilingual",
            ClientName = "Cenovus",
            IssueDate = DateTime.UtcNow.AddDays(-1),
            NumProposed = 0,
            NumCallouts = 0
        };
        private Job EnterpriseArchitect = new Job
        {
            Id = 4,
            Title = "60138 - Information Architect",
            ClientName = "Nexen",
            IssueDate = new DateTime(2015, 11, 14),
            NumProposed = 1,
            NumCallouts = 0
        };


    }
}
