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

namespace ConsultantApp.iOS
{
	partial class RemittancesViewController : UIViewController
	{
		private readonly RemittanceViewModel _remittanceModel;
		private IEnumerable<Remittance> _remittances;
        private LoadingOverlay _overlay;
		private SubtitleHeaderView _subtitleHeaderView;

		public RemittancesViewController (IntPtr handle) : base (handle)
		{
			_remittanceModel = DependencyResolver.Current.Resolve<RemittanceViewModel>();

			EdgesForExtendedLayout = UIRectEdge.None;
		}

		public async void LoadRemittances()
		{
			IndicateLoading();
            if (_remittances == null)
                _remittances = await _remittanceModel.GetRemittances();

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
			

			_subtitleHeaderView = new SubtitleHeaderView ();
			NavigationItem.TitleView = _subtitleHeaderView;

			_subtitleHeaderView.TitleText = "Remittances";
			_subtitleHeaderView.SubtitleText = "4449993 Alberta Co";
			LoadRemittances ();

			NavigationItem.Title = "";

			LogoutManager.CreateNavBarRightButton(this);
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
