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
            var rateSummaries = Enumerable.Empty<ContractorRateSummary>();

            if (status == JobStatus.Shortlisted)
                rateSummaries = _repo.GetShortlistedContractorRateSummaryByJobId(id);
            else if (status == JobStatus.Proposed)
                rateSummaries= _repo.GetProposedContractorRateSummaryByJobId(id);
            else if (status == JobStatus.Callout)
                rateSummaries = _repo.GetCalloutContractorRateSummaryByJobId(id);

            foreach (var rate in rateSummaries)
            {

                rate.GrossMargin = rate.BillRate - rate.PayRate;
                rate.Markup = (rate.BillRate - rate.PayRate) / rate.PayRate;
                rate.Margin = (rate.BillRate - rate.PayRate) / rate.BillRate;
            }
            return rateSummaries;
        }

        public ContractorRateSummary GetRateSummaryByJobIdAndContractorId(int jobId, int contractorId)
        {
            return _repo.GetRateSummaryByJobIdAndContractorId(jobId, contractorId);
        }
    }
}
