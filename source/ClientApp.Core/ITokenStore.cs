using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.Core
{
    public interface ITokenStore
    {
        OAuthToken Store(OAuthToken token);

        OAuthToken Fetch();

        void Remove();
    }
}
