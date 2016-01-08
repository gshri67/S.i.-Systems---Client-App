using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using AccountExecutiveApp.Core.ViewModel;
using Microsoft.Practices.Unity;
using SiSystems.SharedModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CoreGraphics;
using System.Timers;




namespace AccountExecutiveApp.iOS
{
	public partial class SearchTableViewController : UIViewController
	{
		private readonly SearchViewModel _viewModel;
		private LoadingOverlay _overlay;
	    private SearchTableViewSource _tableSource;
        private const int SearchTimerInterval = 1000;

		public const string CellReuseIdentifier = RightDetailCell.CellIdentifier;

		public SearchTableViewController (IntPtr handle) : base (handle)
		{
			_viewModel = DependencyResolver.Current.Resolve<SearchViewModel>();
		}

		private void InstantiateTableViewSource()
		{
			if (TableView == null) return;

            TableView.RegisterClassForCellReuse(typeof(RightDetailCell), CellReuseIdentifier);
			_tableSource = new SearchTableViewSource(this, _viewModel.ClientContacts, _viewModel.Contractors);
		    TableView.Source = _tableSource;
			TableView.ReloadData();
			TableView.ContentInset = new UIEdgeInsets (-35, 0, -35, 0);

            var timer = CreateTimer();

		    SearchBar.TextChanged += delegate
		    {
		        timer.Stop();
                timer.Start();
            };
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			NavigationItem.Title = "";

			LoadSearchData();
			IndicateLoading();

			CancelSearchButton.TouchUpInside += delegate {
		
				NavigationController.SetNavigationBarHidden(false, false);
				NavigationController.PopViewController(true);
			};
				
			CancelSearchButton.SetTitleColor( StyleGuideConstants.RedUiColor, UIControlState.Normal );

            SearchBar.SearchBarStyle = UISearchBarStyle.Minimal;

			AutomaticallyAdjustsScrollViewInsets = false;

			SearchHeaderView.Layer.ShadowOffset = new CGSize ( 0, 1.0f);
			SearchHeaderView.Layer.ShadowOpacity = 0.3f;
			SearchHeaderView.Layer.ShadowRadius = 0.0f;
			SearchHeaderView.Layer.ShadowColor = UIColor.Gray.CGColor;
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			NavigationController.SetNavigationBarHidden(true, true);
		}

		public void UpdateUserInterface()
		{
			InvokeOnMainThread(InstantiateTableViewSource);
			InvokeOnMainThread(RemoveOverlay);
		}

		public void LoadSearchData()
		{
			var task = _viewModel.LoadSearchData();
			task.ContinueWith(_ => UpdateUserInterface());
		}

        public void LoadSearchDataWithFilter( string filter )
        {
            var task = _viewModel.LoadSearchDataWithFilter( filter );
            task.ContinueWith(_ => InvokeOnMainThread(OnFilteredSearchResultsReturned));
        }

	    public void OnFilteredSearchResultsReturned()
	    {
            _tableSource.ReloadWithFilteredContacts( _viewModel.FilteredClientContacts, _viewModel.FilteredContractors );
            TableView.ReloadData();
	    }

        private System.Timers.Timer CreateTimer()
        {
            var timer = new System.Timers.Timer()
            {
                Interval = SearchTimerInterval,
                AutoReset = false,
                Enabled = false //we don't want to start the timer until we change search text
            };

            timer.Elapsed += delegate
            {
                InvokeOnMainThread(delegate
                {
                    LoadSearchDataWithFilter(SearchBar.Text);
                });
            };
     
            return timer;
        }

	    public override bool ShouldPerformSegue(string segueIdentifier, NSObject sender)
		{
            /*
			if (segueIdentifier == ClientSelectedFromJobListSegueIdentifier)
			{
				return false;
			}*/

			return base.ShouldPerformSegue(segueIdentifier, sender);
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
