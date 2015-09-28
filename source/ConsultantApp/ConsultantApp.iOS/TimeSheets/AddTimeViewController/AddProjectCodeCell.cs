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
		public TimeEntry TimeEntry;

		public delegate void CellDelegate();
		public  CellDelegate OnSave;
		public  CellDelegate OnDelete;

		private readonly UIPickerView _picker;
		private readonly UIButton _saveButton;
		private readonly UIButton _deleteButton;
		private readonly UITextField _hoursTextField;
		private PickerViewModel _pickerModel;
		private List<string> _projectCodes;
		private IEnumerable<PayRate> _payRates;

	    private const int MaxFrequentlyUsed = 5;
        private const int PayRateComponentIndex = 1;

	    public AddProjectCodeCell (IntPtr handle) : base (handle)
		{
		    BackgroundColor = StyleGuideConstants.LighterGrayUiColor;

			_picker = new UIPickerView ();
			_pickerModel = new PickerViewModel ();
			_picker.Model = _pickerModel;
			_picker.TranslatesAutoresizingMaskIntoConstraints = false;
			AddSubview(_picker);

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

			SetupConstraints();
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
	
			_pickerModel = new PickerViewModel ();
			if (_projectCodes != null && _payRates != null) 
			{
				_projectCodes.Sort ();

				if (_projectCodes.Count < MaxFrequentlyUsed)
					_projectCodes.Sort (new Comparison<string> ((string pc1, string pc2) => {
						if (!ActiveTimesheetViewModel.ProjectCodeDict.ContainsKey (pc1) || ActiveTimesheetViewModel.ProjectCodeDict.ContainsKey (pc2) && ActiveTimesheetViewModel.ProjectCodeDict [pc2] >= ActiveTimesheetViewModel.ProjectCodeDict [pc1])
							return 1;
						else if (!ActiveTimesheetViewModel.ProjectCodeDict.ContainsKey (pc2) || ActiveTimesheetViewModel.ProjectCodeDict.ContainsKey (pc1) && ActiveTimesheetViewModel.ProjectCodeDict [pc1] >= ActiveTimesheetViewModel.ProjectCodeDict [pc2])
							return -1;
						return 0;
					}));
				else 
				{
					int highest = 0, highestIndex = 0;
					//can make this linear time if need be.
					for (int i = 0; i < MaxFrequentlyUsed; i++) 
					{
						if (!ActiveTimesheetViewModel.ProjectCodeDict.ContainsKey (_projectCodes [i])) {
							highest = -1;
							highestIndex = -1;
						} else 
						{
							highest = ActiveTimesheetViewModel.ProjectCodeDict [_projectCodes [i]];
							highestIndex = i;
						}

						for (int j = i+1; j < _projectCodes.Count; j++) 
						{
							if (ActiveTimesheetViewModel.ProjectCodeDict.ContainsKey (_projectCodes [j]) && ActiveTimesheetViewModel.ProjectCodeDict [_projectCodes [j]] > highest) 
							{
								highest = ActiveTimesheetViewModel.ProjectCodeDict [_projectCodes [j]];
								highestIndex = j;
							}
						}

						if (highestIndex > -1) 
						{
							List<string> list = _projectCodes.ToList ();
							string temp = list [i];
							list [i] = list [highestIndex];
							list [highestIndex] = temp;

							_projectCodes = list;
						}
					}
				}

				//find out how many frequently used items there are, and if it is higher than our limit
				int numFrequentItems = 0;
				for (int i = 0; i < _projectCodes.Count; i++)
					if (ActiveTimesheetViewModel.ProjectCodeDict.ContainsKey (_projectCodes [i]))
						numFrequentItems++;

				if (numFrequentItems > MaxFrequentlyUsed)
					numFrequentItems = MaxFrequentlyUsed;

				_pickerModel.numFrequentItems[0] = numFrequentItems;


			    _pickerModel.items = new List<List<string>>
			    {
			        _projectCodes,
			        _payRates.Select(pr => string.Format("{0} ({1:C})", pr.RateDescription, pr.Rate)).ToList()
			    };
			}
			_picker.Model = _pickerModel;
		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();

			if ( _pickerModel == null || (!_pickerModel.usingFrequentlyUsedSection[0] || _pickerModel.numFrequentItems [0] == 0))
				_picker.Select (0, 0, false);
			else
				_picker.Select (1, 0, false);
			
			_picker.Select (0, 1, false); 
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

			if( !ActiveTimesheetViewModel.ProjectCodeDict.Keys.Contains(TimeEntry.ProjectCode) )
				ActiveTimesheetViewModel.ProjectCodeDict.Add(TimeEntry.ProjectCode, 1);
			else
				ActiveTimesheetViewModel.ProjectCodeDict[TimeEntry.ProjectCode] ++;

			Console.WriteLine( TimeEntry.ProjectCode + " " + ActiveTimesheetViewModel.ProjectCodeDict[TimeEntry.ProjectCode]);

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

