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

        public delegate void EditableCellDelegate(float newValue);
        public EditableCellDelegate OnValueChanged;

        private bool _usingDollarSign = false;
        public bool UsingDollarSign {
            get { return _usingDollarSign; }
            set { _usingDollarSign = value; ApplyDollarSignIfApplicable(); }
        }

        public EditableNumberFieldCell(IntPtr handle)
            : base(handle)
        {
            InitializeCell();
        }

        private void InitializeCell()
        {
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
                TranslatesAutoresizingMaskIntoConstraints = false,
                TextAlignment = UITextAlignment.Right,
                Font = UIFont.FromName("Helvetica", 14f),
                TextColor = StyleGuideConstants.MediumGrayUiColor
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

            RightDetailTextField.EditingChanged += delegate
            {
                float textValue;
                ApplyDollarSignIfApplicable();

                if (RightDetailNumberText() != string.Empty)
                    textValue = float.Parse(RightDetailNumberText());
                else
                    textValue = 0;

                OnValueChanged( textValue );
            };

            RightDetailTextField.EditingDidBegin += delegate
            {
                RightDetailTextField.EditingDidBegin += delegate { this.BeginInvokeOnMainThread(SelectRightDetailTextFieldForEdit); };

                try
                {
                    if (int.Parse(RightDetailNumberText()) == 0)
                        RightDetailTextField.Text = string.Empty;
                }
                catch (Exception){}
            };

            RightDetailTextField.EditingDidEnd += delegate
            {
                if ( RightDetailTextField.Text == string.Empty )
                    RightDetailTextField.Text = "0";

                ApplyDollarSignIfApplicable();
            };

        }
        private void SelectRightDetailTextFieldForEdit()
        {
            RightDetailTextField.SelectedTextRange = RightDetailTextField.GetTextRange(RightDetailTextField.BeginningOfDocument, RightDetailTextField.EndOfDocument);
        }

        private string RightDetailNumberText()
        {
            int dollarIndex = RightDetailTextField.Text.IndexOf('$');

            if (dollarIndex >= 0)
                return RightDetailTextField.Text.Remove(dollarIndex, 1);
            return RightDetailTextField.Text;
        }

        private void ApplyDollarSignIfApplicable()
        {
            int dollarIndex = RightDetailTextField.Text.IndexOf('$');

            if (dollarIndex >= 0)
                RightDetailTextField.Text = RightDetailTextField.Text.Remove(dollarIndex, 1);

            if (UsingDollarSign)
                RightDetailTextField.Text = RightDetailTextField.Text.Insert(0, "$");        
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
            AddConstraint(NSLayoutConstraint.Create(RightDetailTextField, NSLayoutAttribute.Left, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.6f, 0f));
            //AddConstraint(NSLayoutConstraint.Create(RightDetailTextView, NSLayoutAttribute.Left, NSLayoutRelation.LessThanOrEqual, this, NSLayoutAttribute.Right, 0.80f, 0f));
            AddConstraint(NSLayoutConstraint.Create(RightDetailTextField, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterY, 1.0f, 0f));
            AddConstraint(NSLayoutConstraint.Create(RightDetailTextField, NSLayoutAttribute.Right, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.90f, 0f));
        }
        private void AddShrunkRightDetailTextLabelConstraints()
        {
            AddConstraint(NSLayoutConstraint.Create(RightDetailTextField, NSLayoutAttribute.Right, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.90f, 0f));
            AddConstraint(NSLayoutConstraint.Create(RightDetailTextField, NSLayoutAttribute.Left, NSLayoutRelation.Equal, MainTextLabel, NSLayoutAttribute.Right, 1.0f, 15f));
            //AddConstraint(NSLayoutConstraint.Create(RightDetailTextView, NSLayoutAttribute.Left, NSLayoutRelation.LessThanOrEqual, this, NSLayoutAttribute.Right, 0.80f, 0f));
            AddConstraint(NSLayoutConstraint.Create(RightDetailTextField, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterY, 1.0f, 0f));
        }

        public void UpdateCell(string mainText, string rightDetailText)
        {
            MainTextLabel.Text = mainText;
            RightDetailTextField.Text = rightDetailText;
        }
    }
}
