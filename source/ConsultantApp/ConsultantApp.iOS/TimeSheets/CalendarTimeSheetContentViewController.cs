using System;
using Factorymind.Components;
using UIKit;

namespace ConsultantApp.iOS.TimeSheets
{
    public partial class CalendarTimeSheetContentViewController : UIViewController
    {
		/*
        UIViewController parentController;

        public CalendarTimeSheetContentViewController() 
        {
            FMCalendar fmCalendar = new FMCalendar(View.Bounds);

            View.AddSubview(fmCalendar);

            fmCalendar.DateSelected = delegate(DateTime date)
            {
                DayTimeSheetViewController dayTSVC = (DayTimeSheetViewController)Storyboard.InstantiateViewController("DayTimeSheetViewController");
                //PresentViewController( dayTSVC, true, null );
                parentController.NavigationController.PushViewController(dayTSVC, true);
            };
        }

        public CalendarTimeSheetContentViewController( UIViewController parentVC )
        {
            FMCalendar fmCalendar = new FMCalendar(View.Bounds);

            View.AddSubview(fmCalendar);

            parentController = parentVC;

            fmCalendar.DateSelected = delegate(DateTime date)
            {
                DayTimeSheetViewController dayTSVC = (DayTimeSheetViewController)Storyboard.InstantiateViewController("DayTimeSheetViewController");
                //PresentViewController( dayTSVC, true, null );

                //DismissViewController(false, null);

                parentController.NavigationController.PushViewController(dayTSVC, true);
            };
        }

        public CalendarTimeSheetContentViewController(IntPtr handle)
            : base(handle)
        {

            FMCalendar fmCalendar = new FMCalendar(View.Bounds);

            View.AddSubview(fmCalendar);

            EdgesForExtendedLayout = UIRectEdge.None;

            fmCalendar.DateSelected = delegate(DateTime date)
            {
                DayTimeSheetViewController dayTSVC = (DayTimeSheetViewController)Storyboard.InstantiateViewController("DayTimeSheetViewController");
                //PresentViewController( dayTSVC, true, null );
                NavigationController.PushViewController(dayTSVC, true);
            };
        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        #region View lifecycle

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Perform any additional setup after loading the view, typically from a nib.
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
        }

        #endregion

	*/
	}
}