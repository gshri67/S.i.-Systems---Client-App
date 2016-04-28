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
        private readonly IJobsRepository _jobsRepo;
        private readonly ISessionContext _session;

        public ContractCreationService(IContractorRateRepository rateRepo, IJobsRepository jobsRepo, ISessionContext session)
        {
            _rateRepo = rateRepo;
            _jobsRepo = jobsRepo;
            _session = session;
        }

        private IEnumerable<Rate> ProposedContractorRatesForJobAndCandidate(int jobId, int candidateId)
        {
            return
                new List<Rate>
                {
                    _rateRepo.GetProposedRateSummaryByJobIdAndContractorId(jobId, candidateId)
                }.AsEnumerable();
        }

        public ContractCreationDetails GetContractDetailsByJobIdAndCandidateId(int jobId, int candidateId)
        {
            Job job = _jobsRepo.GetJobWithJobId(jobId);

            DateTime startDate;

            if (job.StartDate.CompareTo(DateTime.Now) < 0)
                startDate = DateTime.Now;
            else
                startDate = job.StartDate;

            return new ContractCreationDetails
            {
                JobTitle = job.Title,
                StartDate = startDate,
                EndDate = job.EndDate,
                //TimeFactor =
                DaysCancellation = 10,
                //LimitationExpense = 
                //LimitationOfContractType = 
                //LimitationOfContractValue = 
                //PaymentPlan = 
                //AccountExecutive =
                //GMAssigned = 
                //ComissionAssigned = 
                //InvoiceFrequency = 
                //InvoiceFormat = 
                //UsingProjectCode = 
                UsingQuickPay = false,

                Rates = ProposedContractorRatesForJobAndCandidate(jobId, candidateId)
            };
        }
    }
}
