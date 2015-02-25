using SiSystems.ClientApp.SharedModels;
using UIKit;

namespace ClientApp.iOS.Alumni
{
    public class RatingImage
    {
        private static readonly UIImage GoldStar = UIImage.FromBundle("goldstar");
        private static readonly UIImage BlankStar = UIImage.FromBundle("star");
        private readonly MatchGuideConstants.ResumeRating _rating;

        public RatingImage(MatchGuideConstants.ResumeRating? resumeRating)
        {
            _rating = resumeRating.GetValueOrDefault();
        }

        public UIImage GetRightStarImage()
        {
            return GoldStar;
        }

        private bool IsStandardOrAboveRating()
        {
            return _rating == MatchGuideConstants.ResumeRating.Standard ||
                   _rating == MatchGuideConstants.ResumeRating.AboveStandard;
        }

        private bool IsAboveStandardRating()
        {
            return _rating == MatchGuideConstants.ResumeRating.AboveStandard;
        }

        public UIImage GetMiddleStarImage()
        {
            return IsStandardOrAboveRating() ? GoldStar : BlankStar;
        }

        public UIImage GetLeftStarImage()
        {
            return IsAboveStandardRating() ? GoldStar : BlankStar;
        }
    }
}