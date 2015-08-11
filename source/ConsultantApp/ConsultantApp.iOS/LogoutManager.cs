using System;
using UIKit;
using ConsultantApp.Core.ViewModels;
using Microsoft.Practices.Unity;

namespace ConsultantApp.iOS
{
	public class LogoutManager
	{
		public LogoutManager ()
		{
		}

		public static void CreateNavBarLeftButton ( UIViewController vc)
		{
			var buttonImage = UIImage.FromBundle("app-button").ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal);
			vc.NavigationItem.SetLeftBarButtonItem(
				new UIBarButtonItem(buttonImage
					, UIBarButtonItemStyle.Plain
					, (sender, args) =>
					{
						AdditionalActions_Pressed( vc );
					})
				, true);
		}

		delegate void LogoutDelegate();

		private static void AdditionalActions_Pressed( UIViewController vc )
		{
			Action<UIAlertAction> logoutDelegate = delegate { //copied down below
				LogoutViewModel logoutViewModel = DependencyResolver.Current.Resolve<LogoutViewModel> ();
				logoutViewModel.Logout();

				vc.NavigationController.PushViewController(vc.NavigationController.Storyboard.InstantiateViewController("LoginViewController"), false);
			};

			if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
			{
				var controller = UIAlertController.Create(null, null, UIAlertControllerStyle.ActionSheet);
				var logoutAction = UIAlertAction.Create("Logout", UIAlertActionStyle.Destructive, logoutDelegate);
				var cancelAction = UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, null);
				controller.AddAction(logoutAction);
				controller.AddAction(cancelAction);
				vc.PresentViewController(controller, true, null);
			}
			else
			{
				var sheet = new UIActionSheet();
				sheet.AddButton("Logout");
				sheet.AddButton("Cancel");
				sheet.DestructiveButtonIndex = 0;
				sheet.CancelButtonIndex = 1;
				sheet.Clicked += delegate (object sender, UIButtonEventArgs args)
				{
					//if logout button tapped
					if (args.ButtonIndex == 0)
					{
						LogoutViewModel logoutViewModel = DependencyResolver.Current.Resolve<LogoutViewModel> ();
						logoutViewModel.Logout();

						vc.NavigationController.PushViewController(vc.NavigationController.Storyboard.InstantiateViewController("LoginViewController"), false);
					}
				};
				sheet.ShowFromTabBar(vc.NavigationController.TabBarController.TabBar);// ShowInView(View);
			}
		}
	}
}

