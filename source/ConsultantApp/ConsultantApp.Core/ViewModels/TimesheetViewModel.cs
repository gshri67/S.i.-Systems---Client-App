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
	    public string AlertText { get; set; }

	    private IEnumerable<DirectReport> _approvers;
        private const string DateLabelFormat = "MMMMM yyyy";
	    private const float Tolerance = (float) 0.00001;

	    public Task LoadingTimesheet;
        public Task LoadingTimesheetApprovers;
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

	    public Task<Timesheet> SaveTimesheet(Timesheet timesheet)
		{
		    return _api.SaveTimesheet(timesheet);
		}

		public Task<IEnumerable<string>> GetProjectCodes()
		{
			return _api.GetProjectCodes();
		}

		public Task<IEnumerable<PayRate>> GetPayRates(int contractId)
		{
			return _api.GetPayRates(contractId);
		}

        public Task<IEnumerable<DirectReport>> GetTimesheetApproversByTimesheetId(int timesheetId)
        {
            return _api.GetTimesheetApproversByTimesheetId(timesheetId);
        }

	    public void SubmitTimesheet()
	    {
	        SubmittingTimesheet = SubmitCurrentTimesheet();
	        SubmittingTimesheet.ContinueWith(_ => AlertText = "Successfully submitted timesheet",
	            TaskContinuationOptions.OnlyOnRanToCompletion);
            SubmittingTimesheet.ContinueWith(_ => AlertText = "Failed to submit the timesheet",
                TaskContinuationOptions.NotOnRanToCompletion);
	    }

        private async Task SubmitCurrentTimesheet()
	    {
	        Timesheet = await _api.SubmitTimesheet(Timesheet);
	    }

        public void WithdrawTimesheet()
        {
            WithdrawingTimesheet = WithdrawCurrentTimesheet();
            WithdrawingTimesheet.ContinueWith(_ => AlertText = "Successfully withdrew timesheet",
                TaskContinuationOptions.OnlyOnRanToCompletion);
            WithdrawingTimesheet.ContinueWith(_ => AlertText = "Failed to withdraw the timesheet",
                TaskContinuationOptions.NotOnRanToCompletion);
        }
	    private async Task WithdrawCurrentTimesheet()
	    {
            //todo:Make the Withdraw Timesheet Call to the API
	        Timesheet.Status = MatchGuideConstants.TimesheetStatus.Open;
	    }

	    public void SetTimesheet(Timesheet timesheet)
	    {
            LoadingTimesheet = LoadTimesheet(timesheet);
            // LoadingTimesheet.ContinueWith() //whatever work needs to be done in this class when we finish loading the timesheet
	    }

	    public void GetTimesheetApprovers()
	    {
	        LoadingTimesheetApprovers = LoadTimesheetApprovers();
	    }

	    private Task LoadTimesheet(Timesheet timesheet)
	    {
	        return Task.Run(() =>
	        {
                Timesheet = timesheet;
	        });
	    }

	    private async Task LoadTimesheetApprovers()
	    {
            _approvers = await GetTimesheetApproversByTimesheetId(Timesheet.Id);
	    }

	    public string TotalHoursText()
	    {
            return Timesheet == null
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

	    public bool TimesheetIsSubmitted()
	    {
	        return Timesheet != null && Timesheet.Status == MatchGuideConstants.TimesheetStatus.Submitted;
	    }

	    public bool TimesheetIsApproved()
	    {
	        if (Timesheet == null)
	            return false;
	        
            var status = Timesheet.Status;
		    return status == MatchGuideConstants.TimesheetStatus.Approved || 
		           status == MatchGuideConstants.TimesheetStatus.Batched ||
		           status == MatchGuideConstants.TimesheetStatus.Moved ||
		           status == MatchGuideConstants.TimesheetStatus.Accepted;
	    }

	    public bool TimesheetIsOpen()
	    {
	        return Timesheet != null &&
                Timesheet.Status == MatchGuideConstants.TimesheetStatus.Open;
	    }

	    public bool TimesheetIsNull()
	    {
	        return Timesheet == null;
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

        //public string SubmittedSuccessfullyAlertText()
        //{
        //    return "Successfully submitted timesheet";
        //}

        //public string WithdrewSuccessfullyAlertText()
        //{
        //    return "Successfully withdrew timesheet";
        //}

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

            partialApprovers = partialApprovers.OrderByDescending(pa => pa.Email).ToList();

            _approvers = frequentlyUsed.Concat(partialApprovers).ToList();
	        return _approvers.Select(a => a.Email).ToList();
	    }

	    public int NumberOfFrequentlyUsedApprovers()
	    {
            return CountOfFrequentlyUsedapprovers;
	    }
	}
}

