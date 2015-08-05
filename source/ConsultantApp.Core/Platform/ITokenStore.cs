using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsultantApp.Core.ViewModels;

namespace ConsultantApp.Core.Platform
{
    public interface ITokenStore
    {
        OAuthToken SaveToken(OAuthToken token);

        OAuthToken GetDeviceToken();

        void SaveUserName(string username);

        string GetUserName();

        void DeleteDeviceToken();
    }
}
