using System.Threading.Tasks;
using SiSystems.ClientApp.SharedModels;

namespace Shared.Core.ViewModels
{
    public class MessageViewModel : ViewModelBase
    {
        private readonly IMatchGuideApi _api;
        public ConsultantMessage Message { get; set; }

        public MessageViewModel(IMatchGuideApi api)
        {
            this._api = api;
        }

        public async Task SendMessage()
        {
            await this._api.SendMessage(Message);
        }
    }
}
