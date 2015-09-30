using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using ConsultantApp.Core.ViewModels;
using CoreGraphics;
using Foundation;
using Microsoft.Practices.Unity;
using Shared.Core;
using SiSystems.SharedModels;
using UIKit;

namespace ConsultantApp.iOS.TimeEntryViewController
{
    public class AddTimeDelegate : UIViewController
    {
        public delegate void TimeDelegate(Timesheet timesheet);
        public TimeDelegate setTimesheet;
    }

	public partial class TimesheetOverviewViewController : UIViewController
	{
	    private readonly TimesheetViewModel _timesheetModel;
		private Timesheet _curTimesheet;
		private FMCalendar _calendar;
		private SubtitleHeaderView _subtitleHeaderView;
		private UIPickerView _approverPicker;
		private PickerViewModel _approverPickerModel;
		IEnumerable<string> _approvers;
		private const string ScreenTitle = "Timesheet Overview";
	    private const int MaxFrequentlyUsed = 5;

	    public TimesheetOverviewViewController (IntPtr handle) : base (handle)
		{
			_timesheetModel = DependencyResolver.Current.Resolve<TimesheetViewModel>();
		}
        
        public void SetTimesheet(Timesheet timesheet)
        {
            _curTimesheet = timesheet;

			UpdateUI ();

        }

		public void UpdateUI()
		{
			if (_curTimesheet != null) 
			{
				var curMonthDate = _curTimesheet.StartDate;

				if (calendarDateLabel != null)
					calendarDateLabel.Text = curMonthDate.ToString ("MMMMM yyyy");

				if (totalHoursLabel != null)
					totalHoursLabel.Text = _curTimesheet.TimeEntries.Sum (t => t.Hours).ToString ();

				if( submitMonthLabel != null )
					submitMonthLabel.Text = curMonthDate.ToString("MMM").ToUpper();

				if (submitDayYearLabel != null) 
					submitDayYearLabel.Text = _curTimesheet.StartDate.ToString("dd").TrimStart('0') + "-" + _curTimesheet.EndDate.ToString("dd").TrimStart('0') + ", " + curMonthDate.ToString ("yyyy");

			    if (approverNameTextField != null)
			    {
			        approverNameTextField.Text = _curTimesheet.TimesheetApprover.Email;
			    }

				if (_approverPickerModel == null && _approverPicker != null) 
				{
					_approverPickerModel = new PickerViewModel ();
					_approverPicker.Model = _approverPickerModel;
				}
				if (_approverPickerModel != null && _approvers != null ) 
				{
					var mostFrequentlyUsed = ActiveTimesheetViewModel.TopFrequentEntries( ActiveTimesheetViewModel.ApproverDict, MaxFrequentlyUsed);
				    var frequentlyUsed = mostFrequentlyUsed as IList<string> ?? mostFrequentlyUsed.ToList();
                    var partialApprovers = _approvers.Except(frequentlyUsed).ToList();
				    partialApprovers.Sort();

                    _approvers = frequentlyUsed.Concat(partialApprovers).ToList();

				    _approverPickerModel.items = new List<List<string>>
				    {
				        _approvers.ToList()
				    };
				    _approverPickerModel.numFrequentItems[0] = frequentlyUsed.Count;

					LoadSelectedPickerItem ( _curTimesheet.TimesheetApprover.Email, _approvers.ToList(), 0 );
				}
				if( _approverPicker != null )
					_approverPicker.Model = _approverPickerModel;

				if( calendarContainerView != null )
					SetupCalendar ();

				if (submitButton != null && approverNameTextField != null) 
				{
					//if timesheet is submitted we change the name of submit to Withdraw
					if (_curTimesheet.Status == MatchGuideConstants.TimesheetStatus.Submitted) {
						submitButton.SetTitle ("Withdraw", UIControlState.Normal);
						submitButton.Enabled = true;
					} else if (approvedStatus(_curTimesheet.Status)) {
						submitButton.Hidden = true;
					}

					if (_curTimesheet.Status != MatchGuideConstants.TimesheetStatus.Open)
						approverNameTextField.Enabled = false;
					else {
						approverNameTextField.Enabled = true;

						submitButton.SetTitle ("Submit", UIControlState.Normal);
						submitButton.Enabled = true;
					}

					if ( !approvedStatus(_curTimesheet.Status) )
						submitButton.Hidden = false;
				}
			}

			View.SetNeedsLayout ();
		}

		public bool approvedStatus( MatchGuideConstants.TimesheetStatus status )
		{
		    return status == MatchGuideConstants.TimesheetStatus.Approved || 
		           status == MatchGuideConstants.TimesheetStatus.Batched ||
		           status == MatchGuideConstants.TimesheetStatus.Moved ||
		           status == MatchGuideConstants.TimesheetStatus.Accepted;
		}

	    public override void ViewWillAppear(bool animated)
	    {
			base.ViewWillAppear (animated);

            //SetupCalendar();
            UpdateUI();
	    }

	    private void SetupCalendar()
	    {
			if (_calendar != null) 
			{
				DateTime selectedDate = _calendar.SelectedDate;
				_calendar = new FMCalendar (_calendar.Bounds, new CGRect (), _curTimesheet);
				_calendar.SelectedDate = selectedDate;
			}
			else
				_calendar = new FMCalendar(calendarContainerView.Bounds, new CGRect(), _curTimesheet);
			
			_calendar.SundayFirst = true;
            calendarContainerView.AddSubview(_calendar);

            _calendar.DateSelected = delegate(DateTime date)
            {
                AddTimeViewController vc = (AddTimeViewController)Storyboard.InstantiateViewController("AddTimeViewController");
                vc.SetDate(date);
                vc.SetTimesheet(_curTimesheet);

                AddTimeDelegate addTimeDelegate = new AddTimeDelegate();
                addTimeDelegate.setTimesheet = delegate(Timesheet timesheet) { _curTimesheet = timesheet; };
                vc.TimeDelegate = addTimeDelegate;

                NavigationController.PushViewController(vc, true);
            };

            calendarLeftButton.SetTitle("", UIControlState.Normal);
            calendarLeftButton.SetTitleColor(UIColor.Black, UIControlState.Normal);
            calendarLeftButton.SetImage(new UIImage("leftArrow.png"), UIControlState.Normal);


            calendarRightButton.SetTitle("", UIControlState.Normal);
            calendarRightButton.SetTitleColor(UIColor.Black, UIControlState.Normal);
            calendarRightButton.SetImage(new UIImage("rightArrow.png"), UIControlState.Normal);
	    }

        public async void LoadTimesheetApprovers()
        {
            if(_curTimesheet == null) return;

             _approvers = await _timesheetModel.GetTimesheetApproversByTimesheetId(_curTimesheet.Id) ?? new List<string>
             {
                 _curTimesheet.TimesheetApprover.Email
             };

			UpdateUI ();
        }

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

            LoadTimesheetApprovers();

			EdgesForExtendedLayout = UIRectEdge.Bottom;

			addButton.TouchUpInside += delegate {
				
				UIViewController vc = Storyboard.InstantiateViewController("AddTimeEntryViewController");
                
				NavigationController.PushViewController(vc, true);
			};

			submitContainerView.BackgroundColor = UIColor.White;// StyleGuideConstants.LightGrayUiColor;
			approverContainerView.BackgroundColor = UIColor.White;//StyleGuideConstants.LightGrayUiColor;
			calendarHeaderView.BackgroundColor = StyleGuideConstants.LightGrayUiColor;

			approverContainerView.Layer.ShadowColor = UIColor.Black.CGColor;
			approverContainerView.Layer.ShadowOffset = new CGSize(1, 1);
			approverContainerView.Layer.ShadowOpacity = 0.2f;
			approverContainerView.Layer.ShadowRadius = 0.1f;


			DateTime curMonthDate;

			if (_curTimesheet != null )
				curMonthDate = _curTimesheet.StartDate;
			else
				curMonthDate = DateTime.Now;
			
			calendarDateLabel.Text = curMonthDate.ToString("MMMMM yyyy");

			calendarLeftButton.TouchUpInside += delegate 
			{
				_calendar.MoveCalendarMonths(false, true);
				curMonthDate = curMonthDate.AddMonths(-1);
				calendarDateLabel.Text = curMonthDate.ToString("MMMMM yyyy");
			};
			calendarRightButton.TouchUpInside += delegate 
			{ 
				_calendar.MoveCalendarMonths(true, true);
				curMonthDate = curMonthDate.AddMonths(1);
				calendarDateLabel.Text = curMonthDate.ToString("MMMMM yyyy");
			};

			calendarLeftButton.Hidden = true;
			calendarRightButton.Hidden = true;

			submitContainerView.Layer.BorderWidth = 0.5f;
			submitContainerView.Layer.BorderColor = StyleGuideConstants.LightGrayUiColor.CGColor;

		    this.Title = string.Format(_curTimesheet.ClientName);

			//add picker in keyboard when approverTextField tapped
			_approverPicker = new UIPickerView ();
			_approverPicker.BackgroundColor = UIColor.White;
			_approverPickerModel = new PickerViewModel ();
			_approverPickerModel.items = new List<List<string>> ();
			_approverPicker.Model = _approverPickerModel;
			approverNameTextField.InputView = _approverPicker;
			_approverPicker.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
			approverNameTextField.TintColor = UIColor.Clear;//gets rid of cursor in iOS 7+

			//add toolbar with done button on top of picker
			var toolbar = new UIToolbar(new CGRect(0.0f, 0.0f, View.Frame.Size.Width, 44.0f));

			toolbar.Items = new[]
			{
				new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
				new UIBarButtonItem(UIBarButtonSystemItem.Done, doneButtonTapped)
			};

			approverNameTextField.InputAccessoryView = toolbar;

			submittingLabel.Alpha = 0;
			submittingIndicator.Alpha = 0;
			submittedLabel.Alpha = 0;
			emptySubmitButton.Alpha = 0;

            submitButton.TouchUpInside += OpenActionSheet;

			UpdateUI();

			CreateCustomTitleBar();
		}

        private string SubmitTimesheetSheetTitle()
        {
            var allZeros = _curTimesheet.TimeEntries.Sum(t => t.Hours) == 0;
            var sheetTitle = allZeros
                ? "Are you sure you want to submit a timesheet with zero entries?"
                : "Are you sure you want to submit this timesheet?";
            return sheetTitle;
        }

	    private void OpenSubmitActionSheet()
	    {
            OpenActionSheet(
                sheetTitle: SubmitTimesheetSheetTitle(),
                buttonText: "Submit",
                buttonAction: SubmitActionSheetClicked);
	    }

	    private void OpenWithdrawActionSheet()
	    {
            OpenActionSheet(
                sheetTitle:"Are you sure you want to withdraw this timesheet?",
                buttonText:"Withdraw",
                buttonAction:WithdrawActionSheetClicked);
        }

	    private void OpenActionSheet(string sheetTitle, string buttonText, EventHandler<UIButtonEventArgs> buttonAction)
	    {
            var sheet = new UIActionSheet
            {
                Title = sheetTitle,
                DestructiveButtonIndex = 0,
                CancelButtonIndex = 1
            };

            sheet.AddButton(buttonText);
            sheet.Clicked += buttonAction;

            sheet.AddButton("Cancel");
            sheet.ShowFromTabBar(NavigationController.TabBarController.TabBar);

            UpdateUI();
	    }

	    private void OpenActionSheet(object sender, EventArgs e)
	    {
	        if (_curTimesheet.Status == MatchGuideConstants.TimesheetStatus.Open)
	            OpenSubmitActionSheet();
            else if (_curTimesheet.Status == MatchGuideConstants.TimesheetStatus.Submitted)
                OpenWithdrawActionSheet();
	    }

	    private void SubmitActionSheetClicked(object sender, UIButtonEventArgs args)
	    {
	        if (UserClickedCancel(args))
                return;

            ContinueWithSubmission();
	    }

        private void WithdrawActionSheetClicked(object sender, UIButtonEventArgs args)
        {
            if (UserClickedCancel(args))
                return;

            Withdraw();
        }

	    private async void ContinueWithSubmission()
	    {
            SetSubmittingText();

            TransitionToAnimationStart();

            await SubmitTimesheet();

            BeginTransitionToAnimationComplete();

            UpdateUI();
	    }
        private async void Withdraw()
        {
            SetWithdrawText();

            TransitionToAnimationStart();

            await WithdrawTimesheet();

            BeginTransitionToAnimationComplete();

            UpdateUI();
        }

	    private void SetWithdrawText()
	    {
            submittingLabel.Text = "Withdrawing";
            submittedLabel.Text = "Withdrawn";
	    }

	    private void SetSubmittingText()
	    {
            submittingLabel.Text = "Submitting";
            submittedLabel.Text = "Submitted";
	    }

	    private async Task SubmitTimesheet()
	    {
            if (_curTimesheet.Status != MatchGuideConstants.TimesheetStatus.Open) return;

	        _curTimesheet = await _timesheetModel.SubmitTimesheet(_curTimesheet);
            
            var success = _curTimesheet != null;
            DisplayAlert(success);
	    }

        private async Task WithdrawTimesheet()
        {
            _curTimesheet = await _timesheetModel.WithdrawTimesheet(_curTimesheet);

            var success = _curTimesheet != null;
            DisplayAlert(success);
        }

	    private static bool UserClickedCancel(UIButtonEventArgs args)
	    {
	        return args.ButtonIndex == 1;
	    }

	    private void DisplayAlert(bool success)
	    {
	        var alertText = AlertText(success);

            new UIAlertView(alertText, "", null, "Ok").Show();
	    }

	    private string AlertText(bool submitOrWithdrawSucceeded)
	    {
	        return submitOrWithdrawSucceeded
	            ? SuccessAlertText()
	            : FailureAlertText();
	    }

	    private string FailureAlertText()
	    {
	        return _curTimesheet.Status == MatchGuideConstants.TimesheetStatus.Submitted 
                ? "Failed to withdraw the timesheet" 
                : "Failed to submit the timesheet";
	    }

	    private string SuccessAlertText()
	    {
            return _curTimesheet.Status == MatchGuideConstants.TimesheetStatus.Submitted 
                ? "Successfully submitted timesheet" 
                : "Successfully withdrew timesheet";
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
	
		public void doneButtonTapped(object sender, EventArgs args)
		{
			string selectedApprover = _approvers.ElementAt (_approverPickerModel.selectedItemIndex[0]);
			approverNameTextField.Text = selectedApprover;

			//save approver
			_curTimesheet.TimesheetApprover.Email = selectedApprover;

			approverNameTextField.ResignFirstResponder ();

			if( !ActiveTimesheetViewModel.ApproverDict.Keys.Contains(selectedApprover) )
				ActiveTimesheetViewModel.ApproverDict.Add(selectedApprover, 1);
			else
				ActiveTimesheetViewModel.ApproverDict[selectedApprover] ++;
			

			UpdateUI ();
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





		//#region submit animation
		private void TransitionToAnimationStart()
		{
			StartSubmittingAnimation();
			//UIView.Animate(0.7f, 0, UIViewAnimationOptions.TransitionNone, StartsubmittingAnimation, null);
		}

		private void BeginTransitionToAnimationComplete()
		{
			UIView.Animate(0.5f, 0.3f, UIViewAnimationOptions.TransitionNone, HideSubmittingIndicator, SubmittingComplete);
		}

		private void SubmittingComplete()
		{
			submittingIndicator.StopAnimating();

			FadeInSubmittedLabel();
		}

		private void FadeInSubmittedLabel()
		{
			UIView.Animate(0.5f, 0, UIViewAnimationOptions.TransitionNone, ShowSubmittedLabel, FadeOutSubmittedLabel);
		}

		private void FadeOutSubmittedLabel()
		{
			UIView.Animate(0.5f, 0.5f, UIViewAnimationOptions.TransitionNone, HideSubmittedLabel, FadeInSubmitButton);
		}

		private void FadeInSubmitButton()
		{
			UIView.Animate(0.5f, 0, UIViewAnimationOptions.TransitionNone, ShowSubmitButton, null);
		}

		private void ShowSubmitButton()
		{
			submitButton.Alpha = 1;
			emptySubmitButton.Alpha = 0;
		}

		private void HideSubmittedLabel()
		{
			submittedLabel.Alpha = 0;
		}

		private void ShowSubmittedLabel()
		{
			submittingLabel.Alpha = 0;
			submittedLabel.Alpha = 1;
		}

		private void HideSubmittingIndicator()
		{
			submittingIndicator.Alpha = 0;
			submittingLabel.Alpha = 0;
		}

		private void StartSubmittingAnimation()
		{
			submitButton.Alpha = 0;
			emptySubmitButton.Alpha = 1;
			submittingLabel.Alpha = 1;
			submittingIndicator.Alpha = 1;
			submittingIndicator.StartAnimating();
		}
		//#endregion

		//find the index of the input item in the item list (if it exists). Then scroll to that item
		private void LoadSelectedPickerItem( string item, List<string> itemList, int component )
		{
			var itemIndex = 0;

			if (!string.IsNullOrEmpty(item))
				itemIndex = itemList.FindIndex (code => item == code);

			if( itemIndex < 0 )
				itemIndex = 0;

			ScrollToItemInPicker ( itemIndex, component );
		}

		private void ScrollToItemInPicker( int itemIndex, int component  )
		{
            _approverPickerModel.scrollToItemIndex(_approverPicker, itemIndex, component);
            _approverPicker.ReloadAllComponents();
		}

	}
}
