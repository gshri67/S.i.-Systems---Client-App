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
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();


			CreateNavBarLeftButton ();
		}

		private void CreateNavBarLeftButton()
		{
			var buttonImage = UIImage.FromBundle("app-button").ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal);
			NavigationItem.SetLeftBarButtonItem(
				new UIBarButtonItem(buttonImage
					, UIBarButtonItemStyle.Plain
					, (sender, args) =>
					{
						//AdditionalActions_Pressed();
					})
				, true);
		}
	}
}
