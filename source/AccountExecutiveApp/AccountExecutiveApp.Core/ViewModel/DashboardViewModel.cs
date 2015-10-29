using System;
using Shared.Core;
using SiSystems.SharedModels;
using System.Threading.Tasks;

namespace AccountExecutiveApp.Core.ViewModel
{
	public class DashboardViewModel
	{
		private readonly IMatchGuideApi _api;

	    public Task DashboardLoadingTask;
	    private DashboardSummary _dashboardStats;

		public DashboardViewModel(IMatchGuideApi api)
		{
			_api = api;

		    _dashboardStats = new DashboardSummary();
		}

	    public void LoadDashboardInformation()
	    {
	        DashboardLoadingTask = GetDashboadStats();
	        DashboardLoadingTask.ContinueWith(_ => DashboardInfoRetrieved());
	    }

	    private async Task GetDashboadStats()
	    {
	        _dashboardStats = await _api.getDashboardInfo();
	    }

	    private void DashboardInfoRetrieved()
	    {
	        //todo: do whatever you need to do to process that dashboard info
	    }

	    public string FullySourcedEndingContracts()
	    {
	        return _dashboardStats.FullySourcedContracts.Ending.ToString();
	    }

	    public string FullySourcedStartingContracts()
	    {
            return _dashboardStats.FullySourcedContracts.Starting.ToString();
	    }

	    public string FullySourcedCurrentContracts()
	    {
            return _dashboardStats.FullySourcedContracts.Current.ToString();
	    }
        
        public string FlowThruEndingContracts()
        {
            return _dashboardStats.FlowThruContracts.Ending.ToString();
        }

        public string FlowThruStartingContracts()
        {
            return _dashboardStats.FlowThruContracts.Starting.ToString();
        }

        public string FlowThruCurrentContracts()
        {
            return _dashboardStats.FlowThruContracts.Current.ToString();
        }

	    public string AllJobs()
	    {
	        return _dashboardStats.Jobs.All.ToString();
	    }

	    public string ProposedJobs()
	    {
	        return _dashboardStats.Jobs.Proposed.ToString();
	    }

	    public string JobsWithCallouts()
	    {
	        return _dashboardStats.Jobs.Callouts.ToString();
	    }
	}
}

