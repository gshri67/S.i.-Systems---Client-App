using System.Drawing;
using CoreGraphics;
using UIKit;

namespace ClientApp.iOS
{
    public class LoadingOverlay : UIView
    {
        // control declarations
        UIActivityIndicatorView activitySpinner;
        UILabel loadingLabel;

        public LoadingOverlay(CGRect frame)
            : base(frame)
        {
            // configurable bits
            BackgroundColor = UIColor.White;
            AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;
            
            // derive the center x and y
            var centerX = Frame.Width / 2;
            var centerY = Frame.Height / 2;

            // create the activity spinner, center it horizontall and put it 5 points above center x
            activitySpinner = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.Gray);
            activitySpinner.Frame = new CGRect(
                centerX - (activitySpinner.Frame.Width / 2),
                centerY - activitySpinner.Frame.Height - 20,
                activitySpinner.Frame.Width,
                activitySpinner.Frame.Height);
            activitySpinner.AutoresizingMask = UIViewAutoresizing.FlexibleMargins;
            AddSubview(activitySpinner);
            activitySpinner.StartAnimating();
        }

        /// <summary>
        /// Fades out the control and then removes it from the super view
        /// </summary>
        public void Hide()
        {
            Animate(
                0.5, // duration
                () => { Alpha = 0; },
                () => { RemoveFromSuperview(); }
            );
        }
    };
}