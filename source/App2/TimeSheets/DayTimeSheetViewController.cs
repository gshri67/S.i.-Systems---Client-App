using System;
using System.Drawing;

using Foundation;
using UIKit;



namespace App2
{
    public partial class DayTimeSheetViewController : UIViewController
    {
        public DayTimeSheetViewController()
            : base(null, null)
        {
        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = UIColor.Blue;

            // Perform any additional setup after loading the view, typically from a nib.
        }
    }
}