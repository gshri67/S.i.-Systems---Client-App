using Foundation;
using System;
using System.CodeDom.Compiler;
using System.Threading.Tasks;
using AccountExecutiveApp.Core.ViewModel;
using Microsoft.Practices.Unity;
using SiSystems.SharedModels;
using UIKit;

namespace AccountExecutiveApp.iOS
{
	partial class JobDetailViewController : UITableViewController
	{
	    private JobDetailViewModel _viewModel;

		public JobDetailViewController (IntPtr handle) : base (handle)
		{
            _viewModel = DependencyResolver.Current.Resolve<JobDetailViewModel>();
		}

	    public void LoadJob(Job job)
	    {
	        var task = _viewModel.LoadJob(job);

            task.ContinueWith(_ => UpdateUserInterface(), TaskContinuationOptions.OnlyOnRanToCompletion);
	    }

	    private void UpdateUserInterface()
	    {
	        InvokeOnMainThread(InstantiateTableViewSource);
	    }

        private void InstantiateTableViewSource()
        {
            ClientContactName.Text = _viewModel.ClientContactName;
            DirectReportName.Text = _viewModel.DirectReportName;
            ShortListed.TextLabel.Text = "Shortlisted";
            ShortListed.DetailTextLabel.Text = _viewModel.NumberOfShortlistedConsultants.ToString();
            Proposed.TextLabel.Text = "Proposed";
            Proposed.DetailTextLabel.Text = _viewModel.NumberOfProposedContractors.ToString();
            Callouts.TextLabel.Text = "Callouts";
            Callouts.DetailTextLabel.Text = _viewModel.NumberOfContractorsWithCallouts.ToString();
            
            TableView.ReloadData();
        }

	    public override string TitleForHeader(UITableView tableView, nint section)
	    {
	        if (section == 0)
	        {
	            return _viewModel.JobTitle;
	        }
	        return base.TitleForHeader(tableView, section);
	    }

	    public override void ViewDidLoad()
	    {
	        base.ViewDidLoad();
	    }
	}
}
