using System.Drawing;
using CoreGraphics;
using UIKit;
using System;
using System.Threading.Tasks;

namespace ClientApp.iOS
{
    public class LoadingOverlay : UIView
    {
        // control declarations
        UIActivityIndicatorView activitySpinner;
        UIButton refreshButton;
        UILabel errorLabel;

        public bool IsVisible { get; private set; }

        public LoadingOverlay(CGRect frame, Func<Task> retryFn)
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
            var buttonFrame = new CGRect(
                centerX - (activitySpinner.Frame.Width / 2),
                centerY - activitySpinner.Frame.Height - 20,
                activitySpinner.Frame.Width,
                activitySpinner.Frame.Height);
            activitySpinner.Frame = buttonFrame;
            activitySpinner.AutoresizingMask = UIViewAutoresizing.FlexibleMargins;
            AddSubview(activitySpinner);
            activitySpinner.StartAnimating();
            this.IsVisible = true;

            if (retryFn != null)
            {
                SetRetryFunction(retryFn);
            }
        }

        public void SetFailedState(Func<Task> retryFn = null)
        {
            if (retryFn != null)
            {
                SetRetryFunction(retryFn);
            }

            this.IsVisible = true;
            activitySpinner.Hidden = true;
            refreshButton.Hidden = false;
            errorLabel.Hidden = false;
        }

        private void SetRetryFunction(Func<Task> retryFn)
        {
            if (refreshButton == null)
            {
                // TODO: Center these in the frame
                var parentFrame = activitySpinner.Frame;

                var labelFrame = new CGRect(0, 10, parentFrame.Height, parentFrame.Width);
                errorLabel = new UILabel(labelFrame);
                errorLabel.Text = "Something went wrong!";
                errorLabel.SizeToFit();
                errorLabel.AutoresizingMask = UIViewAutoresizing.FlexibleDimensions | UIViewAutoresizing.FlexibleMargins;
                errorLabel.Hidden = true;
                errorLabel.TextAlignment = UITextAlignment.Center;

                var buttonFrame = new CGRect(Frame.Width/2, labelFrame.Height + 10, parentFrame.Height, parentFrame.Width);
                refreshButton = new UIButton(buttonFrame);
                refreshButton.SetTitle("Retry", UIControlState.Normal);
                refreshButton.SetTitleColor(StyleGuideConstants.DarkGrayUiColor, UIControlState.Normal);
                refreshButton.SizeToFit();
                refreshButton.AutoresizingMask = UIViewAutoresizing.FlexibleMargins;
                refreshButton.Hidden = true;

                AddSubview(errorLabel);
                AddSubview(refreshButton);
            }

            refreshButton.TouchUpInside += async (s, e) =>
            {
                activitySpinner.Hidden = false;
                refreshButton.Hidden = true;
                errorLabel.Hidden = true;

                try
                {
                    await retryFn();
                }
                catch
                {
                    SetFailedState();
                }
            };
        }

        /// <summary>
        /// Fades out the control and then removes it from the super view
        /// </summary>
        public void Hide()
        {
            this.IsVisible =false;

            Animate(
                0.5, // duration
                () => { Alpha = 0; },
                () => { RemoveFromSuperview(); }
            );
        }
    };
}