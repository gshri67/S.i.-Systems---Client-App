using System;
using UIKit;
using Foundation;

namespace ConsultantApp.iOS
{
	[Foundation.Register ("RemittanceCell")]
	partial class RemittanceCell : UITableViewCell
	{
		public RemittanceCell (IntPtr handle) : base (handle)
		{
		}

		public RemittanceCell(string cellId)
			: base(UITableViewCellStyle.Default, cellId)
		{

		}

		public void UpdateCell(string depositDate, string documentNumber, float amount, string period)
		{
			depositDateLabel.Text = depositDate;
			documentNumberLabel.Text = documentNumber;
			amountLabel.Text = "$" + amount.ToString ("0.00");
			periodLabel.Text = period;
		}
	}
}

