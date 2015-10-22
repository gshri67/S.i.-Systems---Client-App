using System.Threading.Tasks;
using Shared.Core;

namespace AccountExecutiveApp.Core.ViewModel
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

