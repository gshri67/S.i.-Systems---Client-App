using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using SiSystems.ClientApp.Web.Domain.Repositories.AccountExecutive;
using SiSystems.Web.Domain.Context;
using SiSystems.ConsultantApp.Web.Domain.Repositories;
using SiSystems.SharedModels;

namespace SiSystems.ConsultantApp.Web.Domain.Services
{
    /// <summary>
    /// You might be wondering why there are two layers here.
    /// 
    /// Service intended to perform any logic, transformations, etc..
    /// so that this can be tested. Repo/data access can be mocked out.
    /// </summary>
    public class TimesheetService
    {
        private readonly ITimesheetRepository _timeSheetRepository;
        private readonly ITimeEntryRepository _timeEntryRepository;
        private readonly IDirectReportRepository _directReportRepository;
        private readonly IActivityRepository _activityRepository;
        private readonly IUserContactRepository _userContactRepository;
        private readonly ISessionContext _sessionContext;

        /// <summary>
        /// This is a constant value to correct for possible Floating Point issues
        /// </summary>
        private const float Tolerance = 0.00001F;

        public TimesheetService(ITimesheetRepository timesheetRepository, ITimeEntryRepository timeEntryRepository,
            IDirectReportRepository directReportRepository, IActivityRepository activityRepository, ISessionContext sessionContext, IUserContactRepository userContactRepository)
        {
            _timeSheetRepository = timesheetRepository;
            _timeEntryRepository = timeEntryRepository;
            _directReportRepository = directReportRepository;
            _activityRepository = activityRepository;
            _sessionContext = sessionContext;
            _userContactRepository = userContactRepository;
        }

        public Timesheet SaveTimesheet(Timesheet timesheet)
        {
            UpdateTimesheetApproverIfNecessary(timesheet);

            var userId = _sessionContext.CurrentUser.Id;

            var savedTimesheetId = _timeSheetRepository.SaveTimesheet(timesheet, userId);

            foreach (var entry in timesheet.TimeEntries)
                _timeEntryRepository.SaveTimeEntry(savedTimesheetId, entry);

            timesheet.OpenStatusId = savedTimesheetId;

            return timesheet;
        }

        public Timesheet GetTimesheetById(int id)
        {
            var updatedTimesheet = _timeSheetRepository.GetTimesheetsById(id);

            updatedTimesheet.TimesheetApprover = updatedTimesheet.TimesheetApprover ?? 
                _timeSheetRepository.GetDirectReportByTimesheetId(updatedTimesheet.Id);
            
            updatedTimesheet.TimeEntries = _timeEntryRepository.GetTimeEntriesForTimesheet(updatedTimesheet);

            return updatedTimesheet;
        }

        public Timesheet SubmitTimesheet(Timesheet timesheet)
        {
            UpdateTimesheetApproverIfNecessary(timesheet);

            timesheet = SubmittingZeroTimesheet(timesheet) 
                ? SubmitZeroTime(timesheet) 
                : SubmitTimesheetWithTimeEntries(timesheet);

            return GetTimesheetById(timesheet.Id);
        }

        public Timesheet WithdrawTimesheet(Timesheet timesheet, string cancelReason )
        {
            int createUserId = _timeSheetRepository.GetCreateUserIdFromTimesheet(timesheet.Id);
            string submittedPDF = _timeSheetRepository.GetSubmittedPDFFromTimesheet(timesheet.Id); 
            _timeSheetRepository.WithdrawTimesheet(timesheet.Id, "SubmitCancel", timesheet.CreateUserId, timesheet.CancelledPDFName, cancelReason);
         
            return GetTimesheetById(timesheet.Id);
        }

        private void UpdateTimesheetApproverIfNecessary(Timesheet timesheet)
        {
            var previousTimesheetApprover = _directReportRepository.GetCurrentTimesheetApproverForTimesheet(timesheet.Id);
            if (previousTimesheetApprover != timesheet.TimesheetApprover)
                UpdateTimesheetApprover(timesheet, previousTimesheetApprover);
        }

        private void UpdateTimesheetApprover(Timesheet timesheet, DirectReport previousDirectReport)
        {
            _directReportRepository.UpdateDirectReport(timesheet, previousDirectReport.Id, _sessionContext.CurrentUser.Id);
            _activityRepository.InsertUpdateRecordActivity(timesheet, _sessionContext.CurrentUser.Id);
        }

        private Timesheet SubmitTimesheetWithTimeEntries(Timesheet timesheet)
        {
            timesheet.Id = _timeSheetRepository.SubmitTimesheet(timesheet, _sessionContext.CurrentUser.Id);

            SubmitTimeEntries(timesheet);

            return timesheet;
        }

        private void SubmitTimeEntries(Timesheet timesheet)
        {
            foreach (var timeEntry in timesheet.TimeEntries)
            {
                _timeEntryRepository.SubmitTimeEntry(timesheet.Id, timeEntry);
            }
        }

        private Timesheet SubmitZeroTime(Timesheet timesheet)
        {
            timesheet.Id = _timeSheetRepository.SubmitZeroTimeForUser(timesheet, _sessionContext.CurrentUser.Id);
            
            return timesheet;
        }

        private static bool SubmittingZeroTimesheet(Timesheet timesheet)
        {
            if (timesheet.TimeEntries.Any())
            {
                return Math.Abs(timesheet.TimeEntries.Sum(entry => entry.Hours)) < Tolerance;
            }

            return true;
        }



        public TimesheetSummarySet GetTimesheetsSummary()
        {
            var summary = _timeSheetRepository.GetTimesheetSummaryByAccountExecutiveId(_sessionContext.CurrentUser.Id);

            AssertCurrentUserHasPermissionsToViewTimesheetSummaries(summary);

            return summary;
        }

        private void AssertCurrentUserHasPermissionsToViewTimesheetSummaries(TimesheetSummarySet summaries)
        {
            //todo: add business rules for an AE permissions
            if(false)
                throw new UnauthorizedAccessException();
        }

        public IEnumerable<TimesheetDetails> GetTimesheetsDetails(MatchGuideConstants.TimesheetStatus status)
        {
            IEnumerable<TimesheetDetails> details;
            
            if( status == MatchGuideConstants.TimesheetStatus.Open )    
                details = _timeSheetRepository.GetOpenTimesheetDetailsByAccountExecutiveId(_sessionContext.CurrentUser.Id);
            else if (status == MatchGuideConstants.TimesheetStatus.Submitted)
                details = _timeSheetRepository.GetSubmittedTimesheetDetailsByAccountExecutiveId(_sessionContext.CurrentUser.Id);
            else if (status == MatchGuideConstants.TimesheetStatus.Cancelled)
                details = _timeSheetRepository.GetCancelledTimesheetDetailsByAccountExecutiveId(_sessionContext.CurrentUser.Id);
            else if (status == MatchGuideConstants.TimesheetStatus.Rejected)
                details = _timeSheetRepository.GetRejectedTimesheetDetailsByAccountExecutiveId(_sessionContext.CurrentUser.Id);
            else
                details = Enumerable.Empty<TimesheetDetails>();

            return details;
        }

        //get open Timesheet
        public TimesheetContact GetOpenTimesheetContactByAgreementId(int id)
        {
            var contact = _timeSheetRepository.GetOpenTimesheetContactByAgreementId(id);

            if (contact.Contractor == null)
                contact.Contractor = new UserContact();
            contact.Contractor = _userContactRepository.GetCandidateUserContactByAgreementId(id);

            if (contact.DirectReport == null)
                contact.DirectReport = new UserContact();
            contact.DirectReport = _userContactRepository.GetDirectReportByAgreementId(id);

            return contact;
        }

        //get unopen Timesheet
        public TimesheetContact GetTimesheetContactById(int id)
        {
            var contact = _timeSheetRepository.GetTimesheetContactById(id);

            if(contact.Contractor == null)
                contact.Contractor = new UserContact();
            contact.Contractor = _userContactRepository.GetUserContactById(contact.Contractor.Id);

            if (contact.DirectReport == null)
                contact.DirectReport = new UserContact();
            contact.DirectReport = _userContactRepository.GetUserContactById(contact.DirectReport.Id);

            return contact;
        }

        public void GetPayRatesAndProjectCodesFromTimesheet( Timesheet timesheet )
        {

        }
    }
}
