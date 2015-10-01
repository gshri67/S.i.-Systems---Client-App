﻿using System;
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
		public static Dictionary<string, int> ProjectCodeDict;
        public static Dictionary<string, int> ApproverDict;

	    private const int MaxPeriodHistory = 6;
        private const int MaxFrequentlyUsed = 5;

        public ActiveTimesheetViewModel(IMatchGuideApi matchGuideApi)
	    {
	        _api = matchGuideApi;

			if (ProjectCodeDict == null)
				ProjectCodeDict = new Dictionary<string, int> ();
			if (ApproverDict == null)
                ApproverDict = new Dictionary<string, int>();

			PreloadDictionaries ();
		}

		//use the last few timesheets to populate most frequently used
		private async void PreloadDictionaries()
		{
			var payPeriods = await GetPayPeriods ();

		    if (payPeriods == null) return;
            
            payPeriods = payPeriods.OrderBy(period => period.EndDate);

            foreach (var timesheet in payPeriods.Take(MaxPeriodHistory).SelectMany(period => period.Timesheets))
		    {
		        foreach (var entry in timesheet.TimeEntries)
		        {
                    IncrementProjectCodeCount(entry.ProjectCode);
		        }
                IncrementApproverCount(timesheet.TimesheetApprover);
		    }
		}

	    private static void IncrementProjectCodeCount(string projectCode)
	    {
	        if (string.IsNullOrEmpty(projectCode))
	            return;
	        AddOrIncrementKeyToDictionary(ProjectCodeDict, projectCode);
	    }

        public static void IncrementApproverCount(DirectReport directReport)
        {
            if (directReport == null) return;
            
            AddOrIncrementKeyToDictionary(ApproverDict, directReport.Email);
        }

	    private static void AddOrIncrementKeyToDictionary(IDictionary<string, int> dictionary, string key)
	    {
	        if (key == null) return;

            if (dictionary.ContainsKey(key))
                dictionary[key]++;
            else
                dictionary.Add(key, 1);
	    }

        public Task<IEnumerable<PayPeriod>> GetPayPeriods()
        {
            return _api.GetPayPeriods();
        }

	    public Task<ConsultantDetails> GetConsultantDetails()
	    {
	        return _api.GetCurrentUserConsultantDetails();
	    }

		//Grabs the top "number" of most frequent entries
		public static IEnumerable<string> TopFrequentEntries( Dictionary<string, int> dict, int number)
	    {
	        var sortedList = from entry in dict orderby entry.Value descending select entry.Key;

            return sortedList.Take(number);
	    }

	    public static IEnumerable<string> MostFrequentTimesheetApprovers()
	    {
	        var sortedList = from entry in ApproverDict orderby entry.Value descending select entry.Key;
            
	        return sortedList.Distinct().Take(MaxFrequentlyUsed);
	    }
	}
}

