using System;
using System.Drawing;

using CoreGraphics;
using Foundation;
using UIKit;


namespace AccountExecutiveApp.iOS
{
    [Register("DeletableSection")]
    public class DeletableSection : UIView
    {
        public UILabel SectionLabel;
        public UIButton DeleteButton;

        public DeletableSection()
        {
            Initialize();
        }

        public DeletableSection(RectangleF bounds)
            : base(bounds)
        {
            Initialize();
        }

        void Initialize()
        {
            BackgroundColor = UIColor.Red;

            DeleteButton = new UIButton(new CGRect(0, 0, 25, 25));
            DeleteButton.SetTitleColor(UIColor.Blue, UIControlState.Normal);
            //DeleteButton.SetTitle( "Delete", UIControlState.Normal );
            DeleteButton.SetBackgroundImage(new UIImage("close-circled.png"), UIControlState.Normal);
            DeleteButton.TranslatesAutoresizingMaskIntoConstraints = false;

            SectionLabel = new UILabel();
            SectionLabel.Text = "Section";
            SectionLabel.TextAlignment = UITextAlignment.Left;
            SectionLabel.TranslatesAutoresizingMaskIntoConstraints = false;

            AddSubview(DeleteButton);
            AddSubview(SectionLabel);
            BackgroundColor = UIColor.FromWhiteAlpha(0.95f, 1.0f);
 
            AddConstraint(NSLayoutConstraint.Create(DeleteButton, NSLayoutAttribute.Right, NSLayoutRelation.Equal, Self, NSLayoutAttribute.Right, 1.0f, -12));
            //AddConstraint(NSLayoutConstraint.Create(DeleteButton, NSLayoutAttribute.Top, NSLayoutRelation.Equal, Self, NSLayoutAttribute.Bottom, 0.1f, 0));
            //AddConstraint(NSLayoutConstraint.Create(DeleteButton, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, Self, NSLayoutAttribute.Bottom, 0.9f, 0));
            //AddConstraint(NSLayoutConstraint.Create(DeleteButton, NSLayoutAttribute.Width, NSLayoutRelation.Equal, DeleteButton, NSLayoutAttribute.Height, 1.0f, 0));
            AddConstraint(NSLayoutConstraint.Create(DeleteButton, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterY, 1.0f, 0));
            AddConstraint(NSLayoutConstraint.Create(DeleteButton, NSLayoutAttribute.Width, NSLayoutRelation.Equal, null, NSLayoutAttribute.NoAttribute, 1.0f, 25));
            AddConstraint(NSLayoutConstraint.Create(DeleteButton, NSLayoutAttribute.Height, NSLayoutRelation.Equal, null, NSLayoutAttribute.NoAttribute, 1.0f, 25));

            AddConstraint(NSLayoutConstraint.Create(SectionLabel, NSLayoutAttribute.Left, NSLayoutRelation.Equal, this, NSLayoutAttribute.Left, 1.0f, 0));
            AddConstraint(NSLayoutConstraint.Create(SectionLabel, NSLayoutAttribute.Top, NSLayoutRelation.Equal, this, NSLayoutAttribute.Top, 1.0f, 0));
            AddConstraint(NSLayoutConstraint.Create(SectionLabel, NSLayoutAttribute.Right, NSLayoutRelation.Equal, DeleteButton, NSLayoutAttribute.Left, 1.0f, 0));
            AddConstraint(NSLayoutConstraint.Create(SectionLabel, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, this, NSLayoutAttribute.Bottom, 1.0f, 0));

            UIView LineView = new UIView(new CGRect(0, 0, 200, 1));
            LineView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
            LineView.BackgroundColor = UIColor.FromWhiteAlpha(0.75f, 1.0f);
            AddSubview(LineView);
        }
    }
}