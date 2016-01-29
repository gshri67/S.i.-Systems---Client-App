using System;
using UIKit;
using Foundation;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AccountExecutiveApp.iOS
{
    public class PickerViewModel : UIPickerViewModel
    {
        public delegate void pickerViewDelegate(string item);
        public pickerViewDelegate onSelected;
        public List<List<string>> items;
        public List<int> selectedItemIndex;

        public delegate void PickerDelegate( string newValue);
        public PickerDelegate OnValueChanged;

        public PickerViewModel()
        {
            selectedItemIndex = new List<int>();
            selectedItemIndex.Add(0);
        }

        public string GetSelectedPickerItemValue(int componentId)
        {
            return items.ElementAt(componentId).ElementAt(selectedItemIndex[componentId]);
        }

        public override nint GetComponentCount(UIPickerView picker)
        {
            if (items == null)
                return 1;
            else
            {
                //the picker does not automatically select the first row. As such we need to make sure everything is covered right away, such as having all the selected Indicies preset to 0.
                while (selectedItemIndex.Count() < items.Count())
                {
                    selectedItemIndex.Add(0);
                }

                return items.Count();
            }
        }

        public override nint GetRowsInComponent(UIPickerView picker, nint component)
        {
            if (items != null)
            {
                return items.ElementAt((int)component).Count();
            }
            else
                return 0;
        }

        public override string GetTitle(UIPickerView pickerView, nint row, nint component)
        {
            if (items == null)
                return "";
            else
            {
                return items.ElementAt((int)component).ElementAt((int)row);
            }
        }

        public override void Selected(UIPickerView pickerView, nint row, nint component)
        {
            int oldRow = (int)row;
            row = getIndex((int)row, (int)component);

            if (row >= 0)
                selectedItemIndex[(int)component] = (int)row;
            else
            {
                if (oldRow - 1 > 0 && getIndex((int)oldRow - 1, (int)component) >= 0)
                {
                    row = getIndex((int)oldRow - 1, (int)component);
                    oldRow--;
                }
                else
                {
                    row = getIndex((int)oldRow + 1, (int)component);
                    oldRow++;
                }
                selectedItemIndex[(int)component] = (int)row;
                pickerView.Select(oldRow, component, true);
            }

            if (component == 0)
                Console.WriteLine("Selected item index: " + selectedItemIndex[0]);

            if( OnValueChanged != null )
                OnValueChanged( items[(int)component][(int)row] );
        }

        public override UIView GetView(UIPickerView pickerView, nint row, nint component, UIView view)
        {
            int oldRow = (int)row;
            row = getIndex((int)row, (int)component);

            float width, height;
            if (items != null && items.Count() == 1)
            {
                width = (float)pickerView.Frame.Width;
                height = 40f;
            }
            else
            {
                width = 130f;
                height = 20f;
            }

            UILabel lbl = new UILabel(new CoreGraphics.CGRect(0, 0, width, height));

            if (row >= 0)
            {
                lbl.TextColor = UIColor.Black;
                lbl.Font = UIFont.SystemFontOfSize(12f);
                lbl.TextAlignment = UITextAlignment.Center;

                if (items != null)
                    lbl.Text = items.ElementAt((int)component).ElementAt((int)row);
            }
            else if (oldRow == 0)  //add top separator
            {
                UILabel lineLabel1 = new UILabel(new CoreGraphics.CGRect(lbl.Frame.Width * 0.1f, lbl.Frame.Height * 0.87f, lbl.Frame.Width * 0.8f, 1));
                lineLabel1.BackgroundColor = UIColor.LightGray;
                lbl.AddSubview(lineLabel1);

                UILabel lineLabel2 = new UILabel(new CoreGraphics.CGRect(lbl.Frame.Width * 0.8f, lbl.Frame.Height / 2, lbl.Frame.Width * 0.1f, 1));
                lineLabel2.BackgroundColor = UIColor.Black;
                //lbl.AddSubview (lineLabel2);	

                lbl.TextColor = UIColor.Black;
                lbl.Font = UIFont.SystemFontOfSize(11f);
                lbl.TextAlignment = UITextAlignment.Center;

                lbl.Text = "Frequently Used";
            }
            else //add the separator view
            {
                UILabel lineLabel = new UILabel(new CoreGraphics.CGRect(lbl.Frame.Width * 0.1f, lbl.Frame.Height / 2, lbl.Frame.Width * 0.8f, 1f));
                lineLabel.BackgroundColor = UIColor.LightGray;
                lbl.AddSubview(lineLabel);
            }
            return lbl;
        }

        //one section at the top and one separating frequent items
        public int getIndex(int row, int component)
        {
            return row;
        }
        public int getRowForItemIndex(int itemIndex, int component)
        {
            return itemIndex;
        }

        public void scrollToItemIndex(UIPickerView picker, int itemIndex, int component)
        {
            int row = getRowForItemIndex(itemIndex, component);

            if (row >= 0)
            {
                selectedItemIndex[component] = itemIndex;//if you don't move the picker manually from it's initial position I don't think selected gets called which doesn't update the selectedItemIndex
                picker.Select(row, component, true);
            }
        }
    }
}

