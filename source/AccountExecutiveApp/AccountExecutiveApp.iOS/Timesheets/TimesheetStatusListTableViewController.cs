﻿using Foundation;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Threading.Tasks;
using AccountExecutiveApp.Core.ViewModel;
using CoreGraphics;
using HealthKit;
using Microsoft.Practices.Unity;
using SiSystems.SharedModels;
using UIKit;

namespace AccountExecutiveApp.iOS
{
	public partial class TimesheetStatusListTableViewController : UITableViewController
	{
		private readonly TimesheetStatusListViewModel _viewModel;

		public TimesheetStatusListTableViewController () : base ("TimesheetStatusListTableViewController", null)
		{
		}

		public TimesheetStatusListTableViewController (IntPtr handle) : base (handle)
		{
			_viewModel = DependencyResolver.Current.Resolve<TimesheetStatusListViewModel>();
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			// Perform any additional setup after loading the view, typically from a nib.
		}

		public override void ViewDidAppear (bool animated)
		{
			OpenTimesheetsCell.DetailTextLabel.Text = _viewModel.FormattedNumberOfOpenTimesheets;
			SubmittedTimesheetsCell.DetailTextLabel.Text = _viewModel.FormattedNumberOfSubmittedTimesheets;
			RejectedTimesheetsCell.DetailTextLabel.Text = _viewModel.FormattedNumberOfRejectedTimesheets;
			CancelledTimesheetsCell.DetailTextLabel.Text = _viewModel.FormattedNumberOfCancelledTimesheets;
		}
	}
}

