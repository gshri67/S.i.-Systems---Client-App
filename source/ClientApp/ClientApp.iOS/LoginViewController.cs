
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

            var userError = _loginModel.GetUserNameError();
            if (!string.IsNullOrEmpty(userError))
            {
                var view = new UIAlertView("Oops", userError, null, "Ok");
                view.Show();
                return;
            }

            var passError = _loginModel.GetPasswordError();
            if (!string.IsNullOrEmpty(passError))
            {
                var view = new UIAlertView("Oops", passError, null, "Ok");
                view.Show();
                return;
            }

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
            //TODO pop up some kind of loading animation
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
            //TODO Kill loading animation, display error dialog
        }
    }
}