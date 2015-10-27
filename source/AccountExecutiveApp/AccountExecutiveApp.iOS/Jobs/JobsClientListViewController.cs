using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace AccountExecutiveApp.iOS
{
	partial class JobsClientListViewController : UITableViewController
	{
		private JobsClientListTableViewSource _clientListTableViewSource;

		public JobsClientListViewController (IntPtr handle) : base (handle)
		{
		}

		private void SetupTableViewSource()
		{
			if (TableView == null )
				return;

			RegisterCellsForReuse();
			InstantiateTableViewSource();

			TableView.Source = _clientListTableViewSource;
		}

		private void InstantiateTableViewSource()
		{
			_clientListTableViewSource = new JobsClientListTableViewSource ();

			//_addTimeTableViewSource.OnDataChanged += AddTimeTableDataChanged;
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			SetupTableViewSource ();

			TableView.ReloadData ();
		}

		private void RegisterCellsForReuse()
		{
			if (TableView == null) return;

			TableView.RegisterClassForCellReuse(typeof (RightDetailCell), "RightDetailCell");
			TableView.RegisterClassForCellReuse(typeof (UITableViewCell), "cell");
		}
	}
}
