using System;
using AccountExecutiveApp.Core.ViewModel;
using Foundation;
using Microsoft.Practices.Unity;
using UIKit;

namespace AccountExecutiveApp.iOS
{
	public class SearchManager
	{
	    //private readonly LogoutViewModel _logoutViewModel;
	    public SearchManager()
	    {
          //  _logoutViewModel = DependencyResolver.Current.Resolve<LogoutViewModel>();
	    }

        public static void CreateNavBarRightButton(UIViewController viewController)
        {
            var buttonImage = UIImage.FromBundle("searchStrongIcon").ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal);
            viewController.NavigationItem.SetRightBarButtonItem(
                new UIBarButtonItem(buttonImage
                , UIBarButtonItemStyle.Plain
                , (sender, args) =>
                {
                    AdditionalActions_Pressed(viewController);
                })
            , true);
        }
        
        private void SearchDelegate(UIAlertAction action)
        {
            /*
            _logoutViewModel.Logout();
            var rootController =
                UIStoryboard.FromName("MainStoryboard", NSBundle.MainBundle)
                    .InstantiateViewController("LoginView");
            var navigationController = new UINavigationController(rootController)
            {
                NavigationBarHidden = true
            };
            UIApplication.SharedApplication.Windows[0].RootViewController =
                navigationController;
             */

        }
        /*
        private void LogoutDelegate(object sender, UIButtonEventArgs args)
        {
            if (args.ButtonIndex == 0)
            {
                LogoutDelegate(null);
            }
        }*/

		private static void AdditionalActions_Pressed( UIViewController vc )
		{
			var searchVC = (SearchTableViewController)vc.NavigationController.Storyboard.InstantiateViewController("SearchTableViewController");
            vc.NavigationController.PresentViewController(searchVC, true, null);
            /*
			Action<UIAlertAction> logoutDelegate = delegate { //copied down below
				LogoutViewModel logoutViewModel = DependencyResolver.Current.Resolve<LogoutViewModel> ();
				logoutViewModel.Logout();

                vc.NavigationController.PushViewController(vc.NavigationController.Storyboard.InstantiateViewController("LoginView"), false);
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

                        vc.NavigationController.PushViewController(vc.NavigationController.Storyboard.InstantiateViewController("LoginView"), false);
					}
				};
				sheet.ShowFromTabBar(vc.NavigationController.TabBarController.TabBar);// ShowInView(View);
			}*/
		}
	}
}

