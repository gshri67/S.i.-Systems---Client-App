using System;
using ClientApp.ViewModels;
using Foundation;
using UIKit;

namespace ClientApp.iOS
{
	partial class ConfirmOnboardViewController : UITableViewController
	{
        public NewContractViewModel ViewModel { set; private get; }

	    private ContractSubmissionDelegate _submissionDelegate;
	    private LoadingOverlay _overlay;

        public ConfirmOnboardViewController (IntPtr handle) : base (handle)
        {
        }

	    public override void ViewDidLoad()
	    {
            base.ViewDidLoad();

            var backButton = new UIBarButtonItem { Title = "Back", };
            backButton.Clicked += (sender, args) => { NavigationController.PopViewController(true); };
            NavigationItem.SetLeftBarButtonItem(backButton, false);

            NameLabel.Text = ViewModel.Consultant.FullName;
            TitleLabel.Text = ViewModel.ContractTitle;
            ContractorRateLabel.Text = ToRateString(ViewModel.ContractorRate);
            ServicesRateLabel.Text = ToRateString(ViewModel.ServiceRate);
            TotalRateLabel.Text = ToRateString(ViewModel.TotalRate);
            StartDateLabel.Text = ViewModel.StartDate.ToString("MMM dd, yyyy");
            EndDateLabel.Text = ViewModel.EndDate.ToString("MMM dd, yyyy");
            TimesheetApproverEmailLabel.Text = ViewModel.TimesheetApprovalEmail;
	        ContractApproverEmailLabel.Text = ViewModel.ContractApprovalEmail;
	    }

	    public override async void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
	        if (indexPath.Section == 4 && indexPath.Row == 0)
            {
                _submissionDelegate = new ContractSubmissionDelegate(this);
                try
                {
                    _overlay = new LoadingOverlay(UIScreen.MainScreen.ApplicationFrame);
                    ParentViewController.View.Add(_overlay);
                    await ViewModel.SubmitContract();
                    _overlay.Hide();
                    var view = new UIAlertView("Success", "Your contract proposal has been sent.", _submissionDelegate, "Ok");
                    view.Show();
                }
                catch (Exception ex)
                {
                    _overlay.Hide();
                    var view = new UIAlertView("Error", "There was an error sending the contract proposal to the server. Please try again.", _submissionDelegate, "Ok");
                    view.Show();
                }
	        }
        }

        private static string ToRateString(decimal rate)
        {
            return string.Format("${0,6:N2}/hr", rate);
        }

	    public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
	    {
	        return indexPath.Section == 4 ? 44f : 30f;
	    }

	    class ContractSubmissionDelegate : UIAlertViewDelegate
	    {
	        private readonly UIViewController _controller;

            public ContractSubmissionDelegate(UIViewController controller)
	        {
	            this._controller = controller;
	        }

	        public override void Clicked(UIAlertView alertview, nint buttonIndex)
	        {
                if (alertview.Title == "Success")
	            {
	                _controller.DismissViewController(true, null);
	            }
	        }
	    }
	}
}
