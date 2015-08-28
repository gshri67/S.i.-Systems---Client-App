
//	New implementations, refactoring and restyling - FactoryMind || http://factorymind.com 
//  Converted to MonoTouch on 1/22/09 - Eduardo Scoz || http://escoz.com
//  Originally reated by Devin Ross on 7/28/09  - tapku.com || http://github.com/devinross/tapkulibrary
//
/*
 
 Permission is hereby granted, free of charge, to any person
 obtaining a copy of this software and associated documentation
 files (the "Software"), to deal in the Software without
 restriction, including without limitation the rights to use,
 copy, modify, merge, publish, distribute, sublicense, and/or sell
 copies of the Software, and to permit persons to whom the
 Software is furnished to do so, subject to the following
 conditions:
 
 The above copyright notice and this permission notice shall be
 included in all copies or substantial portions of the Software.
 
 THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
 OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
 HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 OTHER DEALINGS IN THE SOFTWARE.
 
 */

using System;
using System.Drawing;

#if __UNIFIED__
using UIKit;

// Mappings Unified CoreGraphic classes to MonoTouch classes
using CGRect = global::System.Drawing.RectangleF;
using CGSize = global::System.Drawing.SizeF;
using CGPoint = global::System.Drawing.PointF;

// Mappings Unified types to MonoTouch types
using nfloat = global::System.Single;
using nint = global::System.Int32;
using nuint = global::System.UInt32;
#else
using MonoTouch.UIKit;
#endif

namespace ConsultantApp.iOS
{
	sealed class CalendarDayView : UIView
	{
		string text;
		int fontSize = 10;

		public DateTime Date {get;set;}
		bool _active, _today, _selected, _marked, _available;
		public bool Available {get {return _available; } set {_available = value; SetNeedsDisplay(); }}
		public string Text {get { return text; } set { text = value; SetNeedsDisplay(); } }
		public bool Active {get { return _active; } set { _active = value; SetNeedsDisplay();  } }
		public bool Today {get { return _today; } set { _today = value; SetNeedsDisplay(); } }
		public bool Selected {get { return _selected; } set { _selected = value; SetNeedsDisplay(); } }
		public bool Marked {get { return _marked; } set { _marked = value; SetNeedsDisplay(); }  }

		public UIColor SelectionColor { get; set; }
		public UIColor TodayCircleColor { get; set; }

		public string totalHours;

		public CalendarDayView()
		{
			this.BackgroundColor = UIColor.White;
			this.TodayCircleColor = StyleGuideConstants.RedUiColor;//UIColor.FromRGBA(1.0f, 0.3f, 0.0f, 0.5f);
			this.SelectionColor = UIColor.FromRGBA(0.7f, 0.7f, 0.9f, 1.0f);

			unselect ();//apply normal borders to the cell
		}

		#if __UNIFIED__

		public override void Draw (CoreGraphics.CGRect rect)
		{
			PerformDraw ();
		}

		#else

		public override void Draw(RectangleF rect)
		{
		PerformDraw ();
		}

		#endif

		private void PerformDraw()
		{
			UIColor color = UIColor.Black;

			if (Selected) 
			{
				//Layer.BorderWidth = 2f;
				//Layer.BorderColor = UIColor.FromRGB(0.4f, 0.5f, 0.9f).CGColor;
			}

			if (!Active || !Available)
			{
				color = UIColor.LightGray;
				if(Selected)
					color = SelectionColor;
			} else if (Today && Selected)
			{
				color = UIColor.White;
			} else if (Today)
			{
				color = UIColor.White;
			} else if (Selected)
			{
				color = SelectionColor;
			}

			int hoursFontSize = 16;

			//CoreGraphics.CGRect dayTextRect = new CoreGraphics.CGRect (Bounds.Width * 1.2 / 10, Bounds.Height * 1.2 / 10, Bounds.Width, Bounds.Height / 3);
			CoreGraphics.CGRect dayTextRect = new CoreGraphics.CGRect (Bounds.Width * 1.1 / 10, Bounds.Height * 1.1 / 10, Bounds.Width/3, Bounds.Width/3);


			if (Selected || Today )
			{
				var context = UIGraphics.GetCurrentContext();
				var todaySize = (float) Math.Min (Bounds.Height, Bounds.Width);
				if (todaySize > 50)
					todaySize = 50;
				todaySize = Math.Min (fontSize * 2, todaySize)*0.75f;

				if (Selected && Today)
					TodayCircleColor.SetColor ();
				else if (Today)
					UIColor.FromWhiteAlpha ( 0.9f, 0.5f ).SetColor();
				else //if selected and not today
					UIColor.FromWhiteAlpha (0.3f, 1.0f).SetColor();

				context.SetLineWidth(0);

				#if __UNIFIED__

				todaySize = (float) Math.Min (Bounds.Height, Bounds.Width)*0.25f;
				//CoreGraphics.CGRect todayCircleRect = new CoreGraphics.CGRect ((Bounds.Width-todaySize)/2, (Bounds.Height-todaySize*1.2f), todaySize, todaySize);

				CoreGraphics.CGRect todayCircleRect = new CoreGraphics.CGRect (dayTextRect.X, dayTextRect.Y, dayTextRect.Width, dayTextRect.Height);
				
				context.AddEllipseInRect( todayCircleRect );//(new CoreGraphics.CGRect((Bounds.Width / 2) - (todaySize / 2), (Bounds.Height / 2) - (todaySize / 2), todaySize, todaySize));
				
				#else

				context.AddEllipseInRect(new RectangleF((Bounds.Width / 2) - (todaySize / 2), (Bounds.Height / 2) - (todaySize / 2), todaySize, todaySize));

				#endif

				context.FillPath();
			}


			UIColor.Black.SetColor();

			#if __UNIFIED__

			if( totalHours != null && !totalHours.Equals("0") )
				totalHours.DrawString (new CoreGraphics.CGRect (0, (Bounds.Height / 2) - (hoursFontSize / 2), Bounds.Width, Bounds.Height),
					UIFont.SystemFontOfSize (hoursFontSize), UILineBreakMode.WordWrap,
					UITextAlignment.Center);

			if( Selected )
				UIColor.White.SetColor();				
			else if( !Selected && !Today )		
				UIColor.FromWhiteAlpha(0.8f, 1.0f).SetColor();
			else //if it is today and not selected
				TodayCircleColor.SetColor();
			
			Text.DrawString (dayTextRect,
				UIFont.SystemFontOfSize (fontSize), UILineBreakMode.WordWrap,
				UITextAlignment.Center);

			color.SetColor();

			#else

			DrawString(Text, new RectangleF(0, (Bounds.Height / 2) - (fontSize / 2), Bounds.Width, Bounds.Height),
			UIFont.SystemFontOfSize (fontSize), 
			UILineBreakMode.WordWrap, UITextAlignment.Center);

			#endif


			if (Marked)
			{
				var context = UIGraphics.GetCurrentContext();
				if (Selected && !Today) 
				{
					SelectionColor.SetColor ();
				}
				else if (Today)
					UIColor.White.SetColor ();
				else if (!Active || !Available)
					UIColor.LightGray.SetColor ();
				else
					UIColor.Black.SetColor ();

				context.SetLineWidth(0);

				#if __UNIFIED__

				var todaySize = (float) Math.Min (Bounds.Height, Bounds.Width)*0.9f;
				CoreGraphics.CGRect circleRect = new CoreGraphics.CGRect ((Bounds.Width-todaySize)/2, (Bounds.Height-todaySize)/2, todaySize, todaySize);

				//context.AddEllipseInRect( circleRect );
				context.AddEllipseInRect(new CoreGraphics.CGRect(Frame.Size.Width/2 - 2, (Bounds.Height / 2) + (fontSize / 2) + 5, 4, 4));

				#else

				context.AddEllipseInRect(new RectangleF(Frame.Size.Width/2 - 2, (Bounds.Height / 2) + (fontSize / 2) + 5, 4, 4));

				#endif

				context.FillPath();

			}
		}

		public void unselect()
		{
			Selected = false;

			Layer.BorderWidth = 0.5f;
			Layer.BorderColor = StyleGuideConstants.LightGrayUiColor.CGColor;//UIColor.FromWhiteAlpha(0.8f, 1.0f).CGColor;
		}
	}
}