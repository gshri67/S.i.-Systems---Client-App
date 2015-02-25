using System;
using ClientApp.Services.Interfaces;
using ClientApp.ViewModels;
using Microsoft.Practices.Unity;
using UIKit;

namespace ClientApp.iOS
{
	partial class ConfirmContractViewController : UITableViewController
	{
	    private IContractService _contractService;
        public NewContractViewModel ViewModel { set; private get; }
        public ConfirmContractViewController (IntPtr handle) : base (handle)
        {
            _contractService = DependencyResolver.Current.Resolve<IContractService>();
        }

	    public override void ViewDidLoad()
	    {
            base.ViewDidLoad();

            SetupNavigationHeader();

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

        private void SetupNavigationHeader()
        {
            var cancelButton = new UIBarButtonItem { Title = "Cancel" };
            var submitButton = new UIBarButtonItem { Title = "Submit", TintColor = StyleGuideConstants.RedUiColor };
            cancelButton.Clicked += (sender, args) => { NavigationController.PopViewController(true); };
            submitButton.Clicked += (sender, args) =>
            {
                //_contractService.Submit(object)
            };
            NavigationItem.SetLeftBarButtonItem(cancelButton, false);
            NavigationItem.SetRightBarButtonItem(submitButton, false);
        }

        private static string ToRateString(decimal rate)
        {
            return string.Format("$ {0:N2} / hr", rate);
        }
	}
}
