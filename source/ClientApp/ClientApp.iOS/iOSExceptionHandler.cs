using System;
using System.Threading.Tasks;
using Foundation;
using UIKit;
using ClientApp.Core;
using SiSystems.ClientApp.SharedModels;

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
            catch (AccessLevelException ex)
            {
                UIAlertView forbiddenAlert = new UIAlertView(ex.Message, null, null, "OK", null);
                forbiddenAlert.Show();
            }
            catch (Exception)
            {
                // swallow network related exceptions for now
            }
            return default(TResult);
        }
    }
}