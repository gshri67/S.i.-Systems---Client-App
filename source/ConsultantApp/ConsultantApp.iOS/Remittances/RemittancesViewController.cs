using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using ConsultantApp.Core.ViewModels;
using Microsoft.Practices.Unity;
using System.Collections;
using System.Collections.Generic;
using CoreGraphics;
using SiSystems.SharedModels;
using System.Linq;
using Shared.Core;

namespace ConsultantApp.iOS
{
	partial class RemittancesViewController : UIViewController
	{
		private readonly RemittanceViewModel _remittanceModel;
		private IEnumerable<Remittance> _remittances;
        private LoadingOverlay _overlay;
		private SubtitleHeaderView subtitleHeaderView;
		private const string ScreenTitle = "eRemittances";

		public RemittancesViewController (IntPtr handle) : base (handle)
		{
			_remittanceModel = DependencyResolver.Current.Resolve<RemittanceViewModel>();
			EdgesForExtendedLayout = UIRectEdge.None;
		}

		public async void LoadRemittances()
		{
			IndicateLoading();
            if (_remittances == null)
                //_remittances = await _remittanceModel.GetRemittances();
                _remittances = new List<Remittance>
                {
                    new Remittance
                    {
                        StartDate = Convert.ToDateTime("2015-07-01"),
                        EndDate =  Convert.ToDateTime("2015-07-15"),
                        DepositDate = Convert.ToDateTime("2015-07-17"),
                        Amount = (float) 2653.50,
                        DocumentNumber = "6C94239"
                    }
                    ,new Remittance
                    {
                        StartDate = Convert.ToDateTime("2015-07-16"),
                        EndDate =  Convert.ToDateTime("2015-07-31"),
                        DepositDate = Convert.ToDateTime("2015-08-03"),
                        Amount = (float) 2340.00,
                        DocumentNumber = "6D23490"
                    }
                };

			UpdateTableSource();

			if (_remittances == null || _remittances.Count () <= 0)
				noRemittancesLabel.Hidden = false;
			else
				noRemittancesLabel.Hidden = true;
		}

	    private void UpdateTableSource()
	    {
            RemoveOverlay();
	        InvokeOnMainThread(() =>
	        {
                tableview.Source = new RemittancesTableViewSource(this, _remittances);
                tableview.ReloadData();
	        });
	    }

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			LoadRemittances ();

			tableview.ContentInset = new UIEdgeInsets (-35, 0, 0, 0);

			NavigationItem.Title = "";

			LogoutManager.CreateNavBarRightButton(this);

			CreateCustomTitleBar();
		}

		private void CreateCustomTitleBar()
		{
			InvokeOnMainThread(() =>
				{
					subtitleHeaderView = new SubtitleHeaderView();
					NavigationItem.TitleView = subtitleHeaderView;
					subtitleHeaderView.TitleText = ScreenTitle;
					subtitleHeaderView.SubtitleText = CurrentConsultantDetails.CorporationName ?? string.Empty;
					NavigationItem.Title = "";
				});
		}



        #region Overlay

        private void IndicateLoading()
        {
            InvokeOnMainThread(delegate
            {
                if (_overlay != null) return;


                var frame = new CGRect(RemitanceView.Frame.X, RemitanceView.Frame.Y, RemitanceView.Frame.Width, RemitanceView.Frame.Height);
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
