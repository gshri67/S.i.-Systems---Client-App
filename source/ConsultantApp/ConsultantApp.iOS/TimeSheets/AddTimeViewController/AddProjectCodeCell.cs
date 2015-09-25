using System;
using UIKit;
using Foundation;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using SiSystems.SharedModels;
using ConsultantApp.Core.ViewModels;

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

		private static int maxFrequentlyUsed = 5;

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
			saveButton.SetTitle ("Done", UIControlState.Normal);
            saveButton.SetTitleColor(StyleGuideConstants.RedUiColor, UIControlState.Normal);
            saveButton.TintColor = StyleGuideConstants.RedUiColor;
			saveButton.TranslatesAutoresizingMaskIntoConstraints = false;
			saveButton.TouchUpInside += delegate 
			{
				saveChanges();
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
				projectCodes.Sort ();

				if (projectCodes.Count < maxFrequentlyUsed)
					projectCodes.Sort (new Comparison<string> ((string pc1, string pc2) => {
						if (!ActiveTimesheetViewModel.projectCodeDict.ContainsKey (pc1) || ActiveTimesheetViewModel.projectCodeDict.ContainsKey (pc2) && ActiveTimesheetViewModel.projectCodeDict [pc2] >= ActiveTimesheetViewModel.projectCodeDict [pc1])
							return 1;
						else if (!ActiveTimesheetViewModel.projectCodeDict.ContainsKey (pc2) || ActiveTimesheetViewModel.projectCodeDict.ContainsKey (pc1) && ActiveTimesheetViewModel.projectCodeDict [pc1] >= ActiveTimesheetViewModel.projectCodeDict [pc2])
							return -1;
						return 0;
					}));
				else 
				{
					int highest = 0, highestIndex = 0;
					//can make this linear time if need be.
					for (int i = 0; i < maxFrequentlyUsed; i++) 
					{
						if (!ActiveTimesheetViewModel.projectCodeDict.ContainsKey (projectCodes [i])) {
							highest = -1;
							highestIndex = -1;
						} else 
						{
							highest = ActiveTimesheetViewModel.projectCodeDict [projectCodes [i]];
							highestIndex = i;
						}

						for (int j = i+1; j < projectCodes.Count; j++) 
						{
							if (ActiveTimesheetViewModel.projectCodeDict.ContainsKey (projectCodes [j]) && ActiveTimesheetViewModel.projectCodeDict [projectCodes [j]] > highest) 
							{
								highest = ActiveTimesheetViewModel.projectCodeDict [projectCodes [j]];
								highestIndex = j;
							}
						}

						if (highestIndex > -1) 
						{
							List<string> list = projectCodes.ToList ();
							string temp = list [i];
							list [i] = list [highestIndex];
							list [highestIndex] = temp;

							projectCodes = list;
						}
					}
				}

				//find out how many frequently used items there are, and if it is higher than our limit
				int numFrequentItems = 0;
				for (int i = 0; i < projectCodes.Count; i++)
					if (ActiveTimesheetViewModel.projectCodeDict.ContainsKey (projectCodes [i]))
						numFrequentItems++;

				if (numFrequentItems > maxFrequentlyUsed)
					numFrequentItems = maxFrequentlyUsed;

				pickerModel.numFrequentItems[0] = numFrequentItems;


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

			if ( pickerModel == null || (!pickerModel.usingFrequentlyUsedSection[0] || pickerModel.numFrequentItems [0] == 0))
				picker.Select (0, 0, false);
			else
				picker.Select (1, 0, false);
			
			picker.Select (0, 1, false); 
		}

		public void setupConstraints()
		{
		
			int pickerHeight = 162;

			AddConstraint (NSLayoutConstraint.Create (picker, NSLayoutAttribute.Left, NSLayoutRelation.Equal, this, NSLayoutAttribute.Left, 1.0f, 0f));
			AddConstraint (NSLayoutConstraint.Create (picker, NSLayoutAttribute.Right, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 1.0f, 0f));
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

		public void saveChanges()
		{
			timeEntry.ProjectCode = pickerModel.items.ElementAt(0).ElementAt( pickerModel.selectedItemIndex.ElementAt(0) );
			timeEntry.PayRate.RateDescription = pickerModel.items.ElementAt(1).ElementAt( pickerModel.selectedItemIndex.ElementAt(1) );
			//timeEntry.Hours = float.Parse(hoursTextField.Text);

			if( !ActiveTimesheetViewModel.projectCodeDict.Keys.Contains(timeEntry.ProjectCode) )
				ActiveTimesheetViewModel.projectCodeDict.Add(timeEntry.ProjectCode, 1);
			else
				ActiveTimesheetViewModel.projectCodeDict[timeEntry.ProjectCode] ++;

			Console.WriteLine( timeEntry.ProjectCode + " " + ActiveTimesheetViewModel.projectCodeDict[timeEntry.ProjectCode]);

			onSave();
		}
	}
}

