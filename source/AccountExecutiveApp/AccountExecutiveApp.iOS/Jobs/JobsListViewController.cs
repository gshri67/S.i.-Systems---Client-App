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

			//SetupTableViewSource ();

			//TableView.ReloadData ();

			UpdateUI ();


			//TableView.ReloadData ();
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);

			//TableView.ReloadData ();
		}

		public void setJobs( IEnumerable<Job> jobs )
		{
			_jobs = jobs;
			UpdateUI ();
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
			if (_jobs != null && TableView != null )
			{
				SetupTableViewSource ();
				TableView.ReloadData ();
			}
		}
	}
}
