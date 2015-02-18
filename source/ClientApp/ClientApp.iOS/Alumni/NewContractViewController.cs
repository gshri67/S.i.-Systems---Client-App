using Foundation;
using System;
using System.CodeDom.Compiler;
using SiSystems.ClientApp.SharedModels;
using UIKit;

namespace ClientApp.iOS
{
	partial class NewContractViewController : UITableViewController
	{
        public Consultant Consultant { get; set; }

        public NewContractViewController (IntPtr handle) : base (handle)
		{
		}
	}
}
