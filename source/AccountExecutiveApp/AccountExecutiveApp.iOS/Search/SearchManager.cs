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
		/*
			UINavigationController navVC = new UINavigationController(searchVC);
			navVC.SetNavigationBarHidden (true, false);
			navVC.NavigationBar.Translucent = false;
			//navVC.SetToolbarHidden (false, false);

			//var navVC = (SearchTableNavigationController)vc.NavigationController.Storyboard.InstantiateViewController("SearchTableNavigationController");

			//vc.NavigationController.ShowViewController(navVC, vc.NavigationController);

			//vc.NavigationController.AddChildViewController (searchVC);
			//vc.ShowViewController(searchVC, vc);
			//vc.NavigationController.ShowViewController(searchVC, vc.NavigationController);

			vc.TabBarController.AddChildViewController (navVC);
			vc.NavigationController.ShowViewController(navVC, vc.NavigationController);
*/

			vc.ShowViewController(searchVC, vc);
        }
	}
}

