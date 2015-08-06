using System;
using System.Threading.Tasks;

namespace Shared.Core.ViewModels
{
    public class ResetPasswordViewModel : ViewModelBase
    {
        private readonly IMatchGuideApi _api;

        public string EmailAddress { get; set; }

        public ResetPasswordViewModel(IMatchGuideApi api)
        {
            this._api = api;
        }

        public async Task<string> ResetPassword()
        {
            try
            {
                var resetResult = await this._api.ResetPassword(this.EmailAddress);
                return resetResult.Description;
            }
            catch(NullReferenceException e)
            {
                return "Could not communicate with the server, please try again";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}
