using SiSystems.ClientApp.SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.Core.ViewModels
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
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}
