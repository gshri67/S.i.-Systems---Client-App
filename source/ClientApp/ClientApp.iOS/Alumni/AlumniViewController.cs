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

	    public override void TouchesBegan(NSSet touches, UIEvent evt)
	    {
	        base.TouchesBegan(touches, evt);

	        //contractorSearch.ResignFirstResponder();
	    }

        private void ConfigureSearchEvents()
        {
            var timer = CreateTimer();

            AlumniSearch.TextChanged += delegate
            {
                _displaySearchbar = true;

                //note that this resets the timer's countdown
                timer.Start();
            };
            AlumniSearch.SearchButtonClicked += delegate
            {
                AlumniSearch.ResignFirstResponder();
            };
            AlumniSearch.CancelButtonClicked += delegate
            {
                AlumniSearch.Text = string.Empty;
                AlumniSearch.ResignFirstResponder();
                InvokeOnMainThread(LoadConsultantGroups);
            };
            //show and hide the cancel search button
            AlumniSearch.OnEditingStarted += delegate
            {
                AlumniSearch.SetShowsCancelButton(true, true);
            };
            AlumniSearch.OnEditingStopped += delegate
            {
                DisplaySearchCancelIfNotEmpty();
            };
        }

	    private void DisplaySearchCancelIfNotEmpty()
	    {
            if (AlumniSearch.Text.Equals(string.Empty))
                AlumniSearch.SetShowsCancelButton(false, true);
	    }

	    private Timer CreateTimer()
	    {
	        var timer = new Timer()
	        {
	            Interval = 1000,
	            AutoReset = false,
	            Enabled = false //we don't want to start the timer until we change search text
	        };
	        timer.Elapsed += delegate { InvokeOnMainThread(LoadConsultantGroups); };
	        return timer;
	    }

	    #region View lifecycle

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            
            //set the source for our table's data
            LoadConsultantGroups();

            //Initially hide the search bar while retrieving data from the server
            SetSearchbarVisibility();

            ConfigureSearchEvents();
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
            AlumniSearch.ResignFirstResponder();
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
                               });
	    }

	    private void SetSearchbarVisibility()
	    {
	        if (!_displaySearchbar)
	        {
	            DisplaySearchCancelIfNotEmpty();
	            SpecializationTable.SetContentOffset(
	                new CGPoint(0, AlumniSearch.Frame.Height + SpecializationTable.ContentOffset.Y), true);
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
                    var consultantGroup = source.GetItem(rowpath.Row);
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
