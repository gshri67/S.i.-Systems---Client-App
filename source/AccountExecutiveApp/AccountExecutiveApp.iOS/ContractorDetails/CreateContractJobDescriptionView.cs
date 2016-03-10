using System;

using Foundation;
using UIKit;

namespace AccountExecutiveApp.iOS
{
    public class CreateContractJobDescriptionView : UIView
    {
        private string _description = string.Empty; 

        public CreateContractJobDescriptionView(IntPtr handle)
            : base(handle)
        {
            InitializeCell();
        }

        public CreateContractJobDescriptionView()
        {
            InitializeCell();
        }

        public CreateContractJobDescriptionView( string text )
        {
            _description = text;
            InitializeCell();
        }

        public ContractCreationDetailsHeaderView TextView;
        public BorderedButton CreateContractButton;
        private UIView ContainerView;

        private void InitializeCell()
        {
            UIImage backgroundImage = UIImage.FromFile("subtle_dots");
            BackgroundColor = UIColor.FromPatternImage(backgroundImage);

            ContainerView = new UIView();
            ContainerView.TranslatesAutoresizingMaskIntoConstraints = false;
            AddSubview(ContainerView);

            TextView = new ContractCreationDetailsHeaderView(_description);
            TextView.TranslatesAutoresizingMaskIntoConstraints = false;
            TextView.BackgroundColor = UIColor.Clear;
            ContainerView.AddSubview(TextView);

            CreateContractButton = new BorderedButton();
            CreateContractButton.SetTitle("Create Contract", UIControlState.Normal);
            CreateContractButton.SetTitleColor(StyleGuideConstants.RedUiColor, UIControlState.Normal);
            CreateContractButton.TranslatesAutoresizingMaskIntoConstraints = false;
            ContainerView.AddSubview(CreateContractButton);

            CreateContractButton.BackgroundColor = UIColor.White;

            SetupConstraints();
        }

        public void SetupConstraints()
        {
            int separation = 8;

            AddConstraint(NSLayoutConstraint.Create(ContainerView, NSLayoutAttribute.Left, NSLayoutRelation.Equal, this, NSLayoutAttribute.Left, 1.0f, 0f));
            AddConstraint(NSLayoutConstraint.Create(ContainerView, NSLayoutAttribute.Right, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 1.0f, 0f));
            AddConstraint(NSLayoutConstraint.Create(ContainerView, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterY, 1.0f, 0f));
            
            //Add text description view to the top
            AddConstraint(NSLayoutConstraint.Create(TextView, NSLayoutAttribute.Left, NSLayoutRelation.Equal, ContainerView, NSLayoutAttribute.Left, 1.0f, 0f));
            AddConstraint(NSLayoutConstraint.Create(TextView, NSLayoutAttribute.Right, NSLayoutRelation.Equal, ContainerView, NSLayoutAttribute.Right, 1.0f, 0f));
            AddConstraint(NSLayoutConstraint.Create(TextView, NSLayoutAttribute.Top, NSLayoutRelation.Equal, ContainerView, NSLayoutAttribute.Top, 1.0f, 0f));

            AddConstraint(NSLayoutConstraint.Create(CreateContractButton, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, ContainerView, NSLayoutAttribute.CenterX, 1.0f, 0f));
            AddConstraint(NSLayoutConstraint.Create(CreateContractButton, NSLayoutAttribute.Width, NSLayoutRelation.Equal, null, NSLayoutAttribute.NoAttribute, 1.0f, 175f));
            AddConstraint(NSLayoutConstraint.Create(CreateContractButton, NSLayoutAttribute.Top, NSLayoutRelation.Equal, TextView, NSLayoutAttribute.Bottom, 1.0f, separation/2));
            AddConstraint(NSLayoutConstraint.Create(CreateContractButton, NSLayoutAttribute.Height, NSLayoutRelation.Equal, null, NSLayoutAttribute.NoAttribute, 1.0f, 45));

            //Pin to subviews
            AddConstraint(NSLayoutConstraint.Create(ContainerView, NSLayoutAttribute.Top, NSLayoutRelation.Equal, TextView, NSLayoutAttribute.Top, 1.0f, 0f));
            AddConstraint(NSLayoutConstraint.Create(ContainerView, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, CreateContractButton, NSLayoutAttribute.Bottom, 1.0f, 0f));

            //Make sure it's no bigger than superview
            AddConstraint(NSLayoutConstraint.Create(ContainerView, NSLayoutAttribute.Top, NSLayoutRelation.GreaterThanOrEqual, this, NSLayoutAttribute.Top, 1.0f, 4f));
            AddConstraint(NSLayoutConstraint.Create(ContainerView, NSLayoutAttribute.Bottom, NSLayoutRelation.LessThanOrEqual, this, NSLayoutAttribute.Bottom, 1.0f, -4f));
        }

    }
}

