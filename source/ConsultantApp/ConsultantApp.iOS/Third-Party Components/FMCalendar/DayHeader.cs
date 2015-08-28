using System;
using UIKit;
using Foundation;
using System.Collections.Generic;

namespace ConsultantApp.iOS
{
	public class DayHeader : UIView
	{
		List<UILabel> dayLabelList;

		public DayHeader (CoreGraphics.CGRect rect)
		{
			Frame = rect;
			dayLabelList = new List<UILabel> ();

			float dayWidth = (float)rect.Width/7.0f, dayHeight = (float)rect.Height;
			string[] dayLetter = new string[]{ "S", "M", "T", "W", "T", "F", "S"  };

			for (int i = 0; i < 7; i++) 
			{
				dayLabelList.Add ( new UILabel( new CoreGraphics.CGRect(i*dayWidth, 0, dayWidth, dayHeight)  ) );
				dayLabelList [i].BackgroundColor = UIColor.White;
				dayLabelList [i].Text = dayLetter [i];
				dayLabelList [i].TextColor = UIColor.Black;
				dayLabelList [i].TextAlignment = UITextAlignment.Center;
				dayLabelList [i].Font = UIFont.SystemFontOfSize (10);

				AddSubview(dayLabelList[i]);
			}
		}
	}
}

