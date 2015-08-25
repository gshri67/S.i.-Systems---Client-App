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

			updateUI ();

        }

		//if the timesheet changes this should be called
		public void updateUI()
		{
			if (_curTimesheet != null) 
			{
				DateTime curMonthDate = _curTimesheet.StartDate;

				if (calendarDateLabel != null)
					calendarDateLabel.Text = curMonthDate.ToString ("MMMMM yyyy");

				//sum up timsheet hours
				if (totalHoursLabel != null)
					totalHoursLabel.Text = _curTimesheet.TimeEntries.Sum (t => t.Hours).ToString ();

				if( submitMonthLabel != null )
					submitMonthLabel.Text = curMonthDate.ToString ("MMM");

				if (submitDayYearLabel != null) 
					submitDayYearLabel.Text = _curTimesheet.StartDate.ToString("dd").TrimStart('0') + "-" + _curTimesheet.EndDate.ToString("dd").TrimStart('0') + ", " + curMonthDate.ToString ("yyyy");
			}
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
				vc.SetDate( date );
				vc.SetTimesheet( _curTimesheet );
				NavigationController.PushViewController(vc, true);
			};

			calendarLeftButton.SetTitle("", UIControlState.Normal);
			calendarLeftButton.SetTitleColor(UIColor.Black, UIControlState.Normal);
			calendarLeftButton.SetImage( new UIImage("leftArrow.png"), UIControlState.Normal );


			calendarRightButton.SetTitle("", UIControlState.Normal);
			calendarRightButton.SetTitleColor(UIColor.Black, UIControlState.Normal);
			calendarRightButton.SetImage( new UIImage("rightArrow.png"), UIControlState.Normal );


			submitContainerView.BackgroundColor = UIColor.White;// StyleGuideConstants.LightGrayUiColor;
			approverContainerView.BackgroundColor = UIColor.White;//StyleGuideConstants.LightGrayUiColor;
			calendarHeaderView.BackgroundColor = StyleGuideConstants.LightGrayUiColor;

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

			calendarLeftButton.Hidden = true;
			calendarRightButton.Hidden = true;

			submitContainerView.Layer.BorderWidth = 0.5f;
			submitContainerView.Layer.BorderColor = StyleGuideConstants.LightGrayUiColor.CGColor;

			updateUI();
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
