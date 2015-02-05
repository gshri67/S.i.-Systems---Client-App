using System;
using ClientApp.ViewModels;
using Foundation;
using Microsoft.Practices.Unity;
using SiSystems.ClientApp.SharedModels;
using UIKit;

namespace ClientApp.iOS
{
    public partial class LoginViewController : UIViewController
    {
        private readonly LoginViewModel _loginModel;
        private Eula _eula;
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
            NavigationController.SetNavigationBarHidden(true, true);
            base.ViewWillAppear(animated);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
        }

        public override void ViewWillDisappear(bool animated)
        {
            NavigationController.SetNavigationBarHidden(false, true);
            base.ViewWillDisappear(animated);
        }

        public override void ViewDidDisappear(bool animated)
        {
            loginActivityIndicator.StopAnimating();
            base.ViewDidDisappear(animated);
        }

        #endregion

        partial void login_TouchUpInside(UIButton sender)
        {
            var userName = username.Text;
            var result = _loginModel.IsValidUserName(userName);
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

            var loginTask = _loginModel.LoginAsync(userName, password.Text);
            loginTask.ContinueWith(task =>
            {
                if (task.Result.IsValid)
                {
                    CheckEulaService(userName);
                }
                else
                {
                    DisplayInvalidCredentials(task.Result.Message);
                }
            });
        }

        partial void resetPassword_TouchUpInside(UIButton sender)
        {
            //TODO: use proper reset password url
            UIApplication.SharedApplication.OpenUrl(new NSUrl("http://www.google.ca"));
        }

        private async void CheckEulaService(string userName)
        {
            _eula = await _loginModel.GetCurrentEulaAsync();

            var storageString = NSUserDefaults.StandardUserDefaults.StringForKey("eulaVersions");
            var hasReadEula = _loginModel.UserHasReadLatestEula(userName, _eula.Version, storageString);

            NSOperationQueue.MainQueue.AddOperation(
                () => { PerformSegue(hasReadEula ? "alumniPushSegue" : "eulaPushSegue", this); });

        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);

            if (segue.Identifier == "eulaPushSegue")
            {
                var view = (EulaViewController)segue.DestinationViewController;
                view.CurrentEula = _eula;
                view.UserName = username.Text;
                view.EulaModel = new EulaViewModel(_loginModel.EulaVersions);
            }
        }

        private void DisplayInvalidCredentials(string message)
        {
            NSOperationQueue.MainQueue.AddOperation(
                () =>
                {
                    loginActivityIndicator.StopAnimating();
                    var view = new UIAlertView("Oops", message, null, "Ok");
                    view.Show();
                });
        }
    }
}