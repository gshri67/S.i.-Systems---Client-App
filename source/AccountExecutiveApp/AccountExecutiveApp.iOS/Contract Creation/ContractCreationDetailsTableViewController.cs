
using System;
using AccountExecutiveApp.Core.ViewModel;
using Foundation;
using UIKit;
using Microsoft.Practices.Unity;

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

            UIButton nextButton = new UIButton();
            nextButton.SetTitleColor(UIColor.Blue, UIControlState.Normal);
            nextButton.SetTitle("Next", UIControlState.Normal);

            TableView.TableFooterView = nextButton;
            
            TableView.ReloadData();
		}

	    public override void ViewDidLoad()
	    {
	        base.ViewDidLoad();

	        InstantiateTableViewSource();
	        // Perform any additional setup after loading the view, typically from a nib.
	    }
	}
}

