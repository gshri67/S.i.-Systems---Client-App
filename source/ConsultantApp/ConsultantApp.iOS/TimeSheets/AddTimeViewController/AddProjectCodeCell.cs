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
		private UITextField hoursTextField;
		private UIButton subtractButton;
		private UIButton addButton;
		private PickerViewModel pickerModel;
		private List<string> projectCodes;
		private List<string> payRates;

		public AddProjectCodeCell (IntPtr handle) : base (handle)
		{
			BackgroundColor = StyleGuideConstants.LighterGrayUiColor;

			picker = new UIPickerView ();//(new CoreGraphics.CGRect(0, 0, Frame.Size.Width, 162.0));
			pickerModel = new PickerViewModel ();
			picker.Model = pickerModel;
			//picker.BackgroundColor = UIColor.Blue;
			picker.TranslatesAutoresizingMaskIntoConstraints = false;
			AddSubview(picker);



			saveButton = new BorderedButton ();
			saveButton.SetTitle ("Save", UIControlState.Normal);
            saveButton.SetTitleColor(StyleGuideConstants.RedUiColor, UIControlState.Normal);
            saveButton.TintColor = StyleGuideConstants.RedUiColor;
			saveButton.TranslatesAutoresizingMaskIntoConstraints = false;
			saveButton.TouchUpInside += delegate 
			{
				timeEntry.ProjectCode = pickerModel.items.ElementAt(0).ElementAt( pickerModel.selectedItemIndex.ElementAt(0) );
				timeEntry.PayRate = pickerModel.items.ElementAt(1).ElementAt( pickerModel.selectedItemIndex.ElementAt(1) );
				//timeEntry.Hours = float.Parse(hoursTextField.Text);
				onSave();
			};
			AddSubview (saveButton);

			deleteButton = new BorderedButton ();
			deleteButton.SetTitle ("Delete", UIControlState.Normal);
            deleteButton.SetTitleColor(StyleGuideConstants.RedUiColor, UIControlState.Normal);
            deleteButton.TintColor = StyleGuideConstants.RedUiColor;
			deleteButton.TranslatesAutoresizingMaskIntoConstraints = false;
			deleteButton.TouchUpInside += delegate 
			{
				onDelete();
			};
			AddSubview (deleteButton);

			hoursTextField = new UITextField ();
			hoursTextField.Text = "7.5";
		    hoursTextField.Font = UIFont.SystemFontOfSize(17f);
			hoursTextField.TextAlignment = UITextAlignment.Center;
			hoursTextField.UserInteractionEnabled = false;
			hoursTextField.TranslatesAutoresizingMaskIntoConstraints = false;
			AddSubview (hoursTextField);

			subtractButton = new UIButton ();
			subtractButton.SetTitle ("-", UIControlState.Normal);
            subtractButton.SetTitleColor(new UIColor(UIColor.Black.CGColor), UIControlState.Normal);
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
            addButton.SetTitleColor(new UIColor(UIColor.Black.CGColor), UIControlState.Normal);
			addButton.Layer.BorderWidth = 1;
			addButton.Layer.BorderColor = UIColor.Black.CGColor;
			addButton.TranslatesAutoresizingMaskIntoConstraints = false;
			addButton.TouchUpInside += delegate 
			{
				hoursTextField.Text = (float.Parse(hoursTextField.Text) + 0.5f).ToString();
			};
			AddSubview (addButton);


			addButton.Hidden = true;
			subtractButton.Hidden = true;
			hoursTextField.Hidden = true;

			setupConstraints();
		}

		public void setTimeEntry( TimeEntry entry )
		{
			timeEntry = entry;
			updateUI ();
		}
		public void setProjectCodes( IEnumerable<string> projectCodes )//list of project codes to pick from
		{
			this.projectCodes = projectCodes.ToList();
			updateUI ();
		}
		public void setPayRates( IEnumerable<string> payRates )//list of project codes to pick from
		{
			this.payRates = payRates.ToList();
			updateUI ();
		}

		public void setData( TimeEntry entry, IEnumerable<string> projectCodes, IEnumerable<string> payRates )
		{
			timeEntry = entry;
			this.projectCodes = projectCodes.ToList();
			this.payRates = payRates.ToList();
			updateUI ();
		}

		public void updateUI()
		{
			if( timeEntry != null )
				hoursTextField.Text = timeEntry.Hours.ToString();	
	
			pickerModel = new PickerViewModel ();
			if (projectCodes != null && payRates != null) 
			{
				pickerModel.items = new List<List<string>> ();
				pickerModel.items.Add( projectCodes );
				pickerModel.items.Add( payRates );
			}
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


			picker.Select (0, 0, false);
			picker.Select (0, 1, false); 
		}

		public void setupConstraints()
		{
		
			int pickerHeight = 162;

			AddConstraint (NSLayoutConstraint.Create (picker, NSLayoutAttribute.Left, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.15f, 0f));
			AddConstraint (NSLayoutConstraint.Create (picker, NSLayoutAttribute.Right, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.85f, 0f));
			AddConstraint (NSLayoutConstraint.Create (picker, NSLayoutAttribute.Top, NSLayoutRelation.Equal, this, NSLayoutAttribute.Top, 1.0f, 0f));
			AddConstraint (NSLayoutConstraint.Create (picker, NSLayoutAttribute.Height, NSLayoutRelation.Equal, null, NSLayoutAttribute.NoAttribute, 1.0f, pickerHeight));

			AddConstraint (NSLayoutConstraint.Create (saveButton, NSLayoutAttribute.Top, NSLayoutRelation.Equal, picker, NSLayoutAttribute.Bottom, 1.0f, 0.0f));
			AddConstraint (NSLayoutConstraint.Create (saveButton, NSLayoutAttribute.Left, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.2f, 0.0f));
			AddConstraint (NSLayoutConstraint.Create (saveButton, NSLayoutAttribute.Right, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.45f, 0.0f));
			AddConstraint (NSLayoutConstraint.Create (saveButton, NSLayoutAttribute.Height, NSLayoutRelation.Equal, null, NSLayoutAttribute.NoAttribute, 1.0f, 30f));

			AddConstraint (NSLayoutConstraint.Create (deleteButton, NSLayoutAttribute.Top, NSLayoutRelation.Equal, picker, NSLayoutAttribute.Bottom, 1.0f, 0.0f));
			AddConstraint (NSLayoutConstraint.Create (deleteButton, NSLayoutAttribute.Left, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.55f, 0.0f));
			AddConstraint (NSLayoutConstraint.Create (deleteButton, NSLayoutAttribute.Right, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.8f, 0.0f));
			AddConstraint (NSLayoutConstraint.Create (deleteButton, NSLayoutAttribute.Height, NSLayoutRelation.Equal, saveButton, NSLayoutAttribute.Height, 1.0f, 0.0f));
		}


		public class PickerViewModel : UIPickerViewModel
		{
			public delegate void pickerViewDelegate( string item );
			public pickerViewDelegate onSelected;
			public List<List<string>> items;
			public List<int> selectedItemIndex;

			public PickerViewModel()
			{
				selectedItemIndex = new List<int>();
				selectedItemIndex.Add(0);
			}

			public override nint GetComponentCount (UIPickerView picker)
			{
				if (items == null)
					return 1;
				else 
				{
					//the picker does not automatically select the first row. As such we need to make sure everything is covered right away, such as having all the selected Indicies preset to 0.
					while( selectedItemIndex.Count() < items.Count() )
						selectedItemIndex.Add (0);
					
					return items.Count ();
				}
			}

			public override nint GetRowsInComponent (UIPickerView picker, nint component)
			{
				if (items != null)
					return items.ElementAt((int)component).Count();
				else
					return 0;
			}

			public override string GetTitle (UIPickerView pickerView, nint row, nint component)
			{
				if (items == null)
					return "";
				else
					return items.ElementAt((int)component).ElementAt((int)row);
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

				selectedItemIndex[(int)component] = (int)row;
			}

			public override UIView GetView (UIPickerView pickerView, nint row, nint component, UIView view)
			{
				UILabel lbl = new UILabel(new CoreGraphics.CGRect(0, 0, 130f, 40f));
				lbl.TextColor = UIColor.Black;
				lbl.Font = UIFont.SystemFontOfSize(12f);
				lbl.TextAlignment = UITextAlignment.Center;

				if (items != null )
					lbl.Text = items.ElementAt((int)component).ElementAt((int)row);
	
				return lbl;
			}
		}
	}
}

