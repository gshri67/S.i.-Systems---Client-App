using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using AccountExecutiveApp.Core.ViewModel;
using Microsoft.Practices.Unity;
using SiSystems.SharedModels;
using System.Collections.Generic;
using System.Linq;

namespace AccountExecutiveApp.iOS
{
	[Register ("ContractsListViewController")]
	partial class ContractsListViewController : UITableViewController
	{
        private ContractsListTableViewSource _listTableViewSource;
		public IEnumerable<ConsultantContract> _contracts;
		public string Subtitle;
	    private SubtitleHeaderView _subtitleHeaderView;

        public ContractsListViewController(IntPtr handle)
            : base(handle)
		{
		}

		private void SetupTableViewSource()
		{
			if (TableView == null || _contracts == null )
				return;

			RegisterCellsForReuse();
			InstantiateTableViewSource();

			TableView.Source = _listTableViewSource;
		}

		private void InstantiateTableViewSource()
		{
			_listTableViewSource = new ContractsListTableViewSource ( this, _contracts );

			//_addTimeTableViewSource.OnDataChanged += AddTimeTableDataChanged;
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			//SetupTableViewSource ();

			//TableView.ReloadData ();

            CreateCustomTitleBar();

			UpdateUI ();


			//TableView.ReloadData ();
		}

		public void setContracts( IEnumerable<ConsultantContract> contracts )
		{
			_contracts = contracts;
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);

			//TableView.ReloadData ();
		}

		private void RegisterCellsForReuse()
		{
			if (TableView == null) return;
		
			//TableView.RegisterClassForCellReuse(typeof (RightDetailCell), "RightDetailCell");
			//TableView.RegisterClassForCellReuse(typeof (UITableViewCell), "cell");
			TableView.RegisterClassForCellReuse(typeof(SubtitleWithRightDetailCell), "SubtitleWithRightDetailCell");
		}

		public void UpdateUI()
		{
			if (_contracts != null && TableView != null)
			{
				SetupTableViewSource ();
				TableView.ReloadData ();
			}
		}

        private void CreateCustomTitleBar()
        {
            InvokeOnMainThread(() =>
            {
                _subtitleHeaderView = new SubtitleHeaderView();
                NavigationItem.TitleView = _subtitleHeaderView;
                _subtitleHeaderView.TitleText = Title;
                _subtitleHeaderView.SubtitleText = Subtitle;
                NavigationItem.Title = "";
            });
        }
	}
}
