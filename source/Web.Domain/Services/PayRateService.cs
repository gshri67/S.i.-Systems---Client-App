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

        public TimesheetSupport GetTimesheetSupportByTimesheet(Timesheet timesheet)
        {
            var projectCodeIds = _payRateRepository.GetProjectIdsFromTimesheet(timesheet);
            
            var payRateIds = timesheet.Status == MatchGuideConstants.TimesheetStatus.Open
                ? _payRateRepository.GetContractRateIdFromOpenTimesheet(timesheet)
                : _payRateRepository.GetContractRateIdFromSavedOrSubmittedTimesheet(timesheet);
            
            var projectCodeRateDetails = _payRateRepository.GetProjectCodesAndPayRatesFromIds(projectCodeIds, payRateIds);

            return new TimesheetSupport
            {
                ProjectCodeOptions = projectCodeRateDetails
            };
        }
    }
}
