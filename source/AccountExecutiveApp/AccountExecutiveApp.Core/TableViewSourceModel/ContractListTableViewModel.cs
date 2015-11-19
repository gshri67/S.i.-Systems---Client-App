using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiSystems.SharedModels;

namespace AccountExecutiveApp.Core.TableViewSourceModel
{
    public class ContractListTableViewModel
    {
        private List<ConsultantContractSummary> _contracts;

        private MatchGuideConstants.AgreementSubTypes _typeOfContract;
        private ContractStatusType _contractStatus;

        public ContractListTableViewModel( IEnumerable<ConsultantContractSummary> contracts )
        {
            if( contracts != null )
                _contracts = contracts.ToList();
            else
                _contracts = new List<ConsultantContractSummary>();

            _typeOfContract = _contracts[0].AgreementSubType;
            _contractStatus = _contracts[0].StatusType;

            List<ConsultantContractSummary> sortedContracts = SortContracts(_contracts);
        }

        public ConsultantContractSummary ContractAtIndex(int index)
        {
            return _contracts[index];
        }

        public int NumberOfContracts()
        {
            return _contracts.Count;
        }

        public bool HasContracts()
        {
            if (NumberOfContracts() > 0)
                return true;
            return false;
        }

        public string DateDetailStringAtIndex( int index )
        {
            ConsultantContractSummary curContract = _contracts[index];

            if ( _contractStatus == ContractStatusType.Starting)
                return  string.Format( "Starts {0}", curContract.StartDate.ToString("MMM dd, yyyy") );
            else
                return string.Format("Ends {0}", curContract.EndDate.ToString("MMM dd, yyyy"));
        }

//Private Methods
        private List<ConsultantContractSummary> SortContracts( List<ConsultantContractSummary> contracts )
        {
            if ( contracts != null && NumberOfContracts() > 1 )
            {
                if ( contracts[0].StatusType == ContractStatusType.Starting )
                    SortContractsByStartDate( contracts );
                else
                    SortContractsByEndDate( contracts );
            }
            return contracts;
        }

        private void SortContractsByStartDate( List<ConsultantContractSummary> contracts )
        {
            contracts.Sort((d1, d2) => DateTime.Compare(d1.StartDate, d2.StartDate));
        }
        private void SortContractsByEndDate( List<ConsultantContractSummary> contracts )
        {
            contracts.Sort((d1, d2) => DateTime.Compare(d1.EndDate, d2.EndDate));
        }

        public int ContractIdByIndex(int item)
        {
            return _contracts[item].ContractId;
        }
    }
}