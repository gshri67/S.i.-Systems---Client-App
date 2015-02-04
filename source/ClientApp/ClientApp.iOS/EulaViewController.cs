using Foundation;
using System;
using System.CodeDom.Compiler;
using SiSystems.ClientApp.SharedModels;
using UIKit;

namespace ClientApp.iOS
{
	partial class EulaViewController : UIViewController
	{
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
            //TODO Save to local storage the current user and which version of the EULA they agreed to

            PerformSegue("alumniPushSegue", sender);
        }

	    public override void LoadView()
	    {
	        base.LoadView();

	        if (CurrentEula != null)
	        {
                eulaText.Text = _eula.Text;
                eulaText.TextColor = UIColor.White;
	        }
	    }
	}
}
