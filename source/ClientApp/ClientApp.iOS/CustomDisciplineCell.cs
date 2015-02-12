using Foundation;
using System;
using System.CodeDom.Compiler;
using CoreGraphics;
using UIKit;

namespace ClientApp.iOS
{
	partial class CustomDisciplineCell : UITableViewCell
	{
		public CustomDisciplineCell (IntPtr handle) : base (handle)
		{
		}

        public CustomDisciplineCell(string cellId)
            : base(UITableViewCellStyle.Default, cellId)
        {

        }

        public void UpdateCell(string name, string dates, string rate, int? rating)
        {
            //ratingImage.Image = rating;
            //todo: make the rating magic happen. 
            fullName.Text = name;
            contractDates.Text = dates;
            lastContractRate.Text = rate;
        }
        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
        }
	}
}