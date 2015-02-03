
using System;
using System.Drawing;

using Foundation;
using UIKit;

namespace ClientApp.iOS
{
    public partial class LoginViewController : UIViewController
    {
        static bool UserInterfaceIdiomIsPhone
        {
            get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
        }

        public LoginViewController(IntPtr handle)
            : base(handle)
        {
        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        #region View lifecycle

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Perform any additional setup after loading the view, typically from a nib.
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
        }

        #endregion

        partial void login_TouchUpInside(UIButton sender)
        {
            //loginActivityIndicator.StopAnimating();
            if (string.IsNullOrEmpty(username.Text))
            {
                var view = new UIAlertView("Oops", "Please enter a username.", null, "Ok");
                //view.Dismissed += (sender, e) => username.BecomeFirstResponder();
                view.Show();
                return;
            }

            if (string.IsNullOrEmpty(password.Text))
            {
                var view = new UIAlertView("Oops", "Please enter a password.", null, "Ok");
                //view.Dismissed += (sender, e) => password.BecomeFirstResponder();
                view.Show();
                return;
            }

        }

        partial void resetPassword_TouchUpInside(UIButton sender)
        {
            //UIApplication.SharedApplication.OpenUrl(new NSUrl("www.google.com"));
        }
    }
}