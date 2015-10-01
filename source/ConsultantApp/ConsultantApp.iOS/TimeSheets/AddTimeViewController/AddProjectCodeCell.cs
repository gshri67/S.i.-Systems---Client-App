using System;
using UIKit;
using Foundation;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using SiSystems.SharedModels;
using ConsultantApp.Core.ViewModels;

using CoreGraphics;
using Microsoft.Practices.Unity;
using Shared.Core;

namespace ConsultantApp.iOS
{
	public class AddProjectCodeCell : UITableViewCell
	{
		public TimeEntry TimeEntry;

		public delegate void CellDelegate();
		public  CellDelegate OnSave;
		public  CellDelegate OnDelete;

		private UIPickerView _picker;
		private UIButton _saveButton;
		private UIButton _deleteButton;
		private UITextField _hoursTextField;
		private PickerViewModel _pickerModel;
		private List<string> _projectCodes;
		private IEnumerable<PayRate> _payRates;

        private const int PayRateComponentIndex = 1;

	    public AddProjectCodeCell (IntPtr handle) : base (handle)
		{
		    BackgroundColor = StyleGuideConstants.LighterGrayUiColor;

			setupPicker ();
			setupSaveButton ();
			setupDeleteButton ();
			setupHoursTextField ();

			SetupConstraints();
		}

		private void setupPicker()
		{
			_picker = new UIPickerView ();
			_pickerModel = new PickerViewModel ();
			_picker.Model = _pickerModel;
			_picker.TranslatesAutoresizingMaskIntoConstraints = false;
			AddSubview(_picker);
		}

		private void setupHoursTextField()
		{
			_hoursTextField = new UITextField
			{
				Text = "7.5",
				Font = UIFont.SystemFontOfSize(17f),
				TextAlignment = UITextAlignment.Center,
				UserInteractionEnabled = false,
				TranslatesAutoresizingMaskIntoConstraints = false
			};
			AddSubview (_hoursTextField);
			_hoursTextField.Hidden = true;			
		}

		private void setupSaveButton()
		{
			_saveButton = new BorderedButton ();
			_saveButton.SetTitle ("Done", UIControlState.Normal);
			_saveButton.SetTitleColor(StyleGuideConstants.RedUiColor, UIControlState.Normal);
			_saveButton.TintColor = StyleGuideConstants.RedUiColor;
			_saveButton.TranslatesAutoresizingMaskIntoConstraints = false;
			_saveButton.TouchUpInside += delegate 
			{
				SaveChanges();
			};
			AddSubview (_saveButton);
		}

		private void setupDeleteButton()
		{
			_deleteButton = new BorderedButton ();
			_deleteButton.SetTitle ("Delete", UIControlState.Normal);
			_deleteButton.SetTitleColor(StyleGuideConstants.RedUiColor, UIControlState.Normal);
			_deleteButton.TintColor = StyleGuideConstants.RedUiColor;
			_deleteButton.TranslatesAutoresizingMaskIntoConstraints = false;
			_deleteButton.TouchUpInside += delegate 
			{
				OnDelete();
			};
			AddSubview (_deleteButton);
		}

		public void SetTimeEntry( TimeEntry entry )
		{
			TimeEntry = entry;
			UpdateUI ();
		}

		public void SetData( TimeEntry entry, IEnumerable<string> projectCodes, IEnumerable<PayRate> payRates )
		{
			TimeEntry = entry;
			this._projectCodes = projectCodes.ToList();
			this._payRates = payRates;
			UpdateUI();
		}

		public void UpdateUI()
		{
			if( TimeEntry != null )
				_hoursTextField.Text = TimeEntry.Hours.ToString();	
	
			updatePickerModel ();
		}

		private void updatePickerModel()
		{
			if ( _picker == null || _pickerModel == null || TimeEntry == null || _projectCodes == null || _payRates == null)
				
				return;
			

			var mostFrequentlyUsed = ActiveTimesheetViewModel.MostFrequentProjectCodes();
			var frequentlyUsed = mostFrequentlyUsed as IList<string> ?? mostFrequentlyUsed.ToList();
			var notFrequentlyUsed = _projectCodes.Except(frequentlyUsed).ToList();
			notFrequentlyUsed.Sort();

			_projectCodes = frequentlyUsed.Concat(notFrequentlyUsed).ToList();

			_pickerModel.items = new List<List<string>>
			{
				_projectCodes.ToList()
			};

			_pickerModel.numFrequentItems[0] = frequentlyUsed.Count;

			List<string> payRateStringList = _payRates.Select (pr => string.Format ("{0} ({1:C})", pr.RateDescription, pr.Rate)).ToList ();

			_pickerModel.items = new List<List<string>>
			{
				_projectCodes,
				payRateStringList
			};


			loadSelectedPickerItem ( TimeEntry.ProjectCode, _projectCodes, 0 );

			if( TimeEntry.PayRate != null )
				loadSelectedPickerItem ( string.Format ("{0} ({1:C})", TimeEntry.PayRate.RateDescription, TimeEntry.PayRate.Rate), payRateStringList, 1 );
			else
				loadSelectedPickerItem ( null, payRateStringList, 1 );
			
		}

		//find the index of the input item in the item list (if it exists). Then scroll to that item
		private void loadSelectedPickerItem( string item, List<string> itemList, int component )
		{
			int itemIndex = 0;

			if (item != null && item.Length > 0)
				itemIndex = itemList.FindIndex ((string code) => { return item == code; });

			if( itemIndex < 0 )
				itemIndex = 0;

			scrollToItemInPicker ( itemIndex, component );
		}

		private void scrollToItemInPicker( int itemIndex, int component  )
		{
			_pickerModel.scrollToItemIndex (_picker, itemIndex, component);
			_picker.ReloadAllComponents ();
		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
		}

		public void SetupConstraints()
		{
		
			const int pickerHeight = 162;

			AddConstraint (NSLayoutConstraint.Create (_picker, NSLayoutAttribute.Left, NSLayoutRelation.Equal, this, NSLayoutAttribute.Left, 1.0f, 0f));
			AddConstraint (NSLayoutConstraint.Create (_picker, NSLayoutAttribute.Right, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 1.0f, 0f));
			AddConstraint (NSLayoutConstraint.Create (_picker, NSLayoutAttribute.Top, NSLayoutRelation.Equal, this, NSLayoutAttribute.Top, 1.0f, 0f));
			AddConstraint (NSLayoutConstraint.Create (_picker, NSLayoutAttribute.Height, NSLayoutRelation.Equal, null, NSLayoutAttribute.NoAttribute, 1.0f, pickerHeight));

			AddConstraint (NSLayoutConstraint.Create (_saveButton, NSLayoutAttribute.Top, NSLayoutRelation.Equal, _picker, NSLayoutAttribute.Bottom, 1.0f, 0.0f));
			AddConstraint (NSLayoutConstraint.Create (_saveButton, NSLayoutAttribute.Left, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.2f, 0.0f));
			AddConstraint (NSLayoutConstraint.Create (_saveButton, NSLayoutAttribute.Right, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.45f, 0.0f));
			AddConstraint (NSLayoutConstraint.Create (_saveButton, NSLayoutAttribute.Height, NSLayoutRelation.Equal, null, NSLayoutAttribute.NoAttribute, 1.0f, 30f));

			AddConstraint (NSLayoutConstraint.Create (_deleteButton, NSLayoutAttribute.Top, NSLayoutRelation.Equal, _picker, NSLayoutAttribute.Bottom, 1.0f, 0.0f));
			AddConstraint (NSLayoutConstraint.Create (_deleteButton, NSLayoutAttribute.Left, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.55f, 0.0f));
			AddConstraint (NSLayoutConstraint.Create (_deleteButton, NSLayoutAttribute.Right, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 0.8f, 0.0f));
			AddConstraint (NSLayoutConstraint.Create (_deleteButton, NSLayoutAttribute.Height, NSLayoutRelation.Equal, _saveButton, NSLayoutAttribute.Height, 1.0f, 0.0f));
		}

		public void SaveChanges()
		{
			TimeEntry.ProjectCode = _pickerModel.items.ElementAt(0).ElementAt( _pickerModel.selectedItemIndex.ElementAt(0) );

			SetTimeEntryPayRateToSelectedRate();

			ActiveTimesheetViewModel.IncrementProjectCodeCount(TimeEntry.ProjectCode);
			
			OnSave();
		}

	    private void SetTimeEntryPayRateToSelectedRate()
	    {
            TimeEntry.PayRate = _payRates.ElementAt(GetSelectedIndexForComponent(PayRateComponentIndex));
	    }

	    private int GetSelectedIndexForComponent(int componentIndex)
	    {
	        return _pickerModel.selectedItemIndex.ElementAt(componentIndex);
	    }
	}
}

