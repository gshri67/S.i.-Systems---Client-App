using System;
using AccountExecutiveApp.Core.ViewModel;
using AccountExecutiveApp.iOS.Startup;
using CoreGraphics;
using Foundation;
using Microsoft.Practices.Unity;
using Shared.Core;
using Shared.Core.Platform;
using UIKit;

namespace AccountExecutiveApp.iOS
{
    public partial class LoginViewController : UIViewController
    {
        private readonly LoginViewModel _loginModel;
        private readonly ITokenStore _tokenStore;
        private CGRect _defaultFrame;
        static bool UserInterfaceIdiomIsPhone
        {
            get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
        }

        public LoginViewController(IntPtr handle)
            : base(handle)
        {
			HidesBottomBarWhenPushed = true;

            _loginModel = DependencyResolver.Current.Resolve<LoginViewModel>();
            _tokenStore = DependencyResolver.Current.Resolve<ITokenStore>();
        }

		public override void ViewWillLayoutSubviews ()
		{
			base.ViewWillLayoutSubviews ();

		}

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        //dismiss keyboard when tapping outside of text fields
        //public override void TouchesBegan(NSSet touches, UIEvent evt)
        //{
        //    base.TouchesBegan(touches, evt);

        //    username.ResignFirstResponder();
        //    password.ResignFirstResponder();
        //}

        //#region View lifecycle

        //public override void ViewDidLoad()
        //{
        //    base.ViewDidLoad();

        //    username.ShouldReturn += (textField) =>
        //    {
        //        password.BecomeFirstResponder();
        //        return true;
        //    };

        //    password.ShouldReturn += (textField) =>
        //    {
        //        textField.ResignFirstResponder();
        //        login_TouchUpInside(null);
        //        return true;
        //    };

        //    //Force Sign In button to always be visible even when they keyboard tries to cover it
        //    _defaultFrame = LoginView.Frame;
        //    NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillShowNotification, ShowKeyboard);
        //    NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, HideKeyboard);

        //    var previousUsername = _tokenStore.GetUserName();
        //    username.Text = previousUsername;
        //}

        //private void ShowKeyboard(NSNotification notification)
        //{
        //    var spaceFromBottom = UIScreen.MainScreen.Bounds.Height - login.Frame.Y - login.Frame.Height;
        //    var keyHeight = UIKeyboard.FrameEndFromNotification(notification).Height;
        //    if (spaceFromBottom < keyHeight)
        //    {
        //        InvokeOnMainThread(() =>
        //        {
        //            UIView.Animate(1, () =>
        //            {
        //                LoginView.Frame = new CGRect(_defaultFrame.X,
        //                    _defaultFrame.Y + (spaceFromBottom - keyHeight),
        //                    _defaultFrame.Width, _defaultFrame.Height);
        //            });
        //        });
        //    }
        //}

        //private void HideKeyboard(NSNotification notification)
        //{
        //    if (LoginView.Frame.Y != _defaultFrame.Y)
        //    {
        //        UIView.Animate(0.2, () =>
        //        {
        //            LoginView.Frame = new CGRect(_defaultFrame.X, _defaultFrame.Y,
        //                _defaultFrame.Width, _defaultFrame.Height);
        //        });
        //    }
        //}

        //public override void ViewWillAppear(bool animated)
        //{
        //    base.ViewWillAppear(animated);

        //    NavigationController.NavigationBar.Hidden = true;
        //}

        //public override void ViewDidAppear(bool animated)
        //{
        //    base.ViewDidAppear(animated);
        //}

        //public override void ViewWillDisappear(bool animated)
        //{
        //    base.ViewWillDisappear(animated);
        //}

        //public override void ViewDidDisappear(bool animated)
        //{
        //    if (loginActivityIndicator != null) loginActivityIndicator.StopAnimating();
        //    base.ViewDidDisappear(animated);
        //}

        //#endregion

        //async partial void login_TouchUpInside(UIButton sender)
        //{
        //    var userName = username.Text;
        //    var result = _loginModel.IsValidUserName(userName);
        //    if (!result.IsValid)
        //    {
        //        var view = new UIAlertView("Error", result.Message, null, "Ok");
        //        view.Show();
        //        return;
        //    }

        //    result = _loginModel.IsValidPassword(password.Text);
        //    if (!result.IsValid)
        //    {
        //        var view = new UIAlertView("Error", result.Message, null, "Ok");
        //        view.Show();
        //        return;
        //    }

        //    DisableControls();

        //    var loginTask = await _loginModel.LoginAsync(userName, password.Text);

        //    if (loginTask.IsValid)
        //    {
        //        CurrentUser.Email = _loginModel.UserName;
        //        RunMainStoryboard();
        //    }
        //    else
        //    {
        //        DisplayInvalidCredentials(loginTask.Message);
        //    };
        //}

        //private static void RunMainStoryboard()
        //{
        //    UIApplication.SharedApplication.Windows[0].RootViewController = UIStoryboard.FromName("MainStoryboard", NSBundle.MainBundle).InstantiateInitialViewController();
        //}

        //public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        //{
        //    base.PrepareForSegue(segue, sender);

        //    if (segue.Identifier == "forgotPasswordSegue")
        //    {
        //        UINavigationController navigationController = (UINavigationController)segue.DestinationViewController;
        //        ResetPasswordViewController viewController = (ResetPasswordViewController)navigationController.TopViewController;
        //        viewController.Initialize("Reset Password", username.Text);
        //    }
        //}

        //private void DisplayInvalidCredentials(string message)
        //{
        //    InvokeOnMainThread(delegate
        //    {
        //        loginActivityIndicator.StopAnimating();
        //        var view = new UIAlertView(message, null, null, "Ok");
        //        view.Show();
        //        EnableControls();
        //    });
        //}

        //private void DisableControls()
        //{
        //    username.Enabled = false;
        //    username.BackgroundColor = StyleGuideConstants.LightGrayUiColor;
        //    password.Enabled = false;
        //    password.BackgroundColor = StyleGuideConstants.LightGrayUiColor;
        //    login.Enabled = false;
        //    ForgotPasswordButton.Enabled = false;
        //    loginActivityIndicator.StartAnimating();
        //}

        //private void EnableControls()
        //{
        //    username.Enabled = true;
        //    username.BackgroundColor = UIColor.White;
        //    password.Enabled = true;
        //    password.BackgroundColor = UIColor.White;
        //    login.Enabled = true;
        //    ForgotPasswordButton.Enabled = true;
        //    loginActivityIndicator.StopAnimating();
        //}
    }
}