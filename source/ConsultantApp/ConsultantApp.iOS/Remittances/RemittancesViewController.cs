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

		public RemittancesViewController (IntPtr handle) : base (handle)
		{
			_remittanceModel = DependencyResolver.Current.Resolve<RemittanceViewModel>();

			EdgesForExtendedLayout = UIRectEdge.None;
		}

		public async void LoadRemittances()
		{
			//IndicateLoading();
			if (_remittances == null)
				_remittances = await _remittanceModel.GetRemittances();

			//UpdateTableSource();
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			LoadRemittances ();

			tableview.Source = new RemittancesTableViewSource (this, _remittances);
			tableview.ReloadData();
		}
	}
}
