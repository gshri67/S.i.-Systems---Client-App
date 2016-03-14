using System;
using UIKit;
using Foundation;
using SiSystems.SharedModels;

namespace ConsultantApp.iOS
{
    public class TimeEntryCell : UITableViewCell
    {
        private TimeEntry Entry { get; set; }
		private UILabel clientField;
        private UILabel projectCodeField;
        private UITextField hoursField;
		private UILabel payRateLabel;

        private float _maxNumberOfHours;

        //Blocks
        public delegate void EntryChangedDelegate( TimeEntry entry );
        public EntryChangedDelegate EntryChanged;

        public TimeEntryCell (IntPtr handle) : base (handle)
        {
            TextLabel.Hidden = true;

			clientField = new UILabel();
			clientField.Text = "DevFacto";
			clientField.TranslatesAutoresizingMaskIntoConstraints = false;
			AddSubview( clientField );

			projectCodeField = new UILabel();
			projectCodeField.Text = "Project Code";
			projectCodeField.TranslatesAutoresizingMaskIntoConstraints = false;
			projectCodeField.TextAlignment = UITextAlignment.Left;
			projectCodeField.Font =  UIFont.FromName("Helvetica-Bold", 12f);
			AddSubview( projectCodeField );

			payRateLabel = new UILabel();
			payRateLabel.TranslatesAutoresizingMaskIntoConstraints = false;
			payRateLabel.TextAlignment = UITextAlignment.Left;
			payRateLabel.Font =  UIFont.FromName("Helvetica", 10f);
			payRateLabel.TextColor = StyleGuideConstants.MediumGrayUiColor;
			AddSubview( payRateLabel );

			clientField.TextColor = UIColor.FromWhiteAlpha (0.55f, 1.0f);

			clientField.Hidden = true;

            hoursField = new UITextField();
            hoursField.Text = "8";
            hoursField.TranslatesAutoresizingMaskIntoConstraints = false;
            hoursField.TextAlignment = UITextAlignment.Right;
            hoursField.EditingDidEnd += delegate
            {
                Entry.Hours = ValidatedHours();

                EntryChanged(Entry);
            };

			hoursField.EditingDidBegin += delegate {
				this.BeginInvokeOnMainThread ( delegate {
					hoursField.SelectedTextRange = hoursField.GetTextRange(hoursField.BeginningOfDocument, hoursField.EndOfDocument);
				});
			};

			hoursField.KeyboardType = UIKeyboardType.DecimalPad;

			var toolbar = new UIToolbar(new CoreGraphics.CGRect(0.0f, 0.0f, Frame.Size.Width, 44.0f));

			toolbar.Items = new[]
			{
				new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
				new UIBarButtonItem(UIBarButtonSystemItem.Done, doneButtonTapped)
			};

			hoursField.InputAccessoryView = toolbar;
			hoursField.BorderStyle = UITextBorderStyle.RoundedRect;

            AddSubview( hoursField );

            setupConstraints();
        }

        private float ValidatedHours()
        {
            var hours = HoursEnteredIfParsable();

            if (hours < 0)
            {
                AlertOfMinimumValue();
                hours = 0;
            } else if (hours > _maxNumberOfHours)
            {
                AlertOfMaximumValue();
                hours = _maxNumberOfHours;
            }

            return hours;
        }

        private void AlertOfMinimumValue()
        {
            InvokeOnMainThread(() =>
            {
                var invalidAlertView = new UIAlertView("Invalid Time", "Unable to enter negative time entries.", null, "Ok");
                invalidAlertView.Show();
            });
        }

        private void AlertOfMaximumValue()
        {
            InvokeOnMainThread(() =>
            {
                var invalidAlertView = new UIAlertView("Invalid Time", "Please enter less than 24 hours of total entries for the day.", null, "Ok");
                invalidAlertView.Show();
            });
        }

        private float HoursEnteredIfParsable()
        {
            float hours;
            var parsed = float.TryParse(hoursField.Text, out hours);
            return parsed ? hours : 0;
        }

        public void doneButtonTapped(object sender, EventArgs args)
		{
			hoursField.ResignFirstResponder ();
		}

		//dismiss keyboard when tapping outside of text fields
		public override void TouchesBegan(NSSet touches, UIEvent evt)
		{
			base.TouchesBegan(touches, evt);

			hoursField.ResignFirstResponder();
		}

        public void setupConstraints() 
        {
			AddConstraint( NSLayoutConstraint.Create(projectCodeField, NSLayoutAttribute.Left, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.05f, 0f));
			AddConstraint(NSLayoutConstraint.Create(projectCodeField, NSLayoutAttribute.Top, NSLayoutRelation.Equal, this, NSLayoutAttribute.Bottom, 0.05f, 0f));
			AddConstraint(NSLayoutConstraint.Create(projectCodeField, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, this, NSLayoutAttribute.Bottom, 0.65f, 0f));
			AddConstraint(NSLayoutConstraint.Create(projectCodeField, NSLayoutAttribute.Right, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.75f, 0f));

			AddConstraint( NSLayoutConstraint.Create(payRateLabel, NSLayoutAttribute.Left, NSLayoutRelation.Equal, projectCodeField, NSLayoutAttribute.Left, 1.0f, 0f));
			AddConstraint(NSLayoutConstraint.Create(payRateLabel, NSLayoutAttribute.Top, NSLayoutRelation.Equal, projectCodeField, NSLayoutAttribute.Bottom, 1.0f, 0f));
			AddConstraint(NSLayoutConstraint.Create(payRateLabel, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, this, NSLayoutAttribute.Bottom, 0.95f, 0f));
			AddConstraint(NSLayoutConstraint.Create(payRateLabel, NSLayoutAttribute.Right, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.75f, 0f));

			AddConstraint(NSLayoutConstraint.Create(hoursField, NSLayoutAttribute.Left, NSLayoutRelation.Equal, payRateLabel, NSLayoutAttribute.Right, 1.0f, 0f));
			AddConstraint(NSLayoutConstraint.Create(hoursField, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterY, 1.0f, 0f));
			AddConstraint(NSLayoutConstraint.Create(hoursField, NSLayoutAttribute.Height, NSLayoutRelation.Equal, this, NSLayoutAttribute.Height, 0.6f, 0f));
            AddConstraint(NSLayoutConstraint.Create(hoursField, NSLayoutAttribute.Right, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.95f, 0f));
        }

		public void enable(bool shouldEnable )
		{
			if (shouldEnable) 
			{
				hoursField.BorderStyle = UITextBorderStyle.RoundedRect;
				hoursField.Enabled = true;
				hoursField.TextColor = UIColor.Black;
			} else 
			{
				hoursField.BorderStyle = UITextBorderStyle.None;
				hoursField.Enabled = false;
				hoursField.TextColor = UIColor.LightGray;
			}
		}

        public void UpdateCell(TimeEntry timeEntry, float maxNumberOfHours)
        {
            Entry = timeEntry;

            projectCodeField.Text = Entry.CodeRate.PONumber;
            payRateLabel.Text = Entry.CodeRate.ratedescription;
            hoursField.Text = string.Format("{0}", Entry.Hours);
            _maxNumberOfHours = maxNumberOfHours;
        }

        public void FocusOnHours()
        {
            hoursField.BecomeFirstResponder();
        }
    }
}