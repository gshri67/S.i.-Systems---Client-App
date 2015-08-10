using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace ConsultantApp.iOS
{
	partial class RemittancesViewController : UIViewController
	{
		public RemittancesViewController (IntPtr handle) : base (handle)
		{
			TabBarItem.Image = (new UIImage ("social-usd.png")).ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal);
		}
	}
}
