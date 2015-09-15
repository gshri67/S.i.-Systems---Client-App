using System;
using UIKit;
using Foundation;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ConsultantApp.iOS
{
	public class PickerViewModel : UIPickerViewModel
	{
		public delegate void pickerViewDelegate( string item );
		public pickerViewDelegate onSelected;
		public List<List<string>> items;
		public List<int> selectedItemIndex;

		//Frequently used sorting variables
		public List<bool> usingFrequentlyUsedSection;//does each component have a section at the top for most frequently used items?
		public List<int> numFrequentItems;

		public PickerViewModel()
		{
			selectedItemIndex = new List<int>();
			selectedItemIndex.Add(0);

			usingFrequentlyUsedSection = new List<bool> ();
			usingFrequentlyUsedSection.Add (true);

			numFrequentItems = new List<int> ();
			numFrequentItems.Add (0);
		}

		public override nint GetComponentCount (UIPickerView picker)
		{
			if (items == null)
				return 1;
			else 
			{
				//the picker does not automatically select the first row. As such we need to make sure everything is covered right away, such as having all the selected Indicies preset to 0.
				while (selectedItemIndex.Count () < items.Count ()) 
				{
					selectedItemIndex.Add (0);
					usingFrequentlyUsedSection.Add (false);
					numFrequentItems.Add (0);
				}

				return items.Count ();
			}
		}

		public override nint GetRowsInComponent (UIPickerView picker, nint component)
		{
			if (items != null) 
			{
				if( !usingFrequentlyUsedSection[(int)component] || numFrequentItems [(int)component] == 0 )
					return items.ElementAt ((int)component).Count ();
				else
					return items.ElementAt ((int)component).Count () + 2; 
			}
			else
				return 0;
		}

		public override string GetTitle (UIPickerView pickerView, nint row, nint component)
		{
			if (items == null)
				return "";
			else 
			{
				return items.ElementAt ((int)component).ElementAt ((int)row);
			}
		}

		public override void Selected (UIPickerView pickerView, nint row, nint component)
		{
			int oldRow = (int)row;
			row = getIndex ( (int)row, (int)component );

			/*
				//projectcodes should be updated for selected client
			if (pickerView == clientPickerView) 
			{
				projectCodes = clients.ElementAt ((int)row).projectCodes;
				projectCodePickerView.ReloadAllComponents ();
			}

			onSelected(pickerView, row);*/

			if (row >= 0)
				selectedItemIndex [(int)component] = (int)row;
			else 
			{
				if (oldRow - 1 > 0 && getIndex ((int)oldRow - 1, (int)component) >= 0) {
					row = getIndex ((int)oldRow - 1, (int)component);
					oldRow--;
				} else 
				{
					row = getIndex ((int)oldRow + 1, (int)component);
					oldRow++;
				}
				selectedItemIndex [(int)component] = (int)row;
				pickerView.Select ( oldRow, component, true );
			}
		}

		public override UIView GetView (UIPickerView pickerView, nint row, nint component, UIView view)
		{
			int oldRow = (int)row;
			row = getIndex ( (int)row, (int)component );

			float width, height;
			if (items != null && items.Count () == 1) {
				width = (float)pickerView.Frame.Width;
				height = 40f;
			} else 
			{
				width = 130f;
				height = 20f;
			}

			UILabel lbl = new UILabel (new CoreGraphics.CGRect (0, 0, width, height));

			if (row >= 0) {
				lbl.TextColor = UIColor.Black;
				lbl.Font = UIFont.SystemFontOfSize (12f);
				lbl.TextAlignment = UITextAlignment.Center;

				if (items != null)
					lbl.Text = items.ElementAt ((int)component).ElementAt ((int)row);

				//if (TimesheetViewModel.projectCodeDict.ContainsKey (items.ElementAt ((int)component).ElementAt ((int)row)) && row < maxFrequentlyUsed) 
				if (usingFrequentlyUsedSection [(int)component] && row < numFrequentItems [(int)component]) {
					//lbl.TextColor = UIColor.Blue;
					//lbl.Font = UIFont.SystemFontOfSize (14f);
				} 
				/*
			else if (row > 0 && row < maxFrequentlyUsed + 1 && TimesheetViewModel.projectCodeDict.ContainsKey (items.ElementAt ((int)component).ElementAt ((int)row - 1))) 
			{
				UILabel lineLabel = new UILabel (new CoreGraphics.CGRect(0, 5, lbl.Frame.Width, 4));
				lineLabel.BackgroundColor = UIColor.Black;
				lbl.AddSubview (lineLabel);
			}
*/

			}
			else if (oldRow == 0)  //add top separator
			{
				UILabel lineLabel1 = new UILabel (new CoreGraphics.CGRect(lbl.Frame.Width*0.1f, lbl.Frame.Height*0.87f, lbl.Frame.Width*0.8f, 1));
				lineLabel1.BackgroundColor = UIColor.LightGray;
				lbl.AddSubview (lineLabel1);	

				UILabel lineLabel2 = new UILabel (new CoreGraphics.CGRect(lbl.Frame.Width*0.8f, lbl.Frame.Height/2, lbl.Frame.Width*0.1f, 1));
				lineLabel2.BackgroundColor = UIColor.Black;
				//lbl.AddSubview (lineLabel2);	

				lbl.TextColor = UIColor.Black;
				lbl.Font = UIFont.SystemFontOfSize (11f);
				lbl.TextAlignment = UITextAlignment.Center;

				lbl.Text = "Frequently Used";
			}
			else //add the separator view
			{
				UILabel lineLabel = new UILabel (new CoreGraphics.CGRect(lbl.Frame.Width*0.1f, lbl.Frame.Height/2, lbl.Frame.Width*0.8f, 1f));
				lineLabel.BackgroundColor = UIColor.LightGray;
				lbl.AddSubview (lineLabel);				
			}
			return lbl;
		}

		//one section at the top and one separating frequent items
		public int getIndex( int row, int component )
		{
			if ( !usingFrequentlyUsedSection[component] || numFrequentItems [component] == 0)
				return row;
			else if (usingFrequentlyUsedSection [component] && row < numFrequentItems [component] + 1 )
				return row-1;
			else if( row > numFrequentItems [component] + 1 )
				return row-2;
			return -1;
		}
	}
}

