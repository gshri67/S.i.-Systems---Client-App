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
        //ContractorRateSummary GetContractorRateSummaryByContractId(int contractId);
        IEnumerable<ContractorRateSummary> GetContractorRateSummaryWithJobIdAndStatus(int jobId, JobStatus status );
    }

    public class ContractorRateRepository : IContractorRateRepository
    {
        /*
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
        }*/


        public IEnumerable<ContractorRateSummary> GetContractorRateSummaryWithJobIdAndStatus(int jobId, JobStatus status)
        {
            return Enumerable.Empty<ContractorRateSummary>();
        }
    }

    public class MockContractorRateRepository : IContractorRateRepository
    {
        public IEnumerable<ContractorRateSummary> GetContractorRateSummaryWithJobIdAndStatus(int jobId, JobStatus status)
        {
            return Enumerable.Empty<ContractorRateSummary>();
        }
    }
}
