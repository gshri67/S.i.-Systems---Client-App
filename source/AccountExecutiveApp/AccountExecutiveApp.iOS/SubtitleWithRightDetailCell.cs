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

		public SubtitleWithRightDetailCell (IntPtr handle) : base (handle)
		{
			MainTextLabel = new UILabel();
			MainTextLabel.TranslatesAutoresizingMaskIntoConstraints = false;
			MainTextLabel.TextAlignment = UITextAlignment.Left;
			MainTextLabel.Font =  UIFont.FromName("Helvetica", 12f);
			MainTextLabel.TextColor = StyleGuideConstants.DarkerGrayUiColor;
			AddSubview( MainTextLabel );

            SubtitleTextLabel = new UILabel();
            SubtitleTextLabel.TranslatesAutoresizingMaskIntoConstraints = false;
            SubtitleTextLabel.TextAlignment = UITextAlignment.Left;
            SubtitleTextLabel.Font = UIFont.FromName("Helvetica", 10f);
            SubtitleTextLabel.TextColor = StyleGuideConstants.MediumGrayUiColor;
            AddSubview(SubtitleTextLabel);

            RightDetailTextLabel = new UILabel();
            RightDetailTextLabel.TranslatesAutoresizingMaskIntoConstraints = false;
            RightDetailTextLabel.TextAlignment = UITextAlignment.Left;
            RightDetailTextLabel.Font = UIFont.FromName("Helvetica", 10f);
            RightDetailTextLabel.TextColor = StyleGuideConstants.MediumGrayUiColor;
            AddSubview(RightDetailTextLabel);
		}

		public void setupConstraints() 
		{
			AddConstraint( NSLayoutConstraint.Create(MainTextLabel, NSLayoutAttribute.Left, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.05f, 0f));
			AddConstraint(NSLayoutConstraint.Create(MainTextLabel, NSLayoutAttribute.Top, NSLayoutRelation.Equal, this, NSLayoutAttribute.Bottom, 0.05f, 0f));
			AddConstraint(NSLayoutConstraint.Create(MainTextLabel, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, this, NSLayoutAttribute.Bottom, 0.65f, 0f));
			AddConstraint(NSLayoutConstraint.Create(MainTextLabel, NSLayoutAttribute.Right, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.75f, 0f));

			
			AddConstraint( NSLayoutConstraint.Create(SubtitleTextLabel, NSLayoutAttribute.Left, NSLayoutRelation.Equal, MainTextLabel, NSLayoutAttribute.Left, 1.0f, 0f));
			AddConstraint(NSLayoutConstraint.Create(SubtitleTextLabel, NSLayoutAttribute.Top, NSLayoutRelation.Equal, MainTextLabel, NSLayoutAttribute.Bottom, 1.0f, 0f));
			AddConstraint(NSLayoutConstraint.Create(SubtitleTextLabel, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, this, NSLayoutAttribute.Bottom, 0.95f, 0f));
			AddConstraint(NSLayoutConstraint.Create(SubtitleTextLabel, NSLayoutAttribute.Right, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.75f, 0f));

			AddConstraint(NSLayoutConstraint.Create(RightDetailTextLabel, NSLayoutAttribute.Left, NSLayoutRelation.Equal, SubtitleTextLabel, NSLayoutAttribute.Right, 1.0f, 0f));
			AddConstraint(NSLayoutConstraint.Create(RightDetailTextLabel, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterY, 1.0f, 0f));
			AddConstraint(NSLayoutConstraint.Create(RightDetailTextLabel, NSLayoutAttribute.Height, NSLayoutRelation.Equal, this, NSLayoutAttribute.Height, 0.6f, 0f));
			AddConstraint(NSLayoutConstraint.Create(RightDetailTextLabel, NSLayoutAttribute.Right, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.95f, 0f));
		}

		public void UpdateCell(string mainText, string subtitleText, string rightDetailText )
		{
			MainTextLabel.Text = mainText;
			SubtitleTextLabel.Text = subtitleText;
			RightDetailTextLabel.Text = rightDetailText;
		}
	}
}
