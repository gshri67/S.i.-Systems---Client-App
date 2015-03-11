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
        private readonly bool[] _isActiveContract;
        private const string RateWitheldText = "Rate Witheld";

        public ContractsTableViewSource(IList<Contract> contracts)
        {
            _contracts = contracts;
            _isActiveContract = new bool[contracts.Count];
            for (var i = 0; i < contracts.Count; i++)
            {
                _isActiveContract[i] = contracts[i].EndDate > DateTime.Today;
            }
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            // if there are no cells to reuse, create a new one
            var cell = tableView.DequeueReusableCell(CellIdentifier) as CustomContractCell ??
                       new CustomContractCell(CellIdentifier);

            var contract = _contracts[indexPath.Row];
            var contractDate = string.Format("{0:MMM dd, yyyy} {2} {1:MMM dd, yyyy}", contract.StartDate, contract.EndDate,
                StyleGuideConstants.DateSeperator);

            var rate = contract.RateWitheld ? RateWitheldText : string.Format("{0:C}/h as {1}", contract.Rate, contract.SpecializationNameShort);

            cell.UpdateCell(rate, contractDate, contract.Contact.FullName, _isActiveContract[indexPath.Row]);

            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return _contracts.Count;
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return _isActiveContract[indexPath.Row] ? 80 : 60;
        }
    }
}