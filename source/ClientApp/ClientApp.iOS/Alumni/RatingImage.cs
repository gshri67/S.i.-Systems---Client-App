using SiSystems.ClientApp.SharedModels;
using UIKit;

namespace ClientApp.iOS.Alumni
{
    public class RatingImage
    {
        private static readonly UIImage OneStar = UIImage.FromBundle("thumb-up-8x.png");
        private static readonly UIImage TwoStar = UIImage.FromBundle("thumb-up-8x.png");
        private static readonly UIImage ThreeStar = UIImage.FromBundle("thumb-up-8x.png");

        public static UIImage GetImageForRating(MatchGuideConstants.ResumeRating? rating)
        {
            if (rating == null) return null;

            switch (rating.Value)
            {
                case MatchGuideConstants.ResumeRating.AboveStandard:
                    return ThreeStar;
                case MatchGuideConstants.ResumeRating.Standard:
                    return TwoStar;
                case MatchGuideConstants.ResumeRating.BelowStandard:
                    return OneStar;
                default:
                    return null;
            }
        }
    }
}