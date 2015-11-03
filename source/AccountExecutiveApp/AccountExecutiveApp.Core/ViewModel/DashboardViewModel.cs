using System;
using System.Threading;
using Shared.Core;
using SiSystems.SharedModels;
using System.Threading.Tasks;

namespace AccountExecutiveApp.Core.ViewModel
{
	public class DashboardViewModel
	{
		private readonly IMatchGuideApi _api;

	    private DashboardSummary _dashboardStats;

	    public DashboardViewModel(IMatchGuideApi api)
		{
			_api = api;

		    _dashboardStats = new DashboardSummary();
		}

	    public void LoadDashboardInformation(Action dashboardLoadedCallback)
	    {
            var dashboardLoadingTask = GetDashboadStats();

            dashboardLoadingTask.ContinueWith(_ => DashboardInfoRetrieved());

            if (dashboardLoadedCallback != null)
                dashboardLoadingTask.ContinueWith(_ => dashboardLoadedCallback());
	    }

	    private async Task GetDashboadStats()
	    {
	        _dashboardStats = await _api.getDashboardInfo();
	    }

	    private void DashboardInfoRetrieved()
	    {
            if(_dashboardStats == null)
                _dashboardStats = new DashboardSummary();
	    }

        private static bool LessThanMin(int number)
        {
            const int min = 0;
            return number < min;
        }

	    public string FullySourcedEndingContracts()
	    {
            return (_dashboardStats == null || _dashboardStats.FullySourcedContracts == null || LessThanMin(_dashboardStats.FullySourcedContracts.Ending)) ? 
	            string.Empty : 
                _dashboardStats.FullySourcedContracts.Ending.ToString();
        }

	    public string FullySourcedStartingContracts()
	    {
            return (_dashboardStats == null || _dashboardStats.FullySourcedContracts == null || LessThanMin(_dashboardStats.FullySourcedContracts.Starting)) ?
                string.Empty :
                _dashboardStats.FullySourcedContracts.Starting.ToString();
	    }

	    public string FullySourcedCurrentContracts()
	    {
            return (_dashboardStats == null || _dashboardStats.FullySourcedContracts == null || LessThanMin(_dashboardStats.FullySourcedContracts.Current)) ?
                string.Empty :
                _dashboardStats.FullySourcedContracts.Current.ToString();
	    }
        
        public string FlowThruEndingContracts()
        {
            return (_dashboardStats == null || _dashboardStats.FlowThruContracts == null || LessThanMin(_dashboardStats.FlowThruContracts.Ending)) ?
                string.Empty : 
                _dashboardStats.FlowThruContracts.Ending.ToString();
        }

        public string FlowThruStartingContracts()
        {
            return (_dashboardStats == null || _dashboardStats.FlowThruContracts == null || LessThanMin(_dashboardStats.FlowThruContracts.Starting)) ?
                string.Empty : 
                _dashboardStats.FlowThruContracts.Starting.ToString();
        }

	    public string FlowThruCurrentContracts()
        {
            return (_dashboardStats == null || _dashboardStats.FlowThruContracts == null || LessThanMin(_dashboardStats.FlowThruContracts.Current)) ? 
                string.Empty :
                _dashboardStats.FlowThruContracts.Current.ToString();
        }

	    public string AllJobs()
	    {
            return (_dashboardStats == null || _dashboardStats.Jobs == null || LessThanMin(_dashboardStats.Jobs.All)) ?
               string.Empty : 
               _dashboardStats.Jobs.All.ToString();
	    }

	    public string ProposedJobs()
	    {
            return (_dashboardStats == null || _dashboardStats.Jobs == null || LessThanMin(_dashboardStats.Jobs.Proposed)) ?
               string.Empty :
               _dashboardStats.Jobs.Proposed.ToString();
	    }

	    public string JobsWithCallouts()
	    {
            return (_dashboardStats == null || _dashboardStats.Jobs == null || LessThanMin(_dashboardStats.Jobs.Callouts)) ?
               string.Empty :
               _dashboardStats.Jobs.Callouts.ToString();
	    }
	}
}

