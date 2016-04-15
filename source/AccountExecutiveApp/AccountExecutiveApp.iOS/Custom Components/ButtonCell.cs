using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace AccountExecutiveApp.iOS
{
    partial class ButtonCell : UITableViewCell
    {
        public const string CellIdentifier = "ButtonCell";
        public BorderedButton MainButton;
        public delegate void ButtonCellDelegate();
        public ButtonCellDelegate OnButtonTapped;

        public ButtonCell(IntPtr handle)
            : base(handle)
        {
            InitializeCell();
        }
        public ButtonCell(string cellId)
			: base(UITableViewCellStyle.Default, cellId)
		{
			InitializeCell();
		}
			
        private void InitializeCell()
        {
            CreateAndAddMainButton();
            SetupConstraints();
        }

        private void CreateAndAddMainButton()
        {
            MainButton = new BorderedButton()
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                TitleLabel =
                {
                    TextAlignment = UITextAlignment.Center,
                    Font = UIFont.FromName("Helvetica", 16f),
                    TextColor = StyleGuideConstants.RedUiColor  
                },
            };

            MainButton.SetTitleColor(StyleGuideConstants.RedUiColor, UIControlState.Normal);
            MainButton.SetTitle("Button", UIControlState.Normal); 

            AddSubview(MainButton);

            MainButton.TouchUpInside += delegate
            {
                if( OnButtonTapped != null )
                    OnButtonTapped();
            };
        }

			
        public void SetupConstraints()
        {
            AddMainButtonConstraints();
        }

        private void AddMainButtonConstraints()
        {
            AddConstraint(NSLayoutConstraint.Create(MainButton, NSLayoutAttribute.Left, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.05f, 0f));
            AddConstraint(NSLayoutConstraint.Create(MainButton, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterY, 1.0f, 0f));
            AddConstraint(NSLayoutConstraint.Create(MainButton, NSLayoutAttribute.Right, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.95f, 0f));
            AddConstraint(NSLayoutConstraint.Create(MainButton, NSLayoutAttribute.Height, NSLayoutRelation.Equal, this, NSLayoutAttribute.Height, 0.9f, 0f));
        }

        public void UpdateCell(string buttonText)
        {
            MainButton.SetTitle(buttonText, UIControlState.Normal);
        }
    }
}
