using Foundation;
using UIKit;
using ClientApp.Core;
using ClientApp.Core.Platform;

namespace ClientApp.iOS
{
    public class ErrorReporter : IErrorReporter
    {
        public ErrorReporter(IErrorSource errorSource)
        {
            errorSource.ErrorReported += (s, e) =>
            {
                if (e.Broadcast && !string.IsNullOrEmpty(e.Name))
                {
                    this.Broadcast(e.Name, e.Message);
                }
                if (!string.IsNullOrEmpty(e.Message))
                {
                    this.Display(e.Name, e.Message);
                }
            };
        }

        public void Broadcast(string name, string message)
        {
            NSNotificationCenter.DefaultCenter.PostNotificationName(name, new NSString(message ?? string.Empty));
        }

        public void Display(string name, string message)
        {
            UIApplication.SharedApplication.InvokeOnMainThread(() => {
                UIAlertView alert = new UIAlertView(message, null, null, "OK", null);
                alert.Show();
            });
        }
    }
}