using Foundation;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using ClientApp.ViewModels;
using SiSystems.ClientApp.SharedModels;
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
            NSUserDefaults.StandardUserDefaults.SetString(storageString, "eulaVersions");

            PerformSegue("alumniSegue", sender);
        }

	    public override void LoadView()
	    {
	        base.LoadView();

	        if (CurrentEula != null)
	        {
                eulaText.Text = _eula.Text;
	        }
	    }
	}
}
