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
		private string Subtitle;
	    private MultiSelectTableViewSource _tableSource;
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

	    public void SetData( string[] titles )
	    {
	        if (_tableSource == null)
	        {
	            _tableSource = new MultiSelectTableViewSource(this);
                TableView.Source = _tableSource;
	        }

	        _tableSource.ListTitles = titles;
            TableView.ReloadData();
	       
	    }

	    private void UpdatePageTitle()
		{
			Title = "Contracts";
		}
	}
}
