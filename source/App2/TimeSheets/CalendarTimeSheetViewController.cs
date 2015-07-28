using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

using Factorymind.Components;
using SidebarNavigation;

namespace App2
{
	partial class CalendarTimeSheetViewController : UIViewController
	{
        private UICollectionViewController collectionViewController;
        private SidebarController sidebar;

		public CalendarTimeSheetViewController (IntPtr handle) : base (handle)
		{
            //UICollectionViewFlowLayout layout = new UICollectionViewFlowLayout();
            //collectionViewController = new UICollectionViewController(layout);

            //Add(collectionViewController.CollectionView);
            
            FMCalendar fmCalendar = new FMCalendar(View.Bounds);

            View.AddSubview(fmCalendar);

			EdgesForExtendedLayout = UIRectEdge.None;

            fmCalendar.DateSelected = delegate( DateTime date )
            {
				DayTimeSheetViewController dayTSVC = (DayTimeSheetViewController)Storyboard.InstantiateViewController("DayTimeSheetViewController");
                //PresentViewController( dayTSVC, true, null );
				NavigationController.PushViewController( dayTSVC, true );
            };

            UIBarButtonItem sideBarButton = new UIBarButtonItem("=", UIBarButtonItemStyle.Plain, sideBarButtonTapped);
            sideBarButton.Title = "=";
           
            NavigationItem.SetRightBarButtonItem( sideBarButton, false );

            UIViewController contentAreaController = new UIViewController();// new CalendarTimeSheetContentViewController(handle);
            UIViewController navigationAreaController = new UIViewController();

            //contentAreaController.View.BackgroundColor = UIColor.Orange;
            //navigationAreaController.View.BackgroundColor = UIColor.Blue;

            contentAreaController.View.BackgroundColor = UIColor.FromRGBA(0.2f, 0.2f, 0.7f, 0.1f);
            navigationAreaController.View.BackgroundColor = UIColor.FromRGBA(0.7f, 0.7f, 0.7f, 0.1f);

            sidebar = new SidebarController(this, contentAreaController, navigationAreaController  );
            sidebar.View.UserInteractionEnabled = false;
            sidebar.MenuLocation = SidebarController.MenuLocations.Right;
            sidebar.MenuWidth = 100;
           /* 
            sideBarButton.hand
            {
                if (!sidebar.IsOpen)
                    sidebar.OpenMenu();
                else
                    sidebar.CloseMenu();
            };*/
		}

        public void sideBarButtonTapped(object sender, EventArgs args)
        {
            if (!sidebar.IsOpen)
                sidebar.OpenMenu();
            else
                sidebar.CloseMenu();
        }
	}
}
