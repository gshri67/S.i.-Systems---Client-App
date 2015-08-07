using System;
using UIKit;
using Foundation;
using System.Collections.Generic;
using ConsultantApp.Core.ViewModels;
using SiSystems.ClientApp.SharedModels;
using System.Linq;

namespace ConsultantApp.iOS.TimeEntryViewController.AddTimeEntryViewController
{
	partial class AddTimeEntryViewController : UIViewController
	{
		public static UIPickerView clientPickerView;
		public static UIPickerView projectCodePickerView;
		private PickerViewModel pickerViewModel;

		private ClientViewModel clientViewModel;

		public static List<Client> clients;
		public static List<ProjectCode> projectCodes;

		//must be set so time entry can be saved
		public DateTime currentDate;

		public AddTimeEntryViewController (IntPtr handle) : base (handle)
		{
			clientViewModel = new ClientViewModel ();
			clients = clientViewModel.loadClientsForConsultant (null);

			clientPickerView = new UIPickerView (new CoreGraphics.CGRect(0, 0, 300, 300));
			projectCodePickerView = new UIPickerView (new CoreGraphics.CGRect(0, 0, 300, 300));
			pickerViewModel = new PickerViewModel ();
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			addButton.TouchUpInside += delegate {
				//DismissViewController(true, null);

				//save time entry into timesheet on "currentDate"

				NavigationController.PopViewController(true);
			};

			clientTextField.InputView = clientPickerView;
			projectCodeTextField.InputView = projectCodePickerView;
			projectCodeTextField.UserInteractionEnabled = false;

			pickerViewModel.onSelected = (UIPickerView pickerView, nint row) => 
			{
				if( pickerView == clientPickerView )
					clientTextField.Text = clients.ElementAt((int)row).name;
				else
					projectCodeTextField.Text = projectCodes.ElementAt((int)row).code;

				projectCodeTextField.UserInteractionEnabled = true;
			};

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
			public delegate void pickerViewDelegate( UIPickerView picker, nint row );
			public pickerViewDelegate onSelected;

			public override nint GetComponentCount (UIPickerView picker)
			{
				return 1;
			}

			public override nint GetRowsInComponent (UIPickerView picker, nint component)
			{
				if (picker == clientPickerView)
					return clients.Count;
				else
					return projectCodes.Count;
			}

			public override string GetTitle (UIPickerView pickerView, nint row, nint component)
			{
				if (pickerView == clientPickerView)
					return clients.ElementAt ((int)row).name;
				else
					return projectCodes.ElementAt ((int)row).code;
			}

			public override void Selected (UIPickerView pickerView, nint row, nint component)
			{
				//projectcodes should be updated for selected client
				if (pickerView == clientPickerView) 
				{
					projectCodes = clients.ElementAt ((int)row).projectCodes;
					projectCodePickerView.ReloadAllComponents ();
				}

				onSelected(pickerView, row);
			}
		}
	}
}
