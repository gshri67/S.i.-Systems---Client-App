using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.Core;
using SiSystems.SharedModels;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using ConsultantApp.Core.ViewModels;

namespace ConsultantApp.Core.ViewModels
{
	public class TimesheetViewModel
	{
        private readonly IMatchGuideApi _api;

	    public Timesheet Timesheet { get; private set; }

	    private IEnumerable<DirectReport> _approvers;
	    private TimesheetSupport _timesheetSupport;
        public TimesheetSupport TimesheetSupport
        {
            get { return _timesheetSupport ?? new TimesheetSupport(); }
            set { _timesheetSupport = value ?? new TimesheetSupport(); }
        }

        public DateTime SelectedDate { get; set; }

        private string _alertText;
        public string CancelReason = string.Empty;

        private const string DateLabelFormat = "MMMMM yyyy";
	    private const float Tolerance = (float) 0.00001;

	    public Task LoadingTimesheet;

	    public Task SubmittingTimesheet;
	    public Task WithdrawingTimesheet;

	    public TimesheetViewModel(IMatchGuideApi matchGuideApi)
	    {
	        _api = matchGuideApi;
	    }

	    public string TextForDate()
	    {
	        return Timesheet == null 
                ? string.Empty 
                : Timesheet.StartDate.ToString(DateLabelFormat);
	    }

	    public Task SaveTimesheet()
	    {
	        var savingTimesheetTask = SaveTimesheetCall();
	        return savingTimesheetTask;
	    }

	    private async Task SaveTimesheetCall()
		{
		    Timesheet = await _api.SaveTimesheet(Timesheet);
		}

	    public void SubmitTimesheet()
	    {
	        SubmittingTimesheet = SubmitCurrentTimesheet();
	        SubmittingTimesheet.ContinueWith(_ => _alertText = "Successfully submitted timesheet",
	            TaskContinuationOptions.OnlyOnRanToCompletion);
            SubmittingTimesheet.ContinueWith(_ => _alertText = "Failed to submit the timesheet",
                TaskContinuationOptions.NotOnRanToCompletion);
	    }

        private async Task SubmitCurrentTimesheet()
	    {
	        Timesheet = await _api.SubmitTimesheet(Timesheet);
	    }

        public void WithdrawTimesheet()
        {
            WithdrawingTimesheet = WithdrawCurrentTimesheet();
            WithdrawingTimesheet.ContinueWith(_ => _alertText = "Successfully withdrew timesheet",
                TaskContinuationOptions.OnlyOnRanToCompletion);
            WithdrawingTimesheet.ContinueWith(_ => _alertText = "Failed to withdraw the timesheet",
                TaskContinuationOptions.NotOnRanToCompletion);
        }
	    private async Task WithdrawCurrentTimesheet()
	    {
            Timesheet = await _api.WithdrawTimesheet(Timesheet.Id, CancelReason );
	    }

	    public void SetTimesheet(Timesheet timesheet)
	    {
            LoadingTimesheet = LoadTimesheet(timesheet);
            // LoadingTimesheet.ContinueWith() //whatever work needs to be done in this class when we finish loading the timesheet
	    }

	    public Task GetTimesheetApprovers()
	    {
	        var loadingTimesheetApproversTask = LoadTimesheetApprovers();
	        return loadingTimesheetApproversTask;
	    }

        private async Task LoadTimesheetApprovers()
        {
            _approvers = await _api.GetTimesheetApproversByAgreementId(Timesheet.AgreementId);
        }


	    private Task LoadTimesheet(Timesheet timesheet)
	    {
	        return Task.Run(() =>
	        {
                Timesheet = timesheet;
	        });
	    }

        public Task LoadTimesheetSupport()
	    {
            var timesheetSupportTask = GetTimesheetSupport();
	        return timesheetSupportTask;
	    }

        private async Task GetTimesheetSupport()
        {
            TimesheetSupport = await _api.GetTimesheetSupportForTimesheet(Timesheet);
        }

	    public string TotalHoursText()
	    {
	        return (Timesheet == null || Timesheet.TimeEntries == null)
                ? string.Empty
                : Timesheet.TimeEntries.Sum(t => t.Hours).ToString();
	    }

	    public string MonthText()
	    {
            return Timesheet == null
                ? string.Empty
                : Timesheet.StartDate.ToString("MMM").ToUpper();
	    }

	    public string TimesheetPeriodRange()
	    {
            return Timesheet == null
                ? string.Empty
                : Timesheet.StartDate.ToString("dd").TrimStart('0') + "-" + Timesheet.EndDate.ToString("dd").TrimStart('0') + ", " + Timesheet.StartDate.ToString("yyyy");
	    }

	    public string TimesheetApproverEmail()
	    {
	        if (Timesheet == null || Timesheet.TimesheetApprover == null)
	            return string.Empty;

	        return Timesheet.TimesheetApprover.Email;
	    }

	    public bool CanChangeTimesheetStatus()
	    {
	        return Timesheet.Status == MatchGuideConstants.TimesheetStatus.Submitted
	               || Timesheet.Status == MatchGuideConstants.TimesheetStatus.Open;
	    }

	    public bool TimesheetIsSubmitted()
	    {
            return Timesheet != null && Timesheet.Status == MatchGuideConstants.TimesheetStatus.Submitted;
	    }

	    public bool TimesheetIsOpen()
	    {
	        return Timesheet != null &&
                Timesheet.Status == MatchGuideConstants.TimesheetStatus.Open;
	    }

        public bool TimesheetIsEditable()
        {
            return Timesheet != null && TimesheetIsOpen();
        }

	    public DateTime StartDate()
	    {
	        return Timesheet.StartDate;
	    }
        
        public DateTime EndDate()
        {
            return Timesheet.EndDate;
        }

	    public IEnumerable<TimeEntry> TimeSheetEntries()
	    {
	        return Timesheet.TimeEntries ?? Enumerable.Empty<TimeEntry>();
	    }

	    public float TotalHours()
	    {
	        return Timesheet.TimeEntries.Sum(t => t.Hours);
	    }

	    public bool TimesheetHasZeroHours()
	    {
	        return  Math.Abs(TotalHours()) < Tolerance;
	    }

	    public string SubmitTimesheetConfirmationTitle()
	    {
            return TimesheetHasZeroHours()
                ? "Are you sure you want to submit a timesheet with zero entries?"
                : "Are you sure you want to submit this timesheet?";
	    }

	    public void SetTimesheetApprover(DirectReport selectedApprover)
	    {
            Timesheet.TimesheetApprover = selectedApprover;
            ActiveTimesheetViewModel.IncrementApproverCount(selectedApprover);
	    }

        public void SetTimesheetApproverByEmail(string selectedApproverEmail)
        {
            var selectedApprover = _approvers.FirstOrDefault(a => a.Email == selectedApproverEmail);

            SetTimesheetApprover(selectedApprover);
        }

	    public IEnumerable<DirectReport> MostFrequentlyUsedApprovers()
	    {
            var mostFrequentEmails = ActiveTimesheetViewModel.MostFrequentTimesheetApprovers();

            return _approvers.Where(approver => mostFrequentEmails.Contains(approver.Email));
	    }

	    private int CountOfFrequentlyUsedapprovers;

	    public List<string> ApproverEmailsSortedByFrequency()
	    {
            var mostFrequentlyUsed = MostFrequentlyUsedApprovers();
	        CountOfFrequentlyUsedapprovers = mostFrequentlyUsed.Count();
            var frequentlyUsed = mostFrequentlyUsed as IList<DirectReport> ?? mostFrequentlyUsed.ToList();
            var partialApprovers = _approvers.Except(frequentlyUsed).ToList();

            partialApprovers = partialApprovers.OrderBy(pa => pa.Email).ToList();

            _approvers = frequentlyUsed.Concat(partialApprovers).ToList();
	        return _approvers.Select(a => a.Email).ToList();
	    }

	    public int NumberOfFrequentlyUsedApprovers()
	    {
            return CountOfFrequentlyUsedapprovers;
	    }

	    public string GetAlertText()
	    {
	        return _alertText ?? string.Empty;
	    }

        public IEnumerable<TimeEntry> GetSelectedDatesTimeEntries()
        {
            if (Timesheet == null || Timesheet.TimeEntries == null)
                return Enumerable.Empty<TimeEntry>();

            return Timesheet.TimeEntries.Where(e => e.EntryDate.Equals(SelectedDate));
        }

	    public IEnumerable<TimeEntry> GetTimeEntriesForDate(DateTime date)
	    {
	        if (Timesheet == null || Timesheet.TimeEntries == null)
	            return Enumerable.Empty<TimeEntry>();

            return Timesheet.TimeEntries.Where(e => e.EntryDate.Equals(date));
	    }

	    public float NumberOfHoursForSelectedDate()
        {
            var entries = GetSelectedDatesTimeEntries();
            return entries.Sum(time => time.Hours);
        }

	    public float NumberOfHoursForDate(DateTime date)
	    {
            var entries = GetTimeEntriesForDate(date);
            return entries.Sum(time=>time.Hours);   
	    }
        
	    public void AddTimeEntry(TimeEntry newEntry)
	    {
            Timesheet.TimeEntries.Add(newEntry);
	    }

        public bool SelectedDateHasUnpopulatedRateCodes()
        {
            return GetSelectedDatesTimeEntries().Count() < TimesheetSupport.ProjectCodeOptions.Count();
        }

        #region DayNavigation
        public void NavigateToPreviousDay()
	    {
            if (CanNavigateToPreviousDay())
                SelectedDate = SelectedDate.AddDays(-1);
	    }

	    public void NavigateToNextDay()
	    {
	        if (CanNavigteToNextDay())
	            SelectedDate = SelectedDate.AddDays(1);
	    }

	    public bool CanNavigteToNextDay()
	    {
	        return DateIsContainedWithinTimesheet(SelectedDate.AddDays(1));
	    }

	    public bool CanNavigateToPreviousDay()
	    {
	        return DateIsContainedWithinTimesheet(SelectedDate.AddDays(-1));
	    }

        private bool DateIsContainedWithinTimesheet(DateTime date)
        {
            return date >= Timesheet.StartDate && date <= Timesheet.EndDate;
        }
        #endregion
        
	    private TimeEntry CopyOfPreviousDaysEntryIfOnlyOneValue()
	    {
	        var previousEntries = GetTimeEntriesForDate(SelectedDate.AddDays(-1)).ToList();
            if(previousEntries.Count() != 1)
	            return null;

	        var entry = previousEntries.FirstOrDefault();
	        return new TimeEntry
	        {
                EntryDate = SelectedDate,
	            CodeRate = entry.CodeRate,
                Hours = entry.Hours
	        };
	    }

	    public TimeEntry DefaultNewEntry()
	    {
            //try to set the current values to the same as the previous days entry
            var newEntry = CopyOfPreviousDaysEntryIfOnlyOneValue();
	        if (newEntry != null)
	            return newEntry;
            
            newEntry = new TimeEntry
            {
                EntryDate = SelectedDate,
                Hours = 8,
                CodeRate = TimesheetSupport.ProjectCodeOptions.Count() == 1 
                    ? TimesheetSupport.ProjectCodeOptions.FirstOrDefault() 
                    : new ProjectCodeRateDetails
                    {
                        PONumber = "Project Code", 
                        ratedescription = "Pay Rate"
                    }
            };
            
	        return newEntry;
	    }

	    public void SetSelectedDatesEntries(IEnumerable<TimeEntry> timeEntries)
	    {
            Timesheet.TimeEntries = Timesheet.TimeEntries.Where(entry => entry.EntryDate != SelectedDate).ToList();
	        Timesheet.TimeEntries.AddRange(timeEntries);
	    }

	    public string SubmitButtonText()
	    {
	        return TimesheetIsSubmitted() ? "Withdraw" : "Submit";
	    }

	    private static MatchGuideConstants.TimesheetStatus UserFriendlyStatus(MatchGuideConstants.TimesheetStatus status)
	    {
	        if (status == MatchGuideConstants.TimesheetStatus.Accepted
	            || status == MatchGuideConstants.TimesheetStatus.Approved
	            || status == MatchGuideConstants.TimesheetStatus.Moved
	            || status == MatchGuideConstants.TimesheetStatus.Batched)
	            return MatchGuideConstants.TimesheetStatus.Approved;

	        return status;
	    }

	    public string TimesheetStatus()
	    {
            var status = UserFriendlyStatus(Timesheet.Status);
	        return status.ToString();
	    }
	}
}

