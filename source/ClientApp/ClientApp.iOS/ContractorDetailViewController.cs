using System;
using System.Collections.Generic;
using ClientApp.ViewModels;
using Microsoft.Practices.Unity;
using SiSystems.ClientApp.SharedModels;
using UIKit;

namespace ClientApp.iOS
{
	partial class ContractorDetailViewController : UITableViewController
	{
		private readonly ContractorDetailViewModel _detailViewModel;

        public ContractorDetailViewController (IntPtr handle) : base (handle)
        {
            _detailViewModel = DependencyResolver.Current.Resolve<ContractorDetailViewModel>();
        }

	    public async void LoadConsultant(int id)
	    {
	        var consultant = await _detailViewModel.GetConsultant(id);
	        InvokeOnMainThread(delegate
	                           {
                                   Title = consultant.FullName;
	                               TitleLabel.Text = "Developer, Lead Developer, Project Manager";
	                               //consultant.Contracts.OrderByDescending(c => c.EndDate).First().Title;

	                           });
            
	    }

	    private void AddSpecializationAndSkills(IEnumerable<Specialization> specs, UIView view)
	    {
	        foreach (var spec in specs)
	        {
	            foreach (var skill in spec.Skills)
	            {
	                
	            }
	        }
	    }
	}
}
