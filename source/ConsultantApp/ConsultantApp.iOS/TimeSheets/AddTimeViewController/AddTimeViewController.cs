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
using Shared.Core;

namespace ConsultantApp.iOS
{
	partial class AddTimeViewController : UIViewController
	{

		public DateTime date;

		private Timesheet _curTimesheet;
		private TimesheetViewModel _timesheetModel;
		private AddTimeTableViewSource addTimeTableViewSource;
        private IEnumerable<string> _payRates;
		private SubtitleHeaderView subtitleHeaderView;
		private const string ScreenTitle = "Add/Edit Time";
		private readonly ActiveTimesheetViewModel _activeTimesheetModel;

		public AddTimeViewController (IntPtr handle) : base (handle)
		{
			_timesheetModel = DependencyResolver.Current.Resolve<TimesheetViewModel>();

			EdgesForExtendedLayout = UIRectEdge.None;
			_activeTimesheetModel = DependencyResolver.Current.Resolve<ActiveTimesheetViewModel>();
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

        public async void LoadPayRates()
        {
            if (_payRates == null) { 
                _payRates = await _timesheetModel.GetPayRates();
                updateUI();
            }
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

                if (tableview != null && date != null && _payRates != null) 
				{
					tableview.RegisterClassForCellReuse (typeof(TimeEntryCell), "TimeEntryCell");
					tableview.RegisterClassForCellReuse (typeof(AddProjectCodeCell), "AddProjectCodeCell");

					addTimeTableViewSource = new AddTimeTableViewSource(this, _curTimesheet.TimeEntries.Where(e => e.Date.Equals(date)), _timesheetModel.GetProjectCodes().Result, _payRates);
					addTimeTableViewSource.onDataChanged += delegate(IEnumerable<TimeEntry> timeEntries )
					{
						_curTimesheet.TimeEntries = _curTimesheet.TimeEntries.Where(e => !e.Date.Equals(date) ).Concat(timeEntries);
						headerHoursLabel.Text = "Daily Hours: " + _curTimesheet.TimeEntries.Where(e => e.Date.Equals(date) ).Sum (t => t.Hours).ToString ();

						//updateUI();
					};
					tableview.Source = addTimeTableViewSource;

					tableview.ReloadData();
				}

				//if timesheet is submitted or approved etc.. we cannot add or edit the project codes
				if (_curTimesheet.Status != MatchGuideConstants.TimesheetStatus.Open) 
				{
					if (addButton != null)
						addButton.Enabled = false;

					if (tableview != null)
						tableview.UserInteractionEnabled = false;
				}
				else
				{
					if (addButton != null)
						addButton.Enabled = true;

					if (tableview != null)
						tableview.UserInteractionEnabled = true;
				}
			}

			if (date != null) 
			{
				if (headerDateLabel != null) 
				{
					headerDateLabel.Text = date.ToString("ddd") + " " + date.ToString("MMM") + " " + date.ToString("dd").TrimStart('0');
				}
			}
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

		    headerContainer.BackgroundColor = StyleGuideConstants.LightGrayUiColor;

            LoadPayRates();

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

				//if( !addTimeTableViewSource.mustSave )
				{
					if( inPeriodComparison >= 0 )	
						SetDate(this.date.AddDays(-1));

					if( inPeriodComparison <= 0 )
						leftArrowButton.Hidden = true;
					else
						leftArrowButton.Hidden = false;

					rightArrowButton.Hidden = false;//assuming everything goes well and a date outside period is not selected

					copyOverButton.Hidden = true;

					updateUI();
				}
			};
			rightArrowButton.TouchUpInside += delegate 
			{ 
				int inPeriodComparison = date.AddDays(1).CompareTo(_curTimesheet.EndDate);

				//if( !addTimeTableViewSource.mustSave )
				{
					if( inPeriodComparison <= 0 )
					{
						DateTime oldDate = date;
						SetDate(this.date.AddDays(1));

						//copy over
						if( _curTimesheet.TimeEntries.Where(e => e.Date.Equals(date)).Count() == 0 && _curTimesheet.TimeEntries.Where(e => e.Date.Equals(oldDate)).Count() > 0)
							copyOverButton.Hidden = false;
						else
							copyOverButton.Hidden = true;
					}
					if( inPeriodComparison >= 0 )
						rightArrowButton.Hidden = true;
					else
						rightArrowButton.Hidden = false;

					leftArrowButton.Hidden = false;

					updateUI();
				}
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

				_curTimesheet.TimeEntries = _curTimesheet.TimeEntries.Concat( newEnumerableEntry );// _curTimesheet.TimeEntries
				addTimeTableViewSource._timeEntries = _curTimesheet.TimeEntries.Where(e => e.Date.Equals(date));

				Console.WriteLine(_curTimesheet.TimeEntries.Count());

				addTimeTableViewSource.handleNewCell();

				tableview.ReloadData();

				//save timeentry to timesheet
				_timesheetModel.saveTimesheet(_curTimesheet);

				copyOverButton.Hidden = true;
			};
		    addButton.TintColor = StyleGuideConstants.RedUiColor;

			tableview.ContentInset = new UIEdgeInsets (-35, 0, 0, 0);

			updateUI ();

			copyOverButton.TouchUpInside += (object sender, EventArgs e) => 
			{
				copyOverTime();
				copyOverButton.Hidden = true;
			};

			CreateCustomTitleBar();
		}

		//copies over time entries from previous day
		public void copyOverTime()
		{
			DateTime oldDate = date.AddDays(-1);

			List<TimeEntry> entries = _curTimesheet.TimeEntries.Where(e => e.Date.Equals(oldDate)).ToList();
			TimeEntry[] eCopy = new TimeEntry[entries.Count()];
			entries.CopyTo( eCopy );

			List<TimeEntry> newEntries = new List<TimeEntry>();

			//copy over the previous days time entries by copying them, changing the dates, and concatenating
			for( int i = 0; i < entries.Count(); i ++ )
			{
				TimeEntry newEntry = entries[i].clone();

				newEntry.Date = date;
				newEntries.Add(newEntry);
				Console.WriteLine("Changed hours to 3.5");
			}

			_curTimesheet.TimeEntries = _curTimesheet.TimeEntries.Concat( newEntries.AsEnumerable() );
			updateUI();
		}

		private void CreateCustomTitleBar()
		{
			InvokeOnMainThread(() =>
				{
					subtitleHeaderView = new SubtitleHeaderView();
					NavigationItem.TitleView = subtitleHeaderView;
					subtitleHeaderView.TitleText = ScreenTitle;
					subtitleHeaderView.SubtitleText = CurrentConsultantDetails.CorporationName ?? string.Empty;
					NavigationItem.Title = "";

					if( subtitleHeaderView.SubtitleText.Equals( string.Empty ) )
						RetrieveConsultantDetails();
				});
		}

		private async void RetrieveConsultantDetails()
		{
			var details = await _activeTimesheetModel.GetConsultantDetails();
			SetCurrentConsultantDetails(details);
			subtitleHeaderView.SubtitleText = CurrentConsultantDetails.CorporationName ?? string.Empty;
		}

		private static void SetCurrentConsultantDetails(ConsultantDetails details)
		{
			if (details != null)
				CurrentConsultantDetails.CorporationName = details.CorporationName;
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
