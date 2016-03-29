using Foundation;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using UIKit;

namespace AccountExecutiveApp.iOS
{
    partial class EditablePickerCell : UITableViewCell
    {
        public const string CellIdentifier = "EditablePickerCell";
        public UILabel MainTextLabel;
        public UITextField RightDetailTextField;
        private bool shrinkRightDetailText = true;
        public UIPickerView _picker;
        PickerViewModel _pickerModel;

        public List<string> Values;
        public int selectedIndex = 0;

        public delegate void EditableCellDelegate(string newValue);
        public EditableCellDelegate OnValueChanged;

        public EditablePickerCell(IntPtr handle)
            : base(handle)
        {
            InitializeCell();
        }
        public EditablePickerCell(string cellId)
            : base(UITableViewCellStyle.Default, cellId)
        {
            InitializeCell();
        }

        private void InitializeCell()
        {
            CreateAndAddLabels();

            SetupConstraints();
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

            _picker = new UIPickerView();
            _picker.BackgroundColor = UIColor.White;
            _pickerModel = new PickerViewModel();
            _picker.Model = _pickerModel;
            RightDetailTextField.InputView = _picker;

            _pickerModel.OnValueChanged += delegate(string value)
            {
                RightDetailTextField.Text = value;
                OnValueChanged(value);
            };

            var toolbar = new UIToolbar(new CoreGraphics.CGRect(0.0f, 0.0f, Frame.Size.Width, 44.0f));

            toolbar.Items = new[]
			{
				new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
				new UIBarButtonItem(UIBarButtonSystemItem.Done, doneButtonTapped)
			};

            RightDetailTextField.InputAccessoryView = toolbar;

            AddSubview(RightDetailTextField);
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
                TextColor = UIColor.Black,
                //Lines = 0
            };
            AddSubview(MainTextLabel);
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
            AddConstraint(NSLayoutConstraint.Create(MainTextLabel, NSLayoutAttribute.Right, NSLayoutRelation.GreaterThanOrEqual, this, NSLayoutAttribute.Right, 0.3f, 0f));
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

        public void UpdateCell(string mainText, List<string> newValues, int newSelectedIndex)
        {
            Console.WriteLine("Updating Cell ");

            MainTextLabel.Text = mainText;
            RightDetailTextField.Text = newValues[newSelectedIndex];

            Values = newValues;
            selectedIndex = newSelectedIndex;

            List<List<string>> items = new List<List<string>>();
            items.Add(Values);

            if (_picker != null && _pickerModel != null)
            {
                _pickerModel.items = items;
                _pickerModel.scrollToItemIndex( _picker, newSelectedIndex, 0);
            }
        }
    }
}
