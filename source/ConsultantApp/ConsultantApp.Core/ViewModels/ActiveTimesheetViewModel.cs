using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.Core;
using SiSystems.SharedModels;

namespace ConsultantApp.Core.ViewModels
{
	public class ActiveTimesheetViewModel
	{
        private readonly IMatchGuideApi _api;
		private static Dictionary<string, int> _projectCodeDict;
        private static Dictionary<string, int> _approverDict;

	    private IEnumerable<PayPeriod> _payPeriods;
        public IEnumerable<PayPeriod> PayPeriods
	    {
	        get { return _payPeriods; }
            private set { _payPeriods = value ?? Enumerable.Empty<PayPeriod>(); }
	    }

	    public Task LoadingPayPeriods;
	    
	    private const int MaxPeriodHistory = 6;
        private const int MaxFrequentlyUsed = 5;

        public ActiveTimesheetViewModel(IMatchGuideApi matchGuideApi)
	    {
	        _api = matchGuideApi;
            
            PayPeriods = Enumerable.Empty<PayPeriod>();

            if(_projectCodeDict == null)
                _projectCodeDict = new Dictionary<string, int>();
            
            if (_approverDict == null)
                _approverDict = new Dictionary<string, int>();
        }

	    public void LoadPayPeriods()
	    {
            LoadingPayPeriods = GetPayPeriods();
            LoadingPayPeriods.ContinueWith(_ => BuildDictionaries());
	    }

        private async Task GetPayPeriods()
        {
            PayPeriods = await _api.GetPayPeriods();
        }
        
        public bool UserHasPayPeriods()
        {
            return PayPeriods != null && PayPeriods.Any();
        }

        public static IEnumerable<string> MostFrequentProjectCodes()
        {
            return MostFrequentEntries(_projectCodeDict, MaxFrequentlyUsed);
        }

        public static IEnumerable<string> MostFrequentTimesheetApprovers()
        {
            return MostFrequentEntries(_approverDict, MaxFrequentlyUsed);
        }

        public static void IncrementProjectCodeCount(string projectCode)
        {
            AddOrIncrementKeyToDictionary(_projectCodeDict, projectCode);
        }

        public static void IncrementApproverCount(DirectReport directReport)
        {
            if (directReport == null) return;

            AddOrIncrementKeyToDictionary(_approverDict, directReport.Email);
        }

	    private void BuildDictionaries()
	    {
            if (PayPeriods == null) return;

            var relevantPayPeriods = RelevantPayPeriods();

	        foreach (var timesheet in relevantPayPeriods.SelectMany(period => period.Timesheets))
	        {
	            IncrementProjectCodeDictionary(timesheet);
	            IncrementApproverCount(timesheet.TimesheetApprover);
	        }
	    }

	    private IEnumerable<PayPeriod> RelevantPayPeriods()
	    {
            return PayPeriods.OrderBy(period => period.EndDate).Take(MaxPeriodHistory);
	    }

	    private static void IncrementProjectCodeDictionary(Timesheet timesheet)
	    {
	        foreach (var entry in timesheet.TimeEntries)
	        {
	            IncrementProjectCodeCount(entry.ProjectCode);
	        }
	    }

		private static IEnumerable<string> MostFrequentEntries( Dictionary<string, int> dict, int number)
	    {
	        var sortedList = from entry in dict orderby entry.Value descending select entry.Key;

            return sortedList.Take(number);
	    }

        private static void AddOrIncrementKeyToDictionary(IDictionary<string, int> dictionary, string key)
        {
            if (key == null) return;

            if (dictionary.ContainsKey(key))
                dictionary[key]++;
            else
                dictionary.Add(key, 1);
        }
	}
}

