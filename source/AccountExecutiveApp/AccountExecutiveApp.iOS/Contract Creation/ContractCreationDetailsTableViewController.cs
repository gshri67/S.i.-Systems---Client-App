﻿
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
	public partial class ContractCreationDetailsTableViewController : UITableViewController
	{
	    private readonly ContractCreationViewModel _viewModel;
	    private readonly ContractBodySupportViewModel _optionsModel;
	    private Contractor _contractor;
	    private int _jobId;
        private SubtitleHeaderView _subtitleHeaderView;
        private string Subtitle;
	    private bool _hasLoadedContractDetails = false;
        private bool _hasLoadedContractOptions = false;

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
	        _jobId = jobId;
	        _viewModel.JobId = _jobId;
	        _viewModel.CandidateId = contractor.ContactInformation.Id;

            LoadContractOptions();
            LoadContractCreationDetails();
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
			TableView.Source = new ContractCreationDetailsTableViewSource(this, _viewModel, _optionsModel);
		    //TableView.AllowsSelection = false;

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
            var task = _viewModel.LoadContractCreationInitialPageData();
            task.ContinueWith(_ => InvokeOnMainThread(UpdateInterfaceAfterLoadingContractDetailsIfNeeded), TaskContinuationOptions.OnlyOnRanToCompletion);
            /*
            _viewModel.Contract = new ContractCreationDetails
            {
                JobTitle = "Developer",
                StartDate = new DateTime(2016, 1, 22),
                PaymentPlan = "Part Time",
                DaysCancellation = 10,
            };
             * */
            //UpdateUserInterface();
        }

	    public void UpdateInterfaceAfterLoadingContractDetailsIfNeeded()
	    {
	        _hasLoadedContractDetails = true;

            if( _hasLoadedContractDetails && _hasLoadedContractOptions )
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

	        // Perform any additional setup after loading the view, typically from a nib.


			UIBarButtonItem cancelButton = new UIBarButtonItem("Cancel", UIBarButtonItemStyle.Plain, delegate(object sender, EventArgs e) {
				DismissViewController(true, null);
			} );
	        cancelButton.TintColor = StyleGuideConstants.RedUiColor;


			NavigationItem.SetLeftBarButtonItem(cancelButton, true);


	    }

	    private void LoadContractOptions()
	    {
	        var loadingOptions = _optionsModel.GetContractBodyOptions(_jobId);
            //todo: update ui
            loadingOptions.ContinueWith(_ => InvokeOnMainThread(UpdateInterfaceAfterLoadingContractOptionsIfNeeded), TaskContinuationOptions.OnlyOnRanToCompletion);
	    }

        public void UpdateInterfaceAfterLoadingContractOptionsIfNeeded()
        {
            _hasLoadedContractOptions = true;

            if (_hasLoadedContractDetails && _hasLoadedContractOptions)
                UpdateUserInterface();
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

