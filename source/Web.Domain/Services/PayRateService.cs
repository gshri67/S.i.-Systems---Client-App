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

        public TimesheetSupport GetTimesheetSupportByTimesheetId( int timesheetId )
        {
            Timesheet timesheet = _timesheetRepository.GetTimesheetsById(timesheetId);

            IEnumerable<int> projectCodeIds = _payRateRepository.GetProjectIdFromTimesheet(timesheet);
            IEnumerable<int> payRateIds = Enumerable.Empty<int>();
            
            if( timesheet.Status == MatchGuideConstants.TimesheetStatus.Open)
                payRateIds = _payRateRepository.GetContractRateIdFromOpenTimesheet(timesheet);
            else if (timesheet.Status == MatchGuideConstants.TimesheetStatus.Submitted )
                payRateIds = _payRateRepository.GetContractRateIdFromSavedOrSubmittedTimesheet(timesheet);

            string pcIdString = string.Join(",", projectCodeIds.ToList().Select(n => n.ToString()).ToArray());
            string prIdString = string.Join(",", payRateIds.ToList().Select(n => n.ToString()).ToArray());

            int verticalId = 4;
            IEnumerable<ProjectCodeRateDetails> projectCodeRateDetails = _payRateRepository.GetProjectCodesAndPayRatesFromIds(pcIdString, prIdString, verticalId);

            TimesheetSupport timesheetSupport = new TimesheetSupport();
            IEnumerable<string> pcDescriptions = projectCodeRateDetails.Select((details => details.ProjectCodeDescription)).Distinct();


            List<ProjectCode> projectCodes = new List<ProjectCode>();

            foreach (string desc in pcDescriptions)
            {
                ProjectCode projectCode = new ProjectCode();
                projectCode.Description = desc;
                //add id

                List<PayRate> payRates = new List<PayRate>();
                List<ProjectCodeRateDetails> rateDetails = 
                    projectCodeRateDetails.ToList().Where(details => (details.ProjectCodeDescription == desc)).ToList();

                foreach ( ProjectCodeRateDetails details in rateDetails )
                {
                    PayRate rate = new PayRate();
                    rate.Rate = float.Parse(details.RateAmount);
                    rate.RateType = details.RateType;
                    rate.Id = details.PayRateId;
                    rate.RateDescription = details.RateDescription;

                    payRates.Add(rate);
                }

                projectCode.PayRates = payRates.AsEnumerable();

                projectCodes.Add( projectCode );
            }

            timesheetSupport.ProjectCodeOptions = projectCodes.AsEnumerable();

            return timesheetSupport;
        }
    }
}
