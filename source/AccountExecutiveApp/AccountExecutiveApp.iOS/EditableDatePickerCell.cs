using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace AccountExecutiveApp.iOS
{
    partial class EditableDatePickerCell : UITableViewCell
    {
        public const string CellIdentifier = "EditableDatePickerCell";
        public UILabel MainTextLabel;
        public UITextField RightDetailTextField;
        private bool shrinkRightDetailText = true;

        public delegate void EditableCellDelegate(DateTime newValue);
        public EditableCellDelegate OnValueChanged;

        public EditableDatePickerCell(IntPtr handle)
            : base(handle)
        {
            InitializeCell();
        }
        public EditableDatePickerCell(string cellId)
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
                TranslatesAutoresizingMaskIntoConstraints = false
                //TextAlignment = UITextAlignment.Right,
                //Font = UIFont.FromName("Helvetica", 14f),
                //TextColor = StyleGuideConstants.MediumGrayUiColor
            };

            RightDetailTextField.Text = "Text Field";

            UIDatePicker picker = new UIDatePicker();
            
            RightDetailTextField.InputView = picker;
            
            picker.ValueChanged += delegate
            {
                NSDateFormatter dateFormatter = new NSDateFormatter();
                dateFormatter.DateFormat = "MMM dd, YYYY";
                string dateString = dateFormatter.ToString(picker.Date);

                RightDetailTextField.Text =  dateString;

                OnValueChanged( DateTimeFromNSDate(picker.Date) );
            };
            picker.Mode = UIDatePickerMode.Date;

            AddSubview(RightDetailTextField);

            var toolbar = new UIToolbar(new CoreGraphics.CGRect(0.0f, 0.0f, Frame.Size.Width, 44.0f));

            toolbar.Items = new[]
			{
				new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
				new UIBarButtonItem(UIBarButtonSystemItem.Done, doneButtonTapped)
			};

            RightDetailTextField.InputAccessoryView = toolbar;
        }

        public DateTime DateTimeFromNSDate( NSDate nsDate )
        {
            DateTime dateTime = new DateTime(2001, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);
            return dateTime.AddSeconds(nsDate.SecondsSinceReferenceDate);
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
