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
        private const string RateWitheldText = "Rate Witheld";

		public CustomDisciplineCell (IntPtr handle) : base (handle)
		{
		}

        public CustomDisciplineCell(string cellId)
            : base(UITableViewCellStyle.Default, cellId)
        {

        }


        private void UpdateRateText(ConsultantSummary consultant)
        {
            if (!consultant.RateWitheld)
                lastContractRate.Text = string.Format("{0:C}/h", consultant.MostRecentContractRate);

            lastContractRate.TextColor = StyleGuideConstants.LightGrayUiColor;
            lastContractRate.Text = RateWitheldText;
        }

        private static string FormatDateString(ConsultantSummary consultant)
        {
            return string.Format("{0:MMM dd, yyyy} {2} {1:MMM dd, yyyy}", consultant.MostRecentContractStartDate,
                consultant.MostRecentContractEndDate,
                StyleGuideConstants.DateSeperator);
        }

        private void UpdateRatingDisplay(ConsultantSummary consultant)
        {
            var ratingImages = new RatingImage(consultant.Rating);
            RightStar.Image = ratingImages.GetFirstStar();
            MiddleStar.Image = ratingImages.GetSecondStar();
            LeftStar.Image = ratingImages.GetThirdStar();
            UncheckedLabel.Hidden = ratingImages.HideCheckedLabel();
            UncheckedLabel.TextColor = StyleGuideConstants.DarkerGrayUiColor;
        }

        public void UpdateCell(ConsultantSummary consultant)
        {
            fullName.Text = consultant.FullName;
            contractDates.Text = FormatDateString(consultant);

            UpdateRateText(consultant);

            UpdateRatingDisplay(consultant);
        }
	}
}