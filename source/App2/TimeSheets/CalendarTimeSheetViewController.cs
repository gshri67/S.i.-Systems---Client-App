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

            sidebar = new SidebarController(this, new UIViewController(), new UIViewController()  );
            sidebar.MenuLocation = SidebarController.MenuLocations.Right;

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
