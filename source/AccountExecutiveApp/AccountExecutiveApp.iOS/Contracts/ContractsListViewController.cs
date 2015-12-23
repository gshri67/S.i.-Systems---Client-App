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
	partial class ContractsListViewController : UITableViewController
	{
        private ContractsListTableViewSource _listTableViewSource;
		public IEnumerable<ConsultantContractSummary> _contracts;
		public string Subtitle;
	    private NSAttributedString _attributedTitle;
	    private SubtitleHeaderView _subtitleHeaderView;

	    private bool _contractsWereSet = false;//this is to know whether the contracts were passed in, or if we should call the API
	    ContractsViewModel _contractsViewModel;
        public ContractStatusType StatusType;//atm only used when loading contracts because they were not passed in
        public MatchGuideConstants.AgreementSubTypes TypeOfContract;
	    private LoadingOverlay _overlay;

	    public ContractsListViewController(IntPtr handle)
            : base(handle)
		{
            _contractsViewModel = DependencyResolver.Current.Resolve<ContractsViewModel>();
		}

		private void SetupTableViewSource()
		{
			if (TableView == null || _contracts == null )
				return;

			RegisterCellsForReuse();
			InstantiateTableViewSource();
			TableView.ContentInset = new UIEdgeInsets (-35, 0, -35, 0);
			TableView.Source = _listTableViewSource;
		}

		private void InstantiateTableViewSource()
		{
			_listTableViewSource = new ContractsListTableViewSource ( this, _contracts );

			//_addTimeTableViewSource.OnDataChanged += AddTimeTableDataChanged;
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

		    UpdatePageTitle();

            //LogoutManager.CreateNavBarLeftButton(this);

            if( !_contractsWereSet )
                LoadContracts();
			
            
            CreateCustomTitleBar();

			UpdateUI ();
		}

	    private void UpdatePageTitle()
	    {
	        ContractStatusType status;
            MatchGuideConstants.AgreementSubTypes contractType;

	        if (_contracts != null && _contracts.Any())
	        {
	            status = _contracts.ElementAt(0).StatusType;
	            contractType = _contracts.ElementAt(0).AgreementSubType;
	        }
	        else
	        {
	            status = StatusType;
	            contractType = TypeOfContract;
	        }

	        Title = "";

            if( status == ContractStatusType.Starting )
                _attributedTitle = GetAttributedStringWithImage(new UIImage("plus-round-centred.png"), 15);
            else if( status == ContractStatusType.Ending )
                _attributedTitle = GetAttributedStringWithImage(new UIImage("minus-round-centred.png"), 15);
            else
                Title = string.Format("{0} Contracts", status.ToString());

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

	        Subtitle = string.Format("{0}", contractType.ToString());
	    }
			
		private static NSAttributedString GetAttributedStringWithImage( UIImage image, float size )
		{
			NSTextAttachment textAttachement = new NSTextAttachment ();
			textAttachement.Image = image;
			textAttachement.Bounds = new CoreGraphics.CGRect (0, 0, size, size);
			NSAttributedString attrStringWithImage = NSAttributedString.CreateFrom (textAttachement);
		    return attrStringWithImage;
		}

	    public void setContracts( IEnumerable<ConsultantContractSummary> contracts )
		{
		    _contractsWereSet = true; 
			_contracts = contracts;
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);

			//TableView.ReloadData ();
		}

        public async void LoadContracts()
        {
            if (_contracts != null) return;

            var contracts = await _contractsViewModel.getContracts();

            IndicateLoading();

			if (!_contractsWereSet) 
			{
				_contracts = contracts.Where (c => c.StatusType == StatusType && c.AgreementSubType == TypeOfContract).ToList ();

				if (_contracts.Count() <= 0)
					_contracts = null;
			}

            UpdateUI();
        }

		private void RegisterCellsForReuse()
		{
			if (TableView == null) return;
		
			//TableView.RegisterClassForCellReuse(typeof (RightDetailCell), "RightDetailCell");
			//TableView.RegisterClassForCellReuse(typeof (UITableViewCell), "cell");
			TableView.RegisterClassForCellReuse(typeof(SubtitleWithRightDetailCell), "SubtitleWithRightDetailCell");
		}

		public void UpdateUI()
		{
            RemoveOverlay();

			if (_contracts != null && TableView != null)
			{
				SetupTableViewSource ();
				TableView.ReloadData ();
			}
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
	}
}
