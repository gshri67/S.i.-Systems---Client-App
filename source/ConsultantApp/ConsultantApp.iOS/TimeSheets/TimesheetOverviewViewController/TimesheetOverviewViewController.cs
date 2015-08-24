using System;
using Foundation;
using UIKit;
using System.Collections.Generic;
using System.Linq;
using ConsultantApp.Core.ViewModels;
using Microsoft.Practices.Unity;
using Shared.Core;
using SiSystems.SharedModels;
using System.Threading.Tasks;

namespace ConsultantApp.iOS.TimeEntryViewController
{
	public partial class TimesheetOverviewViewController : UIViewController
	{
	    private readonly TimesheetViewModel _timesheetModel;
		private Timesheet _curTimesheet;
		private FMCalendar calendar;

		public TimesheetOverviewViewController (IntPtr handle) : base (handle)
		{
			//TabBarController.TabBar.TintColor = UIColor.Blue;

            //TabBarController.TabBar.Items [0].Image = new UIImage ("ios7-clock-outline.png");
            //TabBarController.TabBar.Items [1].Image = new UIImage ("social-usd.png");

			_timesheetModel = DependencyResolver.Current.Resolve<TimesheetViewModel>();
		}
        
        public void SetTimesheet(Timesheet timesheet)
        {
            _curTimesheet = timesheet;

			//calendarDateLabel.Text = curMonthDate.ToString("MMMMM yyyy");
        }

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			addButton.TouchUpInside += delegate {
				
				UIViewController vc = Storyboard.InstantiateViewController("AddTimeEntryViewController");

				NavigationController.PushViewController(vc, true);
			};

			calendar = new FMCalendar (calendarContainerView.Bounds, new CoreGraphics.CGRect(), _curTimesheet );
			calendarContainerView.AddSubview (calendar);

			calendar.DateSelected = delegate(DateTime date) {
				AddTimeViewController vc = (AddTimeViewController)Storyboard.InstantiateViewController( "AddTimeViewController" );
				NavigationController.PushViewController(vc, true);
			};

			calendarLeftButton.SetTitle("", UIControlState.Normal);
			calendarLeftButton.SetTitleColor(UIColor.Black, UIControlState.Normal);
			calendarLeftButton.SetImage( new UIImage("leftArrow.png"), UIControlState.Normal );


			calendarRightButton.SetTitle("", UIControlState.Normal);
			calendarRightButton.SetTitleColor(UIColor.Black, UIControlState.Normal);
			calendarRightButton.SetImage( new UIImage("rightArrow.png"), UIControlState.Normal );


			submitContainerView.BackgroundColor = StyleGuideConstants.LightGrayUiColor;
			approverContainerView.BackgroundColor = StyleGuideConstants.LightGrayUiColor;
			calendarHeaderView.BackgroundColor = StyleGuideConstants.MediumGrayUiColor;

			DateTime curMonthDate;

			if (_curTimesheet != null)
				curMonthDate = _curTimesheet.StartDate;
			else
				curMonthDate = DateTime.Now;
			
			calendarDateLabel.Text = curMonthDate.ToString("MMMMM yyyy");

			calendarLeftButton.TouchUpInside += delegate 
			{
				calendar.MoveCalendarMonths(false, true);
				curMonthDate = curMonthDate.AddMonths(-1);
				calendarDateLabel.Text = curMonthDate.ToString("MMMMM yyyy");
			};
			calendarRightButton.TouchUpInside += delegate 
			{ 
				calendar.MoveCalendarMonths(true, true);
				curMonthDate = curMonthDate.AddMonths(1);
				calendarDateLabel.Text = curMonthDate.ToString("MMMMM yyyy");
			};
		}

		//dismiss keyboard when tapping outside of text fields
		public override void TouchesBegan(NSSet touches, UIEvent evt)
		{
			base.TouchesBegan(touches, evt);
			/*
			TimeEntryCell currentCell = (TimeEntryCell)(tableview.CellAt(tableview.IndexPathForSelectedRow));
			if( currentCell != null )
				currentCell.hoursField.ResignFirstResponder();*/
		}


		public override void PrepareForSegue (UIStoryboardSegue segue, NSObject sender)
		{
			base.PrepareForSegue (segue, sender);
		}
	}
}
