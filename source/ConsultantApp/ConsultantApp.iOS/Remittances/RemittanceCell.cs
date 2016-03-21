using System;
using UIKit;
using Foundation;

namespace ConsultantApp.iOS
{
	partial class RemittanceCell : UITableViewCell
	{
		public RemittanceCell (IntPtr handle) : base (handle)
		{
		}

		public RemittanceCell(string cellId)
			: base(UITableViewCellStyle.Default, cellId)
		{

		}

		public void UpdateCell(string depositDate, string documentNumber, decimal amount, string period)
		{
			depositDateLabel.Text = depositDate;
			documentNumberLabel.Text = documentNumber;
           	amountLabel.Text = string.Format("{0:c}", amount);
			periodLabel.Text = period;
		}
	}
}

