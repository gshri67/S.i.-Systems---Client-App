using System;
using System.Collections.Generic;
using System.Linq;
using Shared.Core.Platform;
using Microsoft.Practices.Unity;
using Xamarin;
using Foundation;
using UIKit;

namespace AccountExecutiveApp.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : UIApplicationDelegate
    {
		//5b5b5c in styleguide
		private static readonly UIColor NavBarTextColor = UIColor.FromRGB(91, 91, 92);

        // class-level declarations

        public override UIWindow Window
        {
            get;
            set;
        }

        //
        // This method is invoked when the application is about to move from active to inactive state.
        //
        // OpenGL applications should use this method to pause.
        //
        public override void OnResignActivation(UIApplication application)
        {
        }

        // This method should be used to release shared resources and it should store the application state.
        // If your application supports background exection this method is called instead of WillTerminate
        // when the user quits.
        public override void DidEnterBackground(UIApplication application)
        {
        }

        // This method is called as part of the transiton from background to active state.
        public override void WillEnterForeground(UIApplication application)
        {
        }

        // This method is called when the application is about to terminate. Save data, if needed. 
        public override void WillTerminate(UIApplication application)
        {
        }

		//
		// This method is invoked when the application has loaded and is ready to run. In this 
		// method you should instantiate the window, load the UI into it and then make the window
		// visible.
		//
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		//
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			try
			{
				SetNavbarStyle();

				var tokenStore = DependencyResolver.Current.Resolve<ITokenStore>();
				var token = tokenStore.GetDeviceToken();
				if (tokenStore.GetDeviceToken() == null)
				{
					// User is not logged in - display the login view
					// TODO: We could also proactively check their token expiry here
					var rootController =
						UIStoryboard.FromName("MainStoryboard", NSBundle.MainBundle)
							.InstantiateViewController("LoginView");
					var navigationController = new UINavigationController(rootController) { NavigationBarHidden = true };

					this.Window.RootViewController = navigationController;
				}
				else
				{
					Insights.Identify(token.Username, new Dictionary<string, string>
						{
							{"Token Expires At", token.ExpiresAt},
							{"Token Expires In", token.ExpiresIn.ToString()},
							{"Token Issued At", token.IssuedAt}
						});

					this.Window.RootViewController =
						UIStoryboard.FromName("MainStoryboard", NSBundle.MainBundle).InstantiateInitialViewController();
				}
			}
			catch (Exception e)
			{
				Insights.Report(e, Insights.Severity.Error);
				return false;
			}

			return true;
		}

		private static void SetNavbarStyle()
		{
			UINavigationBar.Appearance.TintColor = NavBarTextColor;

			UINavigationBar.Appearance.SetTitleTextAttributes(new UITextAttributes
				{
					Font = UIFont.SystemFontOfSize(20f),
					TextColor = NavBarTextColor
				});
		}
    }
}