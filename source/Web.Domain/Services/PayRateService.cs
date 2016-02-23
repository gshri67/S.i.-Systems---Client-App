using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using SiSystems.Web.Domain.Context;
using SiSystems.ConsultantApp.Web.Domain.Repositories;
using SiSystems.SharedModels;
using SiSystems.Web.Domain.Context;

namespace SiSystems.ConsultantApp.Web.Domain.Services
{
    public class PayRateService
    {
        private readonly ISessionContext _sessionContext;
        private readonly IPayRateRepository _payRateRepository;
        private readonly ITimesheetRepository _timesheetRepository;

        public PayRateService(ISessionContext sessionContext, IPayRateRepository payRateRepository, ITimesheetRepository timesheetRepository)
        {
            _sessionContext = sessionContext;
            _payRateRepository = payRateRepository;
            _timesheetRepository = timesheetRepository;
        }

        public IEnumerable<PayRate> GetPayRatesByContractId(int id)
        {
            var payRates = _payRateRepository.GetPayRates();

            var list = new List<string>();
            var ratesList = new List<PayRate>();

            foreach (var payRate in payRates.Where(payRate => payRate.RateDescription != null && !list.Contains(payRate.RateDescription)))
            {
                list.Add(payRate.RateDescription);
                ratesList.Add(payRate);
            }  

            return ratesList;
        }

        public TimesheetSupport GetTimesheetSupportByTimesheet(Timesheet timesheet)
        {
            var projectCodeIds = _payRateRepository.GetProjectIdFromTimesheet(timesheet);
            
            var payRateIds = timesheet.Status == MatchGuideConstants.TimesheetStatus.Open
                ? _payRateRepository.GetContractRateIdFromOpenTimesheet(timesheet)
                : _payRateRepository.GetContractRateIdFromSavedOrSubmittedTimesheet(timesheet);
            
            var projectCodeRateDetails = _payRateRepository.GetProjectCodesAndPayRatesFromIds(projectCodeIds, payRateIds);
            
            var projectCodes = ProjectCodesFromCodeRateDescriptions(projectCodeRateDetails);

            return new TimesheetSupport
            {
                ProjectCodeOptions = projectCodes
            };
        }

        private static IEnumerable<ProjectCode> ProjectCodesFromCodeRateDescriptions(IEnumerable<ProjectCodeRateDetails> projectCodeRateDetails)
        {
            var codeRatesGroupedByDescription = projectCodeRateDetails.GroupBy(details => details.ProjectCodeDescription);

            var projectCodes = codeRatesGroupedByDescription.Select(grouping => new ProjectCode
            {
                Description = grouping.Key,
                PayRates = grouping.Select(codeRate => new PayRate
                {
                    Id = codeRate.PayRateId,
                    Rate = float.Parse(codeRate.RateAmount),
                    RateType = codeRate.RateType,
                    RateDescription = codeRate.RateDescription
                })
            });
            return projectCodes;
        }
    }
}
