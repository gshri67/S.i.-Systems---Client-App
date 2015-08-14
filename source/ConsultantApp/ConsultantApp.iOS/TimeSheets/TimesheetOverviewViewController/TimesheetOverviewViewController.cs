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
        private readonly NSObject _tokenExpiredObserver;
	    private readonly TimesheetViewModel _timesheetModel;

	    private IEnumerable<Timesheet> timesheets;
		private Timesheet _curTimesheet;

		public TimesheetOverviewViewController (IntPtr handle) : base (handle)
		{
			//TabBarController.TabBar.TintColor = UIColor.Blue;

            //TabBarController.TabBar.Items [0].Image = new UIImage ("ios7-clock-outline.png");
            //TabBarController.TabBar.Items [1].Image = new UIImage ("social-usd.png");

			LogoutManager.CreateNavBarLeftButton (this);

            _timesheetModel = DependencyResolver.Current.Resolve<TimesheetViewModel>();
            this._tokenExpiredObserver = NSNotificationCenter.DefaultCenter.AddObserver(new NSString("TokenExpired"), this.OnTokenExpired);
		}

        public void OnTokenExpired(NSNotification notifcation)
        {
            // Unsubscribe from this event so that it won't be handled again
            NSNotificationCenter.DefaultCenter.RemoveObserver(_tokenExpiredObserver);

            var loginViewController = this.Storyboard.InstantiateViewController("LoginView");
            var navigationController = new UINavigationController(loginViewController) { NavigationBarHidden = true };

            UIApplication.SharedApplication.Windows[0].RootViewController = navigationController;
        }
			
	    public async void LoadTimesheets()
	    {
            timesheets = await _timesheetModel.GetTimesheets(DateTime.Now);

            var timeEntries = timesheets.SelectMany(timesheet => timesheet.TimeEntries);
            
			/*
            tableview.RegisterClassForCellReuse( typeof(TimeEntryCell), @"TimeEntryCell");
            tableview.Source = new TimesheetOverviewTableViewSource(this, timeEntries);
            tableview.ReloadData ();*/
	    }

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
		    
            //LoadTimesheets();

			addButton.TouchUpInside += delegate {
				
				UIViewController vc = Storyboard.InstantiateViewController("AddTimeEntryViewController");

				NavigationController.PushViewController(vc, true);
			};

			//_curTimesheet = timesheets.First ();

			_curTimesheet = new Timesheet ();
			_curTimesheet.ClientName = "Nexen";
			_curTimesheet.StartDate = new DateTime (2015, 8, 1);
			_curTimesheet.EndDate = new DateTime (2015, 8, 15);

			List<TimeEntry> timeEntries = new List<TimeEntry> ();
			TimeEntry te1 = new TimeEntry();
			te1.Date = new DateTime (2015, 8, 12);
			te1.Hours = 5;
			timeEntries.Add (te1);

			for (int i = 3; i < 11; i++) 
			{
				TimeEntry te2 = new TimeEntry();
				te2.Date = new DateTime (2015, 8, i);
				te2.Hours = 8;
				timeEntries.Add (te2);
			}

			_curTimesheet.TimeEntries = timeEntries;

			FMCalendar calendar = new FMCalendar (calendarContainerView.Bounds, new CoreGraphics.CGRect(), _curTimesheet );
			calendarContainerView.AddSubview (calendar);

			calendar.DateSelected = delegate(DateTime date) {
				//PUSH
			};
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
