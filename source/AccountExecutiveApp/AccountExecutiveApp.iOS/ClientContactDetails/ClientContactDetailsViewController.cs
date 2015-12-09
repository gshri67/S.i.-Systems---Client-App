using Foundation;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountExecutiveApp.Core.ViewModel;
using AccountExecutiveApp.iOS.Jobs.JobDetails.ContractorJobStatusList;
using Microsoft.Practices.Unity;
using SiSystems.SharedModels;
using UIKit;

namespace AccountExecutiveApp.iOS
{
	public partial class ClientContactDetailsViewController : UIViewController
	{
		private readonly ClientContactDetailsViewModel _viewModel;
		public const string CellIdentifier = "ContractorContactInfoCell";
		private bool _needsUpdateInterface = false;
		private string Subtitle;
		private SubtitleHeaderView _subtitleHeaderView;

		public ClientContactDetailsViewController(IntPtr handle)
			: base(handle)
		{
			_viewModel = DependencyResolver.Current.Resolve<ClientContactDetailsViewModel>();
		}

		public void SetContactId(int id, UserContactType contactType)
		{
			LoadContact(id);
            _viewModel.SetContactType(contactType);
		}

		private void InstantiateTableViewSource()
		{
			if (TableView == null)
				return;

			TableView.RegisterClassForCellReuse(typeof(ContractorContactInfoCell), CellIdentifier);
			TableView.RegisterClassForCellReuse(typeof(SubtitleWithRightDetailCell), SubtitleWithRightDetailCell.CellIdentifier);
			TableView.RegisterClassForCellReuse(typeof(RightDetailCell), RightDetailCell.CellIdentifier);
			TableView.RegisterClassForCellReuse(typeof(UITableViewCell), "UITableViewCell");

			TableView.Source = new ClientContactDetailsTableViewSource(this, _viewModel.Contact);
			TableView.ContentInset = new UIEdgeInsets(-35, 0, -35, 0);
			TableView.ReloadData();
		}

		private void UpdateUserInterface()
		{
		    if (TableView == null || DetailsContainerView == null)
		    {
		        _needsUpdateInterface = true;
		        return;
		    }

			InvokeOnMainThread(InstantiateTableViewSource);
            InvokeOnMainThread(UpdateDetailsView);

			InvokeOnMainThread(UpdatePageTitle);
			InvokeOnMainThread(CreateCustomTitleBar);
		}

	    private void UpdateDetailsView()
	    {
            CompanyNameLabel.Text = _viewModel.ClientName;
			AddressLabel.Text = _viewModel.Address;
	    }

	    public override void ViewDidLoad()
		{
			base.ViewDidLoad();
            
            if( _needsUpdateInterface )
                UpdateUserInterface();
		}

		private void UpdatePageTitle()
		{
			Title = _viewModel.PageTitle;
			Subtitle = _viewModel.PageSubtitle;
		}

		public void LoadContact(int id)
		{
			var task = _viewModel.LoadContact(id);
			task.ContinueWith(_ => InvokeOnMainThread(UpdateUserInterface), TaskContinuationOptions.OnlyOnRanToCompletion);
		}

		private void CreateCustomTitleBar()
		{
			InvokeOnMainThread(() =>
				{
					_subtitleHeaderView = new SubtitleHeaderView();
					NavigationItem.TitleView = _subtitleHeaderView;

					_subtitleHeaderView.TitleText = Title;
					_subtitleHeaderView.SubtitleText = Subtitle;
					NavigationItem.Title = "";
				});
		}
	}
}

