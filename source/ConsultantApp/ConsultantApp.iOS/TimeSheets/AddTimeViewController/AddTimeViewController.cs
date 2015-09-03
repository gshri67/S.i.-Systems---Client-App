using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using SiSystems.SharedModels;
using System.Collections.Generic;
using System.Linq;
using ConsultantApp.Core.ViewModels;
using CoreGraphics;
using Microsoft.Practices.Unity;
//using ConsultantApp.SharedModels;

namespace ConsultantApp.iOS
{
	partial class AddTimeViewController : UIViewController
	{

		public DateTime date;

		private Timesheet _curTimesheet;
		private TimesheetViewModel _timesheetModel;
		private AddTimeTableViewSource addTimeTableViewSource;

		public AddTimeViewController (IntPtr handle) : base (handle)
		{
			_timesheetModel = DependencyResolver.Current.Resolve<TimesheetViewModel>();

			EdgesForExtendedLayout = UIRectEdge.None;
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
					tableview.RegisterClassForCellReuse (typeof(AddProjectCodeCell), "AddProjectCodeCell");

					addTimeTableViewSource = new AddTimeTableViewSource(this, _curTimesheet.TimeEntries.Where(e => e.Date.Equals(date)), _timesheetModel.GetProjectCodes().Result, _timesheetModel.GetPayRates().Result);
					addTimeTableViewSource.onDataChanged += delegate(IEnumerable<TimeEntry> timeEntries)
					{
						_curTimesheet.TimeEntries = timeEntries;
						headerHoursLabel.Text = "Daily Hours: " + _curTimesheet.TimeEntries.Where(e => e.Date.Equals(date) ).Sum (t => t.Hours).ToString ();

						//updateUI();
					};
					tableview.Source = addTimeTableViewSource;

					tableview.ReloadData();
				}
			}

			if (date != null) 
			{
				if (headerDateLabel != null) 
				{
					headerDateLabel.Text = date.ToString("MMM") + " " + date.ToString("dd").TrimStart('0');
				}
			}
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
            
		    headerContainer.BackgroundColor = StyleGuideConstants.LightGrayUiColor;

			leftArrowButton.SetTitle("", UIControlState.Normal);
			leftArrowButton.SetImage( new UIImage("leftArrow.png"), UIControlState.Normal );
            leftArrowButton.SetTitleColor(StyleGuideConstants.RedUiColor, UIControlState.Normal);
		    leftArrowButton.TintColor = StyleGuideConstants.RedUiColor;

			rightArrowButton.SetTitle("", UIControlState.Normal);
			rightArrowButton.SetImage( new UIImage("rightArrow.png"), UIControlState.Normal );
            rightArrowButton.SetTitleColor(StyleGuideConstants.RedUiColor, UIControlState.Normal);
            rightArrowButton.TintColor = StyleGuideConstants.RedUiColor;

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

			addButton.TouchUpInside += delegate 
			{
				TimeEntry newEntry = new TimeEntry();
				newEntry.Date = date;
				newEntry.ProjectCode = "Project Code";
				newEntry.Hours = 8;
				newEntry.PayRate = "Pay Rate";

				IEnumerable<TimeEntry> newEnumerableEntry = new List<TimeEntry>(){ newEntry };

				Console.WriteLine(_curTimesheet.TimeEntries.Count());

				_curTimesheet.TimeEntries = addTimeTableViewSource._timeEntries.Concat( newEnumerableEntry );// _curTimesheet.TimeEntries
				addTimeTableViewSource._timeEntries = _curTimesheet.TimeEntries.Where(e => e.Date.Equals(date));

				Console.WriteLine(_curTimesheet.TimeEntries.Count());

				addTimeTableViewSource.handleNewCell();

				tableview.ReloadData();

				//save timeentry to timesheet
				_timesheetModel.saveTimesheet(_curTimesheet);
			};
		    addButton.TintColor = StyleGuideConstants.RedUiColor;

			tableview.ContentInset = new UIEdgeInsets (-35, 0, 0, 0);

			updateUI ();
		}

        //public override void ViewDidLayoutSubviews()
        //{
        //    base.ViewDidLayoutSubviews();
	        
        //    var x = tableview.Frame.X;
        //    var y = tableview.Frame.Y;
        //    var width = tableview.Frame.Width;
        //    var height = 0;
        //    tableview.TableHeaderView = new UIView(new CGRect(x, y, width, height));
        //}
	}
}
