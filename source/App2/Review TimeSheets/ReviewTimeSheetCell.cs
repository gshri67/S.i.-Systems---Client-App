using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace App2
{
	public class ReviewTimeSheetCell : UITableViewCell
	{
		public UILabel clientField;
		public UILabel timePeriodField;

		public ReviewTimeSheetCell (IntPtr handle) : base (handle)
		{
			TextLabel.Hidden = true;

			clientField = new UILabel();
			clientField.Text = "DevFacto";
			clientField.TranslatesAutoresizingMaskIntoConstraints = false;
			AddSubview( clientField );

			timePeriodField = new UILabel();
			timePeriodField.Text = "Oct 16-Oct 31";
			timePeriodField.TextAlignment = UITextAlignment.Left;
			timePeriodField.TranslatesAutoresizingMaskIntoConstraints = false;
			AddSubview( timePeriodField );


			setupConstraints();
		}

		public void setupConstraints() 
		{
			AddConstraint( NSLayoutConstraint.Create(clientField, NSLayoutAttribute.Left, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.1f, 0f));
			AddConstraint(NSLayoutConstraint.Create(clientField, NSLayoutAttribute.Top, NSLayoutRelation.Equal, this, NSLayoutAttribute.Top, 1.0f, 0f));
			AddConstraint(NSLayoutConstraint.Create(clientField, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, this, NSLayoutAttribute.Bottom, 1.0f, 0f));
			AddConstraint(NSLayoutConstraint.Create(clientField, NSLayoutAttribute.Right, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.45f, 0f));

			AddConstraint(NSLayoutConstraint.Create(timePeriodField, NSLayoutAttribute.Left, NSLayoutRelation.Equal, clientField, NSLayoutAttribute.Right, 1.0f, 0f));
			AddConstraint(NSLayoutConstraint.Create(timePeriodField, NSLayoutAttribute.Top, NSLayoutRelation.Equal, this, NSLayoutAttribute.Top, 1.0f, 0f));
			AddConstraint(NSLayoutConstraint.Create(timePeriodField, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, this, NSLayoutAttribute.Bottom, 1.0f, 0f));
			AddConstraint(NSLayoutConstraint.Create(timePeriodField, NSLayoutAttribute.Right, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.9f, 0f));

		}

	}
}