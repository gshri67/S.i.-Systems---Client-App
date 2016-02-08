using Foundation;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountExecutiveApp.Core.ViewModel;
using AccountExecutiveApp.iOS.Jobs.JobDetails.ContractorJobStatusList;
using Microsoft.Practices.Unity;
using SiSystems.SharedModels;
using UIKit;

namespace AccountExecutiveApp.iOS
{
    public partial class ClientContactDetailsTableViewController : UITableViewController
    {
        private readonly ClientContactDetailsViewModel _viewModel;
        public const string CellIdentifier = "ContractorContactInfoCell";
        private int _id;

        public ClientContactDetailsTableViewController(IntPtr handle)
            : base(handle)
        {
            _viewModel = DependencyResolver.Current.Resolve<ClientContactDetailsViewModel>();
        }

        public void setContactId(int Id)
        {
            _id = Id;
            LoadContact();
        }

        private void InstantiateTableViewSource()
        {
            if (TableView == null)
                return;

            TableView.RegisterClassForCellReuse(typeof(ContractorContactInfoCell), CellIdentifier);
            TableView.RegisterClassForCellReuse(typeof(SubtitleWithRightDetailCell), SubtitleWithRightDetailCell.CellIdentifier);
            TableView.RegisterClassForCellReuse(typeof(RightDetailCell), RightDetailCell.CellIdentifier);
            TableView.RegisterClassForCellReuse(typeof(UITableViewCell), "UITableViewCell");

            TableView.Source = new ClientContactDetailsTableViewSource(this, _viewModel.Contact);
            TableView.ContentInset = new UIEdgeInsets(-35, 0, -35, 0);
            TableView.ReloadData();
        }

        private void UpdateUserInterface()
        {
            InvokeOnMainThread(InstantiateTableViewSource);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            UpdatePageTitle();
        }

        private void UpdatePageTitle()
        {
            Title = _viewModel.PageTitle;
        }

        public async void LoadContact()
        {
            var task = _viewModel.LoadContact(_id);
            task.ContinueWith(_ => InvokeOnMainThread(UpdateUserInterface), TaskContinuationOptions.OnlyOnRanToCompletion);
        }
    }
}
