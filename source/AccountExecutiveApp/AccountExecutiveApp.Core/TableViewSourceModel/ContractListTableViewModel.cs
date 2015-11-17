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
        private List<ConsultantContract> _contracts;

        private ContractType _typeOfContract;
        private ContractStatusType _contractStatus;

        public ContractListTableViewModel( IEnumerable<ConsultantContract> contracts )
        {
            if( contracts != null )
                _contracts = contracts.ToList();
            else
                _contracts = new List<ConsultantContract>();

            _typeOfContract = _contracts[0].ContractType;
            _contractStatus = _contracts[0].StatusType;

            List<ConsultantContract> sortedContracts = SortContracts(_contracts);
        }

        public ConsultantContract ContractAtIndex(int index)
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
            ConsultantContract curContract = _contracts[index];

            if ( _contractStatus == ContractStatusType.Starting)
                return  string.Format( "Starts {0}", curContract.StartDate.ToString("MMM dd, yyyy") );
            else
                return string.Format("Ends {0}", curContract.EndDate.ToString("MMM dd, yyyy"));
        }

//Private Methods
        private List<ConsultantContract> SortContracts( List<ConsultantContract> contracts )
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

        private void SortContractsByStartDate( List<ConsultantContract> contracts )
        {
            contracts.Sort((d1, d2) => DateTime.Compare(d1.StartDate, d2.StartDate));
        }
        private void SortContractsByEndDate( List<ConsultantContract> contracts )
        {
            contracts.Sort((d1, d2) => DateTime.Compare(d1.EndDate, d2.EndDate));
        }
    }
}