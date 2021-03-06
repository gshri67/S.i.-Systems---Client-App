using Foundation;
using System;
using System.CodeDom.Compiler;
using SiSystems.SharedModels;
using UIKit;

namespace ClientApp.iOS
{
	public partial class DisciplineViewController : UITableViewController
	{
	    private AlumniViewController _parentController;
	    private ConsultantGroup _consultantGroup;
	    private string _previousScreenTitle;
	    private bool _isActiveConsultants;

		public DisciplineViewController (IntPtr handle) : base (handle)
		{

		}

	    private void ClearParentControllerTitle()
	    {
            _previousScreenTitle = _parentController.Title;
            _parentController.Title = "";
	    }

	    private void SetParentControllerTitle()
	    {
	        _parentController.Title = _previousScreenTitle;
	    }
        
        #region View lifecycle

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            Title = _consultantGroup.Specialization;

            InvokeOnMainThread(delegate
            {
                DisciplineTable.Source = new DisciplineTableViewSource(this, _consultantGroup);
                DisciplineTable.ReloadData();
            });
            
            //set the source for our table's data
            //SpecializationTable.Source = new AlumniTableViewSource(this, consultantGroups);
        }

        #endregion

	    public void SetSpecialization(AlumniViewController parentController, ConsultantGroup consultantGroup, bool isActiveConsultants)
	    {
	        _parentController = parentController;
	        _consultantGroup = consultantGroup;
	        _isActiveConsultants = isActiveConsultants;
	    }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);

            if (segue.Identifier == "ConsultantSelected")
            {
                var view = (ConsultantDetailViewController)segue.DestinationViewController;
                var source = DisciplineTable.Source as DisciplineTableViewSource;
                var rowpath = DisciplineTable.IndexPathForSelectedRow;
                var consultant = source.GetItem(rowpath.Row);

                view.Initialize(consultant);
            }
        }
	}
}
