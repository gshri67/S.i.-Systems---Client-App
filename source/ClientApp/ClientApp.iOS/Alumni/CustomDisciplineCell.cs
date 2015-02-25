using Foundation;
using System;
using System.CodeDom.Compiler;
using ClientApp.iOS.Alumni;
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

        public void UpdateCell(string name, string dates, string rate, MatchGuideConstants.ResumeRating? rating)
        {
            fullName.Text = name;
            contractDates.Text = dates;
            lastContractRate.Text = rate;
            var ratingImages = new RatingImage(rating);
            RightStar.Image = ratingImages.GetFirstStar();
            MiddleStar.Image = ratingImages.GetSecondStar();
            LeftStar.Image = ratingImages.GetThirdStar();
        }
	}
}