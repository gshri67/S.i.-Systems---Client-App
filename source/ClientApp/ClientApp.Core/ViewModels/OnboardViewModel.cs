﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Shared.Core;
using SiSystems.SharedModels;

namespace ClientApp.Core.ViewModels
{
    public class OnboardViewModel : ViewModelBase
    {
        private readonly IMatchGuideApi _contractService;
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
                    IsFullySourced = lastContract.IsFullySourced;
                    LastContractTitle = lastContract.Title;
                    LastContractEndDate = lastContract.EndDate;
                }
            }
        }

        public bool IsActiveConsultant { get; set; }
        public bool IsFullySourced { get; private set; }

        public decimal ServiceFee = CurrentUser.ServiceFee;
        public decimal MspPercent = CurrentUser.MspPercent;
        public int InvoiceFormat = CurrentUser.InvoiceFormat;

        public readonly bool ConsultantPaysServiceFee = CurrentUser.FloThruFeePayment ==
                                                MatchGuideConstants.FloThruFeePayment.ContractorPays;

        public readonly bool ConsultantPaysMspPercent = CurrentUser.FloThruMspPayment ==
                                               MatchGuideConstants.FloThruMspPayment.DeductFromContractorPay;

        public decimal LastContractRate { get; private set; }
        public string LastContractTitle { get; private set; }
        public DateTime LastContractEndDate { get; private set; }

        public decimal ContractorRate { get; set; }

        public decimal TotalRate
        {
            get
            {
                if (!ConsultantPaysMspPercent && !ConsultantPaysServiceFee)
                {
                    return ContractorRate + (ContractorRate*MspPercent/100) + ServiceFee;
                }
                else if(ConsultantPaysServiceFee && !ConsultantPaysMspPercent)
                {
                    return ContractorRate + (ContractorRate*MspPercent/100);
                }
                else if (!ConsultantPaysServiceFee && ConsultantPaysMspPercent)
                {
                    return ContractorRate + ServiceFee;
                }
                else
                {
                    return ContractorRate;
                }
            }
        }

        public decimal GetPayRateFromBillRate(decimal billRate)
        {
            if (!ConsultantPaysMspPercent && !ConsultantPaysServiceFee)
            {
                return (billRate - ServiceFee)/(1 + MspPercent/100);
            }
            else if (ConsultantPaysServiceFee && !ConsultantPaysMspPercent)
            {
                return billRate / (1 + MspPercent / 100);
            }
            else if (!ConsultantPaysServiceFee && ConsultantPaysMspPercent)
            {
                return billRate - ServiceFee;
            }
            else
            {
                return billRate;
            }
        }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string TimesheetApprovalEmail { get; set; }
        public string ContractApprovalEmail { get; set; }
        public string ContractTitle { get; set; }

        public OnboardViewModel(IMatchGuideApi contractService)
        {
            _contractService = contractService;
        }

        public bool ValidateEmailAddress(string email)
        {
            try
            {
                return email.EndsWith(CurrentUser.Domain) && Regex.IsMatch(email, @"^[^@]+@[^@]+\.[^@]+$");
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
                return new ValidationResult(false, "Please enter a valid timesheet approval email address. The address must end in " + CurrentUser.Domain);
            }
            if (!ValidateEmailAddress(ContractApprovalEmail))
            {
                return new ValidationResult(false, "Please enter a valid contract approval email address. The address must end in " + CurrentUser.Domain);
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
                Rate = this.ContractorRate,
                Fee = ServiceFee,
                StartDate = this.StartDate,
                EndDate = this.EndDate,
                MspFeePercentage = MspPercent,
                InvoiceFormat = InvoiceFormat,
                TimesheetApproverEmailAddress = this.TimesheetApprovalEmail,
                ContractApproverEmailAddress = this.ContractApprovalEmail,
                FloThruFeePayment = CurrentUser.FloThruFeePayment,
                FloThruMspPayment = CurrentUser.FloThruMspPayment,
                JobTitle = ContractTitle,
                IsFullySourced = IsFullySourced,
                IsRenewal = IsActiveConsultant
            });
        }

        public string GetRateHeader()
        {
            if (MspPercent == 0 && ServiceFee == 0)
            {
                //Culture to make sure if the app is used outside of north america, it won't show confusing numbers
                return string.Format(new CultureInfo("en-us"),"Last Contract: {0:C}/hr", LastContractRate);
            }
            return string.Format(new CultureInfo("en-us"), "Last Contract: {0:C}/hr", GetPayRateFromBillRate(LastContractRate));
        }

        public IList<string> GetRateFooter()
        {
            var strings = new List<string>();
            if (MspPercent == 0 && ServiceFee == 0)
            {
                strings.Add(string.Empty);
                return strings;
            }

            if (MspPercent > 0)
            {
                strings.Add(string.Format("+ {1}MSP rate ({0}%)", MspPercent, ConsultantPaysMspPercent ? "Contractor Pays " : string.Empty));
            }
            if (ServiceFee > 0)
            {
                strings.Add(string.Format(new CultureInfo("en-us"), "+ {1}Service Fee ({0:C}/hr)", ServiceFee, ConsultantPaysServiceFee ? "Contractor Pays " : string.Empty));
            }
            if ((MspPercent > 0 || ServiceFee > 0) && !ConsultantPaysMspPercent && !ConsultantPaysServiceFee)
            {
                strings.Add(string.Format(new CultureInfo("en-us"), "= {0:C}/hr", TotalRate));
            }
            return strings;
        }
    }
}
