using System;
using UIKit;

namespace ConsultantApp.iOS.TimeSheets.ActiveTimesheets
{
	partial class ActiveTimesheetCell : UITableViewCell
	{
		public ActiveTimesheetCell (IntPtr handle) : base (handle)
		{
		}

        public ActiveTimesheetCell(string cellId)
            : base(UITableViewCellStyle.Default, cellId)
        {

        }

        public void UpdateCell(string company, string timesheetApprover, string hours, string status)
        {
            //Company.Text = company;
            TimesheetApprover.Text = timesheetApprover;
            Hours.Text = string.Format("{0} hrs", hours);
            Status.Text = status;
        }
	}
}
