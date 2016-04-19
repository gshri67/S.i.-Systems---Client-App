using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using SiSystems.SharedModels;

namespace SiSystems.ClientApp.Web.Domain.Repositories.AccountExecutive
{
    public interface IContractorRateRepository
    {
        IEnumerable<Rate> GetProposedContractorRateSummaryByJobId(int id);
        IEnumerable<Rate> GetShortlistedContractorRateSummaryByJobId(int id);
        IEnumerable<Rate> GetCalloutContractorRateSummaryByJobId(int id);
        Rate GetProposedRateSummaryByJobIdAndContractorId(int jobId, int contractorId);
    }

    public class ContractorRateRepository : IContractorRateRepository
    {
        public IEnumerable<Rate> GetProposedContractorRateSummaryByJobId(int id)
        {
            const string contractorsQuery =
                @"SELECT ProposedUsers.pID AS ContractorId,
	                ProposedUsers.DueDateTime AS Date,
	                Users.FirstName, 
	                Users.LastName,
	                Matrix.ProposedBillRate BillRate,
	                Matrix.ProposedPayRate PayRate
                FROM
	                (SELECT ActivityTransaction.CandidateUserID AS pID, ActivityTransaction.DueDateTime
	                FROM ActivityTransaction
	                WHERE ActivityTransaction.AgreementID = @Id
	                AND ActivityTransaction.ActivityTypeID IN (
		                Select ActivityTypeID FROM ActivityType WHERE ActivityTypeName = 'CandidatePropose' AND Inactive = 0
	                )) ProposedUsers
                LEFT JOIN Users ON Users.UserID = ProposedUsers.pID
                LEFT JOIN Agreement_OpportunityCandidateMatrix Matrix ON Matrix.AgreementID = @Id AND Matrix.CandidateUserID = ProposedUsers.pID";

            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {

                var summaries = db.Connection.Query<Rate>(contractorsQuery, new { Id = id });

                return summaries;
            }
        }

        public IEnumerable<Rate> GetShortlistedContractorRateSummaryByJobId(int id)
        {
            const string contractorsQuery =
                @"SELECT Matrix.CandidateUserID AS ContractorId, Matrix.UpdateDateTime as Date, Users.FirstName, Users.LastName
                FROM Agreement
                LEFT JOIN Agreement_OpportunityCandidateMatrix Matrix on Agreement.AgreementID = Matrix.AgreementID
                LEFT JOIN Users ON Matrix.CandidateUserID = Users.UserID
                WHERE Agreement.AgreementID = @Id
                AND Matrix.StatusType IN (
	                SELECT PickListId from dbo.udf_GetPickListIds('CandidateOpportunityStatusType', 'Short List', -1)
                )
                AND Matrix.StatusSubType IN (
	                SELECT PickListId from dbo.udf_GetPickListIds('CandidateOpportunitySubStatusType', 'Pending,Placed,Proposed', -1)
                )";

            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {

                var summaries = db.Connection.Query<Rate>(contractorsQuery, new { Id = id });

                return summaries;
            }
        }

        public IEnumerable<Rate> GetCalloutContractorRateSummaryByJobId(int id)
        {
            const string contractorsQuery =
               @"SELECT ProposedUsers.pID AS ContractorId,
	                ProposedUsers.DueDateTime AS Date,
	                Users.FirstName, 
	                Users.LastName,
	                Matrix.ProposedBillRate BillRate,
	                Matrix.ProposedPayRate PayRate
                FROM
	                (SELECT ActivityTransaction.CandidateUserID AS pID, ActivityTransaction.DueDateTime
	                FROM ActivityTransaction
	                WHERE ActivityTransaction.AgreementID = @Id
	                AND ActivityTransaction.ActivityTypeID IN (
		                Select ActivityTypeID FROM ActivityType WHERE ActivityTypeName = 'OpportunityCallout' AND Inactive = 0
	                )) ProposedUsers
                LEFT JOIN Users ON Users.UserID = ProposedUsers.pID
                LEFT JOIN Agreement_OpportunityCandidateMatrix Matrix ON Matrix.AgreementID = @Id AND Matrix.CandidateUserID = ProposedUsers.pID";

            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                var summaries = db.Connection.Query<Rate>(contractorsQuery, new { Id = id });

                return summaries;
            }
        }

        public Rate GetProposedRateSummaryByJobIdAndContractorId(int jobId, int contractorId)
        {
            const string contractorsQuery =
               @"SELECT CandidateUserID AS ContractorId, 
		            Matrix.ProposedBillRate BillRate,
		            Matrix.ProposedPayRate PayRate
            FROM Agreement_OpportunityCandidateMatrix Matrix
            WHERE Matrix.AgreementID = @JobId
            AND Matrix.CandidateUserID = @CandidateId";

            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                var summary = db.Connection.Query<Rate>(contractorsQuery, new { JobId = jobId, CandidateId = contractorId }).FirstOrDefault();

                return summary;
            }
        }
    }

    public class MockContractorRateRepository : IContractorRateRepository
    {

        public IEnumerable<Rate> GetProposedContractorRateSummaryByJobId(int id)
        {
            return Enumerable.Empty<Rate>();
        }

        public IEnumerable<Rate> GetShortlistedContractorRateSummaryByJobId(int id)
        {
            return Enumerable.Empty<Rate>();
        }

        public IEnumerable<Rate> GetCalloutContractorRateSummaryByJobId(int id)
        {
            return Enumerable.Empty<Rate>();
        }

        public Rate GetProposedRateSummaryByJobIdAndContractorId(int jobId, int contractorId)
        {
            return new Rate();
        }
    }
}
