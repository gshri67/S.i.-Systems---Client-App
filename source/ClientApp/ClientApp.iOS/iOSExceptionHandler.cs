using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using ClientApp.Core;
using System.Threading.Tasks;
using System.Threading;

namespace ClientApp.iOS
{
    public class iOSExceptionHandler: IPlatformExceptionHandler
    {
        private readonly ITokenStore _tokenStore;

        public iOSExceptionHandler(ITokenStore tokenStore)
        {
            this._tokenStore = tokenStore;
        }

        public async Task<TResult> HandleAsync<TResult>(Func<Task<TResult>> action)
        {
            try
            {
                return await action();
            }
            catch (AuthorizationException ex)
            {
                _tokenStore.DeleteDeviceToken();
                NSNotificationCenter.DefaultCenter.PostNotificationName("TokenExpired", new NSObject());
            }
            catch (Exception ex)
            {
                throw;
            }
            return default(TResult);
        }
    }
}