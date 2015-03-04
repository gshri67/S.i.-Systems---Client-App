using System;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using SiSystems.ClientApp.SharedModels;
using UIKit;
using ClientApp.Core.ViewModels;

namespace ClientApp.iOS
{
	partial class ContactAlumniViewController : UIViewController
	{
	    private MessageViewModel _viewModel;
	    private LoadingOverlay _overlay;
        public Consultant Consultant { get; set; }
        
        private string ScreenTitle
        {
            get { return Consultant==null ? "Contact" : string.Format("Contact {0}", Consultant.FirstName); }
        }
        
        public ContactAlumniViewController (IntPtr handle) : base (handle)
        {
            _viewModel = DependencyResolver.Current.Resolve<MessageViewModel>();
        }

	    public override void ViewDidLoad()
	    {
	        base.ViewDidLoad();

            var cancelButton = new UIBarButtonItem { Title = "Cancel" };
            var submitButton = new UIBarButtonItem { Title = "Send", TintColor = StyleGuideConstants.RedUiColor};
            cancelButton.Clicked += (sender, args) => { NavigationController.DismissModalViewController(true); };
            submitButton.Clicked += (sender, args) =>
                {
                    if (string.IsNullOrEmpty(EmailTextField.Text.Trim()))
                    {
                        var error = new UIAlertView("Error", "You must enter some text before sending the email.", null, "Ok");
                        error.Show();
                        return;
                    }
                    var view = new UIAlertView("Confirm", "Are you sure you want to send this email?", null, "Cancel", "Ok");
                    view.Clicked += (o, eventArgs) =>
                        {
                            if (eventArgs.ButtonIndex == 1)
                            {
                                InvokeOnMainThread(() =>
                                    {
                                        _viewModel.Message = new ConsultantMessage() { ConsultantId = Consultant.Id, Text = EmailTextField.Text };
                                        _overlay = new LoadingOverlay(UIScreen.MainScreen.Bounds);
                                        View.Add(_overlay);
                                        Task.Factory.StartNew(() => SendMessage());
                                    });
                            }
                        };
                    view.Show();
                };
            NavigationItem.SetLeftBarButtonItem(cancelButton, false);
            NavigationItem.SetRightBarButtonItem(submitButton, false);
	        EmailTextField.BecomeFirstResponder();

	        Title = ScreenTitle;
	    }

	    private async Task SendMessage()
	    {
            try
	        {
	            InvokeOnMainThread(() =>
	                               {
	                                   _overlay.Hide();
                                       var view = new UIAlertView("Success",
                                           "The message was succesfully sent", successDelegate, "Ok");
                                       view.Show();
	                               });
	        }
            catch (Exception)
	        {
	            InvokeOnMainThread(() =>
	                               {
	                                   _overlay.Hide();
	                                   var view = new UIAlertView("Error",
	                                       "An error has occurred while attempting to send the email. Please try again.",
	                                       null, "Ok");
	                                   view.Show();
	                               });
	        }
	    }
        
        class ContactAlumniDelegate : UIAlertViewDelegate
        {
            private readonly UIViewController _controller;

            public ContactAlumniDelegate(UIViewController controller)
            {
                _controller = controller;
            }

            public override void Clicked(UIAlertView alertview, nint buttonIndex)
            {
                _controller.DismissViewController(true, null);
            }
        }
	}
}
