using System;
using Foundation;
using UIKit;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using ConsultantApp.Core.ViewModels;
using Microsoft.Practices.Unity;
using Shared.Core;
using SiSystems.SharedModels;
using System.Threading.Tasks;
using Shared.Core;

namespace ConsultantApp.iOS.TimeEntryViewController
{
	public partial class TimesheetOverviewViewController : UIViewController
	{
	    private readonly TimesheetViewModel _timesheetModel;
		private readonly ActiveTimesheetViewModel _activeTimesheetModel;
		private Timesheet _curTimesheet;
		private FMCalendar calendar;
		private SubtitleHeaderView subtitleHeaderView;
		private UIPickerView approverPicker;
		private PickerViewModel approverPickerModel;
		IEnumerable<string> _approvers;
		private const string ScreenTitle = "Timesheet Overview";
		private int maxFrequentlyUsed = 2;

		public TimesheetOverviewViewController (IntPtr handle) : base (handle)
		{
			//TabBarController.TabBar.TintColor = UIColor.Blue;

            //TabBarController.TabBar.Items [0].Image = new UIImage ("ios7-clock-outline.png");
            //TabBarController.TabBar.Items [1].Image = new UIImage ("social-usd.png");

			_timesheetModel = DependencyResolver.Current.Resolve<TimesheetViewModel>();
			_activeTimesheetModel = DependencyResolver.Current.Resolve<ActiveTimesheetViewModel>();
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
					List<string> approverList = _approvers.ToList ();

					approverList.Sort ();

					if (approverList.Count < maxFrequentlyUsed)
						approverList.Sort (new Comparison<string> ((string pc1, string pc2) => {
							if (!ActiveTimesheetViewModel.approverDict.ContainsKey (pc1) || ActiveTimesheetViewModel.approverDict.ContainsKey (pc2) && ActiveTimesheetViewModel.approverDict [pc2] >= ActiveTimesheetViewModel.approverDict [pc1])
								return 1;
							else if (!ActiveTimesheetViewModel.approverDict.ContainsKey (pc2) || ActiveTimesheetViewModel.approverDict.ContainsKey (pc1) && ActiveTimesheetViewModel.approverDict [pc1] >= ActiveTimesheetViewModel.approverDict [pc2])
								return -1;
							return 0;
						}));
					else 
					{
						int highest = 0, highestIndex = 0;
						//can make this linear time if need be.
						for (int i = 0; i < maxFrequentlyUsed; i++) 
						{
							if (!ActiveTimesheetViewModel.approverDict.ContainsKey (approverList [i])) {
								highest = -1;
								highestIndex = -1;
							} else 
							{
								highest = ActiveTimesheetViewModel.approverDict [approverList [i]];
								highestIndex = i;
							}

							for (int j = i+1; j < approverList.Count; j++) 
							{
								if (ActiveTimesheetViewModel.approverDict.ContainsKey (approverList [j]) && ActiveTimesheetViewModel.approverDict [approverList [j]] > highest) 
								{
									highest = ActiveTimesheetViewModel.approverDict [approverList [j]];
									highestIndex = j;
								}
							}

							if (highestIndex > -1) 
							{
								string temp = approverList [i];
								approverList [i] = approverList [highestIndex];
								approverList [highestIndex] = temp;
							}
						}
					}

					//find out how many frequently used items there are, and if it is higher than our limit
					int numFrequentItems = 0;
					for (int i = 0; i < approverList.Count; i++)
						if (ActiveTimesheetViewModel.approverDict.ContainsKey (approverList [i]))
							numFrequentItems++;

					if (numFrequentItems > maxFrequentlyUsed)
						numFrequentItems = maxFrequentlyUsed;
					
					_approvers = approverList;

					approverPickerModel.items = new List<List<string>> ();
					approverPickerModel.items.Add (approverList);
					approverPickerModel.numFrequentItems[0] = numFrequentItems;
				}
				if( approverPicker != null )
					approverPicker.Model = approverPickerModel;

				/*
				if (subtitleHeaderView != null) 
				{
					subtitleHeaderView.TitleText = "Timesheet Overview";
					subtitleHeaderView.SubtitleText = _curTimesheet.ClientName;
				}
*/
				if( calendarContainerView != null )
					SetupCalendar ();

				if (submitButton != null && approverNameTextField != null) 
				{
					//if timesheet is submitted we change the name of submit to Withdraw
					if (_curTimesheet.Status == MatchGuideConstants.TimesheetStatus.Submitted) {
						submitButton.SetTitle ("Withdraw", UIControlState.Normal);
						submitButton.Enabled = true;
					} else if (_curTimesheet.Status == MatchGuideConstants.TimesheetStatus.Approved) {
						submitButton.Hidden = true;
					}

					if (_curTimesheet.Status != MatchGuideConstants.TimesheetStatus.Open)
						approverNameTextField.Enabled = false;
					else {
						approverNameTextField.Enabled = true;

						submitButton.SetTitle ("Submit", UIControlState.Normal);
						submitButton.Enabled = true;
					}

					if (_curTimesheet.Status != MatchGuideConstants.TimesheetStatus.Approved)
						submitButton.Hidden = false;
				}
			}

			View.SetNeedsLayout ();
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
				calendar = new FMCalendar (calendar.Bounds, new CoreGraphics.CGRect (), _curTimesheet);
				calendar.SelectedDate = selectedDate;
			}
			else
				calendar = new FMCalendar(calendarContainerView.Bounds, new CoreGraphics.CGRect(), _curTimesheet);
			
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
			/*
			if (_approvers != null) 
			{
				if( approverPickerModel.items.Count > 0 )
					approverPickerModel.items [0] = _approvers.ToList ();
				else
					approverPickerModel.items.Add( _approvers.ToList () );
			}*/

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
			approverContainerView.Layer.ShadowOffset = new CoreGraphics.CGSize(1, 1);
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

			//add toolbar with done button on top of picker
			var toolbar = new UIToolbar(new CoreGraphics.CGRect(0.0f, 0.0f, View.Frame.Size.Width, 44.0f));

			toolbar.Items = new[]
			{
				new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
				new UIBarButtonItem(UIBarButtonSystemItem.Done, doneButtonTapped)
			};

			approverNameTextField.InputAccessoryView = toolbar;

			submitButton.TouchUpInside += delegate(object sender, EventArgs e) {

				if( _curTimesheet.Status == MatchGuideConstants.TimesheetStatus.Open )
				{
					bool allZeros = _curTimesheet.TimeEntries.Sum(t => t.Hours) == 0;
					bool permissionToSubmit = true;

					if( allZeros )
					{
						var sheet = new UIActionSheet();
						sheet.Title = "Are you sure you want to submit a timesheet with zero entries?";
						sheet.AddButton("Submit");
						sheet.AddButton("Cancel");
						sheet.DestructiveButtonIndex = 0;
						sheet.CancelButtonIndex = 1;
						sheet.Clicked += delegate (object sender2, UIButtonEventArgs args)
						{
							//if logout button tapped
							if (args.ButtonIndex == 1)
								permissionToSubmit = false;

							if( permissionToSubmit )
							{
								_curTimesheet.Status = MatchGuideConstants.TimesheetStatus.Submitted;
								updateUI();
								var view = new UIAlertView("Successfully submitted timesheet", "", null, "Ok");
								view.Show();
							}
						};
						sheet.ShowFromTabBar(NavigationController.TabBarController.TabBar);// ShowInView(View);
					}
					//if( permissionToSubmit )
					else
					{
						_curTimesheet.Status = MatchGuideConstants.TimesheetStatus.Submitted;
						var view = new UIAlertView("Successfully submitted timesheet", "", null, "Ok");
						view.Show();
					}
				}
				else if( _curTimesheet.Status == MatchGuideConstants.TimesheetStatus.Submitted )
				{
					_curTimesheet.Status = MatchGuideConstants.TimesheetStatus.Open;
					var view = new UIAlertView("Successfully withdrew timesheet", "", null, "Ok");
					view.Show();
				}
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

			if( !ActiveTimesheetViewModel.approverDict.Keys.Contains(selectedApprover) )
				ActiveTimesheetViewModel.approverDict.Add(selectedApprover, 1);
			else
				ActiveTimesheetViewModel.approverDict[selectedApprover] ++;
			

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
	}
}
