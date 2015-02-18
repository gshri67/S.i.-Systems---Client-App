using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace ClientApp.iOS
{
	partial class CustomContractCell : UITableViewCell
	{
		public CustomContractCell (IntPtr handle) : base (handle)
		{
		}

	    public CustomContractCell(string reuseIdentifier) : base(UITableViewCellStyle.Default, reuseIdentifier)
	    {
	    }

	    public void UpdateCell(string fullName, string rate, string dates, string shortName)
	    {
	        FullNameLabel.Text = fullName;
	        RateLabel.Text = rate;
	        DateLabel.Text = dates;
	        ContractShortNameLabel.Text = shortName;
	    }
	}
}
