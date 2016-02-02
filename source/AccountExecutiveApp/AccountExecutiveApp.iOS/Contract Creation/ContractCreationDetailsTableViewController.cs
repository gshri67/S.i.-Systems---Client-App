﻿
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
	    ContractCreationViewModel _viewModel;

	    public ContractCreationDetailsTableViewController(IntPtr handle)
	        : base(handle)
	    {
            _viewModel = DependencyResolver.Current.Resolve<ContractCreationViewModel>();
	    }
		
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
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
			TableView.RegisterClassForCellReuse(typeof(UITableViewCell), "UITableViewCell");

            TableView.KeyboardDismissMode = UIScrollViewKeyboardDismissMode.OnDrag;
			TableView.Source = new ContractCreationDetailsTableViewSource(this, _viewModel);

            UIButton nextButton = new UIButton( new CGRect(0, 0, 100, 100));
            nextButton.SetTitleColor(UIColor.Blue, UIControlState.Normal);
            nextButton.SetTitle("Next", UIControlState.Normal);

            TableView.TableFooterView = nextButton;
            
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
                PaymentPlan = "part-time"
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
	    }
	}
}

