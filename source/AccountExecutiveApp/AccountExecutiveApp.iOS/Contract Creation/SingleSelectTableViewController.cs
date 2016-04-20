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
	public partial class SingleSelectTableViewController : UITableViewController
	{
	    private SingleSelectTableViewSource _tableSource;

        public delegate void SingleSelectDelegate(UserContact contacts);
        public SingleSelectDelegate OnSelectionChanged;
        
        public SingleSelectTableViewController(IntPtr handle)
			: base(handle) { }

        public SingleSelectTableViewController()
		{
			
		}

		private void InstantiateTableViewSource()
		{
			if (TableView == null)
                TableView = new UITableView(View.Frame, UITableViewStyle.Grouped);

			TableView.RegisterClassForCellReuse(typeof(UITableViewCell),"UITableViewCell");

		    if (_tableSource == null)
		    {
		        _tableSource = new SingleSelectTableViewSource(this);
		        TableView.Source = _tableSource;
		    }

		    TableView.AllowsSelection = true;
            TableView.ReloadData();

            UpdateSelectedTableCell();
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

	    public void SetData( List<UserContact> contacts, UserContact selected  )
	    {
	        if (_tableSource == null)
	        {
	            _tableSource = new SingleSelectTableViewSource(this);
                TableView.Source = _tableSource;
	        }

	        _tableSource.Contacts = contacts;
	        _tableSource.Selected = selected;

            UpdateSelectedTableCell();

            TableView.ReloadData();
	       
	    }

	    private void UpdateSelectedTableCell()
	    {
            if (_tableSource == null || _tableSource.Selected == null || _tableSource.Contacts == null || _tableSource.Contacts.Count == 0)
	            return;

	        int index = -1;

	        try
	        {
	            index = _tableSource.Contacts.FindIndex(c => c.FullName == _tableSource.Selected.FullName);
	        }
	        catch (Exception)
	        {
	        }

	        if (index == -1)
	        {
	            _tableSource.Selected = null;

	            return;
	        }

	        TableView.SelectRow(NSIndexPath.FromItemSection(index, 0), true, UITableViewScrollPosition.Middle);
 
	    }

	    private void UpdatePageTitle()
		{
			Title = string.Empty;
		}

	    public override void ViewWillDisappear(bool animated)
	    {
	        base.ViewWillDisappear(animated);
         
            if( _tableSource.Selected != null )
                OnSelectionChanged( _tableSource.Selected );
	    }
	}
}
