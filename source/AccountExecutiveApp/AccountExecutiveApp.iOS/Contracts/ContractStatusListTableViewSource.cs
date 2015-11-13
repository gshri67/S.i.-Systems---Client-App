using System;
using UIKit;
using Foundation;
using SiSystems.SharedModels;
using System.Collections.Generic;
using System.Linq;

namespace AccountExecutiveApp.iOS
{
	public class ContractStatusListTableViewSource : UITableViewSource
	{
		private readonly UITableViewController _parentController;

		private List<List<ConsultantContract>> FS_contractsByStatus;
		private List<List<ConsultantContract>> FT_contractsByStatus;
	
        public ContractStatusListTableViewSource(UITableViewController parentVC, IEnumerable<ConsultantContract> contracts)
		{
			List<List<ConsultantContract>> contractsByStatus;
            contractsByStatus = new List<List<ConsultantContract>>();
            List<ConsultantContract> contractsList = contracts.ToList();

            for (int i = 0; i < contracts.Count(); i++)
        	{
                bool statusAlreadyAdded = false;
                int statusIndex = -1;

                for (int j = 0; j < contractsByStatus.Count; j++)
	            {
	                if (contractsByStatus[j][0].StatusType == contractsList[i].StatusType)
		            {
		                statusAlreadyAdded = true;
                        statusIndex = j;
	                }
                }

                ConsultantContract contractToAdd = contractsList[i];

                if (statusAlreadyAdded)
                    contractsByStatus[statusIndex].Add(contractToAdd);
                else
                    contractsByStatus.Add(new List<ConsultantContract>() { contractToAdd });
	       	}
		
            //sort contract statuses into Ending, Starting, Current
            for (int listIndex = 0; listIndex < contractsByStatus.Count(); listIndex ++ )
            {
                if (contractsByStatus[listIndex][0].StatusType == MatchGuideConstants.ConsultantContractStatusTypes.Ending && listIndex != 0)
                    swapInContractList(contractsByStatus, 0, listIndex);
                else if (contractsByStatus[listIndex][0].StatusType == MatchGuideConstants.ConsultantContractStatusTypes.Starting && (listIndex > 1 || listIndex == 0 && contractsByStatus.Count > 2))
                    swapInContractList(contractsByStatus, 1, listIndex);
                else if (contractsByStatus[listIndex][0].StatusType == MatchGuideConstants.ConsultantContractStatusTypes.Active && listIndex > 2)
                    swapInContractList(contractsByStatus, 2, listIndex);
            }

			//sort contracts into FS or FT
			FS_contractsByStatus = new List<List<ConsultantContract>>();
			FT_contractsByStatus = new List<List<ConsultantContract>>();

			for (int status = 0; status < contractsByStatus.Count; status++) 
			{
				FS_contractsByStatus.Add ( new List<ConsultantContract>() );
				FT_contractsByStatus.Add ( new List<ConsultantContract>() );

				foreach (ConsultantContract contract in contractsByStatus[status]) 
				{
					if (contract.IsFloThru)
						FT_contractsByStatus[FT_contractsByStatus.Count()-1].Add (contract);
					else
						FS_contractsByStatus[FS_contractsByStatus.Count()-1].Add (contract);
				}

				if (FT_contractsByStatus [FT_contractsByStatus.Count - 1].Count == 0)
					FT_contractsByStatus.RemoveAt (FT_contractsByStatus.Count - 1);

				if (FS_contractsByStatus [FS_contractsByStatus.Count - 1].Count == 0)
					FS_contractsByStatus.RemoveAt (FS_contractsByStatus.Count - 1);
			}

			_parentController = parentVC;
		}

        public void swapInContractList( List<List<ConsultantContract>> list, int i, int j ) 
        {
            List<ConsultantContract> temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }

		public override nint NumberOfSections(UITableView tableView)
		{
			return 2;
		}

		public override string TitleForHeader (UITableView tableView, nint section)
		{
			if (section == 0)
				return "Fully-Sourced";
			else
				return "Flo-Thru";
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			if ( section == 0 && FS_contractsByStatus != null)
				return FS_contractsByStatus.Count();
			else if ( section == 1 && FT_contractsByStatus != null)
				return FT_contractsByStatus.Count();
			else
				return 0;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell ("RightDetailCell");

			if (cell == null)
			{
				Console.WriteLine ("creating new cell");

				cell = new RightDetailCell (UITableViewCellStyle.Value1, "RightDetailCell");
			}

			List<ConsultantContract> contractStatus = null;

			if (indexPath.Section == 0 && FS_contractsByStatus != null )
				contractStatus = FS_contractsByStatus [(int)indexPath.Item];
			else if( indexPath.Section == 1 && FT_contractsByStatus != null )
				contractStatus = FT_contractsByStatus [(int)indexPath.Item];

           if (contractStatus != null)
		   {
               string status = "";
			
		       status = contractStatus[0].StatusType.ToString();
               /*
               if (contractsByStatus[(int)indexPath.Item][0].StatusType == MatchGuideConstants.ConsultantContractStatusTypes.Active)
                   status = "Current";
               else if (contractsByStatus[(int)indexPath.Item][0].StatusType == MatchGuideConstants.ConsultantContractStatusTypes.Starting)
                   status = "Starting";
               else if (contractsByStatus[(int)indexPath.Item][0].StatusType == MatchGuideConstants.ConsultantContractStatusTypes.Ending)
                   status = "Ending";
               */
                cell.TextLabel.Text = status;
			
                if (cell.DetailTextLabel != null)
	                {
						int numContracts = contractStatus.Count();
	                
	                    cell.DetailTextLabel.Text = numContracts.ToString();
	                }
            }
	

			return cell;
		}

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			ContractsListViewController vc = (ContractsListViewController)_parentController.Storyboard.InstantiateViewController ("ContractsListViewController");

			if (indexPath.Section == 0 && FS_contractsByStatus != null) {
				vc.setContracts (FS_contractsByStatus [(int)indexPath.Item]);
				vc.Title = string.Format ("{0} Contracts", FS_contractsByStatus [(int)indexPath.Item] [0].StatusType);
				vc.Subtitle = "Fully-Sourced";
			}
			else if (indexPath.Section == 1 && FT_contractsByStatus != null) 
			{
				vc.setContracts (FT_contractsByStatus [(int)indexPath.Item]);
				vc.Title = string.Format ("{0} Contracts", FT_contractsByStatus [(int)indexPath.Item][0].StatusType);
				vc.Subtitle = "Flo-Thru";
			}
			_parentController.ShowViewController ( vc, _parentController );
		}
	}
}


