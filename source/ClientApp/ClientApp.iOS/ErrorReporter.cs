using Foundation;
using UIKit;
using ClientApp.Core;

namespace ClientApp.iOS
{
    public class ErrorReporter : IErrorReporter
    {
        public void Broadcast(string name, string message)
        {
            NSNotificationCenter.DefaultCenter.PostNotificationName(name, new NSString(message));
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