using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using SiSystems.SharedModels;
using System.Collections.Generic;
using System.Linq;

namespace ConsultantApp.iOS
{
	partial class AddTimeViewController : UIViewController
	{

		public DateTime date;

		private Timesheet _curTimesheet;

		public AddTimeViewController (IntPtr handle) : base (handle)
		{
		}

		public void SetTimesheet(Timesheet timesheet)
		{
			_curTimesheet = timesheet;

			updateUI ();
		}

		public void SetDate(DateTime date)
		{
			this.date = date;

			updateUI ();
		}

		//if the timesheet changes this should be called
		public void updateUI()
		{
			if (_curTimesheet != null ) 
			{
				if (headerHoursLabel != null && date != null ) 
				{
					headerHoursLabel.Text = "Daily Hours: " + _curTimesheet.TimeEntries.Where(e => e.Date.Equals(date) ).Sum (t => t.Hours).ToString (); 
				}

				if (tableview != null && date != null ) 
				{
					tableview.RegisterClassForCellReuse (typeof(TimeEntryCell), "TimeEntryCell");
					tableview.Source = new AddTimeTableViewSource(this, _curTimesheet.TimeEntries.Where(e => e.Date.Equals(date)));
					tableview.ReloadData();
				}
			}

			if (date != null) 
			{
				if (headerDateLabel != null) 
				{
					headerDateLabel.Text = "Date: " + date.ToString("MMM") + " " + date.ToString("dd").TrimStart('0');
				}
			}
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			headerContainer.BackgroundColor = StyleGuideConstants.LightGrayUiColor;
			headerLabelsContainer.BackgroundColor = headerContainer.BackgroundColor;

			leftArrowButton.SetTitle("", UIControlState.Normal);
			leftArrowButton.SetTitleColor(UIColor.Black, UIControlState.Normal);
			leftArrowButton.SetImage( new UIImage("leftArrow.png"), UIControlState.Normal );

			rightArrowButton.SetTitle("", UIControlState.Normal);
			rightArrowButton.SetTitleColor(UIColor.Black, UIControlState.Normal);
			rightArrowButton.SetImage( new UIImage("rightArrow.png"), UIControlState.Normal );

			leftArrowButton.TouchUpInside += delegate 
			{
				int inPeriodComparison = date.AddDays(-1).CompareTo(_curTimesheet.StartDate);

				if( inPeriodComparison >= 0 )	
					SetDate(this.date.AddDays(-1));

				if( inPeriodComparison <= 0 )
					leftArrowButton.Hidden = true;
				else
					leftArrowButton.Hidden = false;

				rightArrowButton.Hidden = false;//assuming everything goes well and a date outside period is not selected
			};
			rightArrowButton.TouchUpInside += delegate 
			{ 
				int inPeriodComparison = date.AddDays(1).CompareTo(_curTimesheet.EndDate);

				if( inPeriodComparison <= 0 )
					SetDate(this.date.AddDays(1));

				if( inPeriodComparison >= 0 )
					rightArrowButton.Hidden = true;
				else
					rightArrowButton.Hidden = false;

				leftArrowButton.Hidden = false;
			};

			if (date.CompareTo (_curTimesheet.StartDate) == 0)
				leftArrowButton.Hidden = true;
			else if(date.CompareTo (_curTimesheet.EndDate) == 0)
				rightArrowButton.Hidden = true;

			updateUI ();
		}
	}
}