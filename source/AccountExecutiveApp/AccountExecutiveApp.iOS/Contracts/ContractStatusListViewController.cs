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
	partial class ContractStatusListViewController : UITableViewController
	{
		private ContractStatusListTableViewSource _clientListTableViewSource;
        private ContractsViewModel _contractsViewModel;
        private IEnumerable<ConsultantContractSummary> _contracts;
	    private LoadingOverlay _overlay;

		public ContractStatusListViewController (IntPtr handle) : base (handle)
		{
            _contractsViewModel = DependencyResolver.Current.Resolve<ContractsViewModel>();
		}

		private void SetupTableViewSource()
		{
			if (TableView == null || _contracts == null)
				return;

			RegisterCellsForReuse();
			InstantiateTableViewSource();
			//TableView.ContentInset = new UIEdgeInsets (-35, 0, -35, 0);
			TableView.Source = _clientListTableViewSource;
		}

		private void InstantiateTableViewSource()
		{
            _clientListTableViewSource = new ContractStatusListTableViewSource(this, _contracts);

			//_addTimeTableViewSource.OnDataChanged += AddTimeTableDataChanged;
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

            this.NavigationController.TabBarItem.SelectedImage = UIImage.FromFile("paper-dark.png");

			LogoutManager.CreateNavBarLeftButton (this);

            LoadContracts();
            IndicateLoading();

			//SetupTableViewSource ();

			//TableView.ReloadData ();
		}

		private void RegisterCellsForReuse()
		{
			if (TableView == null) return;

			TableView.RegisterClassForCellReuse(typeof (SubtitleWithRightDetailCell), "SubtitleWithRightDetailCell");
			TableView.RegisterClassForCellReuse(typeof (UITableViewCell), "cell");
		}

        public void UpdateUI()
        {
			if ( _contracts != null)
            { 
				SetupTableViewSource ();
				TableView.ReloadData ();
                RemoveOverlay();
			}
        }

        public async void LoadContracts()
        {
            if (_contracts != null) return;

            _contracts = await _contractsViewModel.getContracts();


            UpdateUI();
        }


        #region Overlay

        private void IndicateLoading()
        {
            InvokeOnMainThread(delegate
            {
                if (_overlay != null) return;


                var frame = new CGRect(View.Frame.X, View.Frame.Y, View.Frame.Width, View.Frame.Height);
                _overlay = new LoadingOverlay(frame, null);
                View.Add(_overlay);
            });
        }

        private void RemoveOverlay()
        {
            if (_overlay == null) return;

            InvokeOnMainThread(_overlay.Hide);
            _overlay = null;
        }
        #endregion
	}
}
