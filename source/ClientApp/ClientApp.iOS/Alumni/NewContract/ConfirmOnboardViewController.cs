using System;
using ClientApp.Core.ViewModels;
using CoreGraphics;
using Foundation;
using UIKit;

namespace ClientApp.iOS
{
	partial class ConfirmOnboardViewController : UITableViewController
	{
        public OnboardViewModel ViewModel { set; private get; }

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
            ServicesRateLabel.Text = ToRateString(ViewModel.ServiceFee);
	        MspRateLabel.Text = string.Format("{0}%", ViewModel.MspPercent);
            TotalRateLabel.Text = ToRateString(ViewModel.TotalRate);
            StartDateLabel.Text = ViewModel.StartDate.ToString("MMM dd, yyyy");
            EndDateLabel.Text = ViewModel.EndDate.ToString("MMM dd, yyyy");
            TimesheetApproverEmailLabel.Text = ViewModel.TimesheetApprovalEmail;
	        ContractApproverEmailLabel.Text = ViewModel.ContractApprovalEmail;

            //hide empty cells
            MspCell.Hidden = ViewModel.MspPercent == 0 || ViewModel.ConsultantPaysMspPercent;
	        ServicesCell.Hidden = ViewModel.ServiceFee == 0 || ViewModel.ConsultantPaysServiceFee;
	        TotalCell.Hidden = ViewModel.MspPercent == 0 && ViewModel.ServiceFee == 0;

	        if (ViewModel.IsActiveConsultant)
	        {
	            Title = "Confirm Renew";
	            SubmitLabel.Text = "Submit Renewal";
	        }

	        if (ViewModel.IsFullySourced)
	        {
	            ContractorRateTypeLabel.Text = "Bill Rate";
	            MspCell.Hidden = true;
                ServicesCell.Hidden = true;
                TotalCell.Hidden = true;
	        }
	    }

	    public override async void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
	        if (indexPath.Section == 4 && indexPath.Row == 0)
            {
                _submissionDelegate = new ContractSubmissionDelegate(this);
                try
                {
                    _overlay = new LoadingOverlay(UIScreen.MainScreen.ApplicationFrame, null);
                    ParentViewController.View.Add(_overlay);
                    await ViewModel.SubmitContract();
                    _overlay.Hide();
                    var view = new UIAlertView("Success", "Your contract proposal has been sent.", _submissionDelegate, "Ok");
                    view.Show();
                }
                catch (Exception)
                {
                    _overlay.Hide();
                    var view = new UIAlertView("Error", "There was an error sending the contract proposal to the server. Please try again.", _submissionDelegate, "Ok");
                    view.Show();
                }
	        }
        }

        private static string ToRateString(decimal rate)
        {
            return string.Format("${0:N2}/hr", rate);
        }

	    public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
	    {
	        if (indexPath.Section == 1)
	        {
                //MSP
	            if (indexPath.Row == 1)
	            {
	                return MspCell.Hidden ? 0 : 30f;
	            }
                //Service
                if (indexPath.Row == 2)
                {
                    return ServicesCell.Hidden ? 0 : 30f;
                }
                //Total
                if (indexPath.Row == 3)
                {
                    return MspCell.Hidden && ServicesCell.Hidden ? 0 : 30f;
                }
	        }
            return indexPath.Section == 4 ? 44f : 30f;
	    }

	    public override string TitleForHeader(UITableView tableView, nint section)
	    {
	        if (section == 0 && ViewModel.IsActiveConsultant)
	        {
	            return "Renew Details";
	        }
            return base.TitleForHeader(tableView, section);
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
