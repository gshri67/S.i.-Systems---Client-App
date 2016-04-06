
using System;
using AccountExecutiveApp.Core.ViewModel;
using CoreGraphics;
using Foundation;
using UIKit;
using Microsoft.Practices.Unity;
using SiSystems.SharedModels;

namespace AccountExecutiveApp.iOS
{
	public partial class ContractCreationDetailsTableViewController : UITableViewController
	{
	    private readonly ContractCreationViewModel _viewModel;
	    private readonly ContractBodySupportViewModel _optionsModel;
	    private Contractor _contractor;
        private SubtitleHeaderView _subtitleHeaderView;
        private string Subtitle;

	    public ContractCreationDetailsTableViewController(IntPtr handle)
	        : base(handle)
	    {
            _viewModel = DependencyResolver.Current.Resolve<ContractCreationViewModel>();
            _optionsModel = DependencyResolver.Current.Resolve<ContractBodySupportViewModel>();
	    }
		
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

	    public void SetupViewController(Contractor contractor, int jobId)
	    {
	        _contractor = contractor;
            LoadContractOptions(contractor.ContactInformation.Id, jobId);
	    }


	    public void SetConsultant( Contractor contractor )
	    {
	        _contractor = contractor;
	    }

	    private void InstantiateTableViewSource()
		{
			if (TableView == null)
				return;

			TableView.RegisterClassForCellReuse(typeof(SubtitleWithRightDetailCell), SubtitleWithRightDetailCell.CellIdentifier);
			TableView.RegisterClassForCellReuse(typeof(RightDetailCell), RightDetailCell.CellIdentifier);
            TableView.RegisterClassForCellReuse(typeof(EditableTextFieldCell), EditableTextFieldCell.CellIdentifier);
            TableView.RegisterClassForCellReuse(typeof(EditablePickerCell), EditablePickerCell.CellIdentifier);
            TableView.RegisterClassForCellReuse(typeof(EditableDatePickerCell), EditableDatePickerCell.CellIdentifier);
            TableView.RegisterClassForCellReuse(typeof(EditableNumberFieldCell), EditableNumberFieldCell.CellIdentifier);
            TableView.RegisterClassForCellReuse(typeof(EditableBooleanCell), EditableBooleanCell.CellIdentifier);
            TableView.RegisterClassForCellReuse(typeof(EditablePickerWithNumberFieldValueCell), EditablePickerWithNumberFieldValueCell.CellIdentifier);
			TableView.RegisterClassForCellReuse(typeof(UITableViewCell), "UITableViewCell");

            TableView.KeyboardDismissMode = UIScrollViewKeyboardDismissMode.OnDrag;
			TableView.Source = new ContractCreationDetailsTableViewSource(this, _viewModel);
		    TableView.AllowsSelection = false;

		    TableView.AlwaysBounceVertical = false;
		    TableView.Bounces = false;

            ContinueBar continueBar = new ContinueBar();
		    continueBar.Frame = new CGRect(0, 0, 100, 50);

            continueBar.NextButton.TouchUpInside += delegate
            {
                if (_contractor != null)
                    _viewModel.ConsultantName = _contractor.ContactInformation.FullName;

                var vc = (ContractCreationPayRatesTableViewController)Storyboard.InstantiateViewController("ContractCreationPayRatesTableViewController");
                vc.ViewModel = _viewModel;
                ShowViewController(vc, this);
            };
            continueBar.NextTextButton.TouchUpInside += delegate
            {
                if (_contractor != null)
                    _viewModel.ConsultantName = _contractor.ContactInformation.FullName;

                var vc = (ContractCreationPayRatesTableViewController)Storyboard.InstantiateViewController("ContractCreationPayRatesTableViewController");
                vc.ViewModel = _viewModel;
                ShowViewController(vc, this);
            };

            TableView.TableFooterView = continueBar;
            
            TableView.ReloadData();
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
                DaysCancellation = 10,
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

	        Title = "Contract Details";
	        Subtitle = "Contract Creation";
            InvokeOnMainThread(CreateCustomTitleBar);

            LoadContractCreationDetails();
	        // Perform any additional setup after loading the view, typically from a nib.


			UIBarButtonItem cancelButton = new UIBarButtonItem("Cancel", UIBarButtonItemStyle.Plain, delegate(object sender, EventArgs e) {
				DismissViewController(true, null);
			} );
	        cancelButton.TintColor = StyleGuideConstants.RedUiColor;


			NavigationItem.SetLeftBarButtonItem(cancelButton, true);


	    }

	    private void LoadContractOptions(int contractorId, int jobId)
	    {
	        var loadingOptions = _optionsModel.GetContractBodyOptions(contractorId, jobId);
            //todo: update ui
	        //loadingOptions.ContinueWith();
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

