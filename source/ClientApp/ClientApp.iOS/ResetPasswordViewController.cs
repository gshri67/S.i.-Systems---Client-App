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

        public void Init(string title, string emailAddress)
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
                this.ResetPassword().RunSynchronously();
                this.emailAddressField.ResignFirstResponder();
                return true;
            };
        }

        async partial void SubmitButton_TouchUpInside(UIButton sender)
        {
            await this.ResetPassword();
        }

        private async Task ResetPassword()
        {
            await this.viewModel.ResetPassword();
        }

        partial void CancelBarButton_Activated(UIBarButtonItem sender)
        {
            this.DismissViewControllerAsync(true);
        }
    }
}
