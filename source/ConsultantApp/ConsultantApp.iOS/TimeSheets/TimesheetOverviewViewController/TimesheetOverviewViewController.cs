using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using ConsultantApp.Core.ViewModels;
using CoreGraphics;
using Foundation;
using Microsoft.Practices.Unity;
using Shared.Core;
using SiSystems.SharedModels;
using UIKit;

namespace ConsultantApp.iOS.TimeEntryViewController
{
	public partial class TimesheetOverviewViewController : UIViewController
	{
	    private readonly TimesheetViewModel _timesheetModel;
		private Timesheet _curTimesheet;
		private FMCalendar calendar;
		private SubtitleHeaderView subtitleHeaderView;
		private UIPickerView approverPicker;
		private PickerViewModel approverPickerModel;
		IEnumerable<string> _approvers;
		private const string ScreenTitle = "Timesheet Overview";
		private int maxFrequentlyUsed = 5;

		public TimesheetOverviewViewController (IntPtr handle) : base (handle)
		{
			//TabBarController.TabBar.TintColor = UIColor.Blue;

            //TabBarController.TabBar.Items [0].Image = new UIImage ("ios7-clock-outline.png");
            //TabBarController.TabBar.Items [1].Image = new UIImage ("social-usd.png");

			_timesheetModel = DependencyResolver.Current.Resolve<TimesheetViewModel>();
		}
        
        public void SetTimesheet(Timesheet timesheet)
        {
            _curTimesheet = timesheet;

			updateUI ();

        }

		//if the timesheet changes this should be called
		public void updateUI()
		{
			if (_curTimesheet != null) 
			{
				DateTime curMonthDate = _curTimesheet.StartDate;

				if (calendarDateLabel != null)
					calendarDateLabel.Text = curMonthDate.ToString ("MMMMM yyyy");

				//sum up timsheet hours
				if (totalHoursLabel != null)
					totalHoursLabel.Text = _curTimesheet.TimeEntries.Sum (t => t.Hours).ToString ();

				if( submitMonthLabel != null )
					submitMonthLabel.Text = curMonthDate.ToString("MMM").ToUpper();

				if (submitDayYearLabel != null) 
					submitDayYearLabel.Text = _curTimesheet.StartDate.ToString("dd").TrimStart('0') + "-" + _curTimesheet.EndDate.ToString("dd").TrimStart('0') + ", " + curMonthDate.ToString ("yyyy");

			    if (approverNameTextField != null)
			    {
			        approverNameTextField.Text = _curTimesheet.TimesheetApprover;
			    }

				approverPickerModel = new PickerViewModel ();

				if (approverPickerModel != null && _approvers != null ) 
				{
					var mostFrequentlyUsed = ActiveTimesheetViewModel.TopFrequentEntries( ActiveTimesheetViewModel.ApproverDict, maxFrequentlyUsed);
				    var frequentlyUsed = mostFrequentlyUsed as IList<string> ?? mostFrequentlyUsed.ToList();
                    var partialApprovers = _approvers.Except(frequentlyUsed).ToList();
				    partialApprovers.Sort();

                    _approvers = frequentlyUsed.Concat(partialApprovers).ToList();

				    approverPickerModel.items = new List<List<string>>
				    {
				        _approvers.ToList()
				    };
				    approverPickerModel.numFrequentItems[0] = frequentlyUsed.Count;
				}
				if( approverPicker != null )
					approverPicker.Model = approverPickerModel;

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
            updateUI();
	    }

	    private void SetupCalendar()
	    {
			if (calendar != null) 
			{
				DateTime selectedDate = calendar.SelectedDate;
				calendar = new FMCalendar (calendar.Bounds, new CGRect (), _curTimesheet);
				calendar.SelectedDate = selectedDate;
			}
			else
				calendar = new FMCalendar(calendarContainerView.Bounds, new CGRect(), _curTimesheet);
			
			calendar.SundayFirst = true;
            calendarContainerView.AddSubview(calendar);

            calendar.DateSelected = delegate(DateTime date)
            {
                AddTimeViewController vc = (AddTimeViewController)Storyboard.InstantiateViewController("AddTimeViewController");
                vc.SetDate(date);
                vc.SetTimesheet(_curTimesheet);
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
                 _curTimesheet.TimesheetApprover
             };

			updateUI ();
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
				calendar.MoveCalendarMonths(false, true);
				curMonthDate = curMonthDate.AddMonths(-1);
				calendarDateLabel.Text = curMonthDate.ToString("MMMMM yyyy");
			};
			calendarRightButton.TouchUpInside += delegate 
			{ 
				calendar.MoveCalendarMonths(true, true);
				curMonthDate = curMonthDate.AddMonths(1);
				calendarDateLabel.Text = curMonthDate.ToString("MMMMM yyyy");
			};

			calendarLeftButton.Hidden = true;
			calendarRightButton.Hidden = true;

			submitContainerView.Layer.BorderWidth = 0.5f;
			submitContainerView.Layer.BorderColor = StyleGuideConstants.LightGrayUiColor.CGColor;

		    this.Title = string.Format(_curTimesheet.ClientName);

			//add picker in keyboard when approverTextField tapped
			approverPicker = new UIPickerView ();
			approverPicker.BackgroundColor = UIColor.White;
			approverPickerModel = new PickerViewModel ();
			approverPickerModel.items = new List<List<string>> ();
			approverPicker.Model = approverPickerModel;
			approverNameTextField.InputView = approverPicker;
			approverPicker.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
			approverNameTextField.TintColor = UIColor.Clear;//gets rid of cursor in iOS 7+
			approverNameTextField.EditingDidBegin += delegate {approverPicker.Select (1, 0, false);};

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

			submitButton.TouchUpInside += delegate(object sender, EventArgs e) {

				var sheet = new UIActionSheet();
				sheet.Title = "Are you sure you want to submit this timesheet?";
				sheet.DestructiveButtonIndex = 0;
				sheet.CancelButtonIndex = 1;

				var confirmationAlertView = new UIAlertView("Successfully submitted timesheet", "", null, "Ok");
				bool permissionToSubmit = true;

				if( _curTimesheet.Status == MatchGuideConstants.TimesheetStatus.Open )
				{
					bool allZeros = _curTimesheet.TimeEntries.Sum(t => t.Hours) == 0;
						
					if( allZeros )
						sheet.Title = "Are you sure you want to submit a timesheet with zero entries?";

					sheet.AddButton("Submit");
				}
				else if( _curTimesheet.Status == MatchGuideConstants.TimesheetStatus.Submitted )
				{
					confirmationAlertView = new UIAlertView("Successfully withdrew timesheet", "", null, "Ok");
					sheet.Title = "Are you sure you want to withdraw this timesheet?";

					sheet.AddButton("Withdraw");
				}
					
				sheet.Clicked += async delegate (object sender2, UIButtonEventArgs args)
				{
					if (args.ButtonIndex == 1)
						permissionToSubmit = false;

					if( permissionToSubmit )
					{
						TransitionToSubmittingAnimation();

                        if( _curTimesheet.Status == MatchGuideConstants.TimesheetStatus.Open )
    					    _curTimesheet = await _timesheetModel.SubmitTimesheet(_curTimesheet);

						BeginTransitionToSubmittedAnimation();

						updateUI();
						//confirmationAlertView.Show();
					}
				};


				sheet.AddButton("Cancel");
				sheet.ShowFromTabBar(NavigationController.TabBarController.TabBar);// ShowInView(View);


				updateUI();
			};

			updateUI();

			CreateCustomTitleBar();
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
				});
		}
	
		public void doneButtonTapped(object sender, EventArgs args)
		{
			string selectedApprover = _approvers.ElementAt (approverPickerModel.selectedItemIndex[0]);
			approverNameTextField.Text = selectedApprover;

			//save approver
			_curTimesheet.TimesheetApprover = selectedApprover;

			approverNameTextField.ResignFirstResponder ();

			if( !ActiveTimesheetViewModel.ApproverDict.Keys.Contains(selectedApprover) )
				ActiveTimesheetViewModel.ApproverDict.Add(selectedApprover, 1);
			else
				ActiveTimesheetViewModel.ApproverDict[selectedApprover] ++;
			

			updateUI ();
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
		private void TransitionToSubmittingAnimation()
		{
			StartSubmittingAnimation();
			//UIView.Animate(0.7f, 0, UIViewAnimationOptions.TransitionNone, StartsubmittingAnimation, null);
		}

		private void BeginTransitionToSubmittedAnimation()
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
	}
}
