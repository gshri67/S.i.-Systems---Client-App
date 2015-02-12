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
            fullName.Frame = new CGRect(50,0,100,33);
            contractDates.Frame = new CGRect(50, 33, 100, 33);
            lastContractRate.Frame = new CGRect(ContentView.Bounds.Right, 0, 100, 33);
            ratingImage.Frame = new CGRect(0, 0, 50, 50);
        }
	}
}