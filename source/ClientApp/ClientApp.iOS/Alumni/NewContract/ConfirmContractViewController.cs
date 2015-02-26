using System;
using ClientApp.ViewModels;
using Foundation;
using UIKit;

namespace ClientApp.iOS
{
	partial class ConfirmContractViewController : UITableViewController
	{
        public NewContractViewModel ViewModel { set; private get; }

	    private ContractSubmissionDelegate submissionDelegate;

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

	    public override async void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
	        if (indexPath.Section == 4 && indexPath.Row == 0)
            {
                submissionDelegate = new ContractSubmissionDelegate(this);
                try
                {
                    await ViewModel.SubmitContract();
                    var view = new UIAlertView("Success", "Your contract proposal has been sent.", submissionDelegate, "Ok");
                    view.Show();
                }
                catch (Exception ex)
                {
                    var view = new UIAlertView("Error", "Something went wrong.", submissionDelegate, "Ok");
                    view.Show();
                }
	        }
        }

        private static string ToRateString(decimal rate)
        {
            return string.Format("${0,6:N2}/hr", rate);
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
	            else
	            {
	                _controller.NavigationController.PopViewController(true);
	            }
	        }
	    }
	}
}
