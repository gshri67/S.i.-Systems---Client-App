using System;
using System.Threading.Tasks;
using Shared.Core;

namespace ClientApp.Core.ViewModels
{
    public class ResetPasswordViewModel : ViewModelBase
    {
        private readonly IMatchGuideApi _api;
        private const string ResetResultNullErrorMessage = "Unable to contact Reset Password Service, please try again.";

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
                return ResetResultNullErrorMessage;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}
