using System;
using AccountExecutiveApp.Core.ViewModel;
using CoreGraphics;
using Foundation;
using UIKit;
using Microsoft.Practices.Unity;
using SiSystems.SharedModels;

namespace AccountExecutiveApp.iOS
{
    public partial class ContractCreationPayRatesTableViewController : UITableViewController
    {
        private ContractCreationViewModel _viewModel;
        private ContractCreationPayRatesTableViewSource _tableSource;

        public ContractCreationPayRatesTableViewController(IntPtr handle)
            : base(handle)
        {
            _viewModel = DependencyResolver.Current.Resolve<ContractCreationViewModel>();
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
            TableView.RegisterClassForCellReuse(typeof(UITableViewCell), "UITableViewCell");

            TableView.KeyboardDismissMode = UIScrollViewKeyboardDismissMode.OnDrag;
            _tableSource = new ContractCreationPayRatesTableViewSource(this, _viewModel);
            TableView.Source = _tableSource;
            TableView.AllowsSelection = false;
            TableView.SeparatorColor = UIColor.Clear;

            ContinueBar continueBar = new ContinueBar();
            continueBar.Frame = new CGRect(0, 0, 100, 50);
            continueBar.NextButton.TouchUpInside += delegate
            {
                //var vc = (ContractCreationPayRatesTableViewController)Storyboard.InstantiateViewController("ContractCreationPayRatesTableViewController");
                //ShowViewController(vc, this);
            };

            TableView.TableFooterView = continueBar;

            //Add one pay rate section to begin with
            _tableSource.AddRatesSection(TableView);
        }

        public async void LoadContractCreationDetails()
        {
            //var task = _viewModel.LoadContractCreationDetails(_id);
            //task.ContinueWith(_ => InvokeOnMainThread(UpdateUserInterface), TaskContinuationOptions.OnlyOnRanToCompletion);
            _viewModel.Contract = new ContractCreationDetails
            {
                JobTitle = "Developer",
                StartDate = new DateTime(2016, 1, 22),
                PaymentPlan = "Part Time",
                DaysCancellation = 10
            };
            UpdateUserInterface();
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

            LoadContractCreationDetails();
            // Perform any additional setup after loading the view, typically from a nib.


            UIBarButtonItem addButton = new UIBarButtonItem("Add", UIBarButtonItemStyle.Plain, delegate(object sender, EventArgs e)
            {
                _tableSource.AddRatesSection(TableView);
            });
            addButton.TintColor = StyleGuideConstants.RedUiColor;

         

            NavigationItem.SetRightBarButtonItem(addButton, true);


        }


    }
}

