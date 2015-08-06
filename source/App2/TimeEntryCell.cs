using System;
using UIKit;

namespace ConsultantApp.iOS
{
    public class TimeEntryCell : UITableViewCell
    {
        public UITextField clientField;
        public UITextField projectCodeField;
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

            //Client
            if (onClientChanged == null)
                onClientChanged = (String newClient) => {};

            clientField = new UITextField();
            clientField.Text = "DevFacto";
            clientField.TranslatesAutoresizingMaskIntoConstraints = false;
            AddSubview( clientField );

            //TESTING <<
            clientField.EditingChanged += delegate
            {
                onClientChanged("test client");
            };

            //Project Code
            if (onProjectCodeChanged == null)
                onProjectCodeChanged = (String newProjectCode ) => { };

            projectCodeField = new UITextField();
            projectCodeField.Text = "Project Code";
            projectCodeField.TranslatesAutoresizingMaskIntoConstraints = false;
            projectCodeField.TextAlignment = UITextAlignment.Center;
            AddSubview( projectCodeField );

            projectCodeField.EditingChanged += delegate
            {
                onProjectCodeChanged("test code");
            };

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

        public void setupConstraints() 
        {
            AddConstraint( NSLayoutConstraint.Create(clientField, NSLayoutAttribute.Left, NSLayoutRelation.Equal, this, NSLayoutAttribute.Left, 1.0f, 0f));
            AddConstraint(NSLayoutConstraint.Create(clientField, NSLayoutAttribute.Top, NSLayoutRelation.Equal, this, NSLayoutAttribute.Top, 1.0f, 0f));
            AddConstraint(NSLayoutConstraint.Create(clientField, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, this, NSLayoutAttribute.Bottom, 1.0f, 0f));
            AddConstraint(NSLayoutConstraint.Create(clientField, NSLayoutAttribute.Right, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.33f, 0f));

            AddConstraint(NSLayoutConstraint.Create(projectCodeField, NSLayoutAttribute.Left, NSLayoutRelation.Equal, clientField, NSLayoutAttribute.Right, 1.0f, 0f));
            AddConstraint(NSLayoutConstraint.Create(projectCodeField, NSLayoutAttribute.Top, NSLayoutRelation.Equal, this, NSLayoutAttribute.Top, 1.0f, 0f));
            AddConstraint(NSLayoutConstraint.Create(projectCodeField, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, this, NSLayoutAttribute.Bottom, 1.0f, 0f));
            AddConstraint(NSLayoutConstraint.Create(projectCodeField, NSLayoutAttribute.Right, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.66f, 0f));

            AddConstraint(NSLayoutConstraint.Create(hoursField, NSLayoutAttribute.Left, NSLayoutRelation.Equal, projectCodeField, NSLayoutAttribute.Right, 1.0f, 0f));
            AddConstraint(NSLayoutConstraint.Create(hoursField, NSLayoutAttribute.Top, NSLayoutRelation.Equal, this, NSLayoutAttribute.Top, 1.0f, 0f));
            AddConstraint(NSLayoutConstraint.Create(hoursField, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, this, NSLayoutAttribute.Bottom, 1.0f, 0f));
            AddConstraint(NSLayoutConstraint.Create(hoursField, NSLayoutAttribute.Right, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 1.0f, 0f));
        }

    }
}