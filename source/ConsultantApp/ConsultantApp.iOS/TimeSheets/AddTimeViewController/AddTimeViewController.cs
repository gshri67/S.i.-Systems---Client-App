using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ConsultantApp.Core.ViewModels;
using Microsoft.Practices.Unity;
using Shared.Core;
using SiSystems.SharedModels;
using UIKit;

namespace ConsultantApp.iOS
{
	partial class AddTimeViewController : UIViewController
	{
        public DateTime Date;
        private Timesheet _curTimesheet;
		private readonly TimesheetViewModel _timesheetModel;
		private AddTimeTableViewSource _addTimeTableViewSource;
        private IEnumerable<string> _payRates;
		private SubtitleHeaderView _subtitleHeaderView;
		private const string ScreenTitle = "Add/Edit Time";

		public AddTimeViewController (IntPtr handle) : base (handle)
		{
			_timesheetModel = DependencyResolver.Current.Resolve<TimesheetViewModel>();

			EdgesForExtendedLayout = UIRectEdge.None;
		}

		public void SetTimesheet(Timesheet timesheet)
		{
			_curTimesheet = timesheet;

			UpdateUI();
		}

		public void SetDate(DateTime date)
		{
			this.Date = date;

			UpdateUI ();
		}

        public async void LoadPayRates()
        {
            if (_payRates != null) return;

            _payRates = await _timesheetModel.GetPayRates();
            UpdateUI();
        }

	    private void SetupTableViewSource()
	    {
            if (tableview == null || _payRates == null || _curTimesheet == null || _addTimeTableViewSource != null) return;
            tableview.RegisterClassForCellReuse(typeof(TimeEntryCell), "TimeEntryCell");
            tableview.RegisterClassForCellReuse(typeof(AddProjectCodeCell), "AddProjectCodeCell");

            _addTimeTableViewSource = new AddTimeTableViewSource(_curTimesheet.TimeEntries.Where(e => e.Date.Equals(Date)), _timesheetModel.GetProjectCodes().Result, _payRates);
            _addTimeTableViewSource.OnDataChanged += delegate(IEnumerable<TimeEntry> timeEntries)
            {
                _curTimesheet.TimeEntries = _curTimesheet.TimeEntries.Where(e => !e.Date.Equals(Date)).Concat(timeEntries);
                headerHoursLabel.Text = _curTimesheet.TimeEntries.Where(e => e.Date.Equals(Date)).Sum(t => t.Hours).ToString(CultureInfo.InvariantCulture);
            };
            tableview.Source = _addTimeTableViewSource;
	    }

	    private void ReloadTableViewData()
	    {
            if (tableview == null) return;
            tableview.ReloadData();
	    }

	    private void SetTimesheetEditability()
	    {
	        var enabled = TimesheetEditable();
            if (addButton != null)
                addButton.Enabled = enabled;

            if (tableview == null || _addTimeTableViewSource == null) return;

            tableview.UserInteractionEnabled = enabled;
            _addTimeTableViewSource.Enable(enabled);
	    }

	    private void SetHeaderHours()
	    {
            if (_curTimesheet == null) return;

            if (headerHoursLabel != null)
            {
                headerHoursLabel.Text = _curTimesheet.TimeEntries.Where(e => e.Date.Equals(Date)).Sum(t => t.Hours).ToString();
            }
	    }

		//if the timesheet changes this should be called
		public void UpdateUI()
		{
            SetHeaderDate();
            SetTimesheetEditability();
		    SetHeaderHours();
		    SetupTableViewSource();
		    ReloadTableViewData();
		}

	    private bool TimesheetEditable()
	    {
            return _curTimesheet.Status == MatchGuideConstants.TimesheetStatus.Open 
                || _curTimesheet.Status == MatchGuideConstants.TimesheetStatus.Rejected;
	    }

	    private void SetHeaderDate()
	    {
            if (headerDateLabel != null)
                headerDateLabel.Text = Date.ToString("MMM") + " " + Date.ToString("dd").TrimStart('0');
            if (headerDayOfWeekLabel != null)
                headerDayOfWeekLabel.Text = Date.ToString("ddd");
	    }

        private void PreviousDay(object sender, EventArgs e)
	    {
            var inPeriodComparison = Date.AddDays(-1).CompareTo(_curTimesheet.StartDate);

            if (inPeriodComparison >= 0)
                SetDate(this.Date.AddDays(-1));

            if (inPeriodComparison <= 0)
                leftArrowButton.Hidden = true;
            else
                leftArrowButton.Hidden = false;

            rightArrowButton.Hidden = false;

            UpdateUI();
	    }

	    private void NextDay(object sender, EventArgs e)
	    {
            var inPeriodComparison = Date.AddDays(1).CompareTo(_curTimesheet.EndDate);

            if (inPeriodComparison <= 0)
                SetDate(this.Date.AddDays(1));
            if (inPeriodComparison >= 0)
                rightArrowButton.Hidden = true;
            else
                rightArrowButton.Hidden = false;

            leftArrowButton.Hidden = false;

            UpdateUI();
	    }

	    private void SetupDayNavigationButton(UIButton button,UIImage image, EventHandler action)
	    {
            SetButtonImage(button, image);
	        if (action != null) button.TouchUpInside += action;
	    }

	    private void SetupDayNavigationButtons()
	    {
            SetupDayNavigationButton(leftArrowButton, new UIImage("leftArrow.png"), PreviousDay);
            SetupDayNavigationButton(rightArrowButton, new UIImage("rightArrow.png"), NextDay);

            if (Date.CompareTo(_curTimesheet.StartDate) == 0)
                leftArrowButton.Hidden = true;
            else if (Date.CompareTo(_curTimesheet.EndDate) == 0)
                rightArrowButton.Hidden = true;
	    }

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

		    headerContainer.BackgroundColor = StyleGuideConstants.LightGrayUiColor;

            LoadPayRates();

            SetupDayNavigationButtons();

			addButton.TouchUpInside += delegate 
			{
			    var newEntry = new TimeEntry
			    {
			        Date = Date, ProjectCode = "Project Code", Hours = 8, PayRate = "Pay Rate"
			    };

			    IEnumerable<TimeEntry> newEnumerableEntry = new List<TimeEntry>{ newEntry };

				_curTimesheet.TimeEntries = _curTimesheet.TimeEntries.Concat( newEnumerableEntry );
				_addTimeTableViewSource.TimeEntries = _curTimesheet.TimeEntries.Where(e => e.Date.Equals(Date));

				_addTimeTableViewSource.HandleNewCell();

				tableview.ReloadData();

				_timesheetModel.SaveTimesheet(_curTimesheet);
			};
		    addButton.TintColor = StyleGuideConstants.RedUiColor;

			savingLabel.Alpha = 0;
			savingIndicator.Alpha = 0;

			saveButton.TouchUpInside += delegate {

				savingLabel.Text = "Saving";

				_timesheetModel.saveTimesheet( _curTimesheet );

				UIView.Animate(0.7f, 0, UIViewAnimationOptions.TransitionNone, () => 
					{
						saveButton.Alpha = 0;
						savingLabel.Alpha = 1;
						savingIndicator.Alpha = 1;
						savingIndicator.StartAnimating();
					}, () => 
					{
						UIView.Animate(0.5f, 0.7f, UIViewAnimationOptions.TransitionNone, () => 
							{
								savingIndicator.Alpha = 0;
								savingLabel.Alpha = 0.5f;
							}, () => 
							{
								savingLabel.Text = "Saved";
								savingIndicator.StopAnimating();

								UIView.Animate(0.5f, 0, UIViewAnimationOptions.TransitionNone, () => 
									{
										savingLabel.Alpha = 1;
									}, () => 
									{
										UIView.Animate(0.5f, 0.5f, UIViewAnimationOptions.TransitionNone, () => 
											{
												savingLabel.Alpha = 0;
											}, () => 
											{	
												UIView.Animate(0.5f, 0, UIViewAnimationOptions.TransitionNone, () => 
													{
														saveButton.Alpha = 1;
													}, () => {});	
											});
									});					
							});						
					});

			};

			tableview.ContentInset = new UIEdgeInsets (-35, 0, 0, 0);

			UpdateUI ();
			CreateCustomTitleBar();
		}

	    private void SetButtonImage(UIButton button, UIImage image)
	    {
	        button.SetTitle("", UIControlState.Normal);
            button.SetImage(image, UIControlState.Normal);
            button.SetTitleColor(StyleGuideConstants.RedUiColor, UIControlState.Normal);
	        button.TintColor = StyleGuideConstants.RedUiColor;
	    }

	    private void CreateCustomTitleBar()
		{
			InvokeOnMainThread(() =>
				{
					_subtitleHeaderView = new SubtitleHeaderView();
					NavigationItem.TitleView = _subtitleHeaderView;
					_subtitleHeaderView.TitleText = ScreenTitle;
					_subtitleHeaderView.SubtitleText = CurrentConsultantDetails.CorporationName ?? string.Empty;
					NavigationItem.Title = "";
				});
		}
	}
}
