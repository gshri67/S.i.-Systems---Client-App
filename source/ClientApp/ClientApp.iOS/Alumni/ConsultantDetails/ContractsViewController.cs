using Foundation;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using SiSystems.ClientApp.SharedModels;
using UIKit;

namespace ClientApp.iOS
{
	partial class ContractsViewController : UITableViewController
	{
        public IEnumerable<Contract> Contracts { get; set; }

		public ContractsViewController (IntPtr handle) : base (handle)
		{
		}

	    public override void ViewDidLoad()
	    {
	        base.ViewDidLoad();
	    }
	}
}
