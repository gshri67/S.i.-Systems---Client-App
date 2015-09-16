using System;
using UIKit;
using Foundation;

namespace ConsultantApp.iOS
{
    public class TimeEntryCell : UITableViewCell
    {
		public UILabel clientField;
        public UILabel projectCodeField;
        public UITextField hoursField;
		public UILabel payRateLabel;

        //Blocks
        public delegate void hoursFieldDelegate( float newHours );
        public hoursFieldDelegate onHoursChanged;

        public delegate void clientFieldDelegate( String newClient );
        public clientFieldDelegate onClientChanged;

        public delegate void projectCodeFieldDelegate(String newProjectCode);
        public projectCodeFieldDelegate onProjectCodeChanged;

		//indication of saving the field
		public UILabel savingLabel;
		public UIActivityIndicatorView savingIndicatorView;

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

			//projectCodeField.TextColor = UIColor.FromWhiteAlpha (0.55f, 1.0f);
			clientField.TextColor = UIColor.FromWhiteAlpha (0.55f, 1.0f);

			clientField.Hidden = true;

            //Hours
            if (onHoursChanged == null)
                onHoursChanged = (float newHours) => { };

            hoursField = new UITextField();
            hoursField.Text = "8";
            hoursField.TranslatesAutoresizingMaskIntoConstraints = false;
            hoursField.TextAlignment = UITextAlignment.Right;
			//hoursField.UserInteractionEnabled = false;
            hoursField.EditingChanged += delegate 
            {
                if (hoursField.Text.Length > 0)
                {
                    try
                    {
                        onHoursChanged(float.Parse(hoursField.Text));
                    }
                    catch //if an invalid string is typed, just default to 0
                    {
                        onHoursChanged(0);
                    }
                }
                else
                    onHoursChanged(0);
            };
			//hoursField.ClearsOnBeginEditing = true;

			hoursField.EditingDidBegin += delegate {
				this.BeginInvokeOnMainThread ( delegate {
					hoursField.SelectedTextRange = hoursField.GetTextRange(hoursField.BeginningOfDocument, hoursField.EndOfDocument);
				});
			};

			//hoursField.ReturnKeyType = UIReturnKeyType.Done;
			hoursField.KeyboardType = UIKeyboardType.DecimalPad;

			var toolbar = new UIToolbar(new CoreGraphics.CGRect(0.0f, 0.0f, Frame.Size.Width, 44.0f));

			toolbar.Items = new[]
			{
				new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
				new UIBarButtonItem(UIBarButtonSystemItem.Done, doneButtonTapped)
			};

			hoursField.InputAccessoryView = toolbar;
			hoursField.BorderStyle = UITextBorderStyle.RoundedRect;

			/*
			UIToolbar toolbar = new UIToolbar ( new CoreGraphics.CGRect(0, 0, Frame.Width, 44) );
			UIBarButtonItem doneButton = new UIBarButtonItem ("Done", UIBarButtonItemStyle.Bordered,  doneButtonTapped );

			toolbar.Items = new UIBarButtonItem[]{ doneButton };
			hoursField.InputAccessoryView = toolbar;*/
			/*
			UIToolbar *toolBar= [[UIToolbar alloc] initWithFrame:CGRectMake(0,0,320,44)];
			[toolBar setBarStyle:UIBarStyleBlackOpaque];
			UIBarButtonItem *barButtonDone = [[UIBarButtonItem alloc] initWithTitle:@"Done" 
				style:UIBarButtonItemStyleBordered target:self action:@selector(changeDateFromLabel:)];
			toolBar.items = @[barButtonDone];
			barButtonDone.tintColor=[UIColor blackColor];
			[pickerView addSubview:toolBar];
*/
            AddSubview( hoursField );

			savingLabel = new UILabel();
			savingLabel.Text = "Saving...";
			savingLabel.TextAlignment = UITextAlignment.Left;
			savingLabel.TextColor = UIColor.Black;
			savingLabel.Font = UIFont.SystemFontOfSize(10);
			savingLabel.TranslatesAutoresizingMaskIntoConstraints = false;
			AddSubview (savingLabel);

			savingIndicatorView = new UIActivityIndicatorView ();
			savingIndicatorView.TranslatesAutoresizingMaskIntoConstraints = false;
			savingIndicatorView.StartAnimating ();
			savingIndicatorView.Color = StyleGuideConstants.DarkGrayUiColor;
			AddSubview (savingIndicatorView);

            setupConstraints();
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
			AddConstraint(NSLayoutConstraint.Create(payRateLabel, NSLayoutAttribute.Right, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.5f, 0f));

			//setup saving indicator before saving label
			AddConstraint(NSLayoutConstraint.Create(savingIndicatorView, NSLayoutAttribute.Left, NSLayoutRelation.Equal, payRateLabel, NSLayoutAttribute.Right, 1.0f, 0f));
			AddConstraint(NSLayoutConstraint.Create(savingIndicatorView, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, payRateLabel, NSLayoutAttribute.CenterY, 1.0f, 0f));
			AddConstraint(NSLayoutConstraint.Create(savingIndicatorView, NSLayoutAttribute.Height, NSLayoutRelation.Equal, payRateLabel, NSLayoutAttribute.Height, 1.0f, 0f));
			AddConstraint(NSLayoutConstraint.Create(savingIndicatorView, NSLayoutAttribute.Right, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.6f, 0f));

			//setup saving label to the right of the indicator
			AddConstraint(NSLayoutConstraint.Create(savingLabel, NSLayoutAttribute.Left, NSLayoutRelation.Equal, savingIndicatorView, NSLayoutAttribute.Right, 1.0f, 0f));
			AddConstraint(NSLayoutConstraint.Create(savingLabel, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, savingIndicatorView, NSLayoutAttribute.CenterY, 1.0f, 0f));
			AddConstraint(NSLayoutConstraint.Create(savingLabel, NSLayoutAttribute.Height, NSLayoutRelation.Equal, savingIndicatorView, NSLayoutAttribute.Height, 1.0f, 0f));
			AddConstraint(NSLayoutConstraint.Create(savingLabel, NSLayoutAttribute.Right, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.75f, 0f));

			AddConstraint(NSLayoutConstraint.Create(hoursField, NSLayoutAttribute.Left, NSLayoutRelation.Equal, savingLabel, NSLayoutAttribute.Right, 1.0f, 0f));
			AddConstraint(NSLayoutConstraint.Create(hoursField, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterY, 1.0f, 0f));
			AddConstraint(NSLayoutConstraint.Create(hoursField, NSLayoutAttribute.Height, NSLayoutRelation.Equal, this, NSLayoutAttribute.Height, 0.6f, 0f));
            AddConstraint(NSLayoutConstraint.Create(hoursField, NSLayoutAttribute.Right, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.95f, 0f));
        }

		//animates saving functionality
		public void activateSave()
		{
			savingLabel.Text = "Saved";
			savingLabel.TextColor = UIColor.Gray;
			savingIndicatorView.StopAnimating ();
			/*
			UIView.Animate ( 5f, () => 
				{
					savingLabel.Alpha = 1.0f;
				}, () => 
				{
					UIView.Animate(2f, ()=>
						{
							savingLabel.Text = "Saved";
							savingLabel.TextColor = UIColor.Gray;
						});	
				} );*/
		}
		public void activateUnsave()
		{
			savingLabel.Text = "Unsaved";
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
    }
}