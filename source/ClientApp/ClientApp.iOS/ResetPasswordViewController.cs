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

        async partial void SubmitButton_TouchUpInside(UIButton sender)
        {
            await this.ResetPassword();
        }

        private async Task ResetPassword()
        {
            this.activityIndicator.StartAnimating();

            this.emailAddressField.Enabled = false;
            this.emailAddressField.BackgroundColor = StyleGuideConstants.LightGrayUiColor;

            this.SubmitButton.Enabled = false;

            var alertViewResponseDelegate = new ResetPasswordResponseViewDelegate(this);
            UIAlertView responseAlertView = new UIAlertView("Your password has been reset.", null, alertViewResponseDelegate, "Ok");
            var isSuccess = await this.viewModel.ResetPassword();
            if (!isSuccess)
            {
                responseAlertView.Title = "Your password has not been reset.";
                responseAlertView.Message = "Please contact your AE for more information.";
            }
            responseAlertView.Show();

            this.activityIndicator.StopAnimating();
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
