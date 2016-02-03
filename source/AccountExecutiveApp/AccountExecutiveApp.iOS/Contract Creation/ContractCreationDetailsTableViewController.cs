
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
		    TableView.AllowsSelection = false;

		    TableView.AlwaysBounceVertical = false;
		    TableView.Bounces = false;

            UIButton nextButton = new UIButton( new CGRect(0, 0, 50, 50));
            nextButton.SetTitleColor(UIColor.Blue, UIControlState.Normal);
            nextButton.SetBackgroundImage( new UIImage("ios7-arrow-forward.png"), UIControlState.Normal);
            //nextButton.SetTitle("Next", UIControlState.Normal);
			//nextButton.BackgroundColor = UIColor.FromWhiteAlpha (0.9f, 0.5f);
		    nextButton.TranslatesAutoresizingMaskIntoConstraints = false;


            UIView tableFooter = new UIView(new CGRect(0, 0, 100, 50));
            tableFooter.AddSubview(nextButton);
		    tableFooter.BackgroundColor = UIColor.FromWhiteAlpha(0.9f, 1.0f);
		    /*
            tableFooter.Layer.ShadowColor = UIColor.Black.CGColor;
		    tableFooter.Layer.ShadowRadius = 1.0f;
            tableFooter.Layer.ShadowOffset = new CGSize(0, 10);
            tableFooter.Layer.ShadowOpacity = 1.0f;
            */
		    tableFooter.AddConstraint(NSLayoutConstraint.Create( nextButton, NSLayoutAttribute.Right, NSLayoutRelation.Equal, tableFooter, NSLayoutAttribute.Right, 1.0f, 0 ));
            tableFooter.AddConstraint(NSLayoutConstraint.Create(nextButton, NSLayoutAttribute.Width, NSLayoutRelation.Equal, nextButton, NSLayoutAttribute.Height, 0.9f, 0));
            tableFooter.AddConstraint(NSLayoutConstraint.Create(nextButton, NSLayoutAttribute.Top, NSLayoutRelation.Equal, tableFooter, NSLayoutAttribute.Top, 1.0f, 0));
            tableFooter.AddConstraint(NSLayoutConstraint.Create(nextButton, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, tableFooter, NSLayoutAttribute.Bottom, 1.0f, 0));

		    UIView LineView = new UIView( new CGRect(0, 0, 200, 1) );
            LineView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
		    LineView.BackgroundColor = UIColor.Black;
		    tableFooter.AddSubview(LineView);

            TableView.TableFooterView = tableFooter;
            

		    //UIToolbar tableToolbar = new UIToolbar( new CGRect(0, 0, TableView.Frame.Size.Width, 50));
            //tableToolbar.add

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


			UIBarButtonItem cancelButton = new UIBarButtonItem("Cancel", UIBarButtonItemStyle.Plain, delegate(object sender, EventArgs e) {
				DismissViewController(true, null);
			} );



			NavigationItem.SetLeftBarButtonItem(cancelButton, true);


	    }
	}
}

