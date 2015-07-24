using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

using Factorymind.Components;

namespace App2
{
	partial class CalendarTimeSheetViewController : UIViewController
	{
        private UICollectionViewController collectionViewController;

		public CalendarTimeSheetViewController (IntPtr handle) : base (handle)
		{
            //UICollectionViewFlowLayout layout = new UICollectionViewFlowLayout();
            //collectionViewController = new UICollectionViewController(layout);

            //Add(collectionViewController.CollectionView);

            FMCalendar fmCalendar = new FMCalendar(View.Bounds);
            View.AddSubview(fmCalendar);



            fmCalendar.DateSelected = delegate( DateTime date )
            {
				DayTimeSheetViewController dayTSVC = (DayTimeSheetViewController)Storyboard.InstantiateViewController("DayTimeSheetViewController");
                PresentViewController( dayTSVC, true, null );
            };
		}
	}
}
