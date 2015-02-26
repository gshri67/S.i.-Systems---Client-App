using Foundation;
using System;
using System.CodeDom.Compiler;
using System.Threading.Tasks;
using ClientApp.ViewModels;
using SiSystems.ClientApp.SharedModels;
using Microsoft.Practices.Unity;
using UIKit;

namespace ClientApp.iOS
{
	partial class ContactAlumniViewController : UIViewController
	{
	    private MessageViewModel _viewModel;
        public Consultant Consultant { get; set; }
        
        public ContactAlumniViewController (IntPtr handle) : base (handle)
        {
            _viewModel = DependencyResolver.Current.Resolve<MessageViewModel>();
        }

	    public override void ViewDidLoad()
	    {
	        base.ViewDidLoad();

            var submitButton = new UIBarButtonItem { Title = "Submit", TintColor = StyleGuideConstants.RedUiColor};
            submitButton.Clicked += (sender, args) =>
                {
                    var view = new UIAlertView("Confirm", "Are you sure you want to send this email?", null, "Cancel", "Ok");
                    view.Clicked += (o, eventArgs) =>
                        {
                            if (eventArgs.ButtonIndex == 1)
                            {
                                InvokeOnMainThread(() =>
                                    {
                                        _viewModel.Message = new ConsultantMessage() { ConsultantId = Consultant.Id, Text = EmailTextField.Text };
                                        Task.Factory.StartNew(() => SendMessage());
                                    });
                            }
                        };
                    view.Show();
                };
            NavigationItem.SetRightBarButtonItem(submitButton, false);
	    }

	    private async Task SendMessage()
	    {
            //TODO display sending message indicator
	        var result = await _viewModel.SendMessage();
	        if (result)
	        {
	            //TODO hide indicator
                NavigationController.PopViewController(true);
	        }
	        else
	        {
                //TODO hide indicator
	            var view = new UIAlertView("Error", "An error has occurred while attempting to send the email. Please try again.", null, "Ok");
                view.Show();
	        }
	    }
	}
}
