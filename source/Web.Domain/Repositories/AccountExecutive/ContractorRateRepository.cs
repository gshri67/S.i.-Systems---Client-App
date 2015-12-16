﻿using System;
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
        //ContractorRateSummary GetContractorRateSummaryByContractId(int contractId);
        IEnumerable<ContractorRateSummary> GetProposedContractorRateSummaryByJobId(int id);
        IEnumerable<ContractorRateSummary> GetShortlistedContractorRateSummaryByJobId(int id);
        IEnumerable<ContractorRateSummary> GetCalloutContractorRateSummaryByJobId(int id);
    }

    public class ContractorRateRepository : IContractorRateRepository
    {
        public IEnumerable<ContractorRateSummary> GetProposedContractorRateSummaryByJobId(int id)
        {
            const string contractorsQuery =
                @"SELECT ActivityTransaction.CandidateUserID AS ContractorId,
                ActivityTransaction.UpdateDateTime AS Date,
                Users.FirstName, 
	            Users.LastName,
				Matrix.ProposedBillRate BillRate,
				Matrix.ProposedPayRate PayRate
                FROM
	                (SELECT DISTINCT(ActivityTransaction.CandidateUserID) AS pID
	                FROM ActivityTransaction
	                WHERE ActivityTransaction.AgreementID = @Id
	                AND ActivityTransaction.ActivityTypeID IN (
		                Select ActivityTypeID FROM ActivityType WHERE ActivityTypeName = 'CandidatePropose' AND Inactive = 0
	                )) ProposedUsers
				LEFT JOIN ActivityTransaction ON ActivityTransaction.AgreementID = @Id
                LEFT JOIN Users ON Users.UserID = ProposedUsers.pID
				LEFT JOIN Agreement_OpportunityCandidateMatrix Matrix ON Matrix.AgreementID = @Id";

            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {

                var summaries = db.Connection.Query<ContractorRateSummary>(contractorsQuery, new { Id = id });

                return summaries;
            }
        }

        public IEnumerable<ContractorRateSummary> GetShortlistedContractorRateSummaryByJobId(int id)
        {
            const string contractorsQuery =
                @"SELECT ActivityTransaction.CandidateUserID AS ContractorId,
                ActivityTransaction.UpdateDateTime AS Date,
                Users.FirstName, 
	            Users.LastName
                FROM Agreement
				LEFT JOIN ActivityTransaction ON ActivityTransaction.AgreementID = @Id
                LEFT JOIN Agreement_OpportunityCandidateMatrix Matrix ON Matrix.AgreementID = @Id
                LEFT JOIN Users ON Users.UserID = Matrix.CandidateUserID
				WHERE Matrix.StatusType IN (
	                SELECT PickListId from dbo.udf_GetPickListIds('CandidateOpportunityStatusType', 'Short List', -1)
                )
                AND Matrix.StatusSubType IN (
	                SELECT PickListId from dbo.udf_GetPickListIds('CandidateOpportunitySubStatusType', 'Pending,Placed,Proposed', -1)
                )";

            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {

                var summaries = db.Connection.Query<ContractorRateSummary>(contractorsQuery, new { Id = id });

                return summaries;
            }
        }

        public IEnumerable<ContractorRateSummary> GetCalloutContractorRateSummaryByJobId(int id)
        {
            const string contractorsQuery =
               @"SELECT ActivityTransaction.CandidateUserID AS ContractorId,
                ActivityTransaction.UpdateDateTime AS Date,
                Users.FirstName, 
	            Users.LastName,
				Matrix.ProposedBillRate BillRate,
				Matrix.ProposedPayRate PayRate
                FROM
	                (SELECT DISTINCT(ActivityTransaction.CandidateUserID) AS pID
	                FROM ActivityTransaction
	                WHERE ActivityTransaction.AgreementID = @Id
	                AND ActivityTransaction.ActivityTypeID IN (
		                Select ActivityTypeID FROM ActivityType WHERE ActivityTypeName = 'OpportunityCallout' AND Inactive = 0
	                )) ProposedUsers
				LEFT JOIN ActivityTransaction ON ActivityTransaction.AgreementID = @Id
                LEFT JOIN Users ON Users.UserID = ProposedUsers.pID
				LEFT JOIN Agreement_OpportunityCandidateMatrix Matrix ON Matrix.AgreementID = @Id";

            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                var summaries = db.Connection.Query<ContractorRateSummary>(contractorsQuery, new { Id = id });

                return summaries;
            }
        }
    }

    public class MockContractorRateRepository : IContractorRateRepository
    {

        public IEnumerable<ContractorRateSummary> GetProposedContractorRateSummaryByJobId(int id)
        {
            return Enumerable.Empty<ContractorRateSummary>();
        }

        public IEnumerable<ContractorRateSummary> GetShortlistedContractorRateSummaryByJobId(int id)
        {
            return Enumerable.Empty<ContractorRateSummary>();
        }

        public IEnumerable<ContractorRateSummary> GetCalloutContractorRateSummaryByJobId(int id)
        {
            return Enumerable.Empty<ContractorRateSummary>();
        }
    }
}