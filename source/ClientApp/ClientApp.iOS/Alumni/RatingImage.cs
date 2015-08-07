using SiSystems.SharedModels;
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

        public bool IsUnchecked
        {
            get
            {
                return _rating == MatchGuideConstants.ResumeRating.NotChecked ||
                       _rating == MatchGuideConstants.ResumeRating.AlsoNotChecked;
            }
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

        public UIImage GetFirstStar()
        {
            return IsUnchecked ? null : GoldStar;
        }

        public UIImage GetSecondStar()
        {
            if (IsUnchecked)
                return null;
            return IsStandardOrAboveRating() ? GoldStar : BlankStar;
        }

        public UIImage GetThirdStar()
        {
            if (IsUnchecked)
                return null;
            return IsAboveStandardRating() ? GoldStar : BlankStar;
        }

        public bool HideCheckedLabel()
        {
            return !IsUnchecked;
        }
    }
}