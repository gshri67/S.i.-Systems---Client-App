using Foundation;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Threading.Tasks;
using AccountExecutiveApp.Core.ViewModel;
using AccountExecutiveApp.Core;
using CoreGraphics;
using HealthKit;
using Microsoft.Practices.Unity;
using SiSystems.SharedModels;
using UIKit;

namespace AccountExecutiveApp.iOS
{
	public partial class TimesheetListTableViewController : UITableViewController
	{
		private readonly TimesheetListViewModel _viewModel;

		public TimesheetListTableViewController (IntPtr handle) : base (handle)
		{
			_viewModel = DependencyResolver.Current.Resolve<TimesheetListViewModel>();
		}

		private void InstantiateTableViewSource()
		{
			if (TableView == null) return;

			RegisterCellsForReuse();
			TableView.Source = new TimesheetListTableViewSource(this, _viewModel.Timesheets);
			TableView.ReloadData();
			//TableView.ContentInset = new UIEdgeInsets (-35, 0, -35, 0);
		}

		private void RegisterCellsForReuse()
		{
			if (TableView == null) return;
		
			TableView.RegisterClassForCellReuse(typeof(SubtitleWithRightDetailCell), "SubtitleWithRightDetailCell");
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
		}

		public override void ViewDidAppear (bool animated)
		{
			LoadTimesheetDetails();
		}

		public void UpdateUserInterface()
		{
			InvokeOnMainThread(InstantiateTableViewSource);
		}


		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();

			// Release any cached data, images, etc that aren't in use.
		}

		public void LoadTimesheetDetails()
		{
			var task = _viewModel.LoadTimesheetDetails();

			task.ContinueWith(_ => InvokeOnMainThread(UpdateUserInterface), TaskContinuationOptions.OnlyOnRanToCompletion);
		}
	}
}

