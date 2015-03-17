using System;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
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
	    private UITextView _emailTextField;
        public Consultant Consultant { private get; set; }
        public string InitialEmailText { private get; set; }

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
                    if (string.IsNullOrEmpty(_emailTextField.Text.Trim()))
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
                                        _viewModel.Message = new ConsultantMessage() { ConsultantId = Consultant.Id, Text = _emailTextField.Text };
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

	        var startOfViewY = UIApplication.SharedApplication.StatusBarFrame.Height +
	                           NavigationController.NavigationBar.Frame.Height;
	        _emailTextField =
	            new UITextView(new CGRect(20, startOfViewY+11, UIScreen.MainScreen.Bounds.Width - 40,
	                UIScreen.MainScreen.Bounds.Height - 22))
	            {
	                Editable = true, 
                    AlwaysBounceVertical = true,
                    Font = UIFont.SystemFontOfSize(14)
	            };
	        View.Add(_emailTextField);

            //resize text field when keyboard appears
	        NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillShowNotification,
	            notification =>
	            {
	                _emailTextField.Frame = new CGRect(_emailTextField.Frame.X, _emailTextField.Frame.Y,
	                    _emailTextField.Frame.Width,
                        UIScreen.MainScreen.Bounds.Height - UIKeyboard.FrameEndFromNotification(notification).Height - startOfViewY - 22);
	            });

	        _emailTextField.Text = InitialEmailText;

            //resign is necessary for sending multiple emails cause the above code will not fire the 2nd time you open the screen
	        _emailTextField.ResignFirstResponder();
	        _emailTextField.BecomeFirstResponder();

	        Title = ScreenTitle;
	    }

        private async Task SendMessage()
        {
            try
            {
                await _viewModel.SendMessage();
                var successDelegate = new ContactAlumniDelegate(this);
                InvokeOnMainThread(() =>
                {
                    _overlay.Hide();
                    var view = new UIAlertView("Success",
                        "The message was succesfully sent", successDelegate, "Ok");
                    view.Show();
                });
            }
            catch(Exception)
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
