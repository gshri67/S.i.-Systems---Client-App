using System;
using System.Threading.Tasks;
using AccountExecutiveApp.Core.ViewModel;
using CoreGraphics;
using Foundation;
using UIKit;
using Microsoft.Practices.Unity;
using SiSystems.SharedModels;

namespace AccountExecutiveApp.iOS
{
    [Register("ContractCreationSendingTableViewController")]
    public partial class ContractCreationSendingTableViewController : UITableViewController
    {
        public ContractCreationViewModel ViewModel;
        private ContractSendingSupportViewModel _supportModel;
        private ContractCreationSendingTableViewSource _tableSource;
        private SubtitleHeaderView _subtitleHeaderView;
        private string Subtitle;

        public ContractCreationSendingTableViewController(IntPtr handle)
            : base(handle)
        {
            _supportModel = DependencyResolver.Current.Resolve<ContractSendingSupportViewModel>();
            //ViewModel = DependencyResolver.Current.Resolve<ContractCreationViewModel>();
        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        private void InstantiateTableViewSource()
        {
            if (TableView == null)
                return;

            TableView.RegisterClassForCellReuse(typeof(SubtitleWithRightDetailCell),
                SubtitleWithRightDetailCell.CellIdentifier);

            TableView.RegisterClassForCellReuse(typeof(RightDetailCell), RightDetailCell.CellIdentifier);
            TableView.RegisterClassForCellReuse(typeof(EditableTextFieldCell), EditableTextFieldCell.CellIdentifier);
            TableView.RegisterClassForCellReuse(typeof(EditablePickerCell), EditablePickerCell.CellIdentifier);
            TableView.RegisterClassForCellReuse(typeof(EditableDatePickerCell), EditableDatePickerCell.CellIdentifier);
            TableView.RegisterClassForCellReuse(typeof(EditableNumberFieldCell), EditableNumberFieldCell.CellIdentifier);
            TableView.RegisterClassForCellReuse(typeof(EditableBooleanCell), EditableBooleanCell.CellIdentifier);
            TableView.RegisterClassForCellReuse(typeof(EditableDoublePickerCell), EditableDoublePickerCell.CellIdentifier);
            TableView.RegisterClassForCellReuse(typeof(EditableFullTextFieldCell), EditableFullTextFieldCell.CellIdentifier);
            TableView.RegisterClassForCellReuse(typeof(UITableViewCell), "UITableViewCell");
            TableView.RegisterClassForCellReuse(typeof(MultiSelectDescriptionCell), MultiSelectDescriptionCell.CellIdentifier);
            
            TableView.KeyboardDismissMode = UIScrollViewKeyboardDismissMode.OnDrag;
            _tableSource = new ContractCreationSendingTableViewSource(this, ViewModel, _supportModel);
            TableView.Source = _tableSource;
            //TableView.AllowsSelection = false;
            TableView.SeparatorColor = UIColor.Clear;
            TableView.RowHeight = UITableView.AutomaticDimension;
            TableView.EstimatedRowHeight = 160.0f;

            ContinueBar continueBar = new ContinueBar();
            continueBar.Frame = new CGRect(0, 0, 100, 50);
            continueBar.NextButton.TouchUpInside += delegate
            {
				var vc = (ContractCreationReviewTableViewController)Storyboard.InstantiateViewController("ContractCreationReviewTableViewController");
                vc.ViewModel = ViewModel;
                ShowViewController(vc, this);
            };
            continueBar.NextTextButton.TouchUpInside += delegate
            {
                var vc = (ContractCreationReviewTableViewController)Storyboard.InstantiateViewController("ContractCreationReviewTableViewController");
                vc.ViewModel = ViewModel;
                ShowViewController(vc, this);
            };

            TableView.TableFooterView = continueBar;
        }

        public void LoadData()
        {
            //var task = ViewModel.LoadContractCreationPayRatePage(0);
            //task.ContinueWith(_ => InvokeOnMainThread(UpdateUserInterface), TaskContinuationOptions.OnlyOnRanToCompletion);
           /*
            ViewModel.Contract = new ContractCreationDetails
            {
                JobTitle = "Developer",
                StartDate = new DateTime(2016, 1, 22),
                PaymentPlan = "Part Time",
                DaysCancellation = 10
            };*/

            var loadingOptions = _supportModel.GetContractSupportOptions(ViewModel.JobId, ViewModel.CandidateId);
            loadingOptions.ContinueWith(_ => InvokeOnMainThread(UpdateUserInterface), TaskContinuationOptions.OnlyOnRanToCompletion);
        }


        private void UpdateUserInterface()
        {
            InvokeOnMainThread(InstantiateTableViewSource);
            //InvokeOnMainThread(UpdatePageTitle);
            //InvokeOnMainThread(RemoveOverlay);
            //InvokeOnMainThread(StopRefreshing);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            Title = "Sending Details";
            Subtitle = "Contract Creation";
            InvokeOnMainThread(CreateCustomTitleBar);

            LoadData();
        }

        private void CreateCustomTitleBar()
        {
            InvokeOnMainThread(() =>
            {
                _subtitleHeaderView = new SubtitleHeaderView();
                NavigationItem.TitleView = _subtitleHeaderView;

                _subtitleHeaderView.TitleText = Title;
                _subtitleHeaderView.SubtitleText = Subtitle;
                NavigationItem.Title = "";
            });
        }
    }
}

