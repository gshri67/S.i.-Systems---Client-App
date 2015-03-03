using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using ClientApp.Services.Interfaces;
using SiSystems.ClientApp.SharedModels;

namespace ClientApp.ViewModels
{
    public class NewContractViewModel : ViewModelBase
    {
        private readonly IContractService _contractService;
        private Consultant _consultant;
        public Consultant Consultant
        {
            get { return _consultant;}
            set
            {
                _consultant = value;
                var lastContract = _consultant.Contracts.OrderByDescending(c => c.EndDate).First();
                if (lastContract != null)
                {
                    LastContractRate = lastContract.Rate;
                }
            }
        }

        //TODO get from Client object
        public const decimal ServiceRate = 3;

        public decimal LastContractRate { get; private set; }
        public decimal ContractorRate { get; set; }
        public decimal TotalRate { get { return ContractorRate + ServiceRate; } }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string TimesheetApprovalEmail { get; set; }
        public string ContractApprovalEmail { get; set; }
        public string ContractTitle { get; set; }

        public NewContractViewModel(IContractService contractService)
        {
            _contractService = contractService;
        }

        public bool ValidateEmailAddress(string email)
        {
            try
            {
                new MailAddress(email);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public ValidationResult Validate()
        {
            if (string.IsNullOrEmpty(ContractTitle))
            {
                return new ValidationResult(false, "The contract requires a Title");
            }
            if (ContractorRate <= 0)
            {
                return new ValidationResult(false, "You must enter a valid contractor rate");
            }
            if (!IsDateTodayOrGreater(StartDate))
            {
                return new ValidationResult(false, "Start Date must be today or greater");
            }
            if (!IsDateTodayOrGreater(EndDate))
            {
                return new ValidationResult(false, "End Date must be today or greater");
            }
            if (EndDate - StartDate <= TimeSpan.Zero)
            {
                return new ValidationResult(false, "End Date must be greater than the Start Date");
            }
            if (!ValidateEmailAddress(TimesheetApprovalEmail))
            {
                return new ValidationResult(false, "Please enter a valid timesheet approval email address");
            }
            if (!ValidateEmailAddress(ContractApprovalEmail))
            {
                return new ValidationResult(false, "Please enter a valid contract approval email address");
            }
            return new ValidationResult(true);
        }

        private bool IsDateTodayOrGreater(DateTime date)
        {
            return date >= DateTime.Now.Date;
        }

        public async Task SubmitContract()
        {
            await _contractService.Submit(new ContractProposal
            {
                ConsultantId = this.Consultant.Id,
                RateToConsultant = this.ContractorRate,
                Fee = ServiceRate,
                StartDate = this.StartDate,
                EndDate = this.EndDate,
                TimesheetApproverEmailAddress = this.TimesheetApprovalEmail,
                ContractApproverEmailAddress = this.ContractApprovalEmail,
            });
        }
    }
}
