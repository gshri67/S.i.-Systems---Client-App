using Foundation;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using ClientApp.iOS.Alumni.ConsultantDetails;
using SiSystems.SharedModels;
using UIKit;

namespace ClientApp.iOS
{
	partial class ContractsViewController : UITableViewController
	{
        public IList<Contract> Contracts { get; set; }

		public ContractsViewController (IntPtr handle) : base (handle)
		{
		}

	    public override void ViewDidLoad()
	    {
	        base.ViewDidLoad();
	    }

	    public override void ViewWillAppear(bool animated)
	    {
	        base.ViewWillAppear(animated);

            InvokeOnMainThread(delegate
                               {
                                   ContractsTable.Source = new ContractsTableViewSource(Contracts);
                                   ContractsTable.ReloadData();
                               });
	    }
	}
}
