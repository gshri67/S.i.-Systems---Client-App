using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Core;

namespace ConsultantApp.Core.ViewModels
{
    public class ResetPasswordViewModel
    {
        private readonly IMatchGuideApi _api;

        public string EmailAddress { get; set; }

        public ResetPasswordViewModel(IMatchGuideApi api)
        {
            this._api = api;
        }

        public async Task<string> ResetConsultantPassword()
        {
            try
            {
                var resetResult = await this._api.ResetPassword(this.EmailAddress, "ConsultantApplication");
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
