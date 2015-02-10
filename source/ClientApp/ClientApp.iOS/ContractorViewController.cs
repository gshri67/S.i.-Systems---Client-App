using System;
using ClientApp.ViewModels;
using Microsoft.Practices.Unity;
using UIKit;

namespace ClientApp.iOS
{
	public partial class ContractorViewController : UIViewController
	{
	    private readonly ContractorViewModel _contractorModel;
        public ContractorViewController (IntPtr handle) : base (handle)
        {
            _contractorModel = DependencyResolver.Current.Resolve<ContractorViewModel>();
        }

        #region View lifecycle

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Perform any additional setup after loading the view, typically from a nib.
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            //set the source for our table's data
            LoadConsultantGroups();
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
        }

	    public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
        }

        #endregion

	    private async void LoadConsultantGroups()
	    {
            //get our list of specializations to display
            var consultantGroups = await  _contractorModel.GetConsultantGroups(""); //TODO Stick search box's text here
            InvokeOnMainThread(delegate
                               {
                                   SpecializationTable.Source = new ContractsTableViewSource(this, consultantGroups);
                                   SpecializationTable.ReloadData();
                               });
	    }

        
	}
}
