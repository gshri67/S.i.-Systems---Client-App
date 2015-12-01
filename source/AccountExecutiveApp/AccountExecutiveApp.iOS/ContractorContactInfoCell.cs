
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace AccountExecutiveApp.iOS
{
    partial class ContractorContactInfoCell : UITableViewCell
    {
        public UILabel ContactTypeTextLabel;//mobile, home, etc..
        public UILabel MainContactTextLabel;
        public UILabel RightDetailIconLabel;
        public UILabel LeftDetailIconLabel;//also on right side, to the left of right detail icon

        public ContractorContactInfoCell(IntPtr handle)
            : base(handle)
        {
            InitializeCell();
        }

        private void InitializeCell()
        {
            this.Accessory = UITableViewCellAccessory.DisclosureIndicator;

            CreateAndAddLabels();

            SetupConstraints();
        }

        private void CreateAndAddLabels()
        {
            CreateAndAddMainContactTextLabel();

            CreateAndAddContactTypeTextLabel();

            CreateAndAddRightDetailIconLabel();
            CreateAndAddLeftDetailIconLabel();
        }

        private void CreateAndAddRightDetailIconLabel()
        {
            RightDetailIconLabel = new UILabel
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                TextAlignment = UITextAlignment.Right,
                Font = UIFont.FromName("Helvetica", 14f),
                TextColor = StyleGuideConstants.MediumGrayUiColor
            };
            AddSubview(RightDetailIconLabel);
        }
        private void CreateAndAddLeftDetailIconLabel()
        {
            LeftDetailIconLabel = new UILabel
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                TextAlignment = UITextAlignment.Right,
                Font = UIFont.FromName("Helvetica", 14f),
                TextColor = StyleGuideConstants.MediumGrayUiColor
            };
            AddSubview(LeftDetailIconLabel);
        }

        private void CreateAndAddContactTypeTextLabel()
        {
            ContactTypeTextLabel = new UILabel
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                TextAlignment = UITextAlignment.Left,
                Font = UIFont.FromName("Helvetica", 12f),
                TextColor = StyleGuideConstants.MediumGrayUiColor
            };
            AddSubview(ContactTypeTextLabel);
        }

        private void CreateAndAddMainContactTextLabel()
        {
            MainContactTextLabel = new UILabel
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                TextAlignment = UITextAlignment.Left,
                Font = UIFont.FromName("Helvetica", 16f),
                TextColor = UIColor.Black
            };
            AddSubview(MainContactTextLabel);
        }

        public ContractorContactInfoCell(string cellId)
            : base(UITableViewCellStyle.Default, cellId)
        {
            InitializeCell();
        }

        public void SetupConstraints()
        {
            AddRightDetailTextLabelContstraints();
            AddLeftDetailTextLabelContstraints();

            AddMainContactTextLabelConstraints();
            AddContactTypeTextLabelConstraints();
        }

        private void AddMainContactTextLabelConstraints()
        {
            AddConstraint(NSLayoutConstraint.Create(MainContactTextLabel, NSLayoutAttribute.Left, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.05f, 0f));
            AddConstraint(NSLayoutConstraint.Create(MainContactTextLabel, NSLayoutAttribute.Right, NSLayoutRelation.Equal, LeftDetailIconLabel, NSLayoutAttribute.Left, 1.0f, 0f));
            AddConstraint(NSLayoutConstraint.Create(MainContactTextLabel, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterY, 1.0f, 9));
        }

        private void AddContactTypeTextLabelConstraints()
        {
            AddConstraint(NSLayoutConstraint.Create(ContactTypeTextLabel, NSLayoutAttribute.Left, NSLayoutRelation.Equal, MainContactTextLabel, NSLayoutAttribute.Left, 1.0f, 0f));
            AddConstraint(NSLayoutConstraint.Create(ContactTypeTextLabel, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, MainContactTextLabel, NSLayoutAttribute.Top, 1.0f, -3f));
            AddConstraint(NSLayoutConstraint.Create(ContactTypeTextLabel, NSLayoutAttribute.Right, NSLayoutRelation.Equal, MainContactTextLabel, NSLayoutAttribute.Right, 1.0f, 0f));
        }

        private void AddRightDetailTextLabelContstraints()
        {
            AddConstraint(NSLayoutConstraint.Create(RightDetailIconLabel, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterY, 1.0f, 0f));
            AddConstraint(NSLayoutConstraint.Create(RightDetailIconLabel, NSLayoutAttribute.Right, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.90f, 0f));
            AddConstraint(NSLayoutConstraint.Create(RightDetailIconLabel, NSLayoutAttribute.Height, NSLayoutRelation.Equal, null, NSLayoutAttribute.NoAttribute, 1.0f, 25f));
            AddConstraint(NSLayoutConstraint.Create(RightDetailIconLabel, NSLayoutAttribute.Width, NSLayoutRelation.Equal, RightDetailIconLabel, NSLayoutAttribute.Height,1.0f, 0f));
        }

        private void AddLeftDetailTextLabelContstraints()
        {
            AddConstraint(NSLayoutConstraint.Create(LeftDetailIconLabel, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterY, 1.0f, 0f));
            AddConstraint(NSLayoutConstraint.Create(LeftDetailIconLabel, NSLayoutAttribute.Right, NSLayoutRelation.Equal, RightDetailIconLabel, NSLayoutAttribute.Left, 1.0f, 8f));
            AddConstraint(NSLayoutConstraint.Create(LeftDetailIconLabel, NSLayoutAttribute.Height, NSLayoutRelation.Equal, RightDetailIconLabel, NSLayoutAttribute.Height, 1.0f, 0f));
            AddConstraint(NSLayoutConstraint.Create(LeftDetailIconLabel, NSLayoutAttribute.Width, NSLayoutRelation.Equal, LeftDetailIconLabel, NSLayoutAttribute.Height, 1.0f, 0f));
        }

        public void UpdateCell(string mainContactText, string contactTypeText, bool canText, bool canPhone, bool canEmail )
        {
            MainContactTextLabel.Text = mainContactText;
            ContactTypeTextLabel.Text = contactTypeText;
            
            if( canPhone )
                AddPhoneIcon();
        }

        public void AddPhoneIcon()
        {
            RightDetailIconLabel.AttributedText = GetAttributedStringWithImage(new UIImage("plus-round-centred.png"), 25);
        }

        private NSAttributedString GetAttributedStringWithImage(UIImage image, float size)
        {
            NSTextAttachment textAttachement = new NSTextAttachment();
            textAttachement.Image = image;
            textAttachement.Bounds = new CoreGraphics.CGRect(0, 0, size, size);
            NSAttributedString attrStringWithImage = NSAttributedString.CreateFrom(textAttachement);
            return attrStringWithImage;
        }
    }
}
