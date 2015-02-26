using System;
using ClientApp.ViewModels;
using Foundation;
using UIKit;

namespace ClientApp.iOS
{
	partial class ConfirmContractViewController : UITableViewController
	{
        public NewContractViewModel ViewModel { set; private get; }
        public ConfirmContractViewController (IntPtr handle) : base (handle)
        {
        }

	    public override void ViewDidLoad()
	    {
            base.ViewDidLoad();

            var backButton = new UIBarButtonItem { Title = "Back", };
            backButton.Clicked += (sender, args) => { NavigationController.PopViewController(true); };
            NavigationItem.SetLeftBarButtonItem(backButton, false);

            NameLabel.Text = ViewModel.Consultant.FullName;
            SpecializationLabel.Text = ViewModel.Specialization.Name;
            TitleLabel.Text = ViewModel.ContractTitle;
            ContractorRateLabel.Text = ToRateString(ViewModel.ContractorRate);
            ServicesRateLabel.Text = ToRateString(NewContractViewModel.ServiceRate);
            TotalRateLabel.Text = ToRateString(ViewModel.TotalRate);
            StartDateLabel.Text = ViewModel.StartDate.ToString("d");
            EndDateLabel.Text = ViewModel.EndDate.ToString("d");
            ApproverEmailLabel.Text = ViewModel.ApproverEmail;
	    }

	    public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
	    {
	        if (indexPath.Section == 5 && indexPath.Row == 0)
	        {
	            ViewModel.SubmitContract();
	        }
	    }

	    private static string ToRateString(decimal rate)
        {
            return string.Format("${0,6:N2}/hr", rate);
        }
	}
}
