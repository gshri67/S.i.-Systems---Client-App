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

        public void UpdateCell(string company, string subtitleText, string hours, string status)
        {
            companyLabel.Text = company;
            timesheetApproverLabel.Text = subtitleText;
            hoursLabel.Text = string.Format("{0} hrs", hours);
            statusLabel.Text = status;
        }
	}
}
