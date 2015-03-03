using System;
using System.Net;
using ClientApp.iOS.Startup;
using ClientApp.ViewModels;
using Foundation;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Security;
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

        //dismiss keyboard when tapping outside of text fields
        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);

            username.ResignFirstResponder();
            password.ResignFirstResponder();
        }

        #region View lifecycle

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            username.ShouldReturn += (textField) =>
                                     {
                                         password.BecomeFirstResponder();
                                         return true;
                                     };

            password.ShouldReturn += (textField) =>
                                     {
                                         textField.ResignFirstResponder();
                                         login_TouchUpInside(null);
                                         return true;
                                     };

            var token = TokenStore.GetDeviceToken();
            if (token == null) return;

            username.Text = token.Username;
            CurrentUser.Email = token.Username;
            password.Text = "aaaaaaaa";
            DisableControls();

            _loginModel.SetAuthToken(token);
            CheckEulaService(token.Username);
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

            DisableControls();

            var loginTask = _loginModel.LoginAsync(userName, password.Text);
            loginTask.ContinueWith(task =>
            {
                if (task.Result.IsValid)
                {
                    TokenStore.SaveToken(_loginModel.GetAuthToken());
                    CurrentUser.Email = _loginModel.UserName;
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
            try
            {
                _eula = await _loginModel.GetCurrentEulaAsync();
            }
            catch (WebException)
            {
                //Authentication failed
                password.Text = "";
                EnableControls();
                return;
            }

            var storageString = NSUserDefaults.StandardUserDefaults.StringForKey("eulaVersions");
            var hasReadEula = _loginModel.UserHasReadLatestEula(userName, _eula.Version, storageString);


            InvokeOnMainThread(delegate
            {
                PerformSegue(hasReadEula ? "alumniSegue" : "eulaSegue", this);
            });

        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);

            if (segue.Identifier == "eulaSegue")
            {
                var view = (EulaViewController)segue.DestinationViewController;
                view.CurrentEula = _eula;
                view.UserName = username.Text;
                view.EulaModel = new EulaViewModel(_loginModel.EulaVersions);
            }
        }

        private void DisplayInvalidCredentials(string message)
        {
            InvokeOnMainThread(delegate
                               {
                                   loginActivityIndicator.StopAnimating();
                                   var view = new UIAlertView("Oops", message, null, "Ok");
                                   view.Show();
                                   EnableControls();
                               });
        }

        private void DisableControls()
        {
            username.Enabled = false;
            username.BackgroundColor = StyleGuideConstants.LightGrayUiColor;
            password.Enabled = false;
            password.BackgroundColor = StyleGuideConstants.LightGrayUiColor;
            login.Enabled = false;
            loginActivityIndicator.StartAnimating();
        }

        private void EnableControls()
        {
            username.Enabled = true;
            username.BackgroundColor = UIColor.White;
            password.Enabled = true;
            password.BackgroundColor = UIColor.White;
            login.Enabled = true;
            loginActivityIndicator.StopAnimating();
        }
    }
}