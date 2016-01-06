using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using AccountExecutiveApp.Core.ViewModel;
using Microsoft.Practices.Unity;
using SiSystems.SharedModels;
using System.Collections.Generic;
using System.Linq;
using CoreGraphics;

namespace AccountExecutiveApp.iOS
{
	public partial class SearchSectionTotalResultsTableViewController : UITableViewController
	{
		private LoadingOverlay _overlay;
	    IEnumerable<UserContact> _contacts;

		public const string CellReuseIdentifier = RightDetailCell.CellIdentifier;

        public SearchSectionTotalResultsTableViewController(IntPtr handle)
            : base(handle){}

	    private bool instantiatedTableView = false;
	    private bool _isClientContacts;

	    public void SetData(IEnumerable<UserContact> contacts, bool isClientContacts)
        {
            _contacts = contacts;
	        _isClientContacts = isClientContacts;

            if (isClientContacts)
                Title = "Client Contacts";
            else
                Title = "Contractors";

            InstantiateTableViewSource();
        }

		private void InstantiateTableViewSource()
		{
			if (TableView == null || _contacts == null || instantiatedTableView) return;

            instantiatedTableView = true;

            TableView.RegisterClassForCellReuse(typeof(RightDetailCell), CellReuseIdentifier);
            TableView.Source = new SectionSearchResultsTableViewSource(this, _contacts, _isClientContacts );
			TableView.ReloadData();
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

            InstantiateTableViewSource();
		}
	}
}


