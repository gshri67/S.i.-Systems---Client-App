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
    public class ContractorRateService
    {
        private readonly IContractorRateRepository _repo;

        public ContractorRateService(IContractorRateRepository repo)
        {
            _repo = repo;
        }

        public object GetContractorRateSummaryWithJobIdAndStatus(int id, JobStatus status)
        {
            var rateSummary = _repo.GetContractorRateSummaryWithJobIdAndStatus(id, status);

            return rateSummary;
        }
    }
}
