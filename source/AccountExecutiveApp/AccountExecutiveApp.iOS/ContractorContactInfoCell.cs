
using Foundation;
using System;
using System.CodeDom.Compiler;
using Contacts;
using MessageUI;
using UIKit;

namespace AccountExecutiveApp.iOS
{
    partial class ContractorContactInfoCell : UITableViewCell
    {
        public UILabel ContactTypeTextLabel;//mobile, home, etc..
        public UILabel MainContactTextLabel;
        public UIButton RightDetailIconButton;
        public UIButton LeftDetailIconButton;//also on right side, to the left of right detail icon
        public UIViewController ParentViewController;

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

            CreateAndAddRightDetailIconButton();
            CreateAndAddLeftDetailIconButton();
        }

        private void CreateAndAddRightDetailIconButton()
        {
            RightDetailIconButton = new UIButton
            {
                TranslatesAutoresizingMaskIntoConstraints = false, 
                HorizontalAlignment = UIControlContentHorizontalAlignment.Right,
                //TextAlignment = UITextAlignment.Right,
                Font = UIFont.FromName("Helvetica", 14f),
                // StyleGuideConstants.MediumGrayUiColor
            };
            AddSubview(RightDetailIconButton);
        }
        private void CreateAndAddLeftDetailIconButton()
        {
            LeftDetailIconButton = new UIButton
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                HorizontalAlignment = UIControlContentHorizontalAlignment.Right,
                //TextAlignment = UITextAlignment.Right,
                Font = UIFont.FromName("Helvetica", 14f),
                // StyleGuideConstants.MediumGrayUiColor
            };
            AddSubview(LeftDetailIconButton);
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
            AddConstraint(NSLayoutConstraint.Create(MainContactTextLabel, NSLayoutAttribute.Right, NSLayoutRelation.Equal, LeftDetailIconButton, NSLayoutAttribute.Left, 1.0f, 0f));
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
            AddConstraint(NSLayoutConstraint.Create(RightDetailIconButton, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterY, 1.0f, 0f));
            AddConstraint(NSLayoutConstraint.Create(RightDetailIconButton, NSLayoutAttribute.Right, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.90f, 0f));
            AddConstraint(NSLayoutConstraint.Create(RightDetailIconButton, NSLayoutAttribute.Height, NSLayoutRelation.Equal, null, NSLayoutAttribute.NoAttribute, 1.0f, 25f));
            AddConstraint(NSLayoutConstraint.Create(RightDetailIconButton, NSLayoutAttribute.Width, NSLayoutRelation.Equal, RightDetailIconButton, NSLayoutAttribute.Height,1.0f, 0f));
        }

        private void AddLeftDetailTextLabelContstraints()
        {
            AddConstraint(NSLayoutConstraint.Create(LeftDetailIconButton, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterY, 1.0f, 0f));
            AddConstraint(NSLayoutConstraint.Create(LeftDetailIconButton, NSLayoutAttribute.Right, NSLayoutRelation.Equal, RightDetailIconButton, NSLayoutAttribute.Left, 1.0f, -10f));
            AddConstraint(NSLayoutConstraint.Create(LeftDetailIconButton, NSLayoutAttribute.Height, NSLayoutRelation.Equal, RightDetailIconButton, NSLayoutAttribute.Height, 1.0f, 0f));
            AddConstraint(NSLayoutConstraint.Create(LeftDetailIconButton, NSLayoutAttribute.Width, NSLayoutRelation.Equal, LeftDetailIconButton, NSLayoutAttribute.Height, 1.0f, 0f));
        }

        public void UpdateCell(string mainContactText, string contactTypeText, bool canText, bool canPhone, bool canEmail )
        {
            MainContactTextLabel.Text = mainContactText;
            ContactTypeTextLabel.Text = contactTypeText;
            
            if( canPhone )
                AddPhoneIcon(mainContactText);
            else if( canEmail )
                AddEmailIcon(mainContactText);

            if( canText )
                AddTextingIcon(mainContactText);
        }

        public void AddPhoneIcon( string phoneNumber )
        {
            RightDetailIconButton.SetAttributedTitle(GetAttributedStringWithImage(new UIImage("ios7-telephone-outline.png"), 25), UIControlState.Normal);

            RightDetailIconButton.TouchUpInside += delegate
            {
                NSUrl url = new NSUrl( string.Format(@"telprompt://{0}", phoneNumber));
                //NSUrl url = new NSUrl(string.Format(@"tel://{0}", phoneNumber));
                if( UIApplication.SharedApplication.CanOpenUrl(url) )
                    UIApplication.SharedApplication.OpenUrl(url);
            };
        }

        public void AddTextingIcon(string phoneNumber)
        {
            LeftDetailIconButton.SetAttributedTitle(GetAttributedStringWithImage(new UIImage("ios7-chatbubble-outline.png"), 25), UIControlState.Normal);

            LeftDetailIconButton.TouchUpInside += delegate
            {
                NSUrl url = new NSUrl(string.Format(@"sms:{0}", phoneNumber));
                if (UIApplication.SharedApplication.CanOpenUrl(url))
                    UIApplication.SharedApplication.OpenUrl(url);
            };
        }
        public void AddEmailIcon(string emailAddress)
        {
            RightDetailIconButton.SetAttributedTitle(GetAttributedStringWithImage(new UIImage("ios7-email-outline.png"), 25), UIControlState.Normal);


            RightDetailIconButton.TouchUpInside += delegate
            {
                if (MFMailComposeViewController.CanSendMail && ParentViewController != null)
                {
                    MFMailComposeViewController mailController = new MFMailComposeViewController();

                    mailController.SetToRecipients(new string[] { emailAddress });

                    mailController.Finished += (s, args) =>
                    {
                        Console.WriteLine(args.Result.ToString());
                        args.Controller.DismissViewController(true, null);
                    };


                    ParentViewController.PresentViewController(mailController, true, null);
                }
            };
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
