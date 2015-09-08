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
			}
		}

	    public override void ViewWillAppear(bool animated)
	    {
			base.ViewWillAppear (animated);

            SetupCalendar();
            updateUI();
	    }

	    private void SetupCalendar()
	    {
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

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			addButton.TouchUpInside += delegate {
				
				UIViewController vc = Storyboard.InstantiateViewController("AddTimeEntryViewController");

				NavigationController.PushViewController(vc, true);
			};

			SetupCalendar();


			submitContainerView.BackgroundColor = UIColor.White;// StyleGuideConstants.LightGrayUiColor;
			approverContainerView.BackgroundColor = UIColor.White;//StyleGuideConstants.LightGrayUiColor;
			calendarHeaderView.BackgroundColor = StyleGuideConstants.LightGrayUiColor;

			DateTime curMonthDate;

			if (_curTimesheet != null)
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

			updateUI();

			UILabel titleLabel = new UILabel ( new CoreGraphics.CGRect(0, 0, 0, 0) );
			titleLabel.BackgroundColor = UIColor.Clear;
			titleLabel.TextColor = UIColor.DarkGray;
			titleLabel.Text = "Timesheet Overview";
			titleLabel.Font = UIFont.FromName ("Arial", 20);
			//titleLabel.Font = UIFont.BoldSystemFontOfSize (17);
			titleLabel.SizeToFit ();
			titleLabel.TextAlignment = UITextAlignment.Center;


			UILabel subtitleLabel = new UILabel ( new CoreGraphics.CGRect(0, 22, 0, 0) );
			subtitleLabel.BackgroundColor = UIColor.Clear;
			subtitleLabel.TextColor = UIColor.DarkGray;
			subtitleLabel.Text = "Cenovus Energy";
			subtitleLabel.Font = UIFont.SystemFontOfSize (12);
			subtitleLabel.TextAlignment = UITextAlignment.Center;
			subtitleLabel.SizeToFit ();

			float maxWidth;

			if (titleLabel.Frame.Width >= subtitleLabel.Frame.Width) 
			{
				maxWidth = (float)titleLabel.Frame.Width;

				subtitleLabel.Frame = new CoreGraphics.CGRect ( subtitleLabel.Frame.X, subtitleLabel.Frame.Y, maxWidth, subtitleLabel.Frame.Height );
			} 
			else 
			{
				maxWidth = (float)subtitleLabel.Frame.Width;

				titleLabel.Frame = new CoreGraphics.CGRect ( titleLabel.Frame.X, titleLabel.Frame.Y, maxWidth, titleLabel.Frame.Height );
			}


			UIView subtitleHeaderView = new UIView( new CoreGraphics.CGRect( 0, 0, maxWidth, 30 ));
			subtitleHeaderView.BackgroundColor = UIColor.Clear;
			NavigationItem.TitleView = subtitleHeaderView;

			subtitleHeaderView.AddSubview (titleLabel);
			subtitleHeaderView.AddSubview (subtitleLabel);

			/*
    UILabel *titleLabel = [[UILabel alloc] initWithFrame:CGRectMake(0, 0, 0, 0)];
    titleLabel.backgroundColor = [UIColor clearColor];
    titleLabel.textColor = [UIColor whiteColor];
    titleLabel.font = [UIFont boldSystemFontOfSize:20];
    titleLabel.text = @"Your Title";
    [titleLabel sizeToFit];

    UILabel *subTitleLabel = [[UILabel alloc] initWithFrame:CGRectMake(0, 22, 0, 0)];
    subTitleLabel.backgroundColor = [UIColor clearColor];
    subTitleLabel.textColor = [UIColor whiteColor];
    subTitleLabel.font = [UIFont systemFontOfSize:12];
    subTitleLabel.text = @"Your subtitle";
    [subTitleLabel sizeToFit];

    UIView *twoLineTitleView = [[UIView alloc] initWithFrame:CGRectMake(0, 0, MAX(subTitleLabel.frame.size.width, titleLabel.frame.size.width), 30)];
    [twoLineTitleView addSubview:titleLabel];
    [twoLineTitleView addSubview:subTitleLabel];

    float widthDiff = subTitleLabel.frame.size.width - titleLabel.frame.size.width;

    if (widthDiff > 0) {
        CGRect frame = titleLabel.frame;
        frame.origin.x = widthDiff / 2;
        titleLabel.frame = CGRectIntegral(frame);
    }else{
        CGRect frame = subTitleLabel.frame;
        frame.origin.x = abs(widthDiff) / 2;
        subTitleLabel.frame = CGRectIntegral(frame);
    }

    self.navigationItem.titleView = twoLineTitleView;
			*/
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
