using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClientApp.iOS.Startup;
using ClientApp.ViewModels;
using CoreGraphics;
using Foundation;
using Microsoft.Practices.Unity;
using SiSystems.ClientApp.SharedModels;
using UIKit;

namespace ClientApp.iOS
{
	partial class NewContractViewController : UITableViewController
	{
	    private readonly UIDatePicker _startDatePicker = new UIDatePicker {Mode = UIDatePickerMode.Date, Hidden = true};
	    private readonly UIDatePicker _endDatePicker = new UIDatePicker {Mode = UIDatePickerMode.Date, Hidden = true};
	    private UITextField _timeSheetEmailField;
	    private UILabel _timeSheetDomainLabel;
	    private UITextField _contractEmailField;
	    private UILabel _contractDomainLabel;
        private readonly NewContractViewModel _viewModel;
        public Consultant Consultant { set { _viewModel.Consultant = value; } }

        public NewContractViewController (IntPtr handle) : base (handle)
        {
            _viewModel = DependencyResolver.Current.Resolve<NewContractViewModel>();
        }

	    public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
	    {
            base.PrepareForSegue(segue, sender);

            if (segue.Identifier == "SubmitSelected")
            {
                var view = (ConfirmOnboardViewController)segue.DestinationViewController;
                view.ViewModel = _viewModel;
            }
	    }

        #region Setup
        public override void ViewDidLoad()
	    {
	        base.ViewDidLoad();

            TitleField.ShouldReturn += SetupCloseKeyboard;

	        SetupNavigationHeader();

	        SetupApproverEmails();

            SetupDatePicker(_startDatePicker, StartDateLabel, DateTime.Now.Date.AddDays(1), true);
            SetupDatePicker(_endDatePicker, EndDateLabel, DateTime.Now.Date.AddDays(1).AddMonths(6), false);

            //Load initial values
            StartDateLabel.Text = DateTime.Now.Date.AddDays(1).ToString("MMM dd, yyyy");
	        EndDateLabel.Text = DateTime.Now.Date.AddDays(1).AddMonths(6).ToString("MMM dd, yyyy");

	        NameLabel.Text = _viewModel.Consultant.FullName;
            
	        RateField.EditingDidEnd += (sender, args) => ValidateRateField();
            AddToolBarToKeyboard(RateField);
	    }

	    private static bool SetupCloseKeyboard(UITextField field)
	    {
	        field.ResignFirstResponder();
	        return true;
	    }

	    private void SetupApproverEmails()
	    {
            //Programatically add the email objects because Storyboard was refusing to resize the text fields properly
            _timeSheetDomainLabel = new UILabel();
            _timeSheetEmailField = new UITextField { ReturnKeyType = UIReturnKeyType.Done };
            TimeSheetCell.Add(_timeSheetEmailField);
            TimeSheetCell.Add(_timeSheetDomainLabel);
            _contractDomainLabel = new UILabel();
            _contractEmailField = new UITextField { ReturnKeyType = UIReturnKeyType.Done };
            ContractCell.Add(_contractDomainLabel);
            ContractCell.Add(_contractEmailField);

            _timeSheetEmailField.Text = CurrentUser.User;
            _timeSheetDomainLabel.Text = CurrentUser.Domain;
            _contractEmailField.Text = CurrentUser.User;
            _contractDomainLabel.Text = CurrentUser.Domain;
            var size = _timeSheetDomainLabel.IntrinsicContentSize;
            _timeSheetDomainLabel.Frame = new CGRect(UIScreen.MainScreen.Bounds.Width - size.Width - 20, 11f, size.Width, size.Height);
            _timeSheetEmailField.Frame = new CGRect(135f, 0, UIScreen.MainScreen.Bounds.Width - 155f - size.Width, 44f);
            _contractDomainLabel.Frame = new CGRect(UIScreen.MainScreen.Bounds.Width - size.Width - 20, 11f, size.Width, size.Height);
            _contractEmailField.Frame = new CGRect(135f, 0, UIScreen.MainScreen.Bounds.Width - 155f - size.Width, 44f);

            _timeSheetEmailField.ShouldReturn += SetupCloseKeyboard;
            _contractEmailField.ShouldReturn += SetupCloseKeyboard;
	    }

	    private void SetupNavigationHeader()
	    {
	        var cancelButton = new UIBarButtonItem {Title = "Cancel"};
	        var submitButton = new UIBarButtonItem {Title = "Next", TintColor = StyleGuideConstants.RedUiColor};
	        cancelButton.Clicked += (sender, args) => { NavigationController.DismissModalViewController(true); };
	        submitButton.Clicked += (sender, args) =>
	                                {
	                                    _viewModel.ContractTitle = TitleField.Text.Trim();
                                        _viewModel.TimesheetApprovalEmail = _timeSheetEmailField.Text.Trim() + _timeSheetDomainLabel.Text;
                                        _viewModel.ContractApprovalEmail = _contractEmailField.Text.Trim() + _contractDomainLabel.Text;
	                                    ValidateRateField();
                                        var result = _viewModel.Validate();
	                                    InvokeOnMainThread(delegate
	                                                       {
	                                                           if (result.IsValid)
	                                                           {
	                                                               PerformSegue("SubmitSelected", submitButton);
	                                                           }
	                                                           else
	                                                           {
	                                                               var view = new UIAlertView("Error", result.Message, null, "Ok");
	                                                               view.Show();
	                                                           }
	                                                       });
	                                };
	        NavigationItem.SetLeftBarButtonItem(cancelButton, false);
	        NavigationItem.SetRightBarButtonItem(submitButton, false);
	    }

	    private void SetupDatePicker(UIDatePicker picker, UILabel label, DateTime setDate, bool isStartDate)
	    {
            label.Text = setDate.ToString("MMM dd, yyyy");
            picker.SetDate(DateTimeToNSDate(setDate), false);
            picker.MinimumDate = DateTimeToNSDate(DateTime.Now.Date);
            picker.ValueChanged += (sender, args) =>
            {
                if (isStartDate)
                {
                    _viewModel.StartDate = NSDateToDateTime(picker.Date);
                    label.Text = _viewModel.StartDate.ToString("MMM dd, yyyy");
                }
                else
                {
                    _viewModel.EndDate = NSDateToDateTime(picker.Date);
                    label.Text = _viewModel.EndDate.ToString("MMM dd, yyyy");
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
	            (sender, args) => 
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
            doneButton.TintColor = StyleGuideConstants.RedUiColor;
	        toolbar.Items = new[] {space, doneButton};

	        field.InputAccessoryView = toolbar;
	    }

        private bool ValidateRateField()
        {
            if (string.IsNullOrEmpty(RateField.Text))
            {
                _viewModel.ContractorRate = 0;
                return true;
            }

            decimal val;
            var result = decimal.TryParse(RateField.Text, out val);
            if(result)
            {
                _viewModel.ContractorRate = val;
                RateField.Text = string.Format("{0:N2}", val);
                NewContractTable.ReloadData();
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
            //Dates Section
            if (indexPath.Section == 2)
            {
                if (indexPath.Row == 0)
                {
                    var shouldDisplayPicker = StartDateCell.Frame.Height == 0;
                    NewContractTable.DeselectRow(indexPath, true);
                    NewContractTable.BeginUpdates();
                    StartDateCell.SendSubviewToBack(NewContractTable);
                    StartDateCell.Frame = new CGRect(StartDateCell.Frame.X, StartDateCell.Frame.Y, StartDateCell.Frame.Width, shouldDisplayPicker ? 216 : 0);
                    _startDatePicker.Hidden = !shouldDisplayPicker;
                    NewContractTable.EndUpdates();
                }
                else if (indexPath.Row == 2)
                {
                    var shouldDisplayPicker = EndDateCell.Frame.Height == 0;
                    NewContractTable.DeselectRow(indexPath, true);
                    NewContractTable.BeginUpdates();
                    EndDateCell.SendSubviewToBack(NewContractTable);
                    EndDateCell.Frame = new CGRect(EndDateCell.Frame.X, EndDateCell.Frame.Y, EndDateCell.Frame.Width, shouldDisplayPicker ? 216 : 0);
                    _endDatePicker.Hidden = !shouldDisplayPicker;
                    NewContractTable.EndUpdates();
                }

            }
        }

	    public override UIView GetViewForFooter(UITableView tableView, nint section)
	    {
	        if (section == 1)
	        {
	            var view = new UIView();
	            var label = new UILabel(new CGRect(20, 4, UIScreen.MainScreen.Bounds.Width - 40, 10))
	                        {
                                Font = UIFont.SystemFontOfSize(10f),
                                TextColor = StyleGuideConstants.MediumGrayUiColor,
                                TextAlignment = UITextAlignment.Right,
                                Text = string.Format("+ Service Fee (${0}/hr) = ${1}", NewContractViewModel.ServiceRate, _viewModel.TotalRate)
	                        };

                view.Add(label);
	            return view;
	        }
            return base.GetViewForFooter(tableView, section);
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
            return string.Format("${0,6:N2}/hr", rate);
	    }

	    #endregion
    }
}
