using System;
using UIKit;
using Foundation;

namespace ConsultantApp.iOS
{
	public class AddProjectCodeCell : UITableViewCell
	{
		private UIPickerView picker;
		private UIButton saveButton;
		private UIButton deleteButton;
		private UILabel hoursLabel;
		private UITextField hoursTextField;
		private UIButton subtractButton;
		private UIButton addButton;

		public AddProjectCodeCell (IntPtr handle) : base (handle)
		{
			BackgroundColor = StyleGuideConstants.LightGrayUiColor;

			picker = new UIPickerView ();//(new CoreGraphics.CGRect(0, 0, Frame.Size.Width, 162.0));
			picker.Model = new PickerViewModel ();
			//picker.BackgroundColor = UIColor.Blue;
			picker.TranslatesAutoresizingMaskIntoConstraints = false;
			AddSubview(picker);



			saveButton = new BorderedButton ();
			saveButton.SetTitle ("Save", UIControlState.Normal);
			//saveButton.Layer.BorderWidth = 1;
			//saveButton.Layer.BorderColor = UIColor.Black.CGColor;
			saveButton.TranslatesAutoresizingMaskIntoConstraints = false;
			AddSubview (saveButton);

			deleteButton = new BorderedButton ();
			deleteButton.SetTitle ("Delete", UIControlState.Normal);
			//deleteButton.Layer.BorderWidth = 1;
			//deleteButton.Layer.BorderColor = UIColor.Black.CGColor;
			deleteButton.TranslatesAutoresizingMaskIntoConstraints = false;
			AddSubview (deleteButton);

			hoursLabel = new UILabel ();
			hoursLabel.TranslatesAutoresizingMaskIntoConstraints = false;
			AddSubview (hoursLabel);

			hoursTextField = new UITextField ();
			hoursTextField.Text = "7.5";
			//hoursTextField.BackgroundColor = UIColor.Cyan;
			hoursTextField.TextAlignment = UITextAlignment.Center;
			hoursTextField.UserInteractionEnabled = false;
			hoursTextField.Layer.BorderWidth = 1;
			hoursTextField.Layer.BorderColor = UIColor.Black.CGColor;
			hoursTextField.TranslatesAutoresizingMaskIntoConstraints = false;
			AddSubview (hoursTextField);

			setupConstraints();
		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
			/*
			CoreGraphics.CGAffineTransform transform = CoreGraphics.CGAffineTransform.MakeIdentity ();
			Transform.Translate (0, picker.Bounds.Size.Height / 2.0f);
			Transform.Scale (0.2f, 0.1f);
			Transform.Translate (0, -picker.Bounds.Size.Height / 2);
			picker.Transform = transform;*/
		}

		public void setupConstraints()
		{
		
			int pickerHeight = 162;

			AddConstraint (NSLayoutConstraint.Create (picker, NSLayoutAttribute.Left, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.2f, 0f));
			AddConstraint (NSLayoutConstraint.Create (picker, NSLayoutAttribute.Right, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.8f, 0f));
			AddConstraint (NSLayoutConstraint.Create (picker, NSLayoutAttribute.Top, NSLayoutRelation.Equal, this, NSLayoutAttribute.Top, 1.0f, 0f));
			AddConstraint (NSLayoutConstraint.Create (picker, NSLayoutAttribute.Height, NSLayoutRelation.Equal, null, NSLayoutAttribute.NoAttribute, 1.0f, pickerHeight));

			AddConstraint (NSLayoutConstraint.Create (hoursTextField, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterX, 1.0f, 0f));
			AddConstraint (NSLayoutConstraint.Create (hoursTextField, NSLayoutAttribute.Top, NSLayoutRelation.Equal, picker, NSLayoutAttribute.Bottom, 1.0f, 0f));
			AddConstraint (NSLayoutConstraint.Create (hoursTextField, NSLayoutAttribute.Height, NSLayoutRelation.Equal, this, NSLayoutAttribute.Height, 0.1f, 0f));
			AddConstraint (NSLayoutConstraint.Create (hoursTextField, NSLayoutAttribute.Width, NSLayoutRelation.Equal, this, NSLayoutAttribute.Width, 0.15f, 0f));

			AddConstraint (NSLayoutConstraint.Create (saveButton, NSLayoutAttribute.Top, NSLayoutRelation.Equal, this, NSLayoutAttribute.Bottom, 0.88f, 0.0f));
			AddConstraint (NSLayoutConstraint.Create (saveButton, NSLayoutAttribute.Left, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.2f, 0.0f));
			AddConstraint (NSLayoutConstraint.Create (saveButton, NSLayoutAttribute.Right, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.45f, 0.0f));
			AddConstraint (NSLayoutConstraint.Create (saveButton, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, this, NSLayoutAttribute.Bottom, 0.98f, 0.0f));

			AddConstraint (NSLayoutConstraint.Create (deleteButton, NSLayoutAttribute.Top, NSLayoutRelation.Equal, this, NSLayoutAttribute.Bottom, 0.88f, 0.0f));
			AddConstraint (NSLayoutConstraint.Create (deleteButton, NSLayoutAttribute.Left, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.55f, 0.0f));
			AddConstraint (NSLayoutConstraint.Create (deleteButton, NSLayoutAttribute.Right, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.8f, 0.0f));
			AddConstraint (NSLayoutConstraint.Create (deleteButton, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, this, NSLayoutAttribute.Bottom, 0.98f, 0.0f));
		}


		public class PickerViewModel : UIPickerViewModel
		{
			public delegate void pickerViewDelegate( UIPickerView picker, nint row );
			public pickerViewDelegate onSelected;

			public override nint GetComponentCount (UIPickerView picker)
			{
				return 1;
			}

			public override nint GetRowsInComponent (UIPickerView picker, nint component)
			{
				return 15;//projectCodes.Count;
			}

			public override string GetTitle (UIPickerView pickerView, nint row, nint component)
			{/*
				if (pickerView == clientPickerView)
					return clients.ElementAt ((int)row).name;
				else
					return projectCodes.ElementAt ((int)row).code;*/

				return "PC777-7777";
			}

			public override void Selected (UIPickerView pickerView, nint row, nint component)
			{
				/*
				//projectcodes should be updated for selected client
				if (pickerView == clientPickerView) 
				{
					projectCodes = clients.ElementAt ((int)row).projectCodes;
					projectCodePickerView.ReloadAllComponents ();
				}

				onSelected(pickerView, row);*/
			}
		}
	}
}

