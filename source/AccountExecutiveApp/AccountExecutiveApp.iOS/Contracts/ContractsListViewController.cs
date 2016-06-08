using Foundation;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using UIKit;
using AccountExecutiveApp.Core.ViewModel;
using Microsoft.Practices.Unity;
using SiSystems.SharedModels;
using System.Collections.Generic;
using System.Linq;
using CoreGraphics;
using CoreText;

namespace AccountExecutiveApp.iOS
{
	[Register ("ContractsListViewController")]
	partial class ContractsListViewController : Si_TableViewController
	{
        private ContractsListTableViewSource _listTableViewSource;
		public string Subtitle;
	    private NSAttributedString _attributedTitle;
	    private SubtitleHeaderView _subtitleHeaderView;

	    readonly ContractsViewModel _contractsViewModel;
        private ContractStatusType _statusType;
        private MatchGuideConstants.AgreementSubTypes _typeOfContract;
	    private LoadingOverlay _overlay;

	    public ContractsListViewController(IntPtr handle)
            : base(handle)
		{
            _contractsViewModel = DependencyResolver.Current.Resolve<ContractsViewModel>();
		}

		private void SetupTableViewSource()
		{
			if (TableView == null)
				return;

			RegisterCellsForReuse();
			InstantiateTableViewSource();
			TableView.ContentInset = new UIEdgeInsets (-35, 0, -35, 0);
			TableView.Source = _listTableViewSource;
		}

		private void InstantiateTableViewSource()
		{
			_listTableViewSource = new ContractsListTableViewSource ( this, _contractsViewModel.Contracts.Where(c=>c.StatusType == _statusType && c.AgreementSubType == _typeOfContract) );
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

            SearchManager.CreateNavBarRightButton(this);

			UpdateUI ();
		}

	    private void UpdatePageTitle()
	    {
	        switch (_statusType)
	        {
	            case ContractStatusType.Starting:
	                _attributedTitle = GetAttributedStringWithImage(new UIImage("plus-round-centred.png"), 15);
	                break;
	            case ContractStatusType.Ending:
	                _attributedTitle = GetAttributedStringWithImage(new UIImage("minus-round-centred.png"), 15);
	                break;
	            default:
	                Title = string.Format("{0} Contracts", _statusType.ToString());
	                _attributedTitle = null;
	                break;
	        }

	        if (_attributedTitle != null)
	        {
                NSMutableAttributedString newAttrTitle = new NSMutableAttributedString();
                NSAttributedString suffix = new NSAttributedString(" Contracts", new CTStringAttributes()
                {
                    Font = new CTFont("Arial", 20)
                });

                newAttrTitle.Append( _attributedTitle );
                newAttrTitle.Append( suffix );
	            _attributedTitle = newAttrTitle;
	        }

	        Subtitle = string.Format("{0}", _typeOfContract);
	    }
			
		private static NSAttributedString GetAttributedStringWithImage( UIImage image, float size )
		{
			NSTextAttachment textAttachement = new NSTextAttachment ();
			textAttachement.Image = image;
			textAttachement.Bounds = new CoreGraphics.CGRect (0, 0, size, size);
			NSAttributedString attrStringWithImage = NSAttributedString.CreateFrom (textAttachement);
		    return attrStringWithImage;
		}

		private void RegisterCellsForReuse()
		{
			if (TableView == null) return;
		
			TableView.RegisterClassForCellReuse(typeof(SubtitleWithRightDetailCell), "SubtitleWithRightDetailCell");
		}

		public void UpdateUI()
		{
            UpdatePageTitle();
            CreateCustomTitleBar();
            RemoveOverlay();

		    if (TableView == null) return;

		    SetupTableViewSource ();
		    TableView.ReloadData ();
		}

        private void CreateCustomTitleBar()
        {
            InvokeOnMainThread(() =>
            {
                _subtitleHeaderView = new SubtitleHeaderView();
                NavigationItem.TitleView = _subtitleHeaderView;
                
                _subtitleHeaderView.TitleText = Title;

                if( _attributedTitle != null )
                    _subtitleHeaderView.AttributedTitleText = _attributedTitle;
                
                _subtitleHeaderView.SubtitleText = Subtitle;
                NavigationItem.Title = "";
            });
        }



        #region Overlay

        private void IndicateLoading()
        {
            InvokeOnMainThread(delegate
            {
                if (_overlay != null) return;


                var frame = new CGRect(View.Frame.X, View.Frame.Y, View.Frame.Width, View.Frame.Height);
                _overlay = new LoadingOverlay(frame, null);
                View.Add(_overlay);
            });
        }

        private void RemoveOverlay()
        {
            if (_overlay == null) return;

            InvokeOnMainThread(_overlay.Hide);
            _overlay = null;
        }
        #endregion

        public void InitiateViewController(ContractStatusType contractStatusType, MatchGuideConstants.AgreementSubTypes agreementSubType, IEnumerable<ConsultantContractSummary> contracts = null)
	    {
	        _statusType = contractStatusType;
	        _typeOfContract = agreementSubType;

            LoadContracts(contracts);
	    }

	    private void LoadContracts(IEnumerable<ConsultantContractSummary> contracts)
	    {
            IndicateLoading();

	        var loading = _contractsViewModel.LoadContracts(contracts);
	        loading.ContinueWith(_ => InvokeOnMainThread(UpdateUI));
	    }
	}
}
