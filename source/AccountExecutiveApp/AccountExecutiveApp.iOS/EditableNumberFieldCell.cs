using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace AccountExecutiveApp.iOS
{
    partial class EditableNumberFieldCell : UITableViewCell
    {
        public const string CellIdentifier = "EditableNumberFieldCell";
        public UILabel MainTextLabel;
        public UITextField RightDetailTextField;
        private bool shrinkRightDetailText = true;

        public delegate void EditableCellDelegate(string newValue);
        public EditableCellDelegate OnValueChanged;

        public EditableNumberFieldCell(IntPtr handle)
            : base(handle)
        {
            InitializeCell();
        }

        private void InitializeCell()
        {
            this.Accessory = UITableViewCellAccessory.DisclosureIndicator;

            CreateAndAddLabels();

            SetupConstraints();

            RightDetailTextField.KeyboardType = UIKeyboardType.DecimalPad;
        }

        private void CreateAndAddLabels()
        {
            CreateAndAddMainTextLabel();

            CreateAndAddRightDetailTextField();
        }

        private void CreateAndAddRightDetailTextField()
        {
            RightDetailTextField = new UITextField
            {
                TranslatesAutoresizingMaskIntoConstraints = false
                //TextAlignment = UITextAlignment.Right,
                //Font = UIFont.FromName("Helvetica", 14f),
                //TextColor = StyleGuideConstants.MediumGrayUiColor
            };

            RightDetailTextField.Text = "Text Field";

            AddSubview(RightDetailTextField);

            var toolbar = new UIToolbar(new CoreGraphics.CGRect(0.0f, 0.0f, Frame.Size.Width, 44.0f));

            toolbar.Items = new[]
			{
				new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
				new UIBarButtonItem(UIBarButtonSystemItem.Done, doneButtonTapped)
			};

            RightDetailTextField.InputAccessoryView = toolbar;
        }

        public void doneButtonTapped(object sender, EventArgs args)
        {
            RightDetailTextField.ResignFirstResponder();
        }

        private void CreateAndAddMainTextLabel()
        {
            MainTextLabel = new UILabel
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                TextAlignment = UITextAlignment.Left,
                Font = UIFont.FromName("Helvetica", 16f),
                TextColor = UIColor.Black
            };
            AddSubview(MainTextLabel);
        }

        public EditableNumberFieldCell(string cellId)
            : base(UITableViewCellStyle.Default, cellId)
        {
            InitializeCell();
        }

        public void SetupConstraints()
        {
            if (shrinkRightDetailText)
            {
                RightDetailTextField.AdjustsFontSizeToFitWidth = true;
                RightDetailTextField.MinimumFontSize = 10;
                AddMainTextLabelConstraintsWithShrunkRightDetail();
                AddShrunkRightDetailTextLabelConstraints();
            }
            else
            {
                AddRightDetailTextLabelConstraints();
                AddMainTextLabelConstraints();
            }
        }

        private void AddMainTextLabelConstraints()
        {
            AddConstraint(NSLayoutConstraint.Create(MainTextLabel, NSLayoutAttribute.Left, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.05f, 0f));
            AddConstraint(NSLayoutConstraint.Create(MainTextLabel, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterY, 1f, 0f));
            AddConstraint(NSLayoutConstraint.Create(MainTextLabel, NSLayoutAttribute.Right, NSLayoutRelation.Equal, RightDetailTextField, NSLayoutAttribute.Left, 1.0f, 0f));
        }
        private void AddMainTextLabelConstraintsWithShrunkRightDetail()
        {
            AddConstraint(NSLayoutConstraint.Create(MainTextLabel, NSLayoutAttribute.Left, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.05f, 0f));
            AddConstraint(NSLayoutConstraint.Create(MainTextLabel, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterY, 1f, 0f));
            AddConstraint(NSLayoutConstraint.Create(MainTextLabel, NSLayoutAttribute.Right, NSLayoutRelation.LessThanOrEqual, this, NSLayoutAttribute.Right, 0.7f, 0f));
        }

        private void AddRightDetailTextLabelConstraints()
        {
            AddConstraint(NSLayoutConstraint.Create(RightDetailTextField, NSLayoutAttribute.Left, NSLayoutRelation.GreaterThanOrEqual, this, NSLayoutAttribute.Right, 0.6f, 0f));
            AddConstraint(NSLayoutConstraint.Create(RightDetailTextField, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterY, 1.0f, 0f));
            AddConstraint(NSLayoutConstraint.Create(RightDetailTextField, NSLayoutAttribute.Right, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.90f, 0f));
        }
        private void AddShrunkRightDetailTextLabelConstraints()
        {
            AddConstraint(NSLayoutConstraint.Create(RightDetailTextField, NSLayoutAttribute.Right, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.90f, 0f));
            AddConstraint(NSLayoutConstraint.Create(RightDetailTextField, NSLayoutAttribute.Left, NSLayoutRelation.GreaterThanOrEqual, MainTextLabel, NSLayoutAttribute.Right, 1.0f, 15f));
            AddConstraint(NSLayoutConstraint.Create(RightDetailTextField, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterY, 1.0f, 0f));
        }

        public void UpdateCell(string mainText, string rightDetailText)
        {
            MainTextLabel.Text = mainText;
            RightDetailTextField.Text = rightDetailText;
        }
    }
}
