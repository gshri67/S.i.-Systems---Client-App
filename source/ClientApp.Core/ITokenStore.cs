using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.Core
{
    public interface ITokenStore
    {
        void SaveToken(OAuthToken token);

        OAuthToken GetDeviceToken();

        void Remove();
    }
}
