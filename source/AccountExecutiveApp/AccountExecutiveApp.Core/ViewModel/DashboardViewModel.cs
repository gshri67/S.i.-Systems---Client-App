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

	    private const int MinimumToDisplay = 0;
	    private DashboardSummary _dashboardStats;

        private ContractSummarySet FullySourced 
        { 
            get
            {
                if(_dashboardStats == null)
                    return new ContractSummarySet();
                return _dashboardStats.FullySourcedContracts ?? new ContractSummarySet();
            } 
        }
        
        private ContractSummarySet FloThru
        {
            get
            {
                if (_dashboardStats == null)
                    return new ContractSummarySet();
                return _dashboardStats.FlowThruContracts ?? new ContractSummarySet();
            }
        }
        
        private JobsSummarySet Jobs
        {
            get
            {
                if (_dashboardStats == null)
                    return new JobsSummarySet();
                return _dashboardStats.Jobs ?? new JobsSummarySet();
            }
        }

	    public Task DashboardIsLoading { get; private set; }

	    public DashboardViewModel(IMatchGuideApi api)
		{
			_api = api;

            _dashboardStats = new DashboardSummary();
		}

	    public void LoadDashboardInformation()
	    {
            DashboardIsLoading = GetDashboadStats();

            DashboardIsLoading.ContinueWith(_ => DashboardInfoRetrieved());
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

	    private string ValueOrMinimumToString(int value)
	    {
	        return Math.Max(value, MinimumToDisplay).ToString();
	    }

	    public string FullySourcedEndingContracts()
	    {
	        return ValueOrMinimumToString(FullySourced.Ending);
        }

	    public string FullySourcedStartingContracts()
	    {
            return ValueOrMinimumToString(FullySourced.Starting);
	    }

	    public string FullySourcedCurrentContracts()
	    {
            return ValueOrMinimumToString(FullySourced.Current);
	    }
        
        public string FloThruEndingContracts()
        {
            return ValueOrMinimumToString(FloThru.Ending);
        }

        public string FloThruStartingContracts()
        {
            return ValueOrMinimumToString(FloThru.Starting);
        }

	    public string FlowThruCurrentContracts()
        {
            return ValueOrMinimumToString(FloThru.Current);
        }

	    public string AllJobs()
	    {
	        return ValueOrMinimumToString(Jobs.All);
	    }

	    public string ProposedJobs()
	    {
	        return ValueOrMinimumToString(Jobs.Proposed);
	    }

	    public string JobsWithCallouts()
	    {
	        return ValueOrMinimumToString(Jobs.Callouts);
	    }
	}
}

