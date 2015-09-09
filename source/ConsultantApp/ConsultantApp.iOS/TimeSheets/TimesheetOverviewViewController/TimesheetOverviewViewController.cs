using System;
using Foundation;
using UIKit;
using System.Collections.Generic;
using System.Linq;
using ConsultantApp.Core.ViewModels;
using Microsoft.Practices.Unity;
using Shared.Core;
using SiSystems.SharedModels;
using System.Threading.Tasks;

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
					//if timesheet is submitted we change the name of submit to cancel
					if (_curTimesheet.Status == MatchGuideConstants.TimesheetStatus.Submitted) {
						submitButton.SetTitle ("Cancel", UIControlState.Normal);
						submitButton.Enabled = true;
					} else if (_curTimesheet.Status == MatchGuideConstants.TimesheetStatus.Approved) {
						submitButton.SetTitle ("Submit", UIControlState.Normal);
						submitButton.Enabled = false;
					}

					if (_curTimesheet.Status != MatchGuideConstants.TimesheetStatus.Open)
						approverNameTextField.Enabled = false;
					else {
						approverNameTextField.Enabled = true;

						submitButton.SetTitle ("Submit", UIControlState.Normal);
						submitButton.Enabled = true;
					}
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
             _approvers = await _timesheetModel.GetTimesheetApprovers(1);

            if( _approvers != null )
                approverPickerModel.items = _approvers.ToList();
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
							}
						};
						sheet.ShowFromTabBar(NavigationController.TabBarController.TabBar);// ShowInView(View);
					}
					//if( permissionToSubmit )
					else
						_curTimesheet.Status = MatchGuideConstants.TimesheetStatus.Submitted;
				}
				else if( _curTimesheet.Status == MatchGuideConstants.TimesheetStatus.Submitted )
					_curTimesheet.Status = MatchGuideConstants.TimesheetStatus.Open;

				updateUI();
			};

			updateUI();

			subtitleHeaderView = new SubtitleHeaderView ();
			NavigationItem.TitleView = subtitleHeaderView;

			subtitleHeaderView.TitleText = "Timesheet Overview";
			subtitleHeaderView.SubtitleText = "4449993 Alberta Co";

			NavigationItem.Title = "";
		}

		public void doneButtonTapped(object sender, EventArgs args)
		{
			string selectedApprover = _approvers.ElementAt (approverPickerModel.selectedItemIndex);
			approverNameTextField.Text = selectedApprover;

			//save approver
			_curTimesheet.TimesheetApprover = selectedApprover;

			approverNameTextField.ResignFirstResponder ();

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

		public class PickerViewModel : UIPickerViewModel
		{
			public delegate void pickerViewDelegate( string item );
			public pickerViewDelegate onSelected;
			public List<string> items;
			public int selectedItemIndex;

			public PickerViewModel()
			{
				selectedItemIndex = 0;
			}

			public override nint GetComponentCount (UIPickerView picker)
			{
				return 1;
			}

			public override nint GetRowsInComponent (UIPickerView picker, nint component)
			{
				if (items != null)
					return items.Count();
				else
					return 0;
			}

			public override string GetTitle (UIPickerView pickerView, nint row, nint component)
			{
				if (items == null)
					return "";
				else
					return items.ElementAt((int)row);
			}

			public override void Selected (UIPickerView pickerView, nint row, nint component)
			{
				/*
				//projectcodes should be updated for selected client
				if (pickerView == clientPickerView) 
				{
					projectCodes = clients.ElementAt ((int)row).projectCodes;
					projectCodePickerView.ReloadAllComponents ();
				}

				onSelected(pickerView, row);*/

				selectedItemIndex = (int)row;
			}

			public override UIView GetView (UIPickerView pickerView, nint row, nint component, UIView view)
			{
				UILabel lbl = new UILabel(new CoreGraphics.CGRect(0, 0, 130f, 40f));
				lbl.TextColor = UIColor.Black;
				lbl.Font = UIFont.SystemFontOfSize(12f);
				lbl.TextAlignment = UITextAlignment.Center;

				if (items != null )
					lbl.Text = items.ElementAt((int)row);

				return lbl;
			}
		}
	}
}
