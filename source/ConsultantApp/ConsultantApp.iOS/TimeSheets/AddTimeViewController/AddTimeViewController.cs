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
        private TimesheetViewModel _timesheetModel;
        private AddTimeTableViewSource _addTimeTableViewSource;
        private SubtitleHeaderView _subtitleHeaderView;
        private int maxFrequentlyUsed = 5;

        public AddTimeDelegate TimeDelegate;

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
            _timesheetModel.SetTimesheet(timesheet);
            UpdateUI();
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
            
            saveButton.Enabled = enabled;
            saveButton.Hidden = !enabled;
        }

        private void SetTimesheetEditability()
        {
            var enabled = _timesheetModel.CurrentTimesheetIsEditable();

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

            if (_timesheetModel.CurrentTimesheetIsEditable())
                LoadPayRates();

            SetupDayNavigationButtons();

            addButton.TouchUpInside += delegate
            {
                //var codeRatesNotYetEntered = _timesheetModel.AvailableCodeRates();
                var entry = _timesheetModel.DefaultNewEntry();
                
                _timesheetModel.AddTimeEntry(entry);
                _addTimeTableViewSource.TimeEntries = _timesheetModel.GetSelectedDatesTimeEntries();
                _addTimeTableViewSource.CodeRateDetails = _timesheetModel.TimesheetSupport.ProjectCodeOptions;
                
                /*
                 * Note that the above might not be needed at all. Here's what I'm thinking:
                      When we save, we actually check each and every TimeEntry in our timesheet for changes. 
                      Since we're doing that anyway, whenever they hit save, we can just fire off the entire batch and
                      not try to pass it back and forth. Then, whenever we update the view, we just reload
                        (would that work?)
                 */

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
				bool saveFailed = false;

				savedLabel.Text = "Saved";

                TransitionToSavingAnimation();

				try{
                    var savingTimesheetTask = _timesheetModel.SaveTimesheet();
                    //todo: convert below to different continue with calls
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
                    TimeDelegate.setTimesheet(_timesheetModel.Timesheet);
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
				_addTimeTableViewSource.SaveOpenExpandedCells ( tableview );
		}
    }
}