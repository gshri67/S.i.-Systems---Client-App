using Foundation;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using UIKit;

namespace AccountExecutiveApp.iOS
{
	partial class EditableDoubleTextFieldCell : UITableViewCell
	{
		public const string CellIdentifier = "EditableDoubleTextFieldCell";
		public UILabel MainTextLabel;
		public UITextField RightDetailTextField;
        public UITextField MidDetailTextField;

		private bool shrinkRightDetailText = true;


		public delegate void EditableCellDelegate(string newValue);
		public EditableCellDelegate OnMidValueChanged;
        public EditableCellDelegate OnRightValueChanged;

		public EditableDoubleTextFieldCell(IntPtr handle)
			: base(handle)
		{
			InitializeCell();
		}
        public EditableDoubleTextFieldCell(string cellId)
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
			AddConstraint(NSLayoutConstraint.Create(RightDetailTextField, NSLayoutAttribute.Left, NSLayoutRelation.GreaterThanOrEqual, MidDetailTextField, NSLayoutAttribute.Right, 1.0f, 10f));
			AddConstraint(NSLayoutConstraint.Create(RightDetailTextField, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterY, 1.0f, 8f));
		}

        private void AddMidDetailTextLabelConstraints()
        {
            AddConstraint(NSLayoutConstraint.Create(MidDetailTextField, NSLayoutAttribute.Left, NSLayoutRelation.Equal, MainTextLabel, NSLayoutAttribute.Left, 1.0f, 8f));
            AddConstraint(NSLayoutConstraint.Create(MidDetailTextField, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterY, 1.0f, 10f));
            AddConstraint(NSLayoutConstraint.Create(MidDetailTextField, NSLayoutAttribute.Width, NSLayoutRelation.GreaterThanOrEqual, null, NSLayoutAttribute.NoAttribute, 1.0f, 40f));
        }

        public void UpdateCell(string mainText, string newMidSelectedValue,
            bool newRightSelectedValue)
        {
            UpdateCell(mainText, newMidSelectedValue, BooleanOptions[IndexBooleanSelectionFromOptions(BooleanOptions, newRightSelectedValue)]);
        }

	    public void UpdateCell(string mainText, string newMidSelectedValue,
	        string newRightSelectedValue)
	    {
            Update(mainText, newMidSelectedValue, newRightSelectedValue);
	    }

	    private void Update(string mainText, string newMidSelectedValue,
	        string newRightSelectedValue)
	    {
            MainTextLabel.Text = mainText;
            RightDetailTextField.Text = newRightSelectedValue;
            MidDetailTextField.Text = newMidSelectedValue;
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
