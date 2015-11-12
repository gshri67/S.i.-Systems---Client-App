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
    public partial class JobsListViewController : UITableViewController
	{
		private readonly JobsListViewModel _jobsListViewModel;
		
	    public const string SubtitleCellIdentifier = "SubtitleWithRightDetailCell";

		public JobsListViewController (IntPtr handle) : base (handle)
		{
			_jobsListViewModel = DependencyResolver.Current.Resolve<JobsListViewModel>();

            TableView.RegisterClassForCellReuse(typeof(SubtitleWithRightDetailCell), SubtitleCellIdentifier);
		}

		private void SetupTableViewSource()
		{
			if (TableView == null)
				return;

            TableView.Source = new JobsListTableViewSource(this, _jobsListViewModel);

            TableView.ReloadData();
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			UpdateUserInterface ();
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);

			//TableView.ReloadData ();
		}

		public void SetJobs( IEnumerable<Job> jobs )
		{
		    var task = _jobsListViewModel.SetJobs(jobs);

            task.ContinueWith(_ => UpdateUserInterface());
		}

		public void UpdateUserInterface()
		{
            InvokeOnMainThread(SetupTableViewSource);
		}
	}
}
