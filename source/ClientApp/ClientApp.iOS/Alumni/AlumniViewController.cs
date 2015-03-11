using System;
using System.Timers;
using ClientApp.iOS.Startup;
using CoreGraphics;
using Foundation;
using Microsoft.Practices.Unity;
using UIKit;
using ClientApp.Core.ViewModels;
using System.Linq;
using System.Net;
using SiSystems.ClientApp.SharedModels;
using ClientApp.Core;

namespace ClientApp.iOS
{
    public partial class AlumniViewController : UIViewController
	{
	    private readonly AlumniViewModel _alumniModel;
        private readonly IActivityManager _activityManager;
        private LoadingOverlay _overlay;
        private const string DisciplineSegueIdentifier = "DisciplineSelected";
        private const string LogoutSegueIdentifier = "logoutSegue";
	    private const int SearchTimerInterval = 1000;

        public AlumniViewController(IntPtr handle)
            : base(handle)
        {
            _alumniModel = DependencyResolver.Current.Resolve<AlumniViewModel>();
            _activityManager = DependencyResolver.Current.Resolve<IActivityManager>();
            NSNotificationCenter.DefaultCenter.AddObserver(new NSString("TokenExpired"), this.OnTokenExpired);
        }

        public void OnTokenExpired(NSNotification notifcation)
        {
            var loginViewController = this.Storyboard.InstantiateViewController("LoginView");
            this.NavigationController.PushViewController(loginViewController, true);
        }

        public override void TouchesBegan(NSSet touches, UIEvent evt)
	    {
	        base.TouchesBegan(touches, evt);
	    }

        private void ConfigureSearchEvents()
        {
            var timer = CreateTimer();

            ConfigureSearchKeyboard();

            AlumniSearch.TextChanged += delegate
            {
                IndicateLoading();

                //note that this resets the timer's countdown
                timer.Stop();
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
                InvokeOnMainThread(LoadAlumniConsultantGroups);
            };
            AlumniSearch.OnEditingStarted += delegate
            {
                AlumniSearch.SetShowsCancelButton(true, true);
            };
            AlumniSearch.OnEditingStopped += delegate
            {
                DisplaySearchCancelButton();
            };
        }

	    private void ConfigureSearchKeyboard()
	    {
	        AlumniSearch.ReturnKeyType = UIReturnKeyType.Done;
	        AlumniSearch.EnablesReturnKeyAutomatically = false;
	    }

	    private void IndicateLoading()
	    {
            InvokeOnMainThread(delegate
            {
                this._activityManager.StartActivity();
                if (_overlay != null) return;

                var offsetForSearchbar = AlumniSearch.Frame.Height + (float)Math.Abs(SpecializationTable.ContentOffset.Y);

                var frame = new CGRect(AlumniView.Frame.X,
                    AlumniView.Frame.Y + offsetForSearchbar,
                    AlumniView.Frame.Width,
                    AlumniView.Frame.Height - offsetForSearchbar);
                _overlay = new LoadingOverlay(frame);
                View.Add(_overlay);
            });
	    }

	    private void DisplaySearchCancelButton()
	    {
            if (AlumniSearch.Text.Equals(string.Empty))
                AlumniSearch.SetShowsCancelButton(false, true);
	    }

	    private Timer CreateTimer()
	    {
	        var timer = new Timer()
	        {
                Interval = SearchTimerInterval,
	            AutoReset = false,
	            Enabled = false //we don't want to start the timer until we change search text
	        };
	        timer.Elapsed += delegate { InvokeOnMainThread(LoadAlumniConsultantGroups); };
	        return timer;
	    }

	    #region View lifecycle

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            CreateNavBarRightButton();

            GetClientDetails();

            IndicateLoading();
            
            //set the source for our table's data
            LoadAlumniConsultantGroups();

            ConfigureSearchEvents();
        }

        private void CreateNavBarRightButton()
        {
            NavigationItem.SetRightBarButtonItem(
                new UIBarButtonItem(UIImage.FromBundle("app-button").ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal)
                , UIBarButtonItemStyle.Plain
                , (sender, args) =>
                {
                    AdditionalActions_Pressed();
                })
            , true);
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

	    private async void LoadAlumniConsultantGroups()
	    {
	        //get our list of specializations to display
            var consultantGroups = await _alumniModel.GetAlumniConsultantGroups(AlumniSearch.Text) ?? Enumerable.Empty<ConsultantGroup>();
            InvokeOnMainThread(delegate
                               {
                                   SpecializationTable.Source = new AlumniTableViewSource(this, consultantGroups);
                                   
                                   SpecializationTable.ReloadData();

                                   SetSearchbarVisibility();
                               });
            RemoveOverlay();
	    }

        private async void LoadActiveConsultantGroups()
        {
            //TODO Actually use this for other search tab
            var consultantGroups = await _alumniModel.GetActiveConsultantGroups(AlumniSearch.Text) ?? Enumerable.Empty<ConsultantGroup>();
        }

	    private void RemoveOverlay()
	    {
            this._activityManager.StopActivity();
	        if (_overlay == null) return;

	        InvokeOnMainThread(_overlay.Hide);
	        _overlay = null;
	    }

	    private void SetSearchbarVisibility()
	    {
	        if (!AlumniSearch.Text.Equals(string.Empty))
	            return;
	        DisplaySearchCancelButton();
	        SpecializationTable.SetContentOffset(
	            new CGPoint(0, AlumniSearch.Frame.Height + SpecializationTable.ContentOffset.Y), true);
	        AlumniSearch.ResignFirstResponder();
	    }

	    public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);
            
            if (segue.Identifier == DisciplineSegueIdentifier)
            {
                var navCtrl = segue.DestinationViewController as DisciplineViewController;

                if (navCtrl != null)
                {
                    var source = SpecializationTable.Source as AlumniTableViewSource;
                    var rowpath = SpecializationTable.IndexPathForSelectedRow;
                    var consultantGroup = source.GetItem(rowpath.Row);
                    //TODO swap this based on if Active or Alumni is selected
                    navCtrl.SetSpecialization(this, consultantGroup, true);
                }
            }
        }

        private void AdditionalActions_Pressed()
        {
            var controller = UIAlertController.Create(null, null, UIAlertControllerStyle.ActionSheet);
            var logoutAction = UIAlertAction.Create("Logout", UIAlertActionStyle.Destructive,
                async delegate
                {
                    await _alumniModel.Logout();
                    var rootController = UIStoryboard.FromName("MainStoryboard", NSBundle.MainBundle).InstantiateViewController("LoginView");
                    var navigationController = new UINavigationController(rootController) { NavigationBarHidden = true };
                    UIApplication.SharedApplication.Windows[0].RootViewController = navigationController;
                });
            var cancelAction = UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, null);
            controller.AddAction(logoutAction);
            controller.AddAction(cancelAction);
            PresentViewController(controller, true, null);
        }

        private async void GetClientDetails()
        {
            var clientDetails = await _alumniModel.GetClientDetailsAsync();
            CurrentUser.ServiceFee = clientDetails.FloThruFee;
            CurrentUser.MspPercent = clientDetails.MspFeePercentage;
            CurrentUser.FloThruFeeType = clientDetails.FloThruFeeType;
            CurrentUser.FloThruFeePayer = clientDetails.FloThruFeePayer;
            CurrentUser.InvoiceFormat = clientDetails.InvoiceFormat;
            CurrentUser.InvoiceFrequency = clientDetails.InvoiceFrequency;
        }
	}
}
