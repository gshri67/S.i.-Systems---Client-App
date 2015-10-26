using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace AccountExecutiveApp.iOS
{
	partial class DashboardViewController : UIViewController
	{
		private readonly NSObject _tokenExpiredObserver;

		public DashboardViewController (IntPtr handle) : base (handle)
		{
			this._tokenExpiredObserver = NSNotificationCenter.DefaultCenter.AddObserver(new NSString("TokenExpired"), this.OnTokenExpired);

			//TabBarController.TabBar.Items [0].Image = new UIImage ("ios7-clock-outline.png");
			//TabBarController.TabBar.Items [1].Image = new UIImage ("social-usd.png");

		}

		public void OnTokenExpired(NSNotification notifcation)
		{
			// Unsubscribe from this event so that it won't be handled again
			NSNotificationCenter.DefaultCenter.RemoveObserver(_tokenExpiredObserver);

			var loginViewController = this.Storyboard.InstantiateViewController("LoginView");
			var navigationController = new UINavigationController(loginViewController) { NavigationBarHidden = true };

			UIApplication.SharedApplication.Windows[0].RootViewController = navigationController;
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad();

			//IndicateLoading();

			//CreateCustomTitleBar();

			LogoutManager.CreateNavBarLeftButton(this);
		}
	}
}
