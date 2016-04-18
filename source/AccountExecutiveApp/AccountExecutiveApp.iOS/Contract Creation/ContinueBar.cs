using System;
using System.Drawing;

using CoreGraphics;
using Foundation;
using UIKit;


namespace AccountExecutiveApp.iOS
{
    [Register("ContinueBar")]
    public class ContinueBar : UIView
    {
        public UIButton NextButton;
        public UIButton NextTextButton;

        public ContinueBar()
        {
            Initialize();
        }

        public ContinueBar(RectangleF bounds)
            : base(bounds)
        {
            Initialize();
        }

        void Initialize()
        {
            BackgroundColor = UIColor.Red;

            NextButton = new UIButton(new CGRect(0, 0, 50, 50));
            NextButton.SetTitleColor(UIColor.Blue, UIControlState.Normal);
            NextButton.SetBackgroundImage(new UIImage("arrow-black-60.png"), UIControlState.Normal);
            NextButton.TranslatesAutoresizingMaskIntoConstraints = false;

            NextTextButton = new UIButton();
            NextTextButton.SetTitleColor(StyleGuideConstants.RedUiColor, UIControlState.Normal);
            NextTextButton.SetTitle("Next", UIControlState.Normal);
            NextTextButton.TitleLabel.TextAlignment = UITextAlignment.Right;
            NextTextButton.TranslatesAutoresizingMaskIntoConstraints = false;

            AddSubview(NextButton);
            AddSubview(NextTextButton);
            BackgroundColor = UIColor.FromWhiteAlpha(0.95f, 1.0f);
 
            AddConstraint(NSLayoutConstraint.Create(NextButton, NSLayoutAttribute.Right, NSLayoutRelation.Equal, Self, NSLayoutAttribute.Right, 1.0f, 0));
            AddConstraint(NSLayoutConstraint.Create(NextButton, NSLayoutAttribute.Width, NSLayoutRelation.Equal, NextButton, NSLayoutAttribute.Height, 0.9f, 0));
            AddConstraint(NSLayoutConstraint.Create(NextButton, NSLayoutAttribute.Top, NSLayoutRelation.Equal, Self, NSLayoutAttribute.Top, 1.0f, 0));
            AddConstraint(NSLayoutConstraint.Create(NextButton, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, Self, NSLayoutAttribute.Bottom, 1.0f, 0));

            AddConstraint(NSLayoutConstraint.Create(NextTextButton, NSLayoutAttribute.Right, NSLayoutRelation.Equal, NextButton, NSLayoutAttribute.Left, 1.0f, 0));
            AddConstraint(NSLayoutConstraint.Create(NextTextButton, NSLayoutAttribute.Top, NSLayoutRelation.Equal, Self, NSLayoutAttribute.Top, 1.0f, 0));
            AddConstraint(NSLayoutConstraint.Create(NextTextButton, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, Self, NSLayoutAttribute.Bottom, 1.0f, 0));


            UIView LineView = new UIView(new CGRect(0, 0, 200, 1));
            LineView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
            LineView.BackgroundColor = UIColor.FromWhiteAlpha(0.75f, 1.0f);
            AddSubview(LineView);
        }
    }
}