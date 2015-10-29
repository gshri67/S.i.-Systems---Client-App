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
			MainTextLabel.Font =  UIFont.FromName("Helvetica", 10f);
			MainTextLabel.TextColor = StyleGuideConstants.MediumGrayUiColor;
			AddSubview( MainTextLabel );
		}

		public void setupConstraints() 
		{
			AddConstraint( NSLayoutConstraint.Create(MainTextLabel, NSLayoutAttribute.Left, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.05f, 0f));
			AddConstraint(NSLayoutConstraint.Create(MainTextLabel, NSLayoutAttribute.Top, NSLayoutRelation.Equal, this, NSLayoutAttribute.Bottom, 0.05f, 0f));
			AddConstraint(NSLayoutConstraint.Create(MainTextLabel, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, this, NSLayoutAttribute.Bottom, 0.65f, 0f));
			AddConstraint(NSLayoutConstraint.Create(MainTextLabel, NSLayoutAttribute.Right, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.75f, 0f));
			/*
			AddConstraint( NSLayoutConstraint.Create(payRateLabel, NSLayoutAttribute.Left, NSLayoutRelation.Equal, projectCodeField, NSLayoutAttribute.Left, 1.0f, 0f));
			AddConstraint(NSLayoutConstraint.Create(payRateLabel, NSLayoutAttribute.Top, NSLayoutRelation.Equal, projectCodeField, NSLayoutAttribute.Bottom, 1.0f, 0f));
			AddConstraint(NSLayoutConstraint.Create(payRateLabel, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, this, NSLayoutAttribute.Bottom, 0.95f, 0f));
			AddConstraint(NSLayoutConstraint.Create(payRateLabel, NSLayoutAttribute.Right, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.75f, 0f));

			AddConstraint(NSLayoutConstraint.Create(hoursField, NSLayoutAttribute.Left, NSLayoutRelation.Equal, payRateLabel, NSLayoutAttribute.Right, 1.0f, 0f));
			AddConstraint(NSLayoutConstraint.Create(hoursField, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterY, 1.0f, 0f));
			AddConstraint(NSLayoutConstraint.Create(hoursField, NSLayoutAttribute.Height, NSLayoutRelation.Equal, this, NSLayoutAttribute.Height, 0.6f, 0f));
			AddConstraint(NSLayoutConstraint.Create(hoursField, NSLayoutAttribute.Right, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.95f, 0f));*/
		}

		public void UpdateCell(string mainText, string subtitleText, string rightDetailText )
		{
			MainTextLabel.Text = mainText;
			//SubtitleTextLabel.Text = subtitleText;
			//RightDetailTextLabel.Text = rightDetailText;
		}
	}
}
