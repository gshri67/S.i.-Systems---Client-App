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
	public partial class TimesheetContactsTableViewController : UITableViewController
	{
		private readonly TimesheetContactsViewModel _viewModel;
	    private SubtitleHeaderView _subtitleHeaderView;
        private bool _needsCreateTitleBar = false;

	    public TimesheetContactsTableViewController(IntPtr handle)
            : base(handle)
		{
		    _viewModel = DependencyResolver.Current.Resolve<TimesheetContactsViewModel>();
		}
        
		private void InstantiateTableViewSource()
		{
			if (TableView == null) return;

			RegisterCellsForReuse();
			TableView.Source = new TimesheetContactsTableViewSource(this, _viewModel.Contact );
			TableView.ReloadData();
			TableView.ContentInset = new UIEdgeInsets (-35, 0, -35, 0);
		}

		private void RegisterCellsForReuse()
		{
			if (TableView == null) return;
		
			TableView.RegisterClassForCellReuse(typeof(SubtitleWithRightDetailCell), "SubtitleWithRightDetailCell");
            TableView.RegisterClassForCellReuse(typeof(ContractorContactInfoCell), ContractorContactInfoCell.CellIdentifier);
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
		}

		public override void ViewDidAppear (bool animated)
		{
            CreateTitleBarIfNeeded();
		}

		public void UpdateUserInterface()
		{
            InvokeOnMainThread( CreateTitleBarIfNeeded );
			InvokeOnMainThread(InstantiateTableViewSource);
		}


		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
		}
        
		public void LoadTimesheetContact( int Id )
		{
			var task = _viewModel.LoadTimesheetContact( Id );

			task.ContinueWith(_ => InvokeOnMainThread(UpdateUserInterface), TaskContinuationOptions.OnlyOnRanToCompletion);
		}

        private void CreateTitleBarIfNeeded()
        {
            if (!_needsCreateTitleBar)
                _needsCreateTitleBar = true;
            else
                InvokeOnMainThread(CreateCustomTitleBar);
        }

        private void CreateCustomTitleBar()
        {
            InvokeOnMainThread(() =>
            {
                _subtitleHeaderView = new SubtitleHeaderView();
                NavigationItem.TitleView = _subtitleHeaderView;
                _subtitleHeaderView.TitleText = _viewModel.PageTitle;
                _subtitleHeaderView.SubtitleText = _viewModel.PageSubtitle;
                NavigationItem.Title = "";
            });
        }
	}
}

