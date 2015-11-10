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

		private List<List<ConsultantContract>> contractsByStatus;

        public ContractStatusListTableViewSource(UITableViewController parentVC, IEnumerable<ConsultantContract> contracts)
		{
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
			return 1;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			if (contractsByStatus != null)
				return contractsByStatus.Count();
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

           if (contractsByStatus != null)
		   {
               string status = "";

		       status = contractsByStatus[(int) indexPath.Item][0].StatusType.ToString();
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
	                    int numContracts = contractsByStatus[(int)indexPath.Item].Count();
	                
	                    cell.DetailTextLabel.Text = numContracts.ToString();
	                }
            }
	

			return cell;
		}

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
            
			ContractsListViewController vc = (ContractsListViewController)_parentController.Storyboard.InstantiateViewController ("ContractsListViewController");
			vc.setContracts( contractsByStatus[(int)indexPath.Item] );
			_parentController.ShowViewController ( vc, _parentController );
		}
	}
}


