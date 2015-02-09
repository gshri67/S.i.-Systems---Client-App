using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace ClientApp.iOS
{
	partial class DisciplineViewController : UIViewController
	{
		public DisciplineViewController (IntPtr handle) : base (handle)
		{
		}
        #region View lifecycle

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Perform any additional setup after loading the view, typically from a nib.
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            //get our list of specializations to display
            //var consultantGroups = GetConsultantGroups();

            //set the source for our table's data
            //SpecializationTable.Source = new ContractsTableViewSource(this, consultantGroups);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
        }

        #endregion
	}
}
