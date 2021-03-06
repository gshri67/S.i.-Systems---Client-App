using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
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
        private TimesheetViewModel _timesheetModel;
        private AddTimeTableViewSource _addTimeTableViewSource;
        private SubtitleHeaderView _subtitleHeaderView;
        
        public AddTimeViewController(IntPtr handle) : base(handle)
        {
            EdgesForExtendedLayout = UIRectEdge.None;
        }

        public void SetViewModel(TimesheetViewModel timesheetModel)
        {
            _timesheetModel = timesheetModel;
        }

        public void SetTimesheet(Timesheet timesheet)
        {
            var loadingTimesheet = _timesheetModel.SetTimesheet(timesheet);
            loadingTimesheet.ContinueWith(_ => InvokeOnMainThread(UpdateUI));
        }

        public void LoadPayRates()
        {
            var loadSupportTask = _timesheetModel.LoadTimesheetSupport();
            loadSupportTask.ContinueWith(_ => InvokeOnMainThread(UpdateUI));
        }

        private void SetupTableViewSource()
        {
            if (tableview == null)
                return;

            RegisterCellsForReuse();
            InstantiateTableViewSource();
        }

        private void InstantiateTableViewSource()
        {
            _addTimeTableViewSource = new AddTimeTableViewSource(
                _timesheetModel.GetSelectedDatesTimeEntries(),
                _timesheetModel.TimesheetSupport.ProjectCodeOptions
            );

            _addTimeTableViewSource.OnDataChanged = AddTimeTableDataChanged;

            tableview.Source = _addTimeTableViewSource;
        }

        private void AddTimeTableDataChanged(IEnumerable<TimeEntry> timeEntries)
        {
            _timesheetModel.SetSelectedDatesEntries(timeEntries);

            UpdateUI();
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
            if (addButton == null)
                return;
            addButton.Enabled = enabled && _timesheetModel.SelectedDateHasUnpopulatedRateCodes();
        }

        private void EnableSaveButton(bool enabled)
        {
            if (saveButton == null)
                return;

            //note that we have to disable to button if there are no timeentries because saving a timesheet with 
            //zero hours breaks the GetOpenTimesheets Stored Procedure (the timesheet will not be returned)
            var hasTimeEntries = _timesheetModel.TimeSheetEntries().Any();

            saveButton.Enabled = enabled && hasTimeEntries;
            saveButton.Hidden = !(enabled && hasTimeEntries);
        }

        private void SetTimesheetEditability()
        {
            var enabled = _timesheetModel.TimesheetIsEditable();

            EnabledAddButton(enabled);
            EnableSaveButton(enabled);
            EnableTableView(enabled);
            EnableTableViewSource(enabled);
        }

        private void SetHeaderHours()
        {
            if (headerHoursLabel != null)
            {
                headerHoursLabel.Text = _timesheetModel.NumberOfHoursForSelectedDate().ToString(CultureInfo.InvariantCulture);
            }
        }

        //if the timesheet changes this should be called
        public void UpdateUI()
        {
            SetHeaderDate();
            SetHeaderHours();
            SetDateNavigation();
            SetupTableViewSource();
            SetTimesheetEditability();
            ReloadTableViewData();
        }

        private void SetHeaderDate()
        {
            if (headerDayOfWeekLabel != null)
                headerDayOfWeekLabel.Text = string.Format("{0:ddd}", _timesheetModel.SelectedDate);

            if (headerDateLabel != null)
                headerDateLabel.Text = string.Format("{0:MMM} {0:%d}", _timesheetModel.SelectedDate);
        }

        private void SetDateNavigation()
        {
            rightArrowButton.Hidden = !_timesheetModel.CanNavigteToNextDay();
            leftArrowButton.Hidden = !_timesheetModel.CanNavigateToPreviousDay();
        }

        private void NavigateDay(object sender, EventArgs e)
        {
			if (_addTimeTableViewSource != null)
				_addTimeTableViewSource.SaveOpenExpandedCells ( tableview );

            if (Equals(sender, leftArrowButton))
                _timesheetModel.NavigateToPreviousDay();
            else
                _timesheetModel.NavigateToNextDay();

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

            SetDateNavigation();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

			headerContainer.BackgroundColor = UIColor.White;// StyleGuideConstants.LightGrayUiColor;

            if (_timesheetModel.TimesheetIsEditable())
                LoadPayRates();

            SetupDayNavigationButtons();

            addButton.TouchUpInside += delegate
            {
                _timesheetModel.AddDefaultTimeEntry();
                _addTimeTableViewSource.TimeEntries = _timesheetModel.GetSelectedDatesTimeEntries();
                _addTimeTableViewSource.CodeRateDetails = _timesheetModel.TimesheetSupport.ProjectCodeOptions;
                
                _addTimeTableViewSource.HandleNewCell();

                tableview.ReloadData();

				_addTimeTableViewSource.ScrollToExpandedCell(tableview);
            };
            addButton.TintColor = StyleGuideConstants.RedUiColor;

			savingLabel.Alpha = 0;
			savingIndicator.Alpha = 0;
			savedLabel.Alpha = 0;
			emptySaveButton.Alpha = 0;

			saveButton.TouchUpInside += async delegate
			{
				TransitionToSavingAnimation();

				var savingTimesheetTask = _timesheetModel.SaveTimesheet();
			    savingTimesheetTask.ContinueWith(_ => InvokeOnMainThread(TimesheetSaveSuccess), TaskContinuationOptions.OnlyOnRanToCompletion);
                savingTimesheetTask.ContinueWith(_ => InvokeOnMainThread(TimesheetSaveFailed), TaskContinuationOptions.OnlyOnFaulted);
			};

            tableview.ContentInset = new UIEdgeInsets(-35, 0, 0, 0);

            UpdateUI();
            CreateCustomTitleBar();
        }

        private void TimesheetSaveSuccess()
        {
            BeginTransitionToSavedAnimation();
        }

        private void TimesheetSaveFailed()
        {
            savedLabel.Text = "Error";

            UIAlertView confirmationAlertView = new UIAlertView("Failed to save changes", "", null, "Ok");
            confirmationAlertView.Show();
        }

        private void TransitionToSavingAnimation()
        {
            StartSavingAnimation();
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
            savedLabel.Text = "Saved";

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
				_addTimeTableViewSource.SaveOpenExpandedCells ( tableview );
		}
    }
}