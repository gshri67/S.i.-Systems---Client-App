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
            MainTextLabel.AdjustsFontSizeToFitWidth = true;
            MainTextLabel.MinimumFontSize = 10;

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
            AddConstraint(NSLayoutConstraint.Create(RightDetailTextField, NSLayoutAttribute.Width, NSLayoutRelation.GreaterThanOrEqual, null, NSLayoutAttribute.NoAttribute, 1.0f, 40f));

        }

        public void EnableInputFields( bool enabled )
        {
            RightDetailTextField.UserInteractionEnabled = enabled;
        }

        public void UpdateCell(string mainText, List<string> newValues, string selectedValue)
        {
            if (newValues == null || newValues.Count == 0)
                Update(mainText, newValues, string.Empty);
            else
            {
                selectedIndex = IndexSelectionFromOptions(newValues, selectedValue);
                Update(mainText, newValues, newValues[selectedIndex]);
            }
        }
        public void UpdateCell(string mainText, List<string> newValues, int newSelectedIndex )
        {
            if (newValues == null || newValues.Count == 0)
                Update(mainText, newValues, string.Empty);
            else
            {
                selectedIndex = newSelectedIndex;
                Update(mainText, newValues, newValues[selectedIndex]);
            }
        }
        //If just using options {Yes, No}
        public void UpdateCell(string mainText, bool selectedValue)
        {
            selectedIndex = IndexBooleanSelectionFromOptions(Values, selectedValue);
            Update(mainText, BooleanOptions, BooleanOptions[BooleanOptionIndex(selectedValue)] );
        }
        private void Update(string mainText, List<string> newValues, string selectedValue )
        {
            Console.WriteLine("Updating Cell ");

            MainTextLabel.Text = mainText;
            RightDetailTextField.Text = selectedValue;
            Values = newValues;
            selectedIndex = IndexSelectionFromOptions(Values, selectedValue);

            List<List<string>> items = new List<List<string>>();
            items.Add(Values);

            if (_picker != null && _pickerModel != null)
            {
                _pickerModel.items = items;
                _pickerModel.scrollToItemIndex(_picker, selectedIndex, 0);
            }
        }

        private int IndexSelectionFromOptions(List<string> options, string value)
        {
            if (options != null && options.Contains(value))
                return options.FindIndex((string option) => { return option == value; });

            return 0;
        }

        private int IndexBooleanSelectionFromOptions(List<string> options, bool booleanValue)
        {
            string value = BooleanOptions[0];

            if (booleanValue == false)
                value = BooleanOptions[1];

            if (options != null && options.Contains(value))
                return options.FindIndex((string option) => { return option == value; });

            return 0;
        }

        public List<string> BooleanOptions { get { return new List<string>(new string[] { "Yes", "No" }); } }

        public int BooleanOptionIndex(bool value)
        {
            if (value == true)
                return 0;
            return 1;
        }
    }
}
