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

        public async Task<ResetPasswordResult> ResetPassword()
        {
            try
            {
                var result = await this._api.ResetPassword(this.EmailAddress);
                return result;
            }
            catch
            {
                return new ResetPasswordResult { ResponseCode = -1, Description = "Something went wrong. Your password was not reset. Please contact your AE." };
            }
        }
    }
}
