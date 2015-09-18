using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using ClientApp.Core.ViewModels;
using CoreGraphics;
using Foundation;
using Microsoft.Practices.Unity;
using Shared.Core;
using SiSystems.SharedModels;
using UIKit;

namespace ClientApp.iOS
{
    public partial class AlumniViewController : UIViewController
	{
	    private readonly AlumniViewModel _alumniModel;
        private LoadingOverlay _overlay;
        private const string DisciplineSegueIdentifier = "DisciplineSelected";
        private const int AlumniSelected = 0;
	    private const int SearchTimerInterval = 1000;
        private readonly NSObject _tokenExpiredObserver;
        private UISegmentedControl _tableSelector;

        //Search caching
        private string _lastActiveSearch;
        private string _lastAlumniSearch;
        private IEnumerable<ConsultantGroup> _lastActiveResults;
        private IEnumerable<ConsultantGroup> _lastAlumniResults;

        public bool IsAlumniSelected { get { return _tableSelector.SelectedSegment == AlumniSelected; } }

        public AlumniViewController(IntPtr handle)
            : base(handle)
        {
            _alumniModel = DependencyResolver.Current.Resolve<AlumniViewModel>();
            this._tokenExpiredObserver = NSNotificationCenter.DefaultCenter.AddObserver(new NSString("TokenExpired"), this.OnTokenExpired);
        }

        public void OnTokenExpired(NSNotification notifcation)
        {
            // Unsubscribe from this event so that it won't be handled again
            NSNotificationCenter.DefaultCenter.RemoveObserver(_tokenExpiredObserver);

            var loginViewController = this.Storyboard.InstantiateViewController("LoginView");
            var navigationController = new UINavigationController(loginViewController) { NavigationBarHidden = true };

            UIApplication.SharedApplication.Windows[0].RootViewController = navigationController;
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
                InvokeOnMainThread(() => LoadConsultantGroups());
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
                if (_overlay != null) return;

                var offsetForSearchbar = AlumniSearch.Frame.Height + (float)Math.Abs(SpecializationTable.ContentOffset.Y);

                var frame = new CGRect(AlumniView.Frame.X,
                    AlumniView.Frame.Y + offsetForSearchbar,
                    AlumniView.Frame.Width,
                    AlumniView.Frame.Height - offsetForSearchbar);
                _overlay = new LoadingOverlay(frame, () => this.LoadConsultantGroups(true));
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
            timer.Elapsed += delegate { InvokeOnMainThread(() => LoadConsultantGroups()); };
	        return timer;
	    }

        private async void CheckEulaService()
        {
            if (EulaHandler.Eula != null)
            {
                //we've already checked the Eula, don't ask the server again
                return;
            }

            EulaHandler.Eula = await _alumniModel.GetCurrentEulaAsync();
            if (EulaHandler.Eula == null)
            {
                // Something went wrong
                // Could mean an exception was handled in an unexpected way
                return;
            }

            var storageString = NSUserDefaults.StandardUserDefaults.StringForKey("eulaVersions");
            var hasReadEula = EulaHandler.UserHasReadLatestEula(CurrentUser.Email, EulaHandler.Eula.Version, storageString);

            if (!hasReadEula)
            {
                PerformSegue("eulaSegue", this);
            }
        }

	    #region View lifecycle

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            CheckEulaService();

            CreateNavBarRightButton();

            AddTableSelector();

            GetClientDetails();
            
            //set the source for our table's data
            LoadConsultantGroups();

            ConfigureSearchEvents();
        }

        private void AddTableSelector()
        {
            _tableSelector = new UISegmentedControl(new CGRect(0, 0, 150, 22));
            _tableSelector.InsertSegment("Alumni", 0, false);
            _tableSelector.InsertSegment("Active", 1, false);
            _tableSelector.SelectedSegment = 0;

            _tableSelector.ValueChanged +=
                (sender, args) =>
                {
                    LoadConsultantGroups();
                };

            NavigationItem.TitleView = _tableSelector;
        }

        private void CreateNavBarRightButton()
        {
            var buttonImage = UIImage.FromBundle("app-button").ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal);
            NavigationItem.SetRightBarButtonItem(
                new UIBarButtonItem(buttonImage
                , UIBarButtonItemStyle.Plain
                , (sender, args) =>
                {
                    AdditionalActions_Pressed();
                })
            , true);
        }

        public override void ViewWillDisappear(bool animated)
        {
            AlumniSearch.ResignFirstResponder();
            base.ViewWillDisappear(animated);
        }

        #endregion

        #region Load Consultant Groups

        private Task LoadConsultantGroups(bool force = false)
        {
            if (_tableSelector.SelectedSegment == AlumniSelected)
            {
                Title = "Alumni";
                return LoadAlumniConsultantGroups(force);
            }
            else
            {
                Title = "Active";
                return LoadActiveConsultantGroups(force);
            }
        }

        private async Task LoadAlumniConsultantGroups(bool force)
        {
            if (force || AlumniSearch.Text != _lastAlumniSearch)
            {
                _lastAlumniSearch = AlumniSearch.Text;
                IndicateLoading();
                _lastAlumniResults = await _alumniModel.GetAlumniConsultantGroups(AlumniSearch.Text);

            }

            if (_tableSelector.SelectedSegment == AlumniSelected)
            {
                UpdateTableSource(_lastAlumniResults);                
            }
	    }

        private async Task LoadActiveConsultantGroups(bool force)
        {
            if (force || AlumniSearch.Text != _lastActiveSearch)
            {
                _lastActiveSearch = AlumniSearch.Text;
                IndicateLoading();
                _lastActiveResults = await _alumniModel.GetActiveConsultantGroups(AlumniSearch.Text);
            }

            if (_tableSelector.SelectedSegment != AlumniSelected)
            {
                UpdateTableSource(_lastActiveResults);
            }
        }

        private void UpdateTableSource(IEnumerable<ConsultantGroup> consultantGroups)
        {
            InvokeOnMainThread(delegate
            {
                if (consultantGroups != null)
                {
                    SpecializationTable.Source = new AlumniTableViewSource(this, consultantGroups);

                    SpecializationTable.ReloadData();

                    SetSearchbarVisibility();

                    RemoveOverlay();
                }
                else
                {
                    // Show refresh button
                    this._overlay.SetFailedState();
                }
            });
        }

        #endregion

        private void RemoveOverlay()
	    {
	        if (_overlay == null) return;

	        InvokeOnMainThread(_overlay.Hide);
	        _overlay = null;
	    }

	    private void SetSearchbarVisibility()
	    {
	        if (!string.IsNullOrEmpty(AlumniSearch.Text))
	        {
                return;
	        }
	        DisplaySearchCancelButton();
            SpecializationTable.SetContentOffset(new CGPoint(0, -20), true);
	        AlumniSearch.ResignFirstResponder();
	    }

	    public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);
            
            if (segue.Identifier == "eulaSegue")
            {
                var view = (EulaViewController)segue.DestinationViewController;
                view.CurrentEula = EulaHandler.Eula;
                view.UserName = CurrentUser.Email;
                view.EulaModel = new EulaViewModel(EulaHandler.EulaVersions);
            }
            if (segue.Identifier == DisciplineSegueIdentifier)
            {
                var navCtrl = segue.DestinationViewController as DisciplineViewController;

                if (navCtrl != null)
                {
                    var source = SpecializationTable.Source as AlumniTableViewSource;
                    var rowpath = SpecializationTable.IndexPathForSelectedRow;
                    var consultantGroup = source.GetItem(rowpath.Row);
                    navCtrl.SetSpecialization(this, consultantGroup, _tableSelector.SelectedSegment != AlumniSelected);
                }
            }
        }

        private void AdditionalActions_Pressed()
        {
            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                var controller = UIAlertController.Create(null, null, UIAlertControllerStyle.ActionSheet);
                var logoutAction = UIAlertAction.Create("Logout", UIAlertActionStyle.Destructive, LogoutDelegate);
                var cancelAction = UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, null);
                controller.AddAction(logoutAction);
                controller.AddAction(cancelAction);
                PresentViewController(controller, true, null);
            }
            else
            {
                var sheet = new UIActionSheet();
                sheet.AddButton("Logout");
                sheet.AddButton("Cancel");
                sheet.DestructiveButtonIndex = 0;
                sheet.CancelButtonIndex = 1;
                sheet.Clicked += LogoutDelegate;
                sheet.ShowInView(View);
            }
        }

        private void LogoutDelegate(UIAlertAction action)
        {
            _alumniModel.Logout();
            var rootController =
                UIStoryboard.FromName("MainStoryboard", NSBundle.MainBundle)
                    .InstantiateViewController("LoginView");
            var navigationController = new UINavigationController(rootController)
            {
                NavigationBarHidden = true
            };
            UIApplication.SharedApplication.Windows[0].RootViewController =
                navigationController;
        }

        private void LogoutDelegate(object sender, UIButtonEventArgs args)
        {
            if (args.ButtonIndex == 0)
            {
                LogoutDelegate(null);
            }
        }

        private async void GetClientDetails()
        {
            var clientDetails = await _alumniModel.GetClientDetailsAsync() ?? new ClientAccountDetails();
            CurrentUser.ServiceFee = clientDetails.FloThruFee;
            CurrentUser.MspPercent = clientDetails.MspFeePercentage;
            CurrentUser.FloThruFeePayment = clientDetails.FloThruFeePayment;
            CurrentUser.ClientInvoiceFrequency = clientDetails.ClientInvoiceFrequency;
            CurrentUser.InvoiceFormat = clientDetails.InvoiceFormat;
        }
	}
}
