using System;
using UIKit;

namespace ConsultantApp.iOS.TimeEntryViewController.AddTimeEntryViewController
{
	partial class AddTimeEntryViewController : UIViewController
	{
		private UIPickerView clientPickerView;
		private UIPickerView projectCodePickerView;
		private PickerViewModel pickerViewModel;

		public AddTimeEntryViewController (IntPtr handle) : base (handle)
		{
			clientPickerView = new UIPickerView (new CoreGraphics.CGRect(0, 0, 300, 300));
			projectCodePickerView = new UIPickerView (new CoreGraphics.CGRect(0, 0, 300, 300));
			pickerViewModel = new PickerViewModel ();
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			addButton.TouchUpInside += delegate {
				//DismissViewController(true, null);
				NavigationController.PopViewController(true);
			};

			clientTextField.InputView = clientPickerView;
			projectCodeTextField.InputView = projectCodePickerView;

			clientPickerView.Model = pickerViewModel;
			projectCodePickerView.Model = pickerViewModel;
		}

		//dismiss keyboard when tapping outside of text fields
		public override void TouchesBegan(NSSet touches, UIEvent evt)
		{
			base.TouchesBegan(touches, evt);

			clientTextField.ResignFirstResponder();
			projectCodeTextField.ResignFirstResponder();
			hoursTextField.ResignFirstResponder();
		}

		public class PickerViewModel : UIPickerViewModel
		{
			public override nint GetComponentCount (UIPickerView picker)
			{
				return 1;
			}

			public override nint GetRowsInComponent (UIPickerView picker, nint component)
			{
				return 5;
			}

			public override string GetTitle (UIPickerView pickerView, nint row, nint component)
			{

				return "Component " + row.ToString();
			}
		}
	}
}
