using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using ConsultantApp.Core.ViewModels;
using Microsoft.Practices.Unity;
using Shared.Core;
using SiSystems.SharedModels;
using UIKit;
using ConsultantApp.iOS.TimeEntryViewController;

namespace ConsultantApp.iOS
{
    partial class AddTimeViewController : UIViewController
    {
        private const string ScreenTitle = "Add/Edit Time";
        private readonly TimesheetViewModel _timesheetModel;
        private AddTimeTableViewSource _addTimeTableViewSource;
        private Timesheet _curTimesheet;
        private IEnumerable<PayRate> _payRates;
        private SubtitleHeaderView _subtitleHeaderView;
        public DateTime Date;
		private int maxFrequentlyUsed = 5;

        public AddTimeDelegate TimeDelegate;

        public AddTimeViewController(IntPtr handle) : base(handle)
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
            Date = date;

            UpdateUI();
        }

        public async void LoadPayRates()
        {
            if (_payRates != null) return;

            //_payRates = _timesheetModel.GetPayRatesForIdAndProjectCode(_curTimesheet.ContractId, string.Empty);

            _timesheetModel.LoadTimesheetSupport();
            _payRates = _timesheetModel.GetPayRatesForIdAndProjectCode(1, string.Empty);

			UpdateUI();
        }

        private void SetupTableViewSource()
        {
            if (tableview == null || _payRates == null || _curTimesheet == null || _addTimeTableViewSource != null)
                return;

            RegisterCellsForReuse();
            InstantiateTableViewSource();

            tableview.Source = _addTimeTableViewSource;
        }

        private void InstantiateTableViewSource()
        {
            _addTimeTableViewSource = new AddTimeTableViewSource(
                GetTimeEntriesForSelectedDate(),
                _timesheetModel.GetProjectCodes(), 
                _payRates
            );

            _addTimeTableViewSource.OnDataChanged += AddTimeTableDataChanged;
        }

        //todo:move to viewmodel
        private IEnumerable<TimeEntry> GetTimeEntriesForSelectedDate()
        {
            return (_curTimesheet == null)
                ? Enumerable.Empty<TimeEntry>()
                : _curTimesheet.TimeEntries.Where(e => e.Date.Equals(Date));
        }

        private void AddTimeTableDataChanged(IEnumerable<TimeEntry> timeEntries)
        {
            _curTimesheet.TimeEntries = _curTimesheet.TimeEntries.Where(e => !e.Date.Equals(Date)).Concat(timeEntries);
            SetHeaderHours();
        }

        private static string TimeEntriesHoursTotal(IEnumerable<TimeEntry> timeEntries)
        {
            return timeEntries.Sum(t => t.Hours).ToString(CultureInfo.InvariantCulture);
        }

        //todo:move to viewmodel
        private string TotalHoursForCurrentDate()
        {
            var currentDateTimeEntries = GetTimeEntriesForSelectedDate();
            return TimeEntriesHoursTotal(currentDateTimeEntries);
        }

        private void RegisterCellsForReuse()
        {
            if (tableview == null) return;
            tableview.RegisterClassForCellReuse(typeof (TimeEntryCell), "TimeEntryCell");
            tableview.RegisterClassForCellReuse(typeof (AddProjectCodeCell), "AddProjectCodeCell");
        }

        private void ReloadTableViewData()
        {
            if (tableview == null) return;
            tableview.ReloadData();
        }

        private void EnableTableView(bool enabled)
        {
            if (tableview == null) return;

            tableview.UserInteractionEnabled = enabled;
            tableview.ReloadData();
        }

        private void EnableTableViewSource(bool enabled)
        {
            if (_addTimeTableViewSource == null) return;

            _addTimeTableViewSource.Enable(enabled);
        }

        private void EnabledAddButton(bool enabled)
        {
            if (addButton != null)
                addButton.Enabled = enabled;
        }

        private void EnableSaveButton(bool enabled)
        {
            if (saveButton == null)
                return;
            
            saveButton.Enabled = enabled;
            saveButton.Hidden = !enabled;
        }

        private void SetTimesheetEditability()
        {
            var enabled = TimesheetEditable();

            EnabledAddButton(enabled);
            EnableSaveButton(enabled);
            EnableTableView(enabled);
            EnableTableViewSource(enabled);
        }

        private void SetHeaderHours()
        {
            if (_curTimesheet == null) return;

            if (headerHoursLabel != null)
            {
                headerHoursLabel.Text = TotalHoursForCurrentDate();
            }
        }

        //if the timesheet changes this should be called
        public void UpdateUI()
        {
            SetHeaderDate();
            SetHeaderHours();
            SetupTableViewSource();
            SetTimesheetEditability();
            ReloadTableViewData();
        }

        private bool TimesheetEditable()
        {
            if (_curTimesheet == null) return false;

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

        private bool DateIsContainedWithinTimesheet(DateTime date)
        {
            return date >= _curTimesheet.StartDate && date <= _curTimesheet.EndDate;
        }

        private void SetCurrentDate(DateTime desiredDate)
        {
            if (DateIsContainedWithinTimesheet(desiredDate))
                SetDate(desiredDate);
        }

        private void ToggleDateNavigation()
        {
            rightArrowButton.Hidden = !DateIsContainedWithinTimesheet(Date.AddDays(1));
            leftArrowButton.Hidden = !DateIsContainedWithinTimesheet(Date.AddDays(-1));
        }

        private void NavigateDay(object sender, EventArgs e)
        {
			if (_addTimeTableViewSource != null)
				_addTimeTableViewSource.saveOpenExpandedCells ( tableview );

            var desiredDate = sender == leftArrowButton
                ? Date.AddDays(-1)
                : Date.AddDays(1);

            SetCurrentDate(desiredDate);
            ToggleDateNavigation();

            UpdateUI();
        }

        private void SetupDayNavigationButton(UIButton button, UIImage image)
        {
            SetButtonImage(button, image);
            button.TouchUpInside += NavigateDay;
        }

        private void SetupDayNavigationButtons()
        {
            SetupDayNavigationButton(leftArrowButton, new UIImage("leftArrow.png"));
            SetupDayNavigationButton(rightArrowButton, new UIImage("rightArrow.png"));

            if (Date.CompareTo(_curTimesheet.StartDate) == 0)
                leftArrowButton.Hidden = true;
            else if (Date.CompareTo(_curTimesheet.EndDate) == 0)
                rightArrowButton.Hidden = true;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

			headerContainer.BackgroundColor = UIColor.White;// StyleGuideConstants.LightGrayUiColor;

            LoadPayRates();

            SetupDayNavigationButtons();

            addButton.TouchUpInside += delegate
            {
                var newEntry = new TimeEntry
                {
                    Date = Date,
                    ProjectCode = "Project Code",
                    Hours = 8,
                    PayRate = new PayRate
                    {
                        RateDescription = "Pay Rate"
                    }
                };

                IEnumerable<TimeEntry> newEnumerableEntry = new List<TimeEntry> {newEntry};

                _curTimesheet.TimeEntries = _curTimesheet.TimeEntries.Concat(newEnumerableEntry);
                _addTimeTableViewSource.TimeEntries = _curTimesheet.TimeEntries.Where(e => e.Date.Equals(Date));

                _addTimeTableViewSource.HandleNewCell();

                tableview.ReloadData();

				_addTimeTableViewSource.scrollToExpandedCell(tableview);
            };
            addButton.TintColor = StyleGuideConstants.RedUiColor;

			savingLabel.Alpha = 0;
			savingIndicator.Alpha = 0;
			savedLabel.Alpha = 0;
			emptySaveButton.Alpha = 0;

			saveButton.TouchUpInside += async delegate
			{
				bool saveFailed = false;

				savedLabel.Text = "Saved";

                TransitionToSavingAnimation();

				try{
                    _curTimesheet = await _timesheetModel.SaveTimesheet( _curTimesheet );

					if( _curTimesheet == null )
						saveFailed = true;
				}
				catch
				{
					saveFailed = true;	
				}

                if (saveFailed)
                {
                    savedLabel.Text = "Error";

                    UIAlertView confirmationAlertView = new UIAlertView("Failed to save changes", "", null, "Ok");
                    confirmationAlertView.Show();
                }
                else
                {
                    TimeDelegate.setTimesheet(_curTimesheet);
                }

                BeginTransitionToSavedAnimation();
			};

            tableview.ContentInset = new UIEdgeInsets(-35, 0, 0, 0);

            UpdateUI();
            CreateCustomTitleBar();
        }

        private void TransitionToSavingAnimation()
        {
            StartSavingAnimation();
            //UIView.Animate(0.7f, 0, UIViewAnimationOptions.TransitionNone, StartSavingAnimation, null);
        }

        private void BeginTransitionToSavedAnimation()
        {
            UIView.Animate(0.5f, 0f, UIViewAnimationOptions.TransitionNone, HideSavingIndicator, SavingComplete);
        }

        private void SavingComplete()
        {
			savingIndicator.StopAnimating();

            FadeInSavedLabel();
        }

        private void FadeInSavedLabel()
        {
            UIView.Animate(0.5f, 0, UIViewAnimationOptions.TransitionNone, ShowSavedLabel, FadeOutSavedLabel);
        }

        private void FadeOutSavedLabel()
        {
            UIView.Animate(0.5f, 0.5f, UIViewAnimationOptions.TransitionNone, HideSavedLabel, FadeInSaveButton);
        }

        private void FadeInSaveButton()
        {
            UIView.Animate(0.5f, 0, UIViewAnimationOptions.TransitionNone, ShowSaveButton, null);
        }

        private void ShowSaveButton()
        {
            saveButton.Alpha = 1;
			emptySaveButton.Alpha = 0;
        }

        private void HideSavedLabel()
        {
            savedLabel.Alpha = 0;
        }

        private void ShowSavedLabel()
        {
            savingLabel.Alpha = 0;
			savedLabel.Alpha = 1;
        }

		private void HideSavingIndicator()
        {
            savingIndicator.Alpha = 0;
            savingLabel.Alpha = 0;
        }

        private void StartSavingAnimation()
        {
            saveButton.Alpha = 0;
			emptySaveButton.Alpha = 1;
            savingLabel.Alpha = 1;
            savingIndicator.Alpha = 1;
            savingIndicator.StartAnimating();
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

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);

			if (_addTimeTableViewSource != null)
				_addTimeTableViewSource.saveOpenExpandedCells ( tableview );
		}
    }
}