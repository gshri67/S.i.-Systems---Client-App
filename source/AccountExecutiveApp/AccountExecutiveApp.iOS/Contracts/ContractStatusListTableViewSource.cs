using System;
using UIKit;
using Foundation;
using SiSystems.SharedModels;
using System.Collections.Generic;
using System.Linq;
using AccountExecutiveApp.Core.TableViewSourceModel;

namespace AccountExecutiveApp.iOS
{
	public class ContractStatusListTableViewSource : UITableViewSource
	{
		private readonly UITableViewController _parentController;
	    private ContractStatusListTableViewModel _contractsTableModel;

        public ContractStatusListTableViewSource(UITableViewController parentVC, IEnumerable<ConsultantContract> contracts)
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
		    return _contractsTableModel.ContractTypeAtIndex((int)section).ToString();
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
			var cell = tableView.DequeueReusableCell ("RightDetailCell");

			var contractsByStatus = GetContractsByStatusAndSection(indexPath);
    
	        UpdateCellText(cell, contractsByStatus);

		    return cell;
		}

	    private static void UpdateCellText(UITableViewCell cell, List<ConsultantContract> contractsByStatus)
	    {
	        if (contractsByStatus == null)
	            return;

	        if (contractsByStatus.Count > 0)
	        {
                cell.TextLabel.TextColor = UIColor.DarkTextColor;

				if (contractsByStatus [0].StatusType == ContractStatusType.Ending) {
					//cell.TextLabel.Text = "-";
					//cell.TextLabel.TextColor = StyleGuideConstants.RedUiColor;
					//cell.TextLabel.Font = UIFont.BoldSystemFontOfSize(50);
					//cell.TextLabel.Font = UIFont.SystemFontOfSize(50);
					//cell.TextLabel.Font = UIFont.FromName("HelveticaNeue-Light", 65f);

					NSTextAttachment textAttachement = new NSTextAttachment ();
					textAttachement.Image = new UIImage ("ios7-minus-empty-centred.png");
					textAttachement.Bounds = new CoreGraphics.CGRect (0, 0, 30, 30);
					NSAttributedString attrStringWithImage = NSAttributedString.CreateFrom (textAttachement);


					cell.TextLabel.AttributedText = attrStringWithImage;
					/*
					NSMutableAttributedString *attributedString = [[NSMutableAttributedString alloc] initWithString:@"like after"];

					NSTextAttachment *textAttachment = [[NSTextAttachment alloc] init];
					textAttachment.image = [UIImage imageNamed:@"ios7-minus-empty.png"];

					NSAttributedString *attrStringWithImage = [NSAttributedString attributedStringWithAttachment:textAttachment];

					[attributedString replaceCharactersInRange:NSMakeRange(4, 1) withAttributedString:attrStringWithImage];
*/

				} else if (contractsByStatus [0].StatusType == ContractStatusType.Starting) {
					//cell.TextLabel.Text = "+";
					//cell.TextLabel.TextColor = StyleGuideConstants.GreenUiColor;
					//cell.TextLabel.Font = UIFont.BoldSystemFontOfSize(45);
					//cell.TextLabel.Font = UIFont.SystemFontOfSize(45);
					//cell.TextLabel.Font = UIFont.FromName("HelveticaNeue-Light", 40f);

					NSTextAttachment textAttachement = new NSTextAttachment ();
					textAttachement.Image = new UIImage ("ios7-plus-empty-centred.png");
					textAttachement.Bounds = new CoreGraphics.CGRect (0, 0, 30, 30);
					NSAttributedString attrStringWithImage = NSAttributedString.CreateFrom (textAttachement);


					cell.TextLabel.AttributedText = attrStringWithImage;
				} else 
					cell.TextLabel.Text = contractsByStatus [0].StatusType.ToString ();
	        }

	        if (cell.DetailTextLabel != null)
	            cell.DetailTextLabel.Text = contractsByStatus.Count().ToString();
	    }

	    private List<ConsultantContract> GetContractsByStatusAndSection(NSIndexPath indexPath)
	    {
	        if ( _contractsTableModel.HasContracts() )
                return _contractsTableModel.ContractsWithTypeIndexAndStatusIndex((int)indexPath.Section, (int)indexPath.Item);

            return new List<ConsultantContract>();
	    }

	    public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
            ContractsListViewController vc = (ContractsListViewController)_parentController.Storyboard.InstantiateViewController ("ContractsListViewController");

	        List<ConsultantContract> contractsByStatus = _contractsTableModel.ContractsWithTypeIndexAndStatusIndex((int) indexPath.Section, (int) indexPath.Item);

            vc.setContracts( contractsByStatus );
            vc.Title = string.Format("{0} Contracts", contractsByStatus[0].StatusType.ToString());
            vc.Subtitle = string.Format("{0} Contracts", contractsByStatus[0].AgreementSubType.ToString());
     
            _parentController.ShowViewController ( vc, _parentController );
		}

	    public override nfloat GetHeightForHeader(UITableView tableView, nint section)
	    {
	        return 35;
	    }
	}
}


