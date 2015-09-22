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
        private const string ScreenTitle = "Add/Edit Time";
        private readonly TimesheetViewModel _timesheetModel;
        private AddTimeTableViewSource _addTimeTableViewSource;
        private Timesheet _curTimesheet;
        private IEnumerable<string> _payRates;
        private SubtitleHeaderView _subtitleHeaderView;
        public DateTime Date;

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

			IEnumerable<PayRate> ratesList = await _timesheetModel.GetPayRates();
            List<String> combinedRatesList = new List<String>();
             
			foreach (PayRate payRate in ratesList)
            	combinedRatesList.Add(
					string.Format("{0} ({1:C})", payRate.RateDescription, payRate.Rate)
					//payRate.RateDescription + "-" + payRate.Rate.ToString() 
				);

            _payRates = combinedRatesList;
            
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
            _addTimeTableViewSource = new AddTimeTableViewSource(GetTimeEntriesForSelectedDate(),
                _timesheetModel.GetProjectCodes().Result, _payRates);

            _addTimeTableViewSource.OnDataChanged += AddTimeTableDataChanged;
        }

        //todo:move to viewmodel
        private IEnumerable<TimeEntry> GetTimeEntriesForSelectedDate()
        {
            return (_curTimesheet == null)
                ? (IEnumerable<TimeEntry>) new List<Timesheet>()
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

        private void SetTimesheetEditability()
        {
            var enabled = TimesheetEditable();

            EnabledAddButton(enabled);
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
            SetTimesheetEditability();
            SetHeaderHours();
            SetupTableViewSource();
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
                    PayRate = "Pay Rate"
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

			saveButton.TouchUpInside += delegate {

				_timesheetModel.SaveTimesheet( _curTimesheet );

				TransitionToSavingAnimation();
			};

            tableview.ContentInset = new UIEdgeInsets(-35, 0, 0, 0);

            UpdateUI();
            CreateCustomTitleBar();
        }

        private void TransitionToSavingAnimation()
        {
            UIView.Animate(0.7f, 0, UIViewAnimationOptions.TransitionNone, StartSavingAnimation, BeginTransitionToSavedAnimation);
        }

        //todo: call when we recieve the save confirmation from the server
        private void BeginTransitionToSavedAnimation()
        {
            UIView.Animate(0.5f, 0.7f, UIViewAnimationOptions.TransitionNone, HideSavingIndicator, SavingComplete);
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
    }
}