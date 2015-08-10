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

        //Blocks
        public delegate void hoursFieldDelegate( int newHours );
        public hoursFieldDelegate onHoursChanged;

        public delegate void clientFieldDelegate( String newClient );
        public clientFieldDelegate onClientChanged;

        public delegate void projectCodeFieldDelegate(String newProjectCode);
        public projectCodeFieldDelegate onProjectCodeChanged;

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
			projectCodeField.Font =  UIFont.FromName("Helvetica", 12f);
			AddSubview( projectCodeField );

			projectCodeField.TextColor = UIColor.FromWhiteAlpha (0.55f, 1.0f);
			clientField.TextColor = UIColor.FromWhiteAlpha (0.55f, 1.0f);
			/*
            //Client
            if (onClientChanged == null)
                onClientChanged = (String newClient) => {};



            //TESTING <<
            clientField.EditingChanged += delegate
            {
                onClientChanged("test client");
            };

            //Project Code
            if (onProjectCodeChanged == null)
                onProjectCodeChanged = (String newProjectCode ) => { };

            

            projectCodeField.EditingChanged += delegate
            {
                onProjectCodeChanged("test code");
            };
			*/
            //Hours
            if (onHoursChanged == null)
                onHoursChanged = (int newHours) => { };

            hoursField = new UITextField();
            hoursField.Text = "5";
            hoursField.TranslatesAutoresizingMaskIntoConstraints = false;
            hoursField.TextAlignment = UITextAlignment.Right;
            hoursField.EditingChanged += delegate 
            {
                if (hoursField.Text.Length > 0)
                {
                    try
                    {
                        onHoursChanged(Int32.Parse(hoursField.Text));
                    }
                    catch //if an invalid string is typed, just default to 0
                    {
                        onHoursChanged(0);
                    }
                }
                else
                    onHoursChanged(0);
            };
            AddSubview( hoursField );

            setupConstraints();
        }

		//dismiss keyboard when tapping outside of text fields
		public override void TouchesBegan(NSSet touches, UIEvent evt)
		{
			base.TouchesBegan(touches, evt);

			hoursField.ResignFirstResponder();
		}

        public void setupConstraints() 
        {
			AddConstraint( NSLayoutConstraint.Create(clientField, NSLayoutAttribute.Left, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.05f, 0f));
			AddConstraint(NSLayoutConstraint.Create(clientField, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, this, NSLayoutAttribute.Bottom, 0.35f, 0f));
			AddConstraint(NSLayoutConstraint.Create(clientField, NSLayoutAttribute.Height, NSLayoutRelation.Equal, this, NSLayoutAttribute.Height, 0.35f, 0f));
            AddConstraint(NSLayoutConstraint.Create(clientField, NSLayoutAttribute.Right, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.5f, 0f));

			AddConstraint(NSLayoutConstraint.Create(projectCodeField, NSLayoutAttribute.Left, NSLayoutRelation.Equal, clientField, NSLayoutAttribute.Right, 0.5f, 0f));
			AddConstraint(NSLayoutConstraint.Create(projectCodeField, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, this, NSLayoutAttribute.Bottom, 0.8f, 0f));
			AddConstraint(NSLayoutConstraint.Create(projectCodeField, NSLayoutAttribute.Height, NSLayoutRelation.Equal, this, NSLayoutAttribute.Height, 0.25f, 0f));
            AddConstraint(NSLayoutConstraint.Create(projectCodeField, NSLayoutAttribute.Right, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.66f, 0f));

            AddConstraint(NSLayoutConstraint.Create(hoursField, NSLayoutAttribute.Left, NSLayoutRelation.Equal, projectCodeField, NSLayoutAttribute.Right, 1.0f, 0f));
            AddConstraint(NSLayoutConstraint.Create(hoursField, NSLayoutAttribute.Top, NSLayoutRelation.Equal, this, NSLayoutAttribute.Top, 1.0f, 0f));
            AddConstraint(NSLayoutConstraint.Create(hoursField, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, this, NSLayoutAttribute.Bottom, 1.0f, 0f));
            AddConstraint(NSLayoutConstraint.Create(hoursField, NSLayoutAttribute.Right, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.95f, 0f));
        }

    }
}