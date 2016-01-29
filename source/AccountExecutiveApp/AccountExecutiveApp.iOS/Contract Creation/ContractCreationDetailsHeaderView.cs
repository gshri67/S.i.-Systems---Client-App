using System;

using Foundation;
using UIKit;

namespace AccountExecutiveApp.iOS
{
	public class ContractCreationDetailsHeaderView : UIView
	{
	    public ContractCreationDetailsHeaderView(IntPtr handle)
	        : base(handle)
	    {
	        InitializeCell();
	    }

	    public ContractCreationDetailsHeaderView()
	    {
            InitializeCell();
	    }

        public ContractCreationDetailsHeaderView(string text)
        {
            InitializeCell();
            MainTextLabel.Text = text;
        }

        public UILabel MainTextLabel;
        public UILabel SubtitleTextLabel;
        public UILabel RightDetailTextLabel;

        public const string CellIdentifier = "SubtitleWithRightDetailCell";

        private void InitializeCell()
        {
            BackgroundColor = UIColor.FromWhiteAlpha(0.95f, 1.0f);

            CreateAndAddLabels();

            SetupConstraints();
        }

        private void CreateAndAddLabels()
        {
            CreateAndAddMainTextLabel();
        }

        private void CreateAndAddMainTextLabel()
        {
            MainTextLabel = new UILabel
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                TextAlignment = UITextAlignment.Center,
                Font = UIFont.FromName("Helvetica", 16f),
                TextColor = UIColor.Black,
                Lines = 0
            };

            AddSubview(MainTextLabel);
        }

        public void SetupConstraints()
        {
            AddMainTextLabelConstraints();
        }

        private void AddMainTextLabelConstraints()
        {
            AddConstraint(NSLayoutConstraint.Create(MainTextLabel, NSLayoutAttribute.Left, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.05f, 0f));
            AddConstraint(NSLayoutConstraint.Create(MainTextLabel, NSLayoutAttribute.Right, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.95f, 0f));
            AddConstraint(NSLayoutConstraint.Create(MainTextLabel, NSLayoutAttribute.Top, NSLayoutRelation.Equal, this, NSLayoutAttribute.Bottom, 0.05f, 0f));
            AddConstraint(NSLayoutConstraint.Create(MainTextLabel, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, this, NSLayoutAttribute.Bottom, 0.95f, 0f));
        }

	}
}

