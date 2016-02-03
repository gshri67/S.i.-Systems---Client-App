using System;

using Foundation;
using UIKit;

namespace AccountExecutiveApp.iOS
{
    public class CreateContractJobDescriptionView : UIView
    {
        public CreateContractJobDescriptionView(IntPtr handle)
            : base(handle)
        {
            InitializeCell();
        }

        public CreateContractJobDescriptionView()
        {
            InitializeCell();
        }

        public ContractCreationDetailsHeaderView TextView;
        public BorderedButton CreateContractButton;

        private void InitializeCell()
        {
            BackgroundColor = UIColor.FromWhiteAlpha(0.95f, 1.0f);

            TextView = new ContractCreationDetailsHeaderView("Please use the Matchguide System to use Third-Party billing");
            TextView.TranslatesAutoresizingMaskIntoConstraints = false;
            AddSubview(TextView);

            CreateContractButton = new BorderedButton();
            CreateContractButton.SetTitle("Create Contract", UIControlState.Normal);
            CreateContractButton.SetTitleColor(UIColor.Blue, UIControlState.Normal);
            CreateContractButton.TranslatesAutoresizingMaskIntoConstraints = false;
            AddSubview(CreateContractButton);

            SetupConstraints();
        }

        public void SetupConstraints()
        {
            //Add text description view to the top
            AddConstraint(NSLayoutConstraint.Create(TextView, NSLayoutAttribute.Left, NSLayoutRelation.Equal, this, NSLayoutAttribute.Left, 1.0f, 0f));
            AddConstraint(NSLayoutConstraint.Create(TextView, NSLayoutAttribute.Right, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 1.0f, 0f));
            AddConstraint(NSLayoutConstraint.Create(TextView, NSLayoutAttribute.Top, NSLayoutRelation.Equal, this, NSLayoutAttribute.Top, 1.0f, 0f));
            AddConstraint(NSLayoutConstraint.Create(TextView, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, this, NSLayoutAttribute.Bottom, 0.5f, 0f));

            AddConstraint(NSLayoutConstraint.Create(CreateContractButton, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterX, 1.0f, 0f));
            AddConstraint(NSLayoutConstraint.Create(CreateContractButton, NSLayoutAttribute.Width, NSLayoutRelation.Equal, null, NSLayoutAttribute.NoAttribute, 1.0f, 175f));
            AddConstraint(NSLayoutConstraint.Create(CreateContractButton, NSLayoutAttribute.Top, NSLayoutRelation.Equal, TextView, NSLayoutAttribute.Bottom, 1.0f, 8f));
            AddConstraint(NSLayoutConstraint.Create(CreateContractButton, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, this, NSLayoutAttribute.Bottom, 1.0f, -8f));
        }

    }
}

