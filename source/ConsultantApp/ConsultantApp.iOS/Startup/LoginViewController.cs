using System;
using Foundation;
using UIKit;

namespace ConsultantApp.iOS.Startup
{
	partial class LoginViewController : UIViewController
	{
		
		//private readonly LoginViewModel _loginModel;
	/*  private readonly ITokenStore _tokenStore;
		private CGRect _defaultFrame;
*/
		public LoginViewController(){
		}

		public LoginViewController (IntPtr handle) : base (handle)
		{
			HidesBottomBarWhenPushed = true;

            //_loginModel = DependencyResolver.Current.Resolve<LoginViewModel>();
			//_tokenStore = DependencyResolver.Current.Resolve<ITokenStore>();
		}

		public override void ViewWillLayoutSubviews ()
		{
			base.ViewWillLayoutSubviews ();
		
			NavigationController.NavigationBar.Hidden = true;

		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			login.TouchUpInside += delegate {

                //var result = _loginModel.IsValidUserName(userName);
                //if (!result.IsValid)
                {
                    var view = new UIAlertView("Error", "Almost couldn't log in", null, "Ok");
                    view.Show();
                    // return;
                }

				NavigationController.NavigationBar.Hidden = false;
				NavigationController.PopViewController(true);



                /*
                    var userName = username.Text;
			        var result = _loginModel.IsValidUserName(userName);
			        if (!result.IsValid)
			        {
				        var view = new UIAlertView("Error", result.Message, null, "Ok");
				        view.Show();
				        return;
			        }

			        result = _loginModel.IsValidPassword(password.Text);
			        if (!result.IsValid)
			        {
				        var view = new UIAlertView("Error", result.Message, null, "Ok");
				        view.Show();
				        return;
			        }

			        DisableControls();

			        var loginTask = await _loginModel.LoginAsync(userName, password.Text);

			        if (loginTask.IsValid)
			        {
				        CurrentUser.Email = _loginModel.UserName;
				        CheckEulaService(userName);
			        }
			        else
			        {
				        DisplayInvalidCredentials(loginTask.Message);
			        };
                 */



            };


			/*
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

			//Force Sign In button to always be visible even when they keyboard tries to cover it
			_defaultFrame = LoginView.Frame;
			NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillShowNotification, ShowKeyboard);
			NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, HideKeyboard);

			var previousUsername = _tokenStore.GetUserName();
			username.Text = previousUsername;*/
		}

		public override void PrepareForSegue (UIStoryboardSegue segue, NSObject sender)
		{
			base.PrepareForSegue (segue, sender);
		}





		static bool UserInterfaceIdiomIsPhone
		{
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}

		//dismiss keyboard when tapping outside of text fields
		public override void TouchesBegan(NSSet touches, UIEvent evt)
		{
			base.TouchesBegan(touches, evt);

			username.ResignFirstResponder();
			password.ResignFirstResponder();
		}
		/*
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

			//Force Sign In button to always be visible even when they keyboard tries to cover it
			_defaultFrame = LoginView.Frame;
			NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillShowNotification, ShowKeyboard);
			NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, HideKeyboard);

			var previousUsername = _tokenStore.GetUserName();
			username.Text = previousUsername;
		}

		private void ShowKeyboard(NSNotification notification)
		{
			var spaceFromBottom = UIScreen.MainScreen.Bounds.Height - login.Frame.Y - login.Frame.Height;
			var keyHeight = UIKeyboard.FrameEndFromNotification(notification).Height;
			if (spaceFromBottom < keyHeight)
			{
				InvokeOnMainThread(() =>
					{
						UIView.Animate(1, () =>
							{
								LoginView.Frame = new CGRect(_defaultFrame.X,
									_defaultFrame.Y + (spaceFromBottom - keyHeight),
									_defaultFrame.Width, _defaultFrame.Height);
							});                       
					});
			}
		}

		private void HideKeyboard(NSNotification notification)
		{
			if (LoginView.Frame.Y != _defaultFrame.Y)
			{
				UIView.Animate(0.2, () =>
					{
						LoginView.Frame = new CGRect(_defaultFrame.X, _defaultFrame.Y,
							_defaultFrame.Width, _defaultFrame.Height);
					});
			}
		}

		public override void ViewDidDisappear(bool animated)
		{
			if (loginActivityIndicator != null) loginActivityIndicator.StopAnimating();
			base.ViewDidDisappear(animated);
		}

		#endregion

		async partial void login_TouchUpInside(UIButton sender)
		{
			var userName = username.Text;
			var result = _loginModel.IsValidUserName(userName);
			if (!result.IsValid)
			{
				var view = new UIAlertView("Error", result.Message, null, "Ok");
				view.Show();
				return;
			}

			result = _loginModel.IsValidPassword(password.Text);
			if (!result.IsValid)
			{
				var view = new UIAlertView("Error", result.Message, null, "Ok");
				view.Show();
				return;
			}

			DisableControls();

			var loginTask = await _loginModel.LoginAsync(userName, password.Text);

			if (loginTask.IsValid)
			{
				CurrentUser.Email = _loginModel.UserName;
				CheckEulaService(userName);
			}
			else
			{
				DisplayInvalidCredentials(loginTask.Message);
			};
		}

		private async void CheckEulaService(string userName)
		{
			EulaHandler.Eula = await _loginModel.GetCurrentEulaAsync();
			if (EulaHandler.Eula == null)
			{
				// Something went wrong
				// Could mean an exception was handled in an unexpected way
				password.Text = "";
				EnableControls();
				return;
			}

			var storageString = NSUserDefaults.StandardUserDefaults.StringForKey("eulaVersions");
			var hasReadEula = EulaHandler.UserHasReadLatestEula(userName, EulaHandler.Eula.Version, storageString);

			if (hasReadEula)
			{
				UIApplication.SharedApplication.Windows[0].RootViewController = UIStoryboard.FromName("MainStoryboard", NSBundle.MainBundle).InstantiateInitialViewController();
			}
			else
			{
				PerformSegue("eulaSegue", this);
			}
		}

		public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
		{
			base.PrepareForSegue(segue, sender);

			if (segue.Identifier == "eulaSegue")
			{
				var view = (EulaViewController)segue.DestinationViewController;
				view.CurrentEula = EulaHandler.Eula;
				view.UserName = username.Text;
				view.EulaModel = new EulaViewModel(EulaHandler.EulaVersions);
			}
			else if (segue.Identifier == "forgotPasswordSegue")
			{
				UINavigationController navigationController = (UINavigationController)segue.DestinationViewController;
				ResetPasswordViewController viewController = (ResetPasswordViewController)navigationController.TopViewController;
				viewController.Initialize("Reset Password", username.Text);
			}
			else if (segue.Identifier == "signUpSegue")
			{
				UINavigationController navigationController = (UINavigationController)segue.DestinationViewController;
				ResetPasswordViewController viewController = (ResetPasswordViewController)navigationController.TopViewController;
				viewController.Initialize("Sign Up", username.Text);
			}
		}

		private void DisplayInvalidCredentials(string message)
		{
			InvokeOnMainThread(delegate
				{
					loginActivityIndicator.StopAnimating();
					var view = new UIAlertView(message, null, null, "Ok");
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
			ForgotPasswordButton.Enabled = false;
			SignUpButton.Enabled = false;
			loginActivityIndicator.StartAnimating();
		}

		private void EnableControls()
		{
			username.Enabled = true;
			username.BackgroundColor = UIColor.White;
			password.Enabled = true;
			password.BackgroundColor = UIColor.White;
			login.Enabled = true;
			ForgotPasswordButton.Enabled = true;
			SignUpButton.Enabled = true;
			loginActivityIndicator.StopAnimating();
		}
	}*/

	}
}
