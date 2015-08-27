using System;
using UIKit;
using Foundation;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using SiSystems.SharedModels;

namespace ConsultantApp.iOS
{
	public class AddProjectCodeCell : UITableViewCell
	{
		public TimeEntry timeEntry;

		public delegate void CellDelegate();
		public  CellDelegate onSave;
		public  CellDelegate onDelete;

		private UIPickerView picker;
		private UIButton saveButton;
		private UIButton deleteButton;
		private UILabel hoursLabel;
		private UITextField hoursTextField;
		private UIButton subtractButton;
		private UIButton addButton;
		private PickerViewModel pickerModel;

		public AddProjectCodeCell (IntPtr handle) : base (handle)
		{
			BackgroundColor = StyleGuideConstants.LightGrayUiColor;

			picker = new UIPickerView ();//(new CoreGraphics.CGRect(0, 0, Frame.Size.Width, 162.0));
			pickerModel = new PickerViewModel ();
			picker.Model = pickerModel;
			//picker.BackgroundColor = UIColor.Blue;
			picker.TranslatesAutoresizingMaskIntoConstraints = false;
			AddSubview(picker);



			saveButton = new BorderedButton ();
			saveButton.SetTitle ("Save", UIControlState.Normal);
			//saveButton.Layer.BorderWidth = 1;
			//saveButton.Layer.BorderColor = UIColor.Black.CGColor;
			saveButton.TranslatesAutoresizingMaskIntoConstraints = false;
			saveButton.TouchUpInside += delegate 
			{
				timeEntry.ProjectCode = pickerModel.items[ pickerModel.selectedItemIndex ];
				timeEntry.Hours = float.Parse(hoursTextField.Text);
				onSave();
			};
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

			subtractButton = new UIButton ();
			subtractButton.SetTitle ("-", UIControlState.Normal);
			subtractButton.Layer.BorderWidth = 1;
			subtractButton.Layer.BorderColor = UIColor.Black.CGColor;
			subtractButton.TranslatesAutoresizingMaskIntoConstraints = false;
			subtractButton.TouchUpInside += delegate 
			{
				hoursTextField.Text = (float.Parse(hoursTextField.Text) - 0.5f).ToString();
			};
			AddSubview (subtractButton);

			addButton = new UIButton ();
			addButton.SetTitle ("+", UIControlState.Normal);
			addButton.Layer.BorderWidth = 1;
			addButton.Layer.BorderColor = UIColor.Black.CGColor;
			addButton.TranslatesAutoresizingMaskIntoConstraints = false;
			addButton.TouchUpInside += delegate 
			{
				hoursTextField.Text = (float.Parse(hoursTextField.Text) + 0.5f).ToString();
			};
			AddSubview (addButton);


			setupConstraints();
		}

		public void setTimeEntry( TimeEntry entry )
		{
			timeEntry = entry;
			updateUI ();
		}
		public void updateUI()
		{
			hoursTextField.Text = timeEntry.Hours.ToString();	
			pickerModel = new PickerViewModel ();
			pickerModel.items = new String[]{"PC123", "PC777", "PC456"};//entry.getAllProjectCodes ();
			picker.Model = pickerModel;
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

			AddConstraint (NSLayoutConstraint.Create (subtractButton, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, hoursTextField, NSLayoutAttribute.CenterY, 1.0f, 0f));
			AddConstraint (NSLayoutConstraint.Create (subtractButton, NSLayoutAttribute.Height, NSLayoutRelation.Equal, hoursTextField, NSLayoutAttribute.Height, 0.66f, 0f));
			AddConstraint (NSLayoutConstraint.Create (subtractButton, NSLayoutAttribute.Width, NSLayoutRelation.Equal, hoursTextField, NSLayoutAttribute.Width, 0.5f, 0f));
			AddConstraint (NSLayoutConstraint.Create (subtractButton, NSLayoutAttribute.Right, NSLayoutRelation.Equal, hoursTextField, NSLayoutAttribute.Left, 1.0f, -15f));

			AddConstraint (NSLayoutConstraint.Create (addButton, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, hoursTextField, NSLayoutAttribute.CenterY, 1.0f, 0f));
			AddConstraint (NSLayoutConstraint.Create (addButton, NSLayoutAttribute.Height, NSLayoutRelation.Equal, hoursTextField, NSLayoutAttribute.Height, 0.66f, 0f));
			AddConstraint (NSLayoutConstraint.Create (addButton, NSLayoutAttribute.Width, NSLayoutRelation.Equal, hoursTextField, NSLayoutAttribute.Width, 0.5f, 0f));
			AddConstraint (NSLayoutConstraint.Create (addButton, NSLayoutAttribute.Left, NSLayoutRelation.Equal, hoursTextField, NSLayoutAttribute.Right, 1.0f, 15f));

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
			public delegate void pickerViewDelegate( string item );
			public pickerViewDelegate onSelected;
			public string[] items;
			public int selectedItemIndex;

			public override nint GetComponentCount (UIPickerView picker)
			{
				return 1;
			}

			public override nint GetRowsInComponent (UIPickerView picker, nint component)
			{
				if (items != null)
					return items.Length;
				else
					return 0;
			}

			public override string GetTitle (UIPickerView pickerView, nint row, nint component)
			{
				if (items == null)
					return "PC777-7777";
				else
					return items [row];
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

				selectedItemIndex = (int)row;
			}
		}
	}
}

