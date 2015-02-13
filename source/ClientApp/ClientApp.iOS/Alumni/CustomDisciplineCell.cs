using Foundation;
using System;
using System.CodeDom.Compiler;
using CoreGraphics;
using SiSystems.ClientApp.SharedModels;
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

	    private void SetRatingImage(int? rating)
	    {
	        if (rating == null)
	            return;

	            if (rating.Equals(MatchGuideConstants.ResumeRating.AboveStandard))
	        {
                ratingImage.Image = UIImage.FromBundle("thumb-up-8x.png");
	        }
	    }

        public void UpdateCell(string name, string dates, string rate, int? rating)
        {
            fullName.Text = name;
            contractDates.Text = dates;
            lastContractRate.Text = rate;
            
            SetRatingImage(rating);
        }
	}
}