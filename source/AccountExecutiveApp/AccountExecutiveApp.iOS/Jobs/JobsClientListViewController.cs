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
	partial class JobsClientListViewController : UITableViewController
	{
		private JobsViewModel _jobsViewModel;

		public JobsClientListViewController (IntPtr handle) : base (handle)
		{
            _jobsViewModel = DependencyResolver.Current.Resolve<JobsViewModel>();
		}

		private void SetupTableViewSource()
		{
            if (TableView == null || _jobsViewModel.Jobs == null)
				return;

			RegisterCellsForReuse();
			InstantiateTableViewSource();
		}

		private void InstantiateTableViewSource()
		{
            TableView.Source = new JobsClientListTableViewSource(this, _jobsViewModel.JobsByClient());
			//_addTimeTableViewSource.OnDataChanged += AddTimeTableDataChanged;
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

            LoadJobs();
		}

		private void RegisterCellsForReuse()
		{
			if (TableView == null) return;

			TableView.RegisterClassForCellReuse(typeof (RightDetailCell), "RightDetailCell");
			TableView.RegisterClassForCellReuse(typeof (UITableViewCell), "cell");
		}

        public void UpdateUI()
        {
            InvokeOnMainThread( delegate{
			    if ( _jobsViewModel.Jobs != null && TableView != null )
                {
                    if (TableView.Source == null) 
    				    SetupTableViewSource ();
				
                    TableView.ReloadData ();
			    }
            });
        }

        public async void LoadJobs()
        {
            if ( _jobsViewModel.Jobs != null) return;

            _jobsViewModel.LoadJobs(UpdateUI);
        }


	}
}
