using System;
using System.Globalization;
using UIKit;
using Foundation;
using SiSystems.SharedModels;

namespace ConsultantApp.iOS
{
    public class TimeEntryCell : UITableViewCell
    {
        private TimeEntry Entry { get; set; }
		private UILabel _projectCodeField;
        private UITextField _hoursField;
		private UILabel _payRateLabel;

        private float _maxNumberOfHours;

        //Blocks
        public delegate void EntryChangedDelegate( TimeEntry entry );
        public EntryChangedDelegate EntryChanged;

        public TimeEntryCell (IntPtr handle) : base (handle)
        {
            TextLabel.Hidden = true;

			AddProjectCodeField();

            AddPayRateLabel();

            AddHoursField();

            setupConstraints();
        }

        private void AddHoursField()
        {
            var toolbar = new UIToolbar(new CoreGraphics.CGRect(0.0f, 0.0f, Frame.Size.Width, 44.0f))
            {
                Items = new[]
                {
                    new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
                    new UIBarButtonItem(UIBarButtonSystemItem.Done, doneButtonTapped)
                }
            };

            _hoursField = new UITextField
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                TextAlignment = UITextAlignment.Right,
                KeyboardType = UIKeyboardType.DecimalPad,
                BorderStyle = UITextBorderStyle.RoundedRect,
                InputAccessoryView = toolbar
            };

            AddHoursFieldDelegates();

            AddSubview(_hoursField);
        }

        private void AddHoursFieldDelegates()
        {
            _hoursField.EditingChanged += delegate
            {
                if (!HoursAreValid())
                    NotifyOfInvalidHoursAndSetToClosestValidHours();
            };
            _hoursField.EditingDidEnd += delegate { SetHoursAndChangeEntry(); };
            _hoursField.EditingDidBegin += delegate { this.BeginInvokeOnMainThread(SelectHoursTextFieldForEdit); };
        }

        private void AddPayRateLabel()
        {
            _payRateLabel = new UILabel
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                TextAlignment = UITextAlignment.Left,
                Font = UIFont.FromName("Helvetica", 10f),
                TextColor = StyleGuideConstants.MediumGrayUiColor
            };
            AddSubview(_payRateLabel);
        }

        private void AddProjectCodeField()
        {
            _projectCodeField = new UILabel
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                TextAlignment = UITextAlignment.Left,
                Font = UIFont.FromName("Helvetica-Bold", 12f)
            };
            AddSubview(_projectCodeField);
        }

        private void SelectHoursTextFieldForEdit()
        {
            _hoursField.SelectedTextRange = _hoursField.GetTextRange(_hoursField.BeginningOfDocument, _hoursField.EndOfDocument);
        }

        private void NotifyOfInvalidHoursAndSetToClosestValidHours()
        {
            SetValidHoursAndNotifyOfReason();
            SelectHoursTextFieldForEdit();
        }

        private void SetValidHoursAndNotifyOfReason()
        {
            if (TooFewHours())
            {
                ResetToMinimumHoursAndNotify();
            }
            if (TooManyHours())
            {
                ResetToMaximumHoursAndNotify();
            }
        }

        private void ResetToMaximumHoursAndNotify()
        {
            ShowInvalidTimeAlert("Please enter less than 24 hours of total entries for the day.");
            _hoursField.Text = _maxNumberOfHours.ToString(CultureInfo.InvariantCulture);
        }

        private void ResetToMinimumHoursAndNotify()
        {
            ShowInvalidTimeAlert("Unable to enter negative time entries.");
            _hoursField.Text = 0.ToString();
        }

        private void SetHoursAndChangeEntry()
        {
            Entry.Hours = HoursEnteredIfParsable();

            EntryChanged(Entry);
        }

        private bool HoursAreValid()
        {
            return !(TooFewHours() || TooManyHours());
        }

        private bool TooManyHours()
        {
            return HoursEnteredIfParsable() > _maxNumberOfHours;
        }

        private bool TooFewHours()
        {
            return HoursEnteredIfParsable() < 0;
        }

        private void ShowInvalidTimeAlert(string message)
        {
            InvokeOnMainThread(() =>
            {
                var invalidAlertView = new UIAlertView("Invalid Time", message, null, "Ok");
                invalidAlertView.Show();
            });
        }

        private float HoursEnteredIfParsable()
        {
            float hours;
            var parsed = float.TryParse(_hoursField.Text, out hours);
            return parsed ? hours : 0;
        }

        public void doneButtonTapped(object sender, EventArgs args)
		{
			_hoursField.ResignFirstResponder ();
		}

		//dismiss keyboard when tapping outside of text fields
		public override void TouchesBegan(NSSet touches, UIEvent evt)
		{
			base.TouchesBegan(touches, evt);

			_hoursField.ResignFirstResponder();
		}

        public void setupConstraints() 
        {
			AddConstraint( NSLayoutConstraint.Create(_projectCodeField, NSLayoutAttribute.Left, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.05f, 0f));
			AddConstraint(NSLayoutConstraint.Create(_projectCodeField, NSLayoutAttribute.Top, NSLayoutRelation.Equal, this, NSLayoutAttribute.Bottom, 0.05f, 0f));
			AddConstraint(NSLayoutConstraint.Create(_projectCodeField, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, this, NSLayoutAttribute.Bottom, 0.65f, 0f));
			AddConstraint(NSLayoutConstraint.Create(_projectCodeField, NSLayoutAttribute.Right, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.75f, 0f));

			AddConstraint( NSLayoutConstraint.Create(_payRateLabel, NSLayoutAttribute.Left, NSLayoutRelation.Equal, _projectCodeField, NSLayoutAttribute.Left, 1.0f, 0f));
			AddConstraint(NSLayoutConstraint.Create(_payRateLabel, NSLayoutAttribute.Top, NSLayoutRelation.Equal, _projectCodeField, NSLayoutAttribute.Bottom, 1.0f, 0f));
			AddConstraint(NSLayoutConstraint.Create(_payRateLabel, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, this, NSLayoutAttribute.Bottom, 0.95f, 0f));
			AddConstraint(NSLayoutConstraint.Create(_payRateLabel, NSLayoutAttribute.Right, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.75f, 0f));

			AddConstraint(NSLayoutConstraint.Create(_hoursField, NSLayoutAttribute.Left, NSLayoutRelation.Equal, _payRateLabel, NSLayoutAttribute.Right, 1.0f, 0f));
			AddConstraint(NSLayoutConstraint.Create(_hoursField, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterY, 1.0f, 0f));
			AddConstraint(NSLayoutConstraint.Create(_hoursField, NSLayoutAttribute.Height, NSLayoutRelation.Equal, this, NSLayoutAttribute.Height, 0.6f, 0f));
            AddConstraint(NSLayoutConstraint.Create(_hoursField, NSLayoutAttribute.Right, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.95f, 0f));
        }

		public void enable(bool shouldEnable )
		{
			if (shouldEnable) 
			{
				_hoursField.BorderStyle = UITextBorderStyle.RoundedRect;
				_hoursField.Enabled = true;
				_hoursField.TextColor = UIColor.Black;
			} else 
			{
				_hoursField.BorderStyle = UITextBorderStyle.None;
				_hoursField.Enabled = false;
				_hoursField.TextColor = UIColor.LightGray;
			}
		}

        public void UpdateCell(TimeEntry timeEntry, float maxNumberOfHours)
        {
            Entry = timeEntry;

            _projectCodeField.Text = Entry.CodeRate.PONumber;
            _payRateLabel.Text = Entry.CodeRate.ratedescription;
            _hoursField.Text = string.Format("{0}", Entry.Hours);
            _maxNumberOfHours = maxNumberOfHours;
        }

        public void FocusOnHours()
        {
            _hoursField.BecomeFirstResponder();
        }
    }
}