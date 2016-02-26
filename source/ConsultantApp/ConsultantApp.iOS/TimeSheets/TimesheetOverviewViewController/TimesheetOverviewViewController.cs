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
		
        private FMCalendar _calendar;
		private SubtitleHeaderView _subtitleHeaderView;
		private UIPickerView _approverPicker;
		private PickerViewModel _approverPickerModel;
		
        private const string ScreenTitle = "Timesheet Overview";

	    public TimesheetOverviewViewController (IntPtr handle) : base (handle)
		{
			_timesheetModel = DependencyResolver.Current.Resolve<TimesheetViewModel>();
		}
        
        public void LoadTimesheet(Timesheet timesheet)
        {
            _timesheetModel.SetTimesheet(timesheet);
            _timesheetModel.LoadingTimesheet.ContinueWith(_ => InvokeOnMainThread(UpdateUI));
           
        }

	    private void SetLabelText(UILabel label, string text)
	    {
	        if (label != null)
	            label.Text = text;
	    }

	    private void UpdateApproverPicker()
	    {
	        if (_approverPickerModel == null && _approverPicker != null) 
		    {
		        _approverPickerModel = new PickerViewModel ();
		        _approverPicker.Model = _approverPickerModel;
		    }
		    if (_approverPickerModel != null)
		    {
		        var approverList = _timesheetModel.ApproverEmailsSortedByFrequency();
		        _approverPickerModel.items = new List<List<string>>
		        {
		            approverList
		        };

		        _approverPickerModel.numFrequentItems[0] = _timesheetModel.NumberOfFrequentlyUsedApprovers();

                LoadSelectedPickerItem(_timesheetModel.TimesheetApproverEmail(), approverList, 0);
            }
            if (_approverPicker != null)
                _approverPicker.Model = _approverPickerModel;
	    }

		public void UpdateUI()
		{
		    if (_timesheetModel.TimesheetIsNull()) return;

		    SetLabelText(calendarDateLabel, _timesheetModel.TextForDate());
            SetLabelText(totalHoursLabel, _timesheetModel.TotalHoursText());
            SetLabelText(submitMonthLabel, _timesheetModel.MonthText());
            SetLabelText(submitDayYearLabel, _timesheetModel.TimesheetPeriodRange());
            SetLabelText(submitDayYearLabel, _timesheetModel.TimesheetPeriodRange());

		    if (approverNameTextField != null)
		        approverNameTextField.Text = _timesheetModel.TimesheetApproverEmail();

		    if( calendarContainerView != null )
		        SetupCalendar ();

		    if (submitButton != null && approverNameTextField != null) 
		    {
		        //if timesheet is submitted we change the name of submit to Withdraw
		        if (_timesheetModel.TimesheetIsSubmitted()) {
		            submitButton.SetTitle ("Withdraw", UIControlState.Normal);
		            submitButton.Enabled = true;
                }
                else if (_timesheetModel.TimesheetIsApproved())
                {
		            submitButton.Hidden = true;
					approvedLabel.Hidden = false;
		        }

		        if (_timesheetModel.TimesheetIsOpen())
		        {
                    approverNameTextField.Enabled = true;

                    submitButton.SetTitle("Submit", UIControlState.Normal);
                    submitButton.Enabled = true;
		        }
		        else {
                    approverNameTextField.Enabled = false;   
		        }

				if (!_timesheetModel.TimesheetIsApproved ()) 
				{
					submitButton.Hidden = false;
					approvedLabel.Hidden = true;
				}
		    }

		    View.SetNeedsLayout();
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
				_calendar = new FMCalendar (_calendar.Bounds, new CGRect (), _timesheetModel.StartDate(), _timesheetModel.EndDate(), _timesheetModel.TimeSheetEntries());
				_calendar.SelectedDate = selectedDate;
			}
			else
                _calendar = new FMCalendar(calendarContainerView.Bounds, new CGRect(), _timesheetModel.StartDate(), _timesheetModel.EndDate(), _timesheetModel.TimeSheetEntries());
			
			_calendar.SundayFirst = true;
            calendarContainerView.AddSubview(_calendar);

            _calendar.DateSelected = delegate(DateTime date)
            {
                AddTimeViewController vc = (AddTimeViewController)Storyboard.InstantiateViewController("AddTimeViewController");
                vc.SetDate(date);
                vc.SetTimesheet(_timesheetModel.Timesheet);//todo: can we not expose timesheet from the timesheetviewmodel and still do this?

                AddTimeDelegate addTimeDelegate = new AddTimeDelegate();
                addTimeDelegate.setTimesheet = delegate(Timesheet timesheet) { _timesheetModel.SetTimesheet(timesheet); };
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

        private void LoadTimesheetApprovers()
        {
            var loadingApproversTask = _timesheetModel.GetTimesheetApprovers();
            loadingApproversTask.ContinueWith(_ => InvokeOnMainThread(UpdateApproverPicker));
        }

        // I don't believe this is a necessary call, and will remove this when my refactor is complete and I 
        // can confirm this.
        //private void LoadTimesheetSupport()
        //{
        //    var loadingSupportTask = _timesheetModel.LoadTimesheetSupport();
        //    //todo: is there any continue with that we need to do with this?
        //}

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

			calendarLeftButton.Hidden = true;
			calendarRightButton.Hidden = true;

			submitContainerView.Layer.BorderWidth = 0.5f;
			submitContainerView.Layer.BorderColor = StyleGuideConstants.LightGrayUiColor.CGColor;
            
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
				new UIBarButtonItem(UIBarButtonSystemItem.Done, DoneButtonTapped)
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

	    private void OpenSubmitActionSheet()
	    {
            OpenActionSheet(
                sheetTitle: _timesheetModel.SubmitTimesheetConfirmationTitle(),
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
	        if (_timesheetModel.TimesheetIsOpen())
	            OpenSubmitActionSheet();
            else if (_timesheetModel.TimesheetIsSubmitted())
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

	    private void ContinueWithSubmission()
	    {
            SetSubmittingText();

            TransitionToAnimationStart();

            SubmitTimesheet();
	    }
        private void Withdraw()
        {
            SetWithdrawText();

            TransitionToAnimationStart();

            WithdrawTimesheet();
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

	    private void SubmitTimesheet()
	    {
            _timesheetModel.SubmitTimesheet();
            _timesheetModel.SubmittingTimesheet.ContinueWith(_ => InvokeOnMainThread(SubmitOrWithrawComplete));
	    }

        private void WithdrawTimesheet()
        {
            _timesheetModel.WithdrawTimesheet();
            _timesheetModel.WithdrawingTimesheet.ContinueWith(_ => InvokeOnMainThread(SubmitOrWithrawComplete));
        }

        private void SubmitOrWithrawComplete()
        {
            DisplayAlert();
            BeginTransitionToAnimationComplete();
            UpdateUI();
        }

	    private static bool UserClickedCancel(UIButtonEventArgs args)
	    {
	        return args.ButtonIndex == 1;
	    }

	    private void DisplayAlert()
	    {
	        var alertText = _timesheetModel.GetAlertText();
            if(!string.IsNullOrEmpty(alertText))
                new UIAlertView(alertText, "", null, "Ok").Show();
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
	
		public void DoneButtonTapped(object sender, EventArgs args)
		{
            var selectedApproverEmail = _approverPickerModel.GetSelectedPickerItemValue(0);

            approverNameTextField.Text = selectedApproverEmail;
            approverNameTextField.ResignFirstResponder();

            _timesheetModel.SetTimesheetApproverByEmail(selectedApproverEmail);
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
