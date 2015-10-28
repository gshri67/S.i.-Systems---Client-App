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
	    private DashboardInfo _dashboardStats;

		public DashboardViewModel(IMatchGuideApi api)
		{
			_api = api;

		    _dashboardStats = new DashboardInfo();
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
	        return _dashboardStats.FS_endingContracts.ToString();
	    }

	    public string FullySourcedStartingContracts()
	    {
	        return _dashboardStats.FS_startingContracts.ToString();
	    }

	    public string FullySourcedCurrentContracts()
	    {
	        return _dashboardStats.FS_curContracts.ToString();
	    }
        
        public string FlowThruEndingContracts()
        {
            return _dashboardStats.FT_endingContracts.ToString();
        }

        public string FlowThruStartingContracts()
        {
            return _dashboardStats.FT_startingContracts.ToString();
        }

        public string FlowThruCurrentContracts()
        {
            return _dashboardStats.FT_curContracts.ToString();
        }

	    public string AllJobs()
	    {
	        return _dashboardStats.curJobs.ToString();
	    }

	    public string ProposedJobs()
	    {
	        return _dashboardStats.proposedJobs.ToString();
	    }

	    public string JobsWithCallouts()
	    {
	        return _dashboardStats.calloutJobs.ToString();
	    }
	}
}

