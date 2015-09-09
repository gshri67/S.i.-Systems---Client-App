using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using ConsultantApp.Core.ViewModels;
using Microsoft.Practices.Unity;
using System.Collections;
using System.Collections.Generic;
using SiSystems.SharedModels;

namespace ConsultantApp.iOS
{
	partial class RemittancesViewController : UIViewController
	{
		private RemittanceViewModel _remittanceModel;
		private IEnumerable<Remittance> _remittances;
		private SubtitleHeaderView subtitleHeaderView;

		public RemittancesViewController (IntPtr handle) : base (handle)
		{
			_remittanceModel = DependencyResolver.Current.Resolve<RemittanceViewModel>();

			EdgesForExtendedLayout = UIRectEdge.None;
		}

		//public async void LoadRemittances()
        public void LoadRemittances()
		{
			//IndicateLoading();
            //if (_remittances == null)
            //    _remittances = await _remittanceModel.GetRemittances();
		    _remittances = tempRemittances;

			UpdateTableSource();
		}

	    private void UpdateTableSource()
	    {
	        InvokeOnMainThread(() =>
	        {
                tableview.Source = new RemittancesTableViewSource(this, _remittances);
                tableview.ReloadData();
	        });
	    }

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			

			subtitleHeaderView = new SubtitleHeaderView ();
			NavigationItem.TitleView = subtitleHeaderView;

			subtitleHeaderView.TitleText = "Remittances";
			subtitleHeaderView.SubtitleText = "4449993 Alberta Co";
			LoadRemittances ();

			NavigationItem.Title = "";
		}

        private static IEnumerable<Remittance> tempRemittances
        {
            get
            {
                return new List<Remittance>
                {
                    new Remittance
                    {
                        StartDate = Convert.ToDateTime("2015-06-01"),
                        EndDate =  Convert.ToDateTime("2015-06-15"),
                        DepositDate = Convert.ToDateTime("2015-06-17"),
                        Amount = (float) 2653.50,
                        DocumentNumber = "6C94239"
                    }
                    ,new Remittance
                    {
                        StartDate = Convert.ToDateTime("2015-06-16"),
                        EndDate =  Convert.ToDateTime("2015-06-30"),
                        DepositDate = Convert.ToDateTime("2015-07-03"),
                        Amount = (float) 2653.50,
                        DocumentNumber = "6D23490"
                    }
                };
            }
        }
	}
}
