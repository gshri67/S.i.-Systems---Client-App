
using System;
using System.Drawing;
using System.Threading.Tasks;
using ClientApp.ViewModels;
using Foundation;
using Microsoft.Practices.Unity;
using SiSystems.ClientApp.SharedModels;
using UIKit;

namespace ClientApp.iOS
{
    public partial class LoginViewController : UIViewController
    {
        private Lazy<DiContainer> _diContainer;
        private LoginViewModel _loginModel;
        static bool UserInterfaceIdiomIsPhone
        {
            get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
        }

        public LoginViewController(IntPtr handle)
            : base(handle)
        {
            _diContainer = new Lazy<DiContainer>();
            //TODO this next line sucks, find better way of hosting container
            _loginModel = _diContainer.Value.Instance.Resolve<LoginViewModel>();
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
            _loginModel.UserName = username.Text;
            _loginModel.Password = password.Text;

            if (!_loginModel.IsValidUserName())
            {
                var view = new UIAlertView("Oops", _loginModel.UserNameError, null, "Ok");
                view.Show();
                return;
            }

            if (!_loginModel.IsValidPassword())
            {
                var view = new UIAlertView("Oops", _loginModel.PasswordError, null, "Ok");
                view.Show();
                return;
            }

            loginActivityIndicator.StartAnimating();

            var loginTask = _loginModel.LoginAsync();
            loginTask.ContinueWith(task => {
                                               if (task.Result)
                                               {
                                                   CheckEulaService();
                                               }
                                               else
                                               {
                                                   DisplayInvalidCredentials();
                                               }
            });
        }

        partial void resetPassword_TouchUpInside(UIButton sender)
        {
            //UIApplication.SharedApplication.OpenUrl(new NSUrl("www.google.com"));
        }

        private void CheckEulaService()
        {
            //TODO Check EULA service, display if needed, otherwise jump to main screen
        }

        private void DisplayInvalidCredentials()
        {
            loginActivityIndicator.StopAnimating();
            var view = new UIAlertView("Oops", "Invalid Authentication Credentials", null, "Ok");
            view.Show();
            return;
        }
    }
}