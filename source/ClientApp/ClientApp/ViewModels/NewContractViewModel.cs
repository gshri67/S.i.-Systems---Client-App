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
        private IContractService _contractService;
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
                    ContractorRate = lastContract.Rate;
                }
            }
        }

        public const decimal ServiceRate = 3;
        public decimal ContractorRate { get; set; }
        public decimal TotalRate { get { return ContractorRate + ServiceRate; } }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ApproverEmail { get; set; }
        public string ContractTitle { get; set; }
        public Specialization Specialization { get; set; }

        public NewContractViewModel(IContractService contractService)
        {
            _contractService = contractService;
        }

        public bool ValidateEmailAddress()
        {
            try
            {
                new MailAddress(ApproverEmail);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public ValidationResult Validate()
        {
            if (Specialization == null || string.IsNullOrEmpty(Specialization.Name))
            {
                return new ValidationResult(false, "The contract requires a Specialization");
            }
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
            if (!ValidateEmailAddress())
            {
                return new ValidationResult(false, "Please enter a valid email address");
            }
            return new ValidationResult(true);
        }

        private bool IsDateTodayOrGreater(DateTime date)
        {
            return date >= DateTime.Now.Date;
        }

        public Task<IList<Specialization>> GetAllSpecializations()
        {
            return _contractService.GetAllSpecializations();
        }
    }
}
