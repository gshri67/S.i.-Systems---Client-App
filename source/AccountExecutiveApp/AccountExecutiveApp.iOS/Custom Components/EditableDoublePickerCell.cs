using Foundation;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using UIKit;

namespace AccountExecutiveApp.iOS
{
	partial class EditableDoublePickerCell : UITableViewCell
	{
		public const string CellIdentifier = "EditableDoublePickerCell";
		public UILabel MainTextLabel;
		public UITextField RightDetailTextField;
        public UITextField MidDetailTextField;

		private bool shrinkRightDetailText = true;

        public UIPickerView _rightPicker;
		PickerViewModel _rightPickerModel;

        public UIPickerView _midPicker;
        PickerViewModel _midPickerModel;

		public List<string> RightValues;
		public int rightSelectedIndex = 0;

        public List<string> MidValues;
        public int midSelectedIndex = 0;

		public delegate void EditableCellDelegate(string newValue);
		public EditableCellDelegate OnMidValueChanged;
        public EditableCellDelegate OnRightValueChanged;

		public EditableDoublePickerCell(IntPtr handle)
			: base(handle)
		{
			InitializeCell();
		}
		public EditableDoublePickerCell(string cellId)
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

			_rightPicker = new UIPickerView();
            _rightPicker.BackgroundColor = UIColor.White;
			_rightPickerModel = new PickerViewModel();
            _rightPicker.Model = _rightPickerModel;
            RightDetailTextField.InputView = _rightPicker;

            _rightPickerModel.OnValueChanged += delegate(string value)
			{
				RightDetailTextField.Text = value;
				OnRightValueChanged(value);
			};

			var toolbar = new UIToolbar(new CoreGraphics.CGRect(0.0f, 0.0f, Frame.Size.Width, 44.0f));

			toolbar.Items = new[]
			{
				new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
				new UIBarButtonItem(UIBarButtonSystemItem.Done, rightDoneButtonTapped)
			};

			RightDetailTextField.InputAccessoryView = toolbar;

			AddSubview(RightDetailTextField);
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
				Lines = 1
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
			AddConstraint(NSLayoutConstraint.Create(RightDetailTextField, NSLayoutAttribute.Left, NSLayoutRelation.GreaterThanOrEqual, MidDetailTextField, NSLayoutAttribute.Right, 1.0f, 10f));
			AddConstraint(NSLayoutConstraint.Create(RightDetailTextField, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterY, 1.0f, 8f));
		}

        private void AddMidDetailTextLabelConstraints()
        {
            AddConstraint(NSLayoutConstraint.Create(MidDetailTextField, NSLayoutAttribute.Left, NSLayoutRelation.Equal, MainTextLabel, NSLayoutAttribute.Left, 1.0f, 8f));
            AddConstraint(NSLayoutConstraint.Create(MidDetailTextField, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterY, 1.0f, 10f));
            AddConstraint(NSLayoutConstraint.Create(MidDetailTextField, NSLayoutAttribute.Width, NSLayoutRelation.GreaterThanOrEqual, null, NSLayoutAttribute.NoAttribute, 1.0f, 40f));
        }

	    public void EnableMiddleValue( bool enabled )
	    {
	        if (!enabled)
	        {
	            MidDetailTextField.UserInteractionEnabled = false;
	            MidDetailTextField.TextColor = MainTextLabel.TextColor;
	        }
            else
            {
                MidDetailTextField.UserInteractionEnabled = true;
                MidDetailTextField.TextColor = RightDetailTextField.TextColor;
            }
	    }

        public void UpdateCell(string mainText, string newMidSelectedValue, bool newRightSelectedValue)
        {
            UpdateCell(mainText, new List<string>(){ newMidSelectedValue }, 0, BooleanOptions, IndexBooleanSelectionFromOptions(BooleanOptions, newRightSelectedValue));
        }
	    public void UpdateCell(string mainText, string newMidSelectedValue, List<string> newRightValues, string newRightSelectedValue)
        {
            UpdateCell(mainText, new List<string>() { newMidSelectedValue }, 0, newRightValues, IndexSelectionFromOptions(newRightValues, newRightSelectedValue));
        }
        public void UpdateCell(string mainText, List<string> newMidValues, string newMidSelectedValue,
            bool newRightSelectedValue)
        {
            UpdateCell(mainText, newMidValues, IndexSelectionFromOptions(newMidValues, newMidSelectedValue), BooleanOptions, IndexBooleanSelectionFromOptions(BooleanOptions, newRightSelectedValue));
        }
	    public void UpdateCell(string mainText, List<string> newMidValues,string newMidSelectedValue,
	        List<string> newRightValues, string newRightSelectedValue)
	    {
            UpdateCell( mainText, newMidValues, IndexSelectionFromOptions(newMidValues, newMidSelectedValue), newRightValues, IndexSelectionFromOptions(newRightValues, newRightSelectedValue) );
	    }
	    public void UpdateCell(string mainText, List<string> newMidValues, int newMidSelectedIndex, List<string> newRightValues, int newRightSelectedIndex)
		{
			MainTextLabel.Text = mainText;
			RightDetailTextField.Text = newRightValues[newRightSelectedIndex];

	        if (newMidSelectedIndex >= 0 && newMidValues != null && newMidSelectedIndex < newMidValues.Count)
	        {
	            MidDetailTextField.Text = newMidValues[newMidSelectedIndex];

	            MidValues = newMidValues;
	            midSelectedIndex = newMidSelectedIndex;

	            List<List<string>> midItems = new List<List<string>>();
	            midItems.Add(MidValues);

	            if (_midPicker != null && _midPickerModel != null)
	            {
	                _midPickerModel.items = midItems;
	                _midPickerModel.scrollToItemIndex(_midPicker, newMidSelectedIndex, 0);
	            }
	        }


	        RightValues = newRightValues;
			rightSelectedIndex = newRightSelectedIndex;

			List<List<string>> rightItems = new List<List<string>>();
			rightItems.Add(RightValues);

            if (_rightPicker != null && _rightPickerModel != null)
            {
                _rightPickerModel.items = rightItems;
                _rightPickerModel.scrollToItemIndex(_rightPicker, newRightSelectedIndex, 0);
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
