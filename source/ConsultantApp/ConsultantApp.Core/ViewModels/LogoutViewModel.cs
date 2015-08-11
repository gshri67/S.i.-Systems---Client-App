using System;
using Shared.Core;
using System.Threading.Tasks;

namespace ConsultantApp.Core.ViewModels
{
	public class LogoutViewModel
	{
		private readonly IMatchGuideApi _api;

		public LogoutViewModel(IMatchGuideApi api)
		{
			this._api = api;
		}

		public Task Logout()
		{
			return _api.Logout();
		}
	}
}

