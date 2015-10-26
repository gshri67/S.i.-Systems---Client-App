using System;

using SiSystems.SharedModels;

namespace AccountExecutiveApp.Core.ViewModel
{
	public class DashboardViewModel
	{
		private readonly IMatchGuideApi _api;

		public DashboardViewModel(IMatchGuideApi api)
		{
			this._api = api;
		}

		public async Task<DashboardInfo> getDashboardInfo
		{
			return await this._api.getDashboardInfo();
		}
	}
}

