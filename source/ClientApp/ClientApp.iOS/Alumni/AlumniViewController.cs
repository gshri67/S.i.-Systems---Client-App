using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using ClientApp.iOS.Startup;
using ClientApp.ViewModels;
using CoreGraphics;
using Foundation;
using Microsoft.Practices.Unity;
using SiSystems.ClientApp.SharedModels;
using UIKit;

namespace ClientApp.iOS
{
	public partial class AlumniViewController : UIViewController
	{
	    private readonly AlumniViewModel _alumniModel;
	    private bool _displaySearchbar = false;

        public AlumniViewController(IntPtr handle)
            : base(handle)
        {
            _alumniModel = DependencyResolver.Current.Resolve<AlumniViewModel>();
        }

	    private void SetSummaryLabel(IEnumerable<ConsultantGroup> consultantGroup)
        {
            var alumniCount = CountAlumni(consultantGroup);
            var specializationsCount = CountSpecializations(consultantGroup);

            //summaryLabel.Text = specializationsCount == 0 ? 
            //    string.Format("There are no records to display.") : 
            //    string.Format("You have {0} alumni in {1} specializations.", alumniCount, specializationsCount);
        }

        private static int CountSpecializations(IEnumerable<ConsultantGroup> consultantGroup)
        {
            return consultantGroup.Count();
        }

        private static int CountAlumni(IEnumerable<ConsultantGroup> consultantGroup)
        {
            return consultantGroup.Sum(x => x.Consultants.Count);
        }

	    public override void TouchesBegan(NSSet touches, UIEvent evt)
	    {
	        base.TouchesBegan(touches, evt);

	        //contractorSearch.ResignFirstResponder();
	    }

        private void SetupSearchTimer()
        {
            var timer = new Timer()
            {
                Interval = 1000,
                AutoReset = false,
                Enabled = false //we don't want to start the timer until we change search text
            };
            timer.Elapsed += delegate
            {
                InvokeOnMainThread(LoadConsultantGroups);
            };

            AlumniSearch.TextChanged += delegate
            {
                _displaySearchbar = true;

                //note that this resets the timer
                timer.Start();
            };
            AlumniSearch.SearchButtonClicked += delegate
            {
                AlumniSearch.ResignFirstResponder();
            };
        }

	    #region View lifecycle

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            
            //set the source for our table's data
            LoadConsultantGroups();

            SetupSearchTimer();
        }

	    public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

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
            var consultantGroups = await  _alumniModel.GetConsultantGroups(AlumniSearch.Text);
            InvokeOnMainThread(delegate
                               {
                                   SpecializationTable.Source = new AlumniTableViewSource(this, consultantGroups);

                                   SetSearchbarVisibility();

                                   SpecializationTable.ReloadData();
                                   SetSummaryLabel(consultantGroups);
                               });
	    }

	    private void SetSearchbarVisibility()
	    {
	        if (!_displaySearchbar)
	        {
	            SpecializationTable.SetContentOffset(
	                new CGPoint(0, AlumniSearch.Frame.Height + SpecializationTable.ContentOffset.Y), false);
	        }
	    }

	    public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);
            
            if (segue.Identifier == "DisciplineSelected")
            {
                var navCtrl = segue.DestinationViewController as DisciplineViewController;

                if (navCtrl != null)
                {
                    var source = SpecializationTable.Source as AlumniTableViewSource;
                    var rowpath = SpecializationTable.IndexPathForSelectedRow;
                    var consultantGroup = source.GetItem(rowpath.Section);
                    navCtrl.SetSpecialization(this, consultantGroup);
                }
            }
        }

        partial void AdditionalActions_Activated(UIBarButtonItem sender)
        {
            var controller = UIAlertController.Create(null, null, UIAlertControllerStyle.ActionSheet);
            var logoutAction = UIAlertAction.Create("Logout", UIAlertActionStyle.Destructive,
                delegate
                {
                    _alumniModel.Logout();
                    TokenStore.DeleteDeviceToken();
                    InvokeOnMainThread(delegate{PerformSegue("logoutSegue", this);});
                });
            var cancelAction = UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, null);
            controller.AddAction(logoutAction);
            controller.AddAction(cancelAction);
            PresentViewController(controller, true, null);
        }
	}
}
