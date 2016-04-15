using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiSystems.ClientApp.Web.Domain.Repositories.AccountExecutive;
using SiSystems.SharedModels;
using SiSystems.Web.Domain.Context;

namespace SiSystems.ClientApp.Web.Domain.Services.AccountExecutive
{
    public class ContractCreationService
    {
        private readonly IContractorRateRepository _rateRepo;
        private readonly ISessionContext _session;

        public ContractCreationService(IContractorRateRepository rateRepo, ISessionContext session)
        {
            _rateRepo = rateRepo;
            _session = session;
        }

        private IEnumerable<ContractorRateSummary> ProposedContractorRatesForJobAndCandidate(int jobId, int candidateId)
        {
            return
                new List<ContractorRateSummary>
                {
                    _rateRepo.GetProposedRateSummaryByJobIdAndContractorId(jobId, candidateId)
                }.AsEnumerable();
        }

        public ContractCreationDetails GetContractDetailsByJobIdAndCandidateId(int jobId, int candidateId)
        {
            return new ContractCreationDetails
            {
                Rates = ProposedContractorRatesForJobAndCandidate(jobId, candidateId)
            };
        }
    }
}
