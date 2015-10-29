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

			_parentController = parentVC;
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
		                cell.TextLabel.Text = contractsByStatus[(int)indexPath.Item][0].StatusType.ToString();
					
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
			vc._contracts = contractsByStatus[(int)indexPath.Item];
			_parentController.ShowViewController ( vc, _parentController );
		}
	}
}


