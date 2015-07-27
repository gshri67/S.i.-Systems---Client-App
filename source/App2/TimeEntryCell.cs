using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace App2
{
    public class TimeEntryCell : UITableViewCell
    {
        public UILabel clientLabel;
        public UILabel projectCodeLabel;
        public UILabel hoursLabel;

        public TimeEntryCell (IntPtr handle) : base (handle)
        {
            TextLabel.Hidden = true;

            clientLabel = new UILabel();
            clientLabel.Text = "DevFacto";
            clientLabel.TranslatesAutoresizingMaskIntoConstraints = false;
            AddSubview( clientLabel );

            projectCodeLabel = new UILabel();
            projectCodeLabel.Text = "Project Code";
            projectCodeLabel.TranslatesAutoresizingMaskIntoConstraints = false;
            projectCodeLabel.TextAlignment = UITextAlignment.Center;
            AddSubview( projectCodeLabel);

            hoursLabel = new UILabel();
            hoursLabel.Text = "5";
            hoursLabel.TranslatesAutoresizingMaskIntoConstraints = false;
            hoursLabel.TextAlignment = UITextAlignment.Right;
            AddSubview( hoursLabel );

            setupConstraints();
        }

        public void setupConstraints() 
        {
            AddConstraint( NSLayoutConstraint.Create(clientLabel, NSLayoutAttribute.Left, NSLayoutRelation.Equal, this, NSLayoutAttribute.Left, 1.0f, 0f));
            AddConstraint(NSLayoutConstraint.Create(clientLabel, NSLayoutAttribute.Top, NSLayoutRelation.Equal, this, NSLayoutAttribute.Top, 1.0f, 0f));
            AddConstraint(NSLayoutConstraint.Create(clientLabel, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, this, NSLayoutAttribute.Bottom, 1.0f, 0f));
            AddConstraint(NSLayoutConstraint.Create(clientLabel, NSLayoutAttribute.Right, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.33f, 0f));

            AddConstraint(NSLayoutConstraint.Create(projectCodeLabel, NSLayoutAttribute.Left, NSLayoutRelation.Equal, clientLabel, NSLayoutAttribute.Right, 1.0f, 0f));
            AddConstraint(NSLayoutConstraint.Create(projectCodeLabel, NSLayoutAttribute.Top, NSLayoutRelation.Equal, this, NSLayoutAttribute.Top, 1.0f, 0f));
            AddConstraint(NSLayoutConstraint.Create(projectCodeLabel, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, this, NSLayoutAttribute.Bottom, 1.0f, 0f));
            AddConstraint(NSLayoutConstraint.Create(projectCodeLabel, NSLayoutAttribute.Right, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.66f, 0f));

            AddConstraint(NSLayoutConstraint.Create(hoursLabel, NSLayoutAttribute.Left, NSLayoutRelation.Equal, projectCodeLabel, NSLayoutAttribute.Right, 1.0f, 0f));
            AddConstraint(NSLayoutConstraint.Create(hoursLabel, NSLayoutAttribute.Top, NSLayoutRelation.Equal, this, NSLayoutAttribute.Top, 1.0f, 0f));
            AddConstraint(NSLayoutConstraint.Create(hoursLabel, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, this, NSLayoutAttribute.Bottom, 1.0f, 0f));
            AddConstraint(NSLayoutConstraint.Create(hoursLabel, NSLayoutAttribute.Right, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 1.0f, 0f));
        }

    }
}