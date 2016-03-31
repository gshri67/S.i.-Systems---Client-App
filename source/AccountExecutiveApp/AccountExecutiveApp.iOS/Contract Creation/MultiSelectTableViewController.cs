using System;
using System.Collections.Generic;
using System.Linq;
using AccountExecutiveApp.Core.ViewModel;
using CoreGraphics;
using Foundation;
using UIKit;
using Microsoft.Practices.Unity;
using SiSystems.SharedModels;

namespace AccountExecutiveApp.iOS
{
	public partial class MultiSelectTableViewController : UITableViewController
	{
	    private MultiSelectTableViewSource _tableSource;

        public delegate void MultiSelectDelegate(List<UserContact> contacts);
        public MultiSelectDelegate OnSelectionChanged;
        
        public MultiSelectTableViewController(IntPtr handle)
			: base(handle) { }

        public MultiSelectTableViewController()
		{
			
		}

		public void setContracts(IEnumerable<ConsultantContract> contracts)
		{
			UpdatePageTitle();
			UpdateUserInterface();
		}

		private void InstantiateTableViewSource()
		{
			if (TableView == null)
                TableView = new UITableView(View.Frame, UITableViewStyle.Grouped);

			TableView.RegisterClassForCellReuse(typeof(UITableViewCell),"UITableViewCell");

		    if (_tableSource == null)
		    {
		        _tableSource = new MultiSelectTableViewSource(this);
		        TableView.Source = _tableSource;
		    }

        

		    //TableView.ContentInset = new UIEdgeInsets(-35, 0, -35, 0);
            TableView.ReloadData();
            TableView.SetEditing(true, true);
            TableView.AllowsMultipleSelectionDuringEditing = true;
            TableView.AllowsSelectionDuringEditing = true;

            UpdateSelectedTableCells();
		}

		private void UpdateUserInterface()
		{
			InvokeOnMainThread(InstantiateTableViewSource);
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			UpdateUserInterface();
		}

	    public void SetData( List<UserContact> contacts, List<UserContact> selected  )
	    {
	        if (_tableSource == null)
	        {
	            _tableSource = new MultiSelectTableViewSource(this);
                TableView.Source = _tableSource;
	        }

	        _tableSource.Contacts = contacts;
	        _tableSource.Selected = selected;

            UpdateSelectedTableCells();

            TableView.ReloadData();
	       
	    }

	    private void UpdateSelectedTableCells()
	    {
	        if (_tableSource.Selected == null || _tableSource.Selected.Count <= 0)
	            return;
           

            TableView.SelectRow( NSIndexPath.FromItemSection(1, 0), true, UITableViewScrollPosition.None );
            //TableView.ReloadData();
	    }

	    private void UpdatePageTitle()
		{
			Title = "Contracts";
		}

	    public override void ViewWillDisappear(bool animated)
	    {
	        base.ViewWillDisappear(animated);

	        List<UserContact> selectedContacts;

            if (TableView.IndexPathsForSelectedRows == null || TableView.IndexPathsForSelectedRows.Length == 0)
	            selectedContacts = new List<UserContact>();
	        else
	            selectedContacts =
	                _tableSource.Contacts.Select((contact, index) => new {contact, index})
	                    .Where(
	                        cPair =>
	                            TableView.IndexPathsForSelectedRows.Select(path => (int) path.Item).Contains(cPair.index))
	                    .Select(cPair => cPair.contact)
	                    .ToList();

            OnSelectionChanged( selectedContacts );

	    }
	}
}
