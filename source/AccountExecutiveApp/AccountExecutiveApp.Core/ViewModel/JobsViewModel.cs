using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Core;
using SiSystems.SharedModels;

namespace AccountExecutiveApp.Core.ViewModel
{
    public class JobsViewModel
	{
		private readonly IMatchGuideApi _api;

		public JobsViewModel(IMatchGuideApi api)
		{
			this._api = api;
		}

        public async Task<IEnumerable<Job>> getJobs()
		{
			return await this._api.getJobs();
		}
    }
}
