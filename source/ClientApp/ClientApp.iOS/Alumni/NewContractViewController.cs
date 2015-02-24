using Foundation;
using System;
using System.CodeDom.Compiler;
using System.Globalization;
using System.Linq;
using ClientApp.ViewModels;
using CoreGraphics;
using ObjCRuntime;
using SiSystems.ClientApp.SharedModels;
using UIKit;

namespace ClientApp.iOS
{
	partial class NewContractViewController : UITableViewController
	{
	    private readonly NewContractViewModel _viewModel;
        public Consultant Consultant { set { _viewModel.Consultant = value; } }

        public NewContractViewController (IntPtr handle) : base (handle)
		{
            _viewModel = new NewContractViewModel();
		}

	    public override void ViewDidLoad()
	    {
	        base.ViewDidLoad();

	        ApproverEmailField.ValueChanged += (sender, args) => { _viewModel.ApproverEmail = ApproverEmailField.Text; };
	        ApproverEmailField.ShouldReturn += field =>
	        {
	            _viewModel.ApproverEmail = field.Text;
                if (_viewModel.ValidateEmailAddress())
                {
                    ApproverEmailField.ResignFirstResponder();
                }
	            return true;
	        };
            //CancelButton.TouchUpInside += (sender, args) => { NavigationController.PopViewController(true); };
            //SubmitButton.TouchUpInside += (sender, args) =>
            //{
            //    var result = _viewModel.Validate();
            //    InvokeOnMainThread(delegate
            //    {
            //        if (result.IsValid)
            //        {
            //            PerformSegue("SubmitSelected", SubmitButton);
            //        }
            //        else
            //        {
            //            var view = new UIAlertView("Error", result.Message, null, "Ok");
            //            view.Show();
            //        }
            //    });
            //};

	        //SetupDatePicker(StartDatePicker, StartDateLabel, DateTime.Now.Date, true);
	        //SetupDatePicker(EndDatePicker, EndDateLabel, DateTime.Now.Date.AddYears(1), false);

	        StartDateLabel.Text = DateTime.Now.Date.ToString("D");
	        EndDateLabel.Text = DateTime.Now.Date.ToString("D");

	        NameLabel.Text = _viewModel.Consultant.FullName;
	        RateField.Text = string.Format("{0:N2}", _viewModel.ContractorRate);
	        ServiceLabel.Text = ToRateString(NewContractViewModel.ServiceRate);
	        TotalLabel.Text = ToRateString(_viewModel.TotalRate);

            AddToolBarToKeyboard(RateField);
	    }

	    private void SetupDatePicker(UIDatePicker picker, UILabel label, DateTime setDate, bool isStartDate)
	    {
            label.Text = setDate.ToString("D");
            picker.SetDate(DateTimeToNSDate(setDate), false);
            picker.MinimumDate = DateTimeToNSDate(DateTime.Now.Date);
            picker.ValueChanged += (sender, args) =>
            {
                if (isStartDate)
                {
                    _viewModel.StartDate = NSDateToDateTime(picker.Date);
                    label.Text = _viewModel.StartDate.ToString("D");
                }
                else
                {
                    _viewModel.EndDate = NSDateToDateTime(picker.Date);
                    label.Text = _viewModel.EndDate.ToString("D");
                }
            };

	        if (isStartDate)
	        {
	            _viewModel.StartDate = setDate;
	        }
	        else
	        {
	            _viewModel.EndDate = setDate;
	        }
	    }

	    private void AddToolBarToKeyboard(UITextField field)
	    {
	        var toolbar = new UIToolbar(new CGRect(0f, 0f, UIScreen.MainScreen.Bounds.Width, 44f));
	        var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done,
	            delegate
	            {
	                if (!ValidateRateField())
	                {
	                    RateField.Text = "";
	                }
	                else
	                {
	                    field.ResignFirstResponder();
	                }
	            });
	        var space = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);
	        toolbar.Items = new[] {space, doneButton};

	        field.InputAccessoryView = toolbar;
	    }

        private bool ValidateRateField()
        {
            if (string.IsNullOrEmpty(RateField.Text))
            {
                _viewModel.ContractorRate = 0;
                TotalLabel.Text = ToRateString(_viewModel.TotalRate);
                return true;
            }

            decimal val;
            var result = decimal.TryParse(RateField.Text, out val);
            if(result)
            {
                _viewModel.ContractorRate = val;
                TotalLabel.Text = ToRateString(_viewModel.TotalRate);
            }
            return result;
        }

	    private static string ToRateString(decimal rate)
	    {
	        return string.Format("$ {0:N2} / hr", rate);
	    }

        private static DateTime NSDateToDateTime(NSDate date)
        {
            DateTime reference = TimeZone.CurrentTimeZone.ToLocalTime(
                new DateTime(2001, 1, 1, 0, 0, 0));
            return reference.AddSeconds(date.SecondsSinceReferenceDate);
        }

        private static NSDate DateTimeToNSDate(DateTime date)
        {
            DateTime reference = TimeZone.CurrentTimeZone.ToLocalTime(
                new DateTime(2001, 1, 1, 0, 0, 0));
            return NSDate.FromTimeIntervalSinceReferenceDate(
                (date - reference).TotalSeconds);
        }
	}
}
