using Foundation;
using System;
using ClientApp.Core.ViewModels;
using SiSystems.SharedModels;
using UIKit;

namespace ClientApp.iOS
{
    partial class EulaViewController : UIViewController
	{
        public EulaViewModel EulaModel { private get; set; }
        public string UserName { private get; set; }

        private Eula _eula;
        public Eula CurrentEula
	    {
            private get { return _eula; }
	        set
	        {
	            _eula = value;
	            if (eulaText != null)
	            {
                    eulaText.Text = value.Text;
	            }
	        }
	    }

	    public EulaViewController (IntPtr handle) : base(handle)
        {
        }

        partial void agree_TouchUpInside(UIButton sender)
        {
            EulaModel.AcceptEula( UserName, _eula.Version);
            var storageString = EulaModel.GetUpdatedStorageString();
            try
            {
                NSUserDefaults.StandardUserDefaults.SetString(storageString, "eulaVersions");
            }
            catch (Exception)
            {
                //Continue if the save fails.  They will just see the Eula again next login
            }
            UIApplication.SharedApplication.Windows[0].RootViewController = UIStoryboard.FromName("MainStoryboard", NSBundle.MainBundle).InstantiateInitialViewController();
        }

	    public override void ViewDidLoad()
	    {
	        base.ViewDidLoad();

	        if (CurrentEula != null)
	        {
                eulaText.Text = _eula.Text;
	        }
	    }
	}
}
