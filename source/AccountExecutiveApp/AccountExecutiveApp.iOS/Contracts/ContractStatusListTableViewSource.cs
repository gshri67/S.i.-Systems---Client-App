using System;
using UIKit;
using Foundation;
using SiSystems.SharedModels;
using System.Collections.Generic;
using System.Linq;
using AccountExecutiveApp.Core.TableViewSourceModel;
using CoreGraphics;

namespace AccountExecutiveApp.iOS
{
	public class ContractStatusListTableViewSource : UITableViewSource
	{
		private readonly UITableViewController _parentController;
	    private ContractStatusListTableViewModel _contractsTableModel;

        public ContractStatusListTableViewSource(UITableViewController parentVC, IEnumerable<ConsultantContractSummary> contracts)
        {
            _contractsTableModel = new ContractStatusListTableViewModel( contracts );

            _parentController = parentVC;
        }

		public override nint NumberOfSections(UITableView tableView)
		{
            if ( _contractsTableModel.HasContracts() )
    			return _contractsTableModel.NumberOfContractTypes();
		    return 1;
		}

		public override string TitleForHeader (UITableView tableView, nint section)
		{
		    return _contractsTableModel.HeaderForSection((int)section);
		}

        public override UIView GetViewForFooter(UITableView tableView, nint section)
        {
            if (_contractsTableModel.HasContracts())
                return null;

            var label = new UILabel
            {
                Text = "You have no contracts available at this time.",
                TextAlignment = UITextAlignment.Center,
                Font = UIFont.SystemFontOfSize(14),
                Lines = 0,
                LineBreakMode = UILineBreakMode.WordWrap
            };
            tableView.Add(label);
            return label;
        }

	    public override nint RowsInSection(UITableView tableview, nint section)
		{
            if (_contractsTableModel.HasContracts() )
                return _contractsTableModel.NumberOfStatusesWithContractsOfTypeIndex( (int)section );
            else
				return 0;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			var cell = (SubtitleWithRightDetailCell)tableView.DequeueReusableCell ("SubtitleWithRightDetailCell");

			var contractsByStatus = GetContractsByStatusAndSection(indexPath);
    
	        UpdateCellText(cell, contractsByStatus);

		    return cell;
		}

		private static void UpdateCellText(SubtitleWithRightDetailCell cell, List<ConsultantContractSummary> contractsByStatus)
	    {
	        if (contractsByStatus == null)
	            return;

	        if (contractsByStatus.Count > 0)
	        {
                cell.TextLabel.TextColor = UIColor.DarkTextColor;

				if (contractsByStatus [0].StatusType == ContractStatusType.Ending)
					AddImageToLabel( new UIImage ("minus-round-centred.png"), cell.TextLabel, 20);

				else if (contractsByStatus [0].StatusType == ContractStatusType.Starting)
					AddImageToLabel( new UIImage ("plus-round-centred.png"), cell.TextLabel, 20);

				else 
					cell.TextLabel.Text = contractsByStatus [0].StatusType.ToString ();
	        }

	        //if (cell.DetailTextLabel != null)
			cell.RightDetailTextLabel.Text = contractsByStatus.Count().ToString();
	    }
			
		private static void AddImageToLabel( UIImage image, UILabel label, float size )
		{
			NSTextAttachment textAttachement = new NSTextAttachment ();
			textAttachement.Image = image;
			textAttachement.Bounds = new CoreGraphics.CGRect (0, 0, size, size);
			NSAttributedString attrStringWithImage = NSAttributedString.CreateFrom (textAttachement);
			label.AttributedText = attrStringWithImage;
		}

	    private List<ConsultantContractSummary> GetContractsByStatusAndSection(NSIndexPath indexPath)
	    {
	        if ( _contractsTableModel.HasContracts() )
                return _contractsTableModel.ContractsWithTypeIndexAndStatusIndex((int)indexPath.Section, (int)indexPath.Item);

            return new List<ConsultantContractSummary>();
	    }

	    public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
            ContractsListViewController vc = (ContractsListViewController)_parentController.Storyboard.InstantiateViewController ("ContractsListViewController");

	        var contractsByStatus = _contractsTableModel.ContractsWithTypeIndexAndStatusIndex((int) indexPath.Section, (int) indexPath.Item);
            vc.InitiateViewController(contractsByStatus[0].StatusType, contractsByStatus[0].AgreementSubType, contractsByStatus);
     
            _parentController.ShowViewController ( vc, _parentController );
		}
	}
}


