using Foundation;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using UIKit;

namespace AccountExecutiveApp.iOS
{
	partial class EditablePickerWithNumberFieldValueCell : UITableViewCell
	{
		public const string CellIdentifier = "EditablePickerWithNumberFieldValueCell";
		public UILabel MainTextLabel;
		public UITextField RightDetailTextField;
        public UITextField MidDetailTextField;

		private bool shrinkRightDetailText = true;

        public UIPickerView _midPicker;
        PickerViewModel _midPickerModel;

        public List<string> MidValues;
        public int midSelectedIndex = 0;

		public delegate void EditableCellDelegate(string newValue);
		public EditableCellDelegate OnMidValueChanged;

        public delegate void EditableCellFloatDelegate(float newValue);
        public EditableCellFloatDelegate OnRightValueChanged;

		public EditablePickerWithNumberFieldValueCell(IntPtr handle)
			: base(handle)
		{
			InitializeCell();
		}
        public EditablePickerWithNumberFieldValueCell(string cellId)
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
            CreateAndAddMidDetailTextField();
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

            RightDetailTextField.EditingDidBegin += delegate
            {
                this.BeginInvokeOnMainThread(SelectRightDetailTextFieldForEdit);

                try
                {
                    if (int.Parse(RightDetailTextField.Text) == 0)
                        RightDetailTextField.Text = string.Empty;
                }
                catch (Exception) { }
            };

            RightDetailTextField.EditingChanged += delegate
            {
                float textValue = 0;

                try
                {
                    if (RightDetailTextField.Text != string.Empty)
                        textValue = float.Parse(RightDetailTextField.Text);
                    else
                        textValue = 0;
                }
                catch (Exception) { }

                OnRightValueChanged(textValue);
            };


			var toolbar = new UIToolbar(new CoreGraphics.CGRect(0.0f, 0.0f, Frame.Size.Width, 44.0f));

			toolbar.Items = new[]
			{
				new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
				new UIBarButtonItem(UIBarButtonSystemItem.Done, rightDoneButtonTapped)
			};

			RightDetailTextField.InputAccessoryView = toolbar;

            RightDetailTextField.KeyboardType = UIKeyboardType.DecimalPad;

			AddSubview(RightDetailTextField);
		}
        private void SelectRightDetailTextFieldForEdit()
        {
            RightDetailTextField.SelectedTextRange = RightDetailTextField.GetTextRange(RightDetailTextField.BeginningOfDocument, RightDetailTextField.EndOfDocument);
        }
        private void CreateAndAddMidDetailTextField()
        {
            MidDetailTextField = new UITextField
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                TextAlignment = UITextAlignment.Right,
                Font = UIFont.FromName("Helvetica", 14f),
                TextColor = StyleGuideConstants.MediumGrayUiColor,
                Placeholder = "Contact Name"
            };

            MidDetailTextField.Text = string.Empty;

            _midPicker = new UIPickerView();
            _midPicker.BackgroundColor = UIColor.White;
            _midPickerModel = new PickerViewModel();
            _midPicker.Model = _midPickerModel;
            MidDetailTextField.InputView = _midPicker;

            _midPickerModel.OnValueChanged += delegate(string value)
            {
                MidDetailTextField.Text = value;
                OnMidValueChanged(value);
            };

            var toolbar = new UIToolbar(new CoreGraphics.CGRect(0.0f, 0.0f, Frame.Size.Width, 44.0f));

            toolbar.Items = new[]
			{
				new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
				new UIBarButtonItem(UIBarButtonSystemItem.Done, midDoneButtonTapped)
			};

            MidDetailTextField.InputAccessoryView = toolbar;

            AddSubview(MidDetailTextField);
        }


		public void rightDoneButtonTapped(object sender, EventArgs args)
		{
			RightDetailTextField.ResignFirstResponder();
		}
        public void midDoneButtonTapped(object sender, EventArgs args)
        {
            MidDetailTextField.ResignFirstResponder();
        }

		private void CreateAndAddMainTextLabel()
		{
			MainTextLabel = new UILabel
			{
				TranslatesAutoresizingMaskIntoConstraints = false,
				TextAlignment = UITextAlignment.Left,
				Font = UIFont.FromName("Helvetica", 16f),
				TextColor = UIColor.Black,
				Lines = 0
			};
			AddSubview(MainTextLabel);
		}

		public void SetupConstraints()
		{
			if (shrinkRightDetailText)
			{
				RightDetailTextField.AdjustsFontSizeToFitWidth = true;
				RightDetailTextField.MinimumFontSize = 10;

                MidDetailTextField.AdjustsFontSizeToFitWidth = true;
                MidDetailTextField.MinimumFontSize = 10;


				AddMainTextLabelConstraintsWithShrunkRightDetail();
                AddMidDetailTextLabelConstraints();
                AddShrunkRightDetailTextLabelConstraints();

			}
			else
			{
				AddRightDetailTextLabelConstraints();
				AddMainTextLabelConstraints();
                AddMidDetailTextLabelConstraints();
			}
		}

		private void AddMainTextLabelConstraints()
		{
			AddConstraint(NSLayoutConstraint.Create(MainTextLabel, NSLayoutAttribute.Left, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.05f, 0f));
			AddConstraint(NSLayoutConstraint.Create(MainTextLabel, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterY, 1f, -10f));
			AddConstraint(NSLayoutConstraint.Create(MainTextLabel, NSLayoutAttribute.Right, NSLayoutRelation.Equal, RightDetailTextField, NSLayoutAttribute.Left, 1.0f, 0f));
		}
		private void AddMainTextLabelConstraintsWithShrunkRightDetail()
		{
			AddConstraint(NSLayoutConstraint.Create(MainTextLabel, NSLayoutAttribute.Left, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.05f, 0f));
			AddConstraint(NSLayoutConstraint.Create(MainTextLabel, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterY, 1f, -10f));
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
			AddConstraint(NSLayoutConstraint.Create(RightDetailTextField, NSLayoutAttribute.Left, NSLayoutRelation.Equal, MidDetailTextField, NSLayoutAttribute.Right, 1.0f, 10f));
			AddConstraint(NSLayoutConstraint.Create(RightDetailTextField, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterY, 1.0f, 10f));
            AddConstraint(NSLayoutConstraint.Create(RightDetailTextField, NSLayoutAttribute.Width, NSLayoutRelation.GreaterThanOrEqual, null, NSLayoutAttribute.NoAttribute, 1.0f, 30f));
		}

        private void AddMidDetailTextLabelConstraints()
        {
            AddConstraint(NSLayoutConstraint.Create(MidDetailTextField, NSLayoutAttribute.Left, NSLayoutRelation.Equal, MainTextLabel, NSLayoutAttribute.Left, 1.0f, 8f));
            AddConstraint(NSLayoutConstraint.Create(MidDetailTextField, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterY, 1.0f, 10f));
            AddConstraint(NSLayoutConstraint.Create(MidDetailTextField, NSLayoutAttribute.Width, NSLayoutRelation.GreaterThanOrEqual, null, NSLayoutAttribute.NoAttribute, 1.0f, 40f));
            AddConstraint(NSLayoutConstraint.Create(MidDetailTextField, NSLayoutAttribute.Width, NSLayoutRelation.LessThanOrEqual, null, NSLayoutAttribute.NoAttribute, 1.0f, 80f));
        }

        public void UpdateCell(string mainText, List<string> newMidValues, string midSelectedValue, string rightDetailText)
        {
            int newMidSelectedIndex = IndexSelectionFromOptions(newMidValues, midSelectedValue);
            Update(mainText, newMidValues, newMidSelectedIndex, rightDetailText);
        }
        public void UpdateCell(string mainText, List<string> newMidValues, int newMidSelectedIndex, string rightDetailText)
		{
			Update(mainText, newMidValues, newMidSelectedIndex, rightDetailText);
		}
        private void Update(string mainText, List<string> newMidValues, int newMidSelectedIndex, string rightDetailText)
		{
			MainTextLabel.Text = mainText;
            MidDetailTextField.Text = newMidValues[newMidSelectedIndex];
            RightDetailTextField.Text = rightDetailText;

			MidValues = newMidValues;
			midSelectedIndex = newMidSelectedIndex;

			List<List<string>> midItems = new List<List<string>>();
			midItems.Add(MidValues);

			if (_midPicker != null && _midPickerModel != null)
			{
				_midPickerModel.items = midItems;
				_midPickerModel.scrollToItemIndex( _midPicker, newMidSelectedIndex, 0);
			}
		}

        private int IndexSelectionFromOptions(List<string> options, string value)
        {
            if (options != null && options.Contains(value))
                return options.FindIndex((string option) => { return option == value; });

            return 0;
        }
	}
}
