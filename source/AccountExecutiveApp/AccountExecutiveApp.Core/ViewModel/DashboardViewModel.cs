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

	    private const int MinimumValueToDisplay = 0;
	    private DashboardSummary _dashboardStats;
		public string UserName { get{ return _dashboardStats.UserName; } }

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

	    private Task DashboardIsLoading { get; set; }

	    public DashboardViewModel(IMatchGuideApi api)
		{
			_api = api;

            _dashboardStats = new DashboardSummary();
		}

	    public Task LoadDashboardInformation()
	    {
            DashboardIsLoading = GetDashboadStats();

            DashboardIsLoading.ContinueWith(_ => DashboardInfoRetrieved());
	        
            return DashboardIsLoading;
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

	    private string MaxOfValueOrZeroToString(int value)
	    {
	        return Math.Max(value, MinimumValueToDisplay).ToString();
	    }

	    public string EndingFullySourcedContracts
	    {
            get { return MaxOfValueOrZeroToString(FullySourced.Ending); }
        }

        public string StartingFullySourcedContracts
	    {
            get { return MaxOfValueOrZeroToString(FullySourced.Starting); }
	    }

	    public string CurrentFullySourcedContracts
	    {
            get { return MaxOfValueOrZeroToString(FullySourced.Current); }
	    }
        
        public string EndingFloThruContracts
        {
            get { return MaxOfValueOrZeroToString(FloThru.Ending); }
        }

	    public string StartingFloThruContracts
	    {
	        get { return MaxOfValueOrZeroToString(FloThru.Starting); }
        }

	    public string CurrentFloThruContracts
        {
            get { return MaxOfValueOrZeroToString(FloThru.Current); }
        }

	    public string AllJobs
	    {
            get { return MaxOfValueOrZeroToString(Jobs.All); }
	    }

	    public string ProposedJobs
	    {
            get { return MaxOfValueOrZeroToString(Jobs.Proposed); }
	    }

	    public string JobsWithCallouts
	    {
            get { return MaxOfValueOrZeroToString(Jobs.Callouts); }
	    }
	}
}

