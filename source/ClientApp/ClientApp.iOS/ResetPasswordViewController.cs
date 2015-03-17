using ClientApp.Core.ViewModels;
using Foundation;
using System;
using System.CodeDom.Compiler;
using Microsoft.Practices.Unity;
using UIKit;
using System.Threading.Tasks;

namespace ClientApp.iOS
{
	partial class ResetPasswordViewController : UIViewController
	{
        private readonly ResetPasswordViewModel viewModel;

        public ResetPasswordViewController (IntPtr handle) : base (handle)
		{
            this.viewModel = DependencyResolver.Current.Resolve<ResetPasswordViewModel>();
        }

        public void Initialize(string title, string emailAddress)
        {
            this.Title = title;
            this.viewModel.EmailAddress = emailAddress;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.emailAddressField.Text = this.viewModel.EmailAddress;
            this.emailAddressField.EditingChanged += (s, e) =>
            {
                var textField = s as UITextField;
                this.viewModel.EmailAddress = textField.Text;
            };
            emailAddressField.ShouldReturn += textField =>
            {
                this.emailAddressField.ResignFirstResponder();
                this.ResetPassword();
                return true;
            };
        }

        partial void SubmitButton_TouchUpInside(UIButton sender)
        {
            this.ResetPassword();
        }

        private void ResetPassword()
        {
            this.activityIndicator.StartAnimating();

            this.emailAddressField.Enabled = false;
            this.emailAddressField.BackgroundColor = StyleGuideConstants.LightGrayUiColor;

            this.SubmitButton.Enabled = false;

            var alertViewResponseDelegate = new ResetPasswordResponseViewDelegate(this);

            this.viewModel.ResetPassword().ContinueWith(t =>
            {
                InvokeOnMainThread(() =>
                {
                    var result = t.Result;
                    var responseMessage = result != null
                        ? result.Description
                        : "Your Client Portal account may not be activated. Please contact your Account Executive to resolve this issue.";
                    UIAlertView responseAlertView = new UIAlertView(responseMessage, null, alertViewResponseDelegate, "Ok");

                    responseAlertView.Show();
                });
            }).ContinueWith(_ =>
            {
                this.activityIndicator.StopAnimating();
            });
        }

        partial void CancelBarButton_Activated(UIBarButtonItem sender)
        {
            this.DismissViewControllerAsync(true);
        }

        class ResetPasswordResponseViewDelegate : UIAlertViewDelegate
        {
            private readonly UIViewController _controller;

            public ResetPasswordResponseViewDelegate(UIViewController controller)
            {
                this._controller = controller;
            }

            public override void Clicked(UIAlertView alertview, nint buttonIndex)
            {
                _controller.DismissViewControllerAsync(true);
            }
        }
    }
}
