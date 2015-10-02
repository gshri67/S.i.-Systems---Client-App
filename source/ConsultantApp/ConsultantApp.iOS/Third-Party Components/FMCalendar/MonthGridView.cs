﻿//	New implementations, refactoring and restyling - FactoryMind || http://factorymind.com 
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
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

#if __UNIFIED__
using Foundation;
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
using MonoTouch.Foundation;
using MonoTouch.UIKit;
#endif

using ConsultantApp.Core.ViewModels;
using SiSystems.SharedModels;
using Shared.Core;
using Microsoft.Practices.Unity;

namespace ConsultantApp.iOS
{
	class MonthGridView : UIView
	{
		private FMCalendar _calendarMonthView;

		public DateTime CurrentDate { get; set; }
		public DateTime SelectedDate { get { return _calendarMonthView.SelectedDate; } }

		private DateTime _currentMonth;
		protected readonly IList<CalendarDayView> _dayTiles = new List<CalendarDayView>();
		public int Lines { get { return 6; } }
		protected CalendarDayView SelectedDayView { get; set; }
		public int weekdayOfFirst;
		public IList<DateTime> Marks { get; set; }

		private float WeekDayWidth { get { return _calendarMonthView.DayCellWidth; } }
		private float WeekDayHeigth { get { return _calendarMonthView.DayCellHeight; } }

		private readonly IEnumerable<TimeEntry> _timeEntries;
	    private readonly DateTime _startDate;
	    private readonly DateTime _endDate;

		public MonthGridView(FMCalendar calendarMonthView, DateTime month, DateTime startDate, DateTime endDate, IEnumerable<TimeEntry> timeEntries )
		{
			_calendarMonthView = calendarMonthView;
			_currentMonth = month.Date;

			BackgroundColor = UIColor.White;//StyleGuideConstants.LightGrayUiColor;// _calendarMonthView.MonthBackgroundColor;

		    _startDate = startDate;
		    _endDate = endDate;
            _timeEntries = timeEntries ?? Enumerable.Empty<TimeEntry>();
		}

		public void Update(){
			foreach (var v in _dayTiles)
				UpdateDayView(v);

			this.SetNeedsDisplay();
		}

		public void UpdateDayView(CalendarDayView dayView){
			dayView.Marked = _calendarMonthView.IsDayMarkedDelegate == null ? 
				false : _calendarMonthView.IsDayMarkedDelegate(dayView.Date);
			dayView.Available = _calendarMonthView.IsDateAvailable == null ? 
				true : _calendarMonthView.IsDateAvailable(dayView.Date);
		}

		private int WeekDayIndex(DayOfWeek day)
		{
			var index = (int)day;

			if (!_calendarMonthView.SundayFirst) {
				if (day == DayOfWeek.Sunday)
					index = (int)DayOfWeek.Saturday;
				else
					index--;
			}

			return index;
		}

		public void BuildGrid()
		{
			//UIColor outsidePeriodColor = UIColor.FromWhiteAlpha (0.7f, 1.0f);//UIColor.Gray;
		    UIColor outsidePeriodColor = StyleGuideConstants.LighterGrayUiColor;

			DateTime previousMonth = _currentMonth.AddMonths(-1);
			DateTime nextMonth = _currentMonth.AddMonths(1);
			var daysInPreviousMonth = DateTime.DaysInMonth(previousMonth.Year, previousMonth.Month);
			var daysInMonth = DateTime.DaysInMonth(_currentMonth.Year, _currentMonth.Month);
			weekdayOfFirst = WeekDayIndex(_currentMonth.DayOfWeek);
			var lead = daysInPreviousMonth - (weekdayOfFirst - 1);

			// build last month's days
			for (int i = 1; i <= weekdayOfFirst; i++)
			{
				var viewDay = new DateTime(previousMonth.Year, previousMonth.Month, i);
				var dayView = new CalendarDayView { SelectionColor = _calendarMonthView.SelectionColor, TodayCircleColor = _calendarMonthView.TodayCircleColor, BackgroundColor = _calendarMonthView.MonthBackgroundColor };

				#if __UNIFIED__

				dayView.Frame = new CoreGraphics.CGRect((i - 1) * WeekDayWidth, 0, WeekDayWidth, WeekDayHeigth);

				#else

				dayView.Frame = new RectangleF((i - 1) * WeekDayWidth, 0, WeekDayWidth, WeekDayHeigth);

				#endif

				dayView.Date = viewDay;
				dayView.Text = lead.ToString();
				dayView.BackgroundColor = outsidePeriodColor;

				AddSubview(dayView);
				_dayTiles.Add(dayView);
				lead++;
			}

			var position = weekdayOfFirst+1;
			var line = 0;

			// current month
			for (int i = 1; i <= daysInMonth; i++)
			{
				var viewDay = new DateTime(_currentMonth.Year, _currentMonth.Month, i);
				var dayView = new CalendarDayView
				{
					#if __UNIFIED__

					Frame = new CoreGraphics.CGRect((position - 1) * WeekDayWidth, line * WeekDayHeigth, WeekDayWidth, WeekDayHeigth),

					#else

					Frame = new RectangleF((position - 1) * WeekDayWidth, line * WeekDayHeigth, WeekDayWidth, WeekDayHeigth),

					#endif

					Text = i.ToString(),

					Active = true,
					Tag = i,
					Selected = viewDay.Date == SelectedDate.Date,
					SelectionColor = _calendarMonthView.SelectionColor,
					TodayCircleColor = _calendarMonthView.TodayCircleColor,
					BackgroundColor = _calendarMonthView.MonthBackgroundColor,
				};

				if (DateTime.Now.ToString ("MMM dd yyyy").Equals (viewDay.Date.ToString ("MMM dd yyyy")))
					dayView.Today = true;

				dayView.Date = viewDay;

				dayView.totalHours = _timeEntries.Where(e => e.Date.Equals(viewDay) ).Sum( t => t.Hours ).ToString();
				if ( !insideTimePeriod(viewDay.Date) )
					dayView.BackgroundColor = outsidePeriodColor;

				UpdateDayView(dayView);

				if (dayView.Selected)
					SelectedDayView = dayView;

				AddSubview(dayView);
				_dayTiles.Add(dayView);

				position++;
				if (position > 7)
				{
					position = 1;
					line++;
				}
			}

			//next month
			if (position != 1)
			{
				int dayCounter = 1;
				for (int i = position; i < 8; i++)
				{
					var viewDay = new DateTime(nextMonth.Year, nextMonth.Month, i);
					var dayView = new CalendarDayView
					{
						#if __UNIFIED__

						Frame = new CoreGraphics.CGRect((i - 1) * WeekDayWidth, line * WeekDayHeigth, WeekDayWidth, WeekDayHeigth),

						#else

						Frame = new RectangleF((i - 1) * WeekDayWidth, line * WeekDayHeigth, WeekDayWidth, WeekDayHeigth),

						#endif

						Text = dayCounter.ToString(),
						SelectionColor = _calendarMonthView.SelectionColor,
						TodayCircleColor = _calendarMonthView.TodayCircleColor,
						BackgroundColor = _calendarMonthView.MonthBackgroundColor,
					};
					dayView.Date = viewDay;
					dayView.BackgroundColor = outsidePeriodColor;
					UpdateDayView(dayView);

					AddSubview(dayView);
					_dayTiles.Add(dayView);
					dayCounter++;
				}
			}

			#if __UNIFIED__

			Frame = new CoreGraphics.CGRect(Frame.Location, new CoreGraphics.CGSize(Frame.Width, (line + 1) * WeekDayHeigth));

			#else

			Frame = new RectangleF(Frame.Location, new SizeF(Frame.Width, (line + 1) * WeekDayHeigth));

			#endif

			//Lines = (position == 1 ? line - 1 : line);

			if (SelectedDayView!=null)
				this.BringSubviewToFront(SelectedDayView);
		}

		public bool insideTimePeriod ( DateTime date )
		{
			if (date.CompareTo (_endDate) == 1 || date.CompareTo (_startDate) == -1 )
				return false;
			return true;
		}

		public override void TouchesBegan (NSSet touches, UIEvent evt)
		{
			base.TouchesBegan (touches, evt);

			DispatchDateSelection (touches);
		}

		public override void TouchesMoved (NSSet touches, UIEvent evt)
		{
			base.TouchesMoved (touches, evt);

			DispatchDateSelection (touches);
		}

		public override void TouchesEnded (NSSet touches, UIEvent evt)
		{
			base.TouchesEnded (touches, evt);

			if (_calendarMonthView.DateSelectionFinished==null) return;

			_calendarMonthView.SelectedDate = new DateTime(_currentMonth.Year, _currentMonth.Month, (int) SelectedDayView.Tag);

			if (_calendarMonthView.IsDateAvailable == null || _calendarMonthView.IsDateAvailable(SelectedDate))
				_calendarMonthView.DateSelectionFinished(SelectedDate);
		}

		void DispatchDateSelection(NSSet touches)
		{
			if (SelectDayView ((UITouch)touches.AnyObject))
			{
				_calendarMonthView.SelectedDate = new DateTime(_currentMonth.Year, _currentMonth.Month, (int) SelectedDayView.Tag);

				if(_calendarMonthView.DateSelected != null)
					_calendarMonthView.DateSelected (SelectedDate);
			}
		}

		private bool SelectDayView(UITouch touch){
			var p = touch.LocationInView(this);

			int index = (int)(p.Y / WeekDayHeigth) * 7 + (int)(p.X / WeekDayWidth);

			if(index<0 || index >= _dayTiles.Count) return false;

			var newSelectedDayView = _dayTiles[index];

			if ( !insideTimePeriod(newSelectedDayView.Date) ) 
				return false;

			if (!newSelectedDayView.Active && touch.Phase!=UITouchPhase.Moved){
				var day = int.Parse(newSelectedDayView.Text);
				if (day > 15)
					_calendarMonthView.MoveCalendarMonths(false, true);
				else
					_calendarMonthView.MoveCalendarMonths(true, true);
				return false;
			} else if (!newSelectedDayView.Active && !newSelectedDayView.Available){
				return false;
			}
			else if (newSelectedDayView.Date.Month != _currentMonth.Month)
			{
				return false;
			}

			if (SelectedDayView != null)
				SelectedDayView.unselect();

			this.BringSubviewToFront(newSelectedDayView);
			newSelectedDayView.Selected = true;

			SelectedDayView = newSelectedDayView;
			SetNeedsDisplay();
			return true;
		}

		public void DeselectDayView(){
			if (SelectedDayView==null) return;
			SelectedDayView.Selected= false;
			SelectedDayView = null;
			SetNeedsDisplay();
		}
	}
}