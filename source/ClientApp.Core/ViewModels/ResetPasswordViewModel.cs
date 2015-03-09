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

        public async Task<bool> ResetPassword()
        {
            try
            {
                await this._api.ResetPassword(this.EmailAddress);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
