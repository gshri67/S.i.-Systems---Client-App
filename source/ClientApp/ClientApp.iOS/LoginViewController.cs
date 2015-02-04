using System;
using ClientApp.ViewModels;
using Microsoft.Practices.Unity;
using UIKit;

namespace ClientApp.iOS
{
    public partial class LoginViewController : UIViewController
    {
        private readonly LoginViewModel _loginModel;
        static bool UserInterfaceIdiomIsPhone
        {
            get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
        }

        public LoginViewController(IntPtr handle)
            : base(handle)
        {
            _loginModel = DependencyResolver.Current.Resolve<LoginViewModel>();
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
            var result = _loginModel.IsValidUserName(username.Text);
            if (!result.IsValid)
            {
                var view = new UIAlertView("Oops", result.Message, null, "Ok");
                view.Show();
                return;
            }

            result = _loginModel.IsValidPassword(password.Text);
            if (!result.IsValid)
            {
                var view = new UIAlertView("Oops", result.Message, null, "Ok");
                view.Show();
                return;
            }

            loginActivityIndicator.StartAnimating();

            var loginTask = _loginModel.LoginAsync(username.Text, password.Text);
            loginTask.ContinueWith(task => {
                                               if (task.Result.IsValid)
                                               {
                                                   CheckEulaService();
                                               }
                                               else
                                               {
                                                   DisplayInvalidCredentials(task.Result.Message);
                                               }
            });
        }

        partial void resetPassword_TouchUpInside(UIButton sender)
        {
            //UIApplication.SharedApplication.OpenUrl(new NSUrl("www.google.com"));
        }

        private async void CheckEulaService()
        {
            var eula = await _loginModel.GetCurrentEulaAsync();

            //TODO check local storage for if this user has already read this version
            var hasReadEula = false;

            if (hasReadEula)
            {
                //GOTO main page
            }
            else
            {
                //GOTO EULA screen
            }
        }

        private void DisplayInvalidCredentials(string message)
        {
            loginActivityIndicator.StopAnimating();
            var view = new UIAlertView("Oops", message, null, "Ok");
            view.Show();
            return;
        }
    }
}