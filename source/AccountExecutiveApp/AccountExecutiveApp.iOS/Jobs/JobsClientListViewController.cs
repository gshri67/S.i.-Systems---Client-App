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
		private readonly JobsViewModel _jobsViewModel;
        private const string ClientSelectedFromJobListSegueIdentifier = "ClientSelectedSegue";

        public const string CellReuseIdentifier = "JobsClientListCell";

		public JobsClientListViewController (IntPtr handle) : base (handle)
		{
            _jobsViewModel = DependencyResolver.Current.Resolve<JobsViewModel>();

            TableView.RegisterClassForCellReuse(typeof(RightDetailCell), CellReuseIdentifier);
		}

		private void InstantiateTableViewSource()
		{
            if (TableView == null) return;
            
            TableView.Source = new JobsClientListTableViewSource(this, _jobsViewModel.Jobs);
            TableView.ReloadData();
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

            LoadJobs();
		}


        public void UpdateUserInterface()
        {
            InvokeOnMainThread(InstantiateTableViewSource);
        }

        public void LoadJobs()
        {
            var task = _jobsViewModel.LoadJobs();
            task.ContinueWith(_ => UpdateUserInterface());
        }

	    public override bool ShouldPerformSegue(string segueIdentifier, NSObject sender)
	    {
            if (segueIdentifier == ClientSelectedFromJobListSegueIdentifier)
            {
                return false;
            }

            return base.ShouldPerformSegue(segueIdentifier, sender);
	    }
	}
}
