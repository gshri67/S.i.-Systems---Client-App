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
	partial class JobsListViewController : UITableViewController
	{
		private JobsListTableViewSource _listTableViewSource;
		private JobsViewModel _jobsViewModel;
		public IEnumerable<Job> _jobs;
	    public const string SubtitleCellIdentifier = "SubtitleWithRightDetailCell";

		public JobsListViewController (IntPtr handle) : base (handle)
		{
			//_jobsViewModel = DependencyResolver.Current.Resolve<JobsViewModel>();
		}

		private void SetupTableViewSource()
		{
			if (TableView == null || _jobs == null )
				return;

			RegisterCellsForReuse();
			InstantiateTableViewSource();

			TableView.Source = _listTableViewSource;
		}

		private void InstantiateTableViewSource()
		{
			_listTableViewSource = new JobsListTableViewSource ( this, _jobs );
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			UpdateUI ();
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);

			//TableView.ReloadData ();
		}

		public void setJobs( IEnumerable<Job> jobs )
		{
			_jobs = jobs;
            SortJobsByIssueDate();
			UpdateUI ();
		}

        private void SortJobsByIssueDate() 
        {
            var list = _jobs.ToList();
            list.Sort((d1, d2) => DateTime.Compare(d1.issueDate, d2.issueDate));
            list.Reverse();
            _jobs = list.AsEnumerable();

        }

		private void RegisterCellsForReuse()
		{
			if (TableView == null) return;
			TableView.RegisterClassForCellReuse(typeof(SubtitleWithRightDetailCell), "SubtitleWithRightDetailCell");
		}

		public void UpdateUI()
		{
			if (_jobs != null && TableView != null )
			{
				SetupTableViewSource ();
				TableView.ReloadData ();
			}
		}
	}
}
