using Foundation;
using System;
using System.CodeDom.Compiler;
using System.Globalization;
using System.Linq;
using ClientApp.iOS.Alumni.ConsultantDetails;
using ClientApp.ViewModels;
using CoreGraphics;
using ObjCRuntime;
using SiSystems.ClientApp.SharedModels;
using UIKit;

namespace ClientApp.iOS
{
	partial class NewContractViewController : UITableViewController
	{
	    private readonly UIDatePicker _startDatePicker = new UIDatePicker {Mode = UIDatePickerMode.Date, Hidden =  true};
        private readonly UIDatePicker _endDatePicker = new UIDatePicker { Mode = UIDatePickerMode.Date, Hidden = true };
        private readonly NewContractViewModel _viewModel;
        public Consultant Consultant { set { _viewModel.Consultant = value; } }

        public NewContractViewController (IntPtr handle) : base (handle)
		{
            _viewModel = new NewContractViewModel();
		}

        #region Setup
        public override void ViewDidLoad()
	    {
	        base.ViewDidLoad();

	        SetupNavigationHeader();

	        SetupApproverEmail();

	        SetupDatePicker(_startDatePicker, StartDateLabel, DateTime.Now.Date, true);
	        SetupDatePicker(_endDatePicker, EndDateLabel, DateTime.Now.Date, false);

            //Load initial values
            StartDateLabel.Text = DateTime.Now.Date.ToString("d");
	        EndDateLabel.Text = DateTime.Now.Date.ToString("d");

	        NameLabel.Text = _viewModel.Consultant.FullName;
	        RateField.Text = string.Format("{0:N2}", _viewModel.ContractorRate);
	        ServiceLabel.Text = ToRateString(NewContractViewModel.ServiceRate);
	        TotalLabel.Text = ToRateString(_viewModel.TotalRate);

            AddToolBarToKeyboard(RateField);
	    }

	    private void SetupApproverEmail()
	    {
	        ApproverEmailField.ValueChanged += (sender, args) => { _viewModel.ApproverEmail = ApproverEmailField.Text; };
	        ApproverEmailField.ShouldReturn += field =>
	                                           {
	                                               _viewModel.ApproverEmail = field.Text;
	                                               if (string.IsNullOrEmpty(field.Text) || _viewModel.ValidateEmailAddress())
	                                               {
	                                                   ApproverEmailField.ResignFirstResponder();
	                                               }
	                                               return true;
	                                           };
	    }

	    private void SetupNavigationHeader()
	    {
	        var cancelButton = new UIBarButtonItem {Title = "Cancel"};
	        var submitButton = new UIBarButtonItem {Title = "Submit", TintColor = StyleGuideConstants.RedUiColor};
	        cancelButton.Clicked += (sender, args) => { NavigationController.DismissModalViewController(true); };
	        submitButton.Clicked += (sender, args) =>
	                                {
	                                    var result = _viewModel.Validate();
	                                    InvokeOnMainThread(delegate
	                                                       {
	                                                           if (result.IsValid)
	                                                           {
	                                                               PerformSegue("SubmitSelected", submitButton);
	                                                           }
	                                                           else
	                                                           {
	                                                               var view = new UIAlertView("Error", result.Message, null,
	                                                                   "Ok");
	                                                               view.Show();
	                                                           }
	                                                       });
	                                };
	        NavigationItem.SetLeftBarButtonItem(cancelButton, false);
	        NavigationItem.SetRightBarButtonItem(submitButton, false);
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
                    label.Text = _viewModel.StartDate.ToString("d");
                }
                else
                {
                    _viewModel.EndDate = NSDateToDateTime(picker.Date);
                    label.Text = _viewModel.EndDate.ToString("d");
                }
            };

	        if (isStartDate)
	        {
	            _viewModel.StartDate = setDate;
                StartDateCell.Add(_startDatePicker);
	        }
	        else
	        {
	            _viewModel.EndDate = setDate;
                EndDateCell.Add(_endDatePicker);
	        }
	    }
        #endregion

        #region Rate Keyboard
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
                RateField.Text = string.Format("{0:N2}", val);
                TotalLabel.Text = ToRateString(_viewModel.TotalRate);
            }
            return result;
        }
        #endregion

        #region Table Delegates
	    public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
	    {
	        if (indexPath.Section == 2 && indexPath.Row == 1)
	        {
	            return StartDateCell.Frame.Height;
	        }
            if (indexPath.Section == 2 && indexPath.Row == 3)
	        {
	            return EndDateCell.Frame.Height;
	        }
            return base.GetHeightForRow(tableView, indexPath);
	    }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            if (indexPath.Section == 2)
            {
                if (indexPath.Row == 0)
                {
                    var shouldDisplayPicker = StartDateCell.Frame.Height == 0;
                    NewContractTable.DeselectRow(indexPath, true);
                    NewContractTable.BeginUpdates();
                    StartDateCell.Frame = new CGRect(0, 0, StartDateCell.Frame.Width, shouldDisplayPicker ? 216 : 0);
                    _startDatePicker.Hidden = !shouldDisplayPicker;
                    NewContractTable.EndUpdates();
                }
                else if (indexPath.Row == 2)
                {
                    var shouldDisplayPicker = EndDateCell.Frame.Height == 0;
                    NewContractTable.DeselectRow(indexPath, true);
                    NewContractTable.BeginUpdates();
                    EndDateCell.Frame = new CGRect(0, 0, EndDateCell.Frame.Width, shouldDisplayPicker ? 216 : 0);
                    _endDatePicker.Hidden = !shouldDisplayPicker;
                    NewContractTable.EndUpdates();
                }

            }
        }

	    #endregion

        #region Data Helpers
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

	    private static string ToRateString(decimal rate)
	    {
	        return string.Format("$ {0:N2} / hr", rate);
	    }

	    #endregion
    }
}
