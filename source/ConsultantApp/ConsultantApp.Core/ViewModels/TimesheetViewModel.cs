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

	    private Timesheet _timesheet;

	    public Timesheet Timesheet
	    {
	        get
	        {
	            return _timesheet;
	        }
	    }

	    private IEnumerable<DirectReport> _approvers;
	    private TimesheetSupport _timesheetSupport;
        public TimesheetSupport TimesheetSupport
        {
            get { return _timesheetSupport ?? new TimesheetSupport(); }
            set { _timesheetSupport = value ?? new TimesheetSupport(); }
        }

        private string _alertText;

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
	        return _timesheet == null 
                ? string.Empty 
                : _timesheet.StartDate.ToString(DateLabelFormat);
	    }

	    public Task<Timesheet> SaveTimesheet(Timesheet timesheet)
		{
		    return _api.SaveTimesheet(timesheet);
		}

        public IEnumerable<string> GetProjectCodes()
        {
            if (TimesheetSupport.ProjectCodeOptions.Select(pc => pc.Description).Any())
                return TimesheetSupport.ProjectCodeOptions.Select(pc => pc.Description);
            return Enumerable.Empty<string>();
        }

        public IEnumerable<PayRate> GetPayRatesForIdAndProjectCode(int contractId, string projectCode )
        {
            if( TimesheetSupport.ProjectCodeOptions.Where(pc => pc.Description == projectCode).Any() )
                return TimesheetSupport.ProjectCodeOptions.Where(pc => pc.Description == projectCode).FirstOrDefault().PayRates;
            return Enumerable.Empty<PayRate>();
        }

        public Task<IEnumerable<DirectReport>> GetTimesheetApproversByTimesheetId(int timesheetId)
        {
            return _api.GetTimesheetApproversByTimesheetId(timesheetId);
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
	        _timesheet = await _api.SubmitTimesheet(_timesheet);
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
            //todo:Make the Withdraw Timesheet Call to the API
	        _timesheet.Status = MatchGuideConstants.TimesheetStatus.Open;
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
            _approvers = await GetTimesheetApproversByTimesheetId(_timesheet.Id);
        }


	    private Task LoadTimesheet(Timesheet timesheet)
	    {
	        return Task.Run(() =>
	        {
                _timesheet = timesheet;
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
            return _timesheet == null
                ? string.Empty
                : _timesheet.TimeEntries.Sum(t => t.Hours).ToString();
	    }

	    public string MonthText()
	    {
            return _timesheet == null
                ? string.Empty
                : _timesheet.StartDate.ToString("MMM").ToUpper();
	    }

	    public string TimesheetPeriodRange()
	    {
            return _timesheet == null
                ? string.Empty
                : _timesheet.StartDate.ToString("dd").TrimStart('0') + "-" + _timesheet.EndDate.ToString("dd").TrimStart('0') + ", " + _timesheet.StartDate.ToString("yyyy");
	    }

	    public string TimesheetApproverEmail()
	    {
	        if (_timesheet == null || _timesheet.TimesheetApprover == null)
	            return string.Empty;

	        return _timesheet.TimesheetApprover.Email;
	    }

	    public bool TimesheetIsSubmitted()
	    {
            return _timesheet != null && _timesheet.Status == MatchGuideConstants.TimesheetStatus.Submitted;
	    }

	    public bool TimesheetIsApproved()
	    {
	        if (_timesheet == null)
	            return false;
	        
            var status = _timesheet.Status;
		    return status == MatchGuideConstants.TimesheetStatus.Approved || 
		           status == MatchGuideConstants.TimesheetStatus.Batched ||
		           status == MatchGuideConstants.TimesheetStatus.Moved ||
		           status == MatchGuideConstants.TimesheetStatus.Accepted;
	    }

	    public bool TimesheetIsOpen()
	    {
	        return _timesheet != null &&
                _timesheet.Status == MatchGuideConstants.TimesheetStatus.Open;
	    }

	    public bool TimesheetIsNull()
	    {
	        return _timesheet == null;
	    }

	    public DateTime StartDate()
	    {
	        return _timesheet.StartDate;
	    }
        
        public DateTime EndDate()
        {
            return _timesheet.EndDate;
        }

	    public IEnumerable<TimeEntry> TimeSheetEntries()
	    {
	        return _timesheet.TimeEntries ?? Enumerable.Empty<TimeEntry>();
	    }

	    public float TotalHours()
	    {
	        return _timesheet.TimeEntries.Sum(t => t.Hours);
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
            _timesheet.TimesheetApprover = selectedApprover;
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

            partialApprovers = partialApprovers.OrderByDescending(pa => pa.Email).ToList();

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
	}
}

