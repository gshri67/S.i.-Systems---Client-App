using System;
using System.Linq;
using ClientApp.Core.ViewModels;
using ClientApp.Core;
using CoreGraphics;
using Foundation;
using Microsoft.Practices.Unity;
using ObjCRuntime;
using SiSystems.ClientApp.SharedModels;
using UIKit;

namespace ClientApp.iOS
{
	partial class OnboardViewController : UITableViewController
	{
	    private readonly UIDatePicker _startDatePicker = new UIDatePicker {Mode = UIDatePickerMode.Date, Hidden = true};
	    private readonly UIDatePicker _endDatePicker = new UIDatePicker {Mode = UIDatePickerMode.Date, Hidden = true};
        private readonly OnboardViewModel _viewModel;
        public Consultant Consultant { set { _viewModel.Consultant = value; } }
        public bool IsActiveConsultant { set { _viewModel.IsActiveConsultant = value; } }

        public OnboardViewController (IntPtr handle) : base (handle)
        {
            _viewModel = DependencyResolver.Current.Resolve<OnboardViewModel>();
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
            TimesheetApproverField.Text = CurrentUser.Email;
            TimesheetApproverField.Placeholder = CurrentUser.Domain;
            ContractApproverField.Text = CurrentUser.Email;
            ContractApproverField.Placeholder = CurrentUser.Domain;

            SetupDatePicker(_startDatePicker, StartDateLabel, DateTime.Now.Date.AddDays(1), true);
            SetupDatePicker(_endDatePicker, EndDateLabel, DateTime.Now.Date.AddDays(1).AddMonths(6), false);

            //Load initial values
            if (_viewModel.IsActiveConsultant)
            {
                Title = "Renew";

                var startdate = _viewModel.LastContractEndDate.AddDays(1);
                var enddate = _viewModel.LastContractEndDate.AddDays(1).AddMonths(6);
                StartDateLabel.Text = startdate.ToString("MMM dd, yyyy");
                EndDateLabel.Text = enddate.ToString("MMM dd, yyyy");
                SetupDatePicker(_startDatePicker, StartDateLabel, startdate, true);
                SetupDatePicker(_endDatePicker, EndDateLabel, enddate, false);

                TitleField.Text = _viewModel.LastContractTitle;
                if (!_viewModel.IsFullySourced)
                {
                    RateField.Text = RateField.Text = string.Format("${0:N2}/hr", _viewModel.GetPayRateFromBillRate(_viewModel.LastContractRate));
                    _viewModel.ContractorRate = _viewModel.GetPayRateFromBillRate(_viewModel.LastContractRate);
                }
                else
                {
                    RateField.Text = RateField.Text = string.Format("${0:N2}/hr", _viewModel.LastContractRate);
                    _viewModel.ContractorRate = _viewModel.LastContractRate;
                }
                
            }
            else
            {
                StartDateLabel.Text = DateTime.Now.Date.AddDays(1).ToString("MMM dd, yyyy");
                EndDateLabel.Text = DateTime.Now.Date.AddDays(1).AddMonths(6).ToString("MMM dd, yyyy");
                SetupDatePicker(_startDatePicker, StartDateLabel, DateTime.Now.Date.AddDays(1), true);
                SetupDatePicker(_endDatePicker, EndDateLabel, DateTime.Now.Date.AddDays(1).AddMonths(6), false);
            }

            if (_viewModel.IsFullySourced)
            {
                RateLabel.Text = "Bill Rate";
            }

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
            TimesheetApproverField.EditingChanged += AppendDomainSetCursor;
	        TimesheetApproverField.EditingDidBegin += (sender, args) => { TimesheetApproverField.PerformSelector(new Selector("selectAll"), null, 0f); };
            ContractApproverField.EditingChanged += AppendDomainSetCursor;
            ContractApproverField.EditingDidBegin += (sender, args) => { ContractApproverField.PerformSelector(new Selector("selectAll"), null, 0f); };
            TimesheetApproverField.ShouldReturn += SetupCloseKeyboard;
            ContractApproverField.ShouldReturn += SetupCloseKeyboard;
            TimesheetApproverField.EditingDidEnd += ValidateEmailField;
            ContractApproverField.EditingDidEnd += ValidateEmailField;
	    }

	    private void ValidateEmailField(object sender, EventArgs eventArgs)
	    {
	        var field = (UITextField) sender;
	        if (!field.Text.Contains("@"))
	        {
	            field.Text = field.Text + CurrentUser.Domain;
	        }
	        if (!_viewModel.ValidateEmailAddress(field.Text))
	        {
	            field.Text = string.Empty;
	        }
	    }

        private void AppendDomainSetCursor(object sender, EventArgs eventArgs)
	    {
            var field = (UITextField)sender;
            if (!field.Text.EndsWith(CurrentUser.Domain) && !field.Text.Contains("@"))
            {
                field.Text = field.Text + CurrentUser.Domain;
                var position = field.GetPosition(field.EndOfDocument, -CurrentUser.Domain.Length);
                field.SelectedTextRange = field.GetTextRange(position, position);
            }
	    }

	    private void SetupNavigationHeader()
	    {
	        var cancelButton = new UIBarButtonItem {Title = "Cancel"};
	        var submitButton = new UIBarButtonItem {Title = "Next", TintColor = StyleGuideConstants.RedUiColor};
	        cancelButton.Clicked += (sender, args) => { NavigationController.DismissModalViewController(true); };
	        submitButton.Clicked += (sender, args) =>
	                                {
	                                    _viewModel.ContractTitle = TitleField.Text.Trim();
                                        _viewModel.TimesheetApprovalEmail = TimesheetApproverField.Text.Trim();
                                        _viewModel.ContractApprovalEmail = ContractApproverField.Text.Trim();
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
            picker.TimeZone = new NSTimeZone("UTC");
            picker.SetDate(DateTimeToNSDate(setDate), false);
            picker.MinimumDate = DateTimeToNSDate(DateTime.Now.Date.AddDays(1));
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
                RateField.Text = string.Format("${0:N2}/hr", val);
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
                RateField.ResignFirstResponder();
            }
        }

	    public override UIView GetViewForFooter(UITableView tableView, nint section)
	    {
	        if (section == 1 && !_viewModel.IsFullySourced)
	        {
	            var view = new UIView();
	            var footerStrings = _viewModel.GetRateFooter();
	            var y = 4;
	            foreach (var footerString in footerStrings)
	            {
                    var label = new UILabel(new CGRect(20, y, UIScreen.MainScreen.Bounds.Width - 40, 10))
                    {
                        Font = UIFont.SystemFontOfSize(10f),
                        TextColor = StyleGuideConstants.MediumGrayUiColor,
                        TextAlignment = UITextAlignment.Right,
                        Text = footerString
                    };
	                y += 12;
                    view.Add(label);
	            }
	            
	            return view;
	        }
            return base.GetViewForFooter(tableView, section);
	    }

	    public override UIView GetViewForHeader(UITableView tableView, nint section)
	    {
	        if (section == 1 && !_viewModel.IsActiveConsultant)
	        {
                var view = new UIView();
                var leftLabel = new UILabel(new CGRect(17, 18, 140, 13))
                {
                    Font = UIFont.SystemFontOfSize(13f),
                    TextColor = UIColor.ScrollViewTexturedBackgroundColor,
                    Text = "RATE"
                };
                var rightLabel = new UILabel(new CGRect(160, 21, UIScreen.MainScreen.Bounds.Width - 180, 10))
                {
                    Font = UIFont.SystemFontOfSize(10f),
                    TextColor = StyleGuideConstants.MediumGrayUiColor,
                    TextAlignment = UITextAlignment.Right,
                    Text = _viewModel.GetRateHeader()
                };
                view.Add(leftLabel);
                view.Add(rightLabel);
                return view;
	        }
            return base.GetViewForHeader(tableView, section);
	    }

	    public override string TitleForHeader(UITableView tableView, nint section)
	    {
	        if (section == 0 && _viewModel.IsActiveConsultant)
	        {
	            return "Renew Details";
	        }
            return base.TitleForHeader(tableView, section);
	    }

	    #endregion

        #region Data Helpers
        private static DateTime NSDateToDateTime(NSDate date)
        {
            DateTime reference = new DateTime(2001, 1, 1, 0, 0, 0);
            return reference.AddSeconds(date.SecondsSinceReferenceDate).Date;
        }

        private static NSDate DateTimeToNSDate(DateTime date)
        {
            if (date.Kind == DateTimeKind.Unspecified)
                date = DateTime.SpecifyKind(date, DateTimeKind.Local);
            return (NSDate) date;
        }
	    #endregion
    }
}
