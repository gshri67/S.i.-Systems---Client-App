﻿using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using Microsoft.Practices.Unity;
using Shared.Core.Platform;
using Xamarin;

namespace ConsultantApp.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : UIApplicationDelegate
    {
        //5b5b5c in styleguide
        private static readonly UIColor NavBarTextColor = UIColor.FromRGB(91, 91, 92);

        //override to allow us to use the storyboard
        public override UIWindow Window
        {
            get;
            set;
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
                //RegisterPushNotifications();

                SetNavbarStyle();

                var tokenStore = DependencyResolver.Current.Resolve<ITokenStore>();
                var defaultStore = DependencyResolver.Current.Resolve<IDefaultStore>();

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
                    Insights.Identify(defaultStore.Username, new Dictionary<string, string>
                    {
                        {"Token Expires At", defaultStore.TokenExpiresAt},
                        {"Token Expires In", defaultStore.TokenExpiresIn.ToString()},
                        {"Token Issued At", defaultStore.TokenIssuedAt}
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

        private static void RegisterPushNotifications()
        {
            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                var pushSettings = UIUserNotificationSettings.GetSettingsForTypes(
                    UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound,
                    new NSSet());

                UIApplication.SharedApplication.RegisterUserNotificationSettings(pushSettings);
                UIApplication.SharedApplication.RegisterForRemoteNotifications();
            }
            else
            {
                UIRemoteNotificationType notificationTypes = UIRemoteNotificationType.Alert | UIRemoteNotificationType.Badge |
                                                             UIRemoteNotificationType.Sound;
                UIApplication.SharedApplication.RegisterForRemoteNotificationTypes(notificationTypes);
            }
        }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            // Get current device token
            var DeviceToken = deviceToken.Description;
            if (!string.IsNullOrWhiteSpace(DeviceToken))
            {
                DeviceToken = DeviceToken.Trim('<').Trim('>');
            }

            // Get previous device token
            var oldDeviceToken = NSUserDefaults.StandardUserDefaults.StringForKey("PushDeviceToken");

            // Has the token changed?
            if (string.IsNullOrEmpty(oldDeviceToken) || !oldDeviceToken.Equals(DeviceToken))
            {
                //TODO: Put your own logic here to notify your server that the device token has changed/been created!
            }

            // Save new device token 
            NSUserDefaults.StandardUserDefaults.SetString(DeviceToken, "PushDeviceToken");
        }
    }
}
