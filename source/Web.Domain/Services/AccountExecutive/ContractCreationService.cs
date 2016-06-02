using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiSystems.ClientApp.Web.Domain.Repositories.AccountExecutive;
using SiSystems.ConsultantApp.Web.Domain.Repositories;
using SiSystems.SharedModels;
using SiSystems.Web.Domain.Context;

namespace SiSystems.ClientApp.Web.Domain.Services.AccountExecutive
{
    public class ContractCreationService
    {
        private readonly IContractorRateRepository _rateRepo;
        private readonly IJobsRepository _jobsRepo;
        private readonly IConsultantContractRepository _contractRepo;
        private readonly IUserContactRepository _usersRepo;
        private readonly ITimesheetRepository _timesheetRepo;
        private readonly IPickListValuesRepository _pickListRepo;
        private readonly ISessionContext _session;

        public ContractCreationService(IContractorRateRepository rateRepo, IJobsRepository jobsRepo, IConsultantContractRepository contractRepo, IUserContactRepository usersRepo, ITimesheetRepository timesheetRepo, IPickListValuesRepository pickListRepo, ISessionContext session)
        {
            _rateRepo = rateRepo;
            _jobsRepo = jobsRepo;
            _contractRepo = contractRepo;
            _usersRepo = usersRepo;
            _timesheetRepo = timesheetRepo;
            _pickListRepo = pickListRepo;
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
                DaysCancellation = "Two",
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

        public ContractCreationDetails_Review GetDetailsForReviewContractCreationForm(int jobId, int candidateId)
        {

            return new ContractCreationDetails_Review()
            {
                WebTimeSheetAccess = true,
                WebTimeSheetProjectAccess = true,
                TimesheetType = "S.i. E-Timesheets"
            };
        }

        public int SubmitContract(int jobId, int candidateId, ContractCreationDetails contractDetails )
        {
            Job job = _jobsRepo.GetJobWithJobId(jobId);
            Timesheet timesheet = new Timesheet();
            string email = string.Empty;
            
            UserContact candidateInfo = _usersRepo.GetUserContactById(candidateId);
            if (candidateInfo.EmailAddresses != null && candidateInfo.EmailAddresses.Any())
                email = candidateInfo.EmailAddresses.ElementAt(0).Email;

            int internalUserId = _session.CurrentUser.Id;
            
            string candidatePaymentType = string.Empty;
            int candidatePaymentId = _pickListRepo.GetPickListIdForTitle(candidatePaymentType);
            int SAID = 0;
            string PamRateId = "";

            _contractRepo.SubmitContract(job, contractDetails, timesheet, candidateId, email, internalUserId, _pickListRepo.GetPickListIdForTitle(contractDetails.TimeFactor), _pickListRepo.GetPickListIdForTitle(contractDetails.PaymentPlan), _pickListRepo.GetPickListIdForTitle(contractDetails.InvoiceFormat), _pickListRepo.GetPickListIdForTitle(contractDetails.InvoiceFrequency), candidatePaymentId, SAID );
            
            foreach (Rate rate in contractDetails.Rates)
            {
                if (candidatePaymentType == "Sole-proprietorship")
                {
                                    /*
                                                 IF Candidate Payment type = 'Sole-proprietorship'
	                                IF Contract Type = ‘Flo Thru’ AND Rate Type = ‘Per Hour’
		                                @PAMRateID = ‘SOLHRS’	
	                                ELSEIF Contract Type = ‘Flo Thru’ AND Rate Type = ‘Per Day’
		                                @PAMRateID = ‘SPDAY’
                                ELSEIF Contract Type = ‘Full Margin’ AND Rate Type = ‘Per Hour’
	                                @PAMRateID = ‘FSSPHR’
                                ELSEIF Contract Type = ‘Full Margin’ AND Rate Type = ‘Per Day’
		                                @PAMRateID = ‘FSSPDY’
	                                ELSE
		                                @PAMRateID = ‘REG’*//*
                                                }
                                                else if (candidatePaymentType == "term")
                                                {
                                                    /*
                                ELSEIF Candidate Payment type = 'term'
                                IF Contract Type = ‘Flo Thru’ AND Rate Type = ‘Per Hour’
                                @PAMRateID = ‘FTHR’	
                                ELSEIF Contract Type = ‘Flo Thru’ AND Rate Type = ‘Per Day’
                                @PAMRateID = ‘FTDY’
                                ELSEIF Contract Type = ‘Full Margin’ AND Rate Type = ‘Per Hour’
                                @PAMRateID = ‘CTHR’
                                ELSEIF Contract Type = ‘Full Margin’ AND Rate Type = ‘Per Day’
                                @PAMRateID = ‘CTDY’
                                ELSE
                                @PAMRateID = ‘REG’
                                ELSE
                                @PAMRateID = ‘REG’

                                */
                }
                else
                    PamRateId = "REG";
 
                _contractRepo.SubmitRateTermDetailsForJob(_session.CurrentUser, _pickListRepo.GetPickListIdForTitle(rate.RateType), jobId, rate, job.StartDate, job.EndDate, PamRateId);
            }

            _contractRepo.SubmitAdminContactDetailsForJob(jobId, internalUserId, contractDetails.DirectReport.Id,
                contractDetails.BillingContact.Id);
            return 1;
        }
    }
}
