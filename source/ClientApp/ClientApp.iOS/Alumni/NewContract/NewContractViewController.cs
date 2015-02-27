using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly UIPickerView _specPicker = new UIPickerView{Hidden = true};
        private readonly NewContractViewModel _viewModel;
        public Consultant Consultant { set { _viewModel.Consultant = value; } }

        public NewContractViewController (IntPtr handle) : base (handle)
        {
            _viewModel = DependencyResolver.Current.Resolve<NewContractViewModel>();
        }

	    private void SetSpecialization(Specialization specialization)
	    {
	        _viewModel.Specialization = specialization;
            InvokeOnMainThread(() => { SpecializationLabel.Text = specialization.Name; });
	    }

	    public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
	    {
            base.PrepareForSegue(segue, sender);

            if (segue.Identifier == "SubmitSelected")
            {
                var view = (ConfirmContractViewController)segue.DestinationViewController;
                view.ViewModel = _viewModel;
            }
	    }

        #region Setup
        public override void ViewDidLoad()
	    {
	        base.ViewDidLoad();

            TitleField.ShouldReturn += field =>
                                       {
                                           field.ResignFirstResponder();
                                           return true;
                                       };

	        SetupNavigationHeader();

	        SetupApproverEmail();

	        SetupDatePicker(_startDatePicker, StartDateLabel, DateTime.Now.Date, true);
	        SetupDatePicker(_endDatePicker, EndDateLabel, DateTime.Now.Date, false);

            //Load initial values
            StartDateLabel.Text = DateTime.Now.Date.ToString("MMM dd, yyyy");
	        EndDateLabel.Text = DateTime.Now.Date.ToString("MMM dd, yyyy");

	        NameLabel.Text = _viewModel.Consultant.FullName;
	        RateField.Text = string.Format("{0:N2}", _viewModel.ContractorRate);
	        ServiceLabel.Text = ToRateString(NewContractViewModel.ServiceRate);
	        TotalLabel.Text = ToRateString(_viewModel.TotalRate);

            RateField.EditingDidEnd += (sender, args) => ValidateRateField();
            AddToolBarToKeyboard(RateField);

            Task.Factory.StartNew(() => GetAllSpecializations());
	    }

	    private async Task GetAllSpecializations()
	    {
	        var specs = await _viewModel.GetAllSpecializations();
            var specModel = new SpecializationPickerModel(specs, _viewModel.Consultant.Specializations, this);
            InvokeOnMainThread(() =>
                               {
                                   _specPicker.Model = specModel;
                                   _specPicker.Select(0, 0, false);
                                   SpecializationCell.Add(_specPicker);                       
                               });
	    }

	    private void SetupApproverEmail()
	    {
	        ApproverEmailField.EditingDidEnd += (sender, args) => { _viewModel.ApproverEmail = ApproverEmailField.Text.Trim(); };
	        ApproverEmailField.ShouldReturn += field =>
	                                           {
	                                               _viewModel.ApproverEmail = field.Text.Trim();
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
	        var submitButton = new UIBarButtonItem {Title = "Next", TintColor = StyleGuideConstants.RedUiColor};
	        cancelButton.Clicked += (sender, args) => { NavigationController.DismissModalViewController(true); };
	        submitButton.Clicked += (sender, args) =>
	                                {
	                                    _viewModel.ContractTitle = TitleField.Text.Trim();
	                                    _viewModel.ApproverEmail = ApproverEmailField.Text.Trim();
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
            if (indexPath.Section == 0 && indexPath.Row == 2)
            {
                return SpecializationCell.Frame.Height;
            } 
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
            //Contract Details Section
            if (indexPath.Section == 0 && indexPath.Row == 1)
            {
                var shouldDisplayPicker = SpecializationCell.Frame.Height == 0;
                NewContractTable.DeselectRow(indexPath, true);
                NewContractTable.BeginUpdates();
                SpecializationCell.SendSubviewToBack(NewContractTable);
                SpecializationCell.Frame = new CGRect(SpecializationCell.Frame.X, SpecializationCell.Frame.Y, SpecializationCell.Frame.Width, shouldDisplayPicker ? 216 : 0);
                _specPicker.Hidden = !shouldDisplayPicker;
                NewContractTable.EndUpdates();

            }
            //Dates Section
            else if (indexPath.Section == 2)
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

	    private class SpecializationPickerModel : UIPickerViewModel
	    {
	        private readonly IList<Tuple<Specialization, bool>> _specs;
	        private NewContractViewController _controller;

	        public SpecializationPickerModel(IEnumerable<Specialization> specs, IEnumerable<Specialization> consultantsSpecs, NewContractViewController controller)
	        {
	            _specs = new List<Tuple<Specialization, bool>>();
                foreach (var spec in specs)
	            {
	                _specs.Add(new Tuple<Specialization, bool>(spec, consultantsSpecs.Any(s => s.Id == spec.Id)));
	            }
	            _specs = _specs.OrderByDescending(t => t.Item2).ThenBy(t => t.Item1.Name).ToList();
                _specs.Insert(0, new Tuple<Specialization, bool>(new Specialization{Name = ""}, false));
	            _controller = controller;
	        }

	        public override nint GetComponentCount(UIPickerView pickerView)
	        {
	            return 1;
	        }

	        public override nint GetRowsInComponent(UIPickerView pickerView, nint component)
	        {
	            return _specs.Count;
	        }

	        public override NSAttributedString GetAttributedTitle(UIPickerView pickerView, nint row, nint component)
	        {
	            return _specs[(int) row].Item2
	                ? new NSAttributedString(_specs[(int) row].Item1.Name,
	                    new UIStringAttributes {ForegroundColor = UIColor.Black})
	                : new NSAttributedString(_specs[(int) row].Item1.Name,
	                    new UIStringAttributes {ForegroundColor = UIColor.DarkGray});
	        }

	        public override void Selected(UIPickerView pickerView, nint row, nint component)
	        {
                _controller.SetSpecialization(_specs[(int)row].Item1);
	        }
	    }
    }
}
