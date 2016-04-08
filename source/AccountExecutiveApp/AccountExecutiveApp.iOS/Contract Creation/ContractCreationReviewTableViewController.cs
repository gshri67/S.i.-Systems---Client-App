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
	//[Register("ContractCreationReviewTableViewController")]
	public partial class ContractCreationReviewTableViewController : UITableViewController
	{
		public ContractCreationViewModel ViewModel;
		private ContractCreationReviewTableViewSource _tableSource;
		private SubtitleHeaderView _subtitleHeaderView;
		private string Subtitle;
        private LoadingOverlay _overlay;

		public ContractCreationReviewTableViewController(IntPtr handle)
			: base(handle)
		{
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
            TableView.RegisterClassForCellReuse(typeof(EditableDoubleTextFieldCell), EditableDoubleTextFieldCell.CellIdentifier);
			TableView.RegisterClassForCellReuse(typeof(EditableFullTextFieldCell), EditableFullTextFieldCell.CellIdentifier);
            TableView.RegisterClassForCellReuse(typeof(EmailCell), EmailCell.CellIdentifier);
            TableView.RegisterClassForCellReuse(typeof(MultiSelectDescriptionCell), MultiSelectDescriptionCell.CellIdentifier);
			TableView.RegisterClassForCellReuse(typeof(UITableViewCell), "UITableViewCell");
			
			TableView.KeyboardDismissMode = UIScrollViewKeyboardDismissMode.OnDrag;
			_tableSource = new ContractCreationReviewTableViewSource(this, ViewModel);
			TableView.Source = _tableSource;
			TableView.AllowsSelection = false;
			TableView.SeparatorColor = UIColor.Clear;

		    TableView.RowHeight = UITableView.AutomaticDimension;
		    TableView.EstimatedRowHeight = 160.0f;

			ContinueBar continueBar = new ContinueBar();
			continueBar.Frame = new CGRect(0, 0, 100, 50);
            continueBar.NextTextButton.SetTitle( "Submit", UIControlState.Normal );
			continueBar.NextButton.TouchUpInside += delegate
			{
				OnSubmitButtonTapped();
			};
            continueBar.NextTextButton.TouchUpInside += delegate
            {
                OnSubmitButtonTapped();
            };

			TableView.TableFooterView = continueBar;
            TableView.ReloadData();
		}

		public async void LoadData()
		{
			//var task = ViewModel.LoadContractCreationPayRatePage(0);
			//task.ContinueWith(_ => InvokeOnMainThread(UpdateUserInterface), TaskContinuationOptions.OnlyOnRanToCompletion);
	
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

            //AddSubmitNavButton();

			Title = "Review Contract";
			Subtitle = "Contract Creation";
			InvokeOnMainThread(CreateCustomTitleBar);

			LoadData();

		}

	    public void AddSubmitNavButton()
	    {
            UIBarButtonItem submitButton = new UIBarButtonItem();
	        submitButton.Title = "Submit";
            submitButton.Clicked += (sender, e) => { OnSubmitButtonTapped(); };
            NavigationItem.RightBarButtonItem = submitButton;
	    }

	    private void OnSubmitButtonTapped()
	    {
            var finishAlertController = UIAlertController.Create("Contract Creation", "Are you sure you want to create this contract?", UIAlertControllerStyle.Alert);

            //Add Actions
            finishAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, alert => 
	        {
                StartContractSubmission();
	        }
	        ));
            finishAlertController.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, alert => Console.WriteLine("Cancel was clicked")));

            //Present Alert
            PresentViewController(finishAlertController, true, null);
	    }

	    private void StartContractSubmission()
	    {
            IndicateLoading();

	        var task = ViewModel.SubmitContract();
	        task.ContinueWith(_ => InvokeOnMainThread(EndContractSubmission), TaskContinuationOptions.OnlyOnRanToCompletion);  
	    }
	    private void EndContractSubmission()
	    {
            RemoveOverlay();
            DismissViewController(true, null);
	    }

	    public override void ViewDidAppear(bool animated)
	    {
	        base.ViewDidAppear(animated);

            if(_tableSource != null )
                TableView.ReloadData();
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


        #region Overlay

        private void IndicateLoading()
        {
            InvokeOnMainThread(delegate
            {
                if (_overlay != null) return;

                var frame = new CGRect(TableView.Frame.X, TableView.Frame.Y, TableView.Frame.Width, TableView.Frame.Height);
                _overlay = new LoadingOverlay(frame, null);
                View.Add(_overlay);
            });
        }

        private void RemoveOverlay()
        {
            if (_overlay == null) return;

            InvokeOnMainThread(_overlay.Hide);
            _overlay = null;
        }
        #endregion
	}
}

