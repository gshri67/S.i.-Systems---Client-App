using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;

namespace ClientApp.iOS
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
            SetNavbarStyle();

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
