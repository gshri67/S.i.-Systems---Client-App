using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace AccountExecutiveApp.iOS
{
    partial class SubtitleWithRightDetailCell : UITableViewCell
    {
        public UILabel MainTextLabel;
        public UILabel SubtitleTextLabel;
        public UILabel RightDetailTextLabel;

        public SubtitleWithRightDetailCell(IntPtr handle)
            : base(handle)
        {
            InitializeCell();
        }

        private void InitializeCell()
        {
            CreateAndAddLabels();

            SetupConstraints();
        }

        private void CreateAndAddLabels()
        {
            CreateAndAddMainTextLabel();

            CreateAndAddSubtitleTextLabel();

            CreateAndAddRightDetailTextLabel();
        }

        private void CreateAndAddRightDetailTextLabel()
        {
            RightDetailTextLabel = new UILabel
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                TextAlignment = UITextAlignment.Right,
                Font = UIFont.FromName("Helvetica", 12f),
                TextColor = StyleGuideConstants.DarkGrayUiColor
            };
            AddSubview(RightDetailTextLabel);
        }

        private void CreateAndAddSubtitleTextLabel()
        {
            SubtitleTextLabel = new UILabel
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                TextAlignment = UITextAlignment.Left,
                Font = UIFont.FromName("Helvetica", 10f),
                TextColor = StyleGuideConstants.MediumGrayUiColor
            };
            AddSubview(SubtitleTextLabel);
        }

        private void CreateAndAddMainTextLabel()
        {
            MainTextLabel = new UILabel
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                TextAlignment = UITextAlignment.Left,
                Font = UIFont.FromName("Helvetica", 14f),
                TextColor = UIColor.Black
            };
            AddSubview(MainTextLabel);
        }

        public SubtitleWithRightDetailCell(string cellId)
            : base(UITableViewCellStyle.Default, cellId)
        {
            InitializeCell();
        }

        public void SetupConstraints()
        {
            AddMainTextLabelConstraints();

            AddSubtitleTextLabelConstraints();

            AddRightDetailTextLabelContstraints();
        }

        private void AddMainTextLabelConstraints()
        {
            AddConstraint(NSLayoutConstraint.Create(MainTextLabel, NSLayoutAttribute.Left, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.05f, 0f));
            AddConstraint(NSLayoutConstraint.Create(MainTextLabel, NSLayoutAttribute.Top, NSLayoutRelation.Equal, this, NSLayoutAttribute.Bottom, 0.05f, 0f));
            AddConstraint(NSLayoutConstraint.Create(MainTextLabel, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, this, NSLayoutAttribute.Bottom, 0.65f, 0f));
            AddConstraint(NSLayoutConstraint.Create(MainTextLabel, NSLayoutAttribute.Right, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.6f, 0f));
        }

        private void AddSubtitleTextLabelConstraints()
        {
            AddConstraint(NSLayoutConstraint.Create(SubtitleTextLabel, NSLayoutAttribute.Left, NSLayoutRelation.Equal, MainTextLabel, NSLayoutAttribute.Left, 1.0f, 0f));
            AddConstraint(NSLayoutConstraint.Create(SubtitleTextLabel, NSLayoutAttribute.Top, NSLayoutRelation.Equal, MainTextLabel, NSLayoutAttribute.Bottom, 1.0f, 0f));
            AddConstraint(NSLayoutConstraint.Create(SubtitleTextLabel, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, this, NSLayoutAttribute.Bottom, 0.95f, 0f));
            AddConstraint(NSLayoutConstraint.Create(SubtitleTextLabel, NSLayoutAttribute.Right, NSLayoutRelation.Equal, MainTextLabel, NSLayoutAttribute.Right, 1.0f, 0f));
        }

        private void AddRightDetailTextLabelContstraints()
        {

            AddConstraint(NSLayoutConstraint.Create(RightDetailTextLabel, NSLayoutAttribute.Left, NSLayoutRelation.GreaterThanOrEqual, SubtitleTextLabel, NSLayoutAttribute.Right, 1.0f, 0f));
            AddConstraint(NSLayoutConstraint.Create(RightDetailTextLabel, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterY, 1.0f, 0f));
            AddConstraint(NSLayoutConstraint.Create(RightDetailTextLabel, NSLayoutAttribute.Height, NSLayoutRelation.Equal, this, NSLayoutAttribute.Height, 0.6f, 0f));
            AddConstraint(NSLayoutConstraint.Create(RightDetailTextLabel, NSLayoutAttribute.Right, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.95f, 0f));
        }

        public void UpdateCell(string mainText, string subtitleText, string rightDetailText)
        {
            MainTextLabel.Text = mainText;
            SubtitleTextLabel.Text = subtitleText;
            RightDetailTextLabel.Text = rightDetailText;
        }
    }
}
