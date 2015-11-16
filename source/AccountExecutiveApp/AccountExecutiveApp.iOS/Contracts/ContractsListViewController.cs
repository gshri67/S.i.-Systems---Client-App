using Foundation;
using System;
using System.CodeDom.Compiler;
using System.Collections;
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

	    private bool _contractsWereSet = false;//this is to know whether the contracts were passed in, or if we should call the API
	    ContractsViewModel _contractsViewModel;
        public ContractStatusType StatusType;//atm only used when loading contracts because they were not passed in
	    public ContractType TypeOfContract;

        public ContractsListViewController(IntPtr handle)
            : base(handle)
		{
            _contractsViewModel = DependencyResolver.Current.Resolve<ContractsViewModel>();
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

            if( !_contractsWereSet )
                LoadContracts();
			
            
            CreateCustomTitleBar();

			UpdateUI ();
		}

		public void setContracts( IEnumerable<ConsultantContract> contracts )
		{
		    _contractsWereSet = true; 
			_contracts = contracts;
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);

			//TableView.ReloadData ();
		}

        public async void LoadContracts()
        {
            if (_contracts != null) return;

            IEnumerable<ConsultantContract> contracts = await _contractsViewModel.getContracts();

			if (!_contractsWereSet) 
			{
				_contracts = contracts.Where (c => c.StatusType == StatusType && c.ContractType == TypeOfContract).ToList ();

				if (_contracts.Count() <= 0)
					_contracts = null;
			}

            UpdateUI();
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
