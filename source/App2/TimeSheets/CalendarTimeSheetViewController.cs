using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace App2
{
	partial class CalendarTimeSheetViewController : UIViewController
	{
        private UICollectionViewController collectionViewController;

		public CalendarTimeSheetViewController (IntPtr handle) : base (handle)
		{
            UICollectionViewFlowLayout layout = new UICollectionViewFlowLayout();
            collectionViewController = new UICollectionViewController(layout);

            Add(collectionViewController.CollectionView);
		}
	}
}
