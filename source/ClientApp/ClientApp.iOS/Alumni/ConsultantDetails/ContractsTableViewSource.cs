using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using SiSystems.ClientApp.SharedModels;
using UIKit;

namespace ClientApp.iOS.Alumni.ConsultantDetails
{
    public class ContractsTableViewSource : UITableViewSource
    {
        private const string CellIdentifier = "ContractCell";
        private readonly IList<Contract> _contracts;

        public ContractsTableViewSource(IList<Contract> contracts)
        {
            _contracts = contracts;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            // if there are no cells to reuse, create a new one
            var cell = tableView.DequeueReusableCell(CellIdentifier) as CustomContractCell ??
                       new CustomContractCell(CellIdentifier);

            var contract = _contracts[indexPath.Row];
            var contractDate = string.Format("{0:MMM dd, yyyy} {2} {1:MMM dd, yyyy}", contract.StartDate, contract.EndDate,
                StyleGuideConstants.DateSeperator);

            var rate = string.Format("{0:C}/h", contract.Rate);

            cell.UpdateCell(rate, contractDate, contract.SpecializationNameShort);

            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return _contracts.Count;
        }
    }
}