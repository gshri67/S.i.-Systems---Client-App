using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using AccountExecutiveApp.Core.ViewModel;
using Microsoft.Practices.Unity;
using SiSystems.SharedModels;
using System.Collections.Generic;
using System.Linq;
using CoreGraphics;

namespace AccountExecutiveApp.iOS
{
	public partial class SearchTableViewController : UIViewController
	{
		private readonly SearchViewModel _viewModel;
		private LoadingOverlay _overlay;
	    private SearchTableViewSource _tableSource;

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
			//TableView.ContentInset = new UIEdgeInsets (-35, 0, -35, 0);

		    SearchBar.TextChanged += delegate
		    {
		        _tableSource.ApplyFilterWithText(SearchBar.Text);
		        TableView.ReloadData();
		    };
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			LoadSearchData();
			IndicateLoading();

			CancelSearchButton.TouchUpInside += delegate {
				DismissViewController( true, null );
			};

            SearchBar.SearchBarStyle = UISearchBarStyle.Minimal;
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
