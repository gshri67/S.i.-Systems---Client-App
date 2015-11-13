using Foundation;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using AccountExecutiveApp.Core.ViewModel;
using Microsoft.Practices.Unity;
using SiSystems.SharedModels;
using UIKit;

namespace AccountExecutiveApp.iOS
{
	partial class ContractorJobStatusListViewController : UITableViewController
	{
	    private ContractorJobStatusListViewModel _viewModel;
		public ContractorJobStatusListViewController (IntPtr handle) : base (handle)
		{
            _viewModel = DependencyResolver.Current.Resolve<ContractorJobStatusListViewModel>();
		}

	    public void LoadConsultants(IEnumerable<IM_Consultant> consultants)
	    {
	        _viewModel.LoadConsultants(consultants);
	    }
	}
}
