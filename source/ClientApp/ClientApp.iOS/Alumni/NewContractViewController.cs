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

            RateField.ValueChanged += RateFieldValueChanged;

	        NameLabel.Text = _viewModel.Consultant.FullName;
	        RateField.Text = string.Format("{0:N2}", _viewModel.ContractorRate);
	        ServiceLabel.Text = ToRateString(NewContractViewModel.ServiceRate);
	        TotalLabel.Text = ToRateString(_viewModel.TotalRate);

            AddToolBarToKeyboard(RateField);
	    }

	    private void AddToolBarToKeyboard(UITextField field)
	    {
	        var toolbar = new UIToolbar(new CGRect(0f, 0f, UIScreen.MainScreen.Bounds.Width, 44f));
	        var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate
	                                                                         {
	                                                                             field.ResignFirstResponder();
	                                                                         });
	        var space = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);
	        toolbar.Items = new[] {space, doneButton};

	        field.InputAccessoryView = toolbar;
	    }

        void RateFieldValueChanged(object sender, EventArgs e)
        {
            decimal val;
            if (decimal.TryParse(RateField.Text, out val))
            {
                _viewModel.ContractorRate = val;
                TotalLabel.Text = ToRateString(_viewModel.TotalRate);
            }
        }

	    private static string ToRateString(decimal rate)
	    {
	        return string.Format("$ {0:N2} / hr", rate);
	    }
	}
}
