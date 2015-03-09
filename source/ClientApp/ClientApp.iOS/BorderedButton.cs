using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using CoreGraphics;

namespace ClientApp.iOS
{
	partial class BorderedButton : UIButton
	{
        public BorderedButton() : base() { }

		public BorderedButton (IntPtr handle) : base (handle)
		{
		}

        public override void Draw(CGRect rect)
        {
            try
            {
                var path = UIBezierPath.FromRoundedRect(rect.Inset(1, 1), StyleGuideConstants.ButtonCornerRadius);
                CurrentTitleColor.SetStroke();
                path.Stroke();
            }
            catch (Exception)
            {
            }
        }
    }
}
