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
        private SubtitleHeaderView _subtitleHeaderView;
	    public const string SubtitleCellIdentifier = "SubtitleWithRightDetailCell";
        public string Subtitle;

		public JobsListViewController (IntPtr handle) : base (handle)
		{
			_jobsListViewModel = DependencyResolver.Current.Resolve<JobsListViewModel>();
		    Title = "Jobs";
		}

		private void SetupTableViewSource()
		{
			if (TableView == null)
				return;

            TableView.RegisterClassForCellReuse(typeof(SubtitleWithRightDetailCell), SubtitleCellIdentifier);
			TableView.ContentInset = new UIEdgeInsets (-35, 0, -35, 0);
            TableView.Source = new JobsListTableViewSource(this, _jobsListViewModel);

            TableView.ReloadData();
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

            CreateCustomTitleBar();

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

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            if (segue.Identifier == "JobSelectedSegue")
            {
                var destinationController = segue.DestinationViewController as JobDetailViewController;

                var source = TableView.Source as JobsListTableViewSource;

                var job = source.JobSelected(TableView.IndexPathForSelectedRow);

                destinationController.LoadJob(job);

            }

            base.PrepareForSegue(segue, sender);
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
