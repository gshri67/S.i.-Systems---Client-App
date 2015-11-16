using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiSystems.SharedModels;

namespace AccountExecutiveApp.Core.TableViewSourceModel
{
    public class ContractStatusListTableViewModel
    {
        private Dictionary<ContractType, Dictionary<ContractStatusType, List<ConsultantContract>>>
            _contractsDictionaryByTypeAndStatus;

        public ContractStatusListTableViewModel( IEnumerable<ConsultantContract> contracts )
        {
            PopulateContractsDictionaryByTypeAndStatus(contracts);
        }

        public List<ConsultantContract> ContractsWithTypeAndStatus(ContractType type, ContractStatusType status)
        {
            return _contractsDictionaryByTypeAndStatus[type][status];
        }
        public List<ConsultantContract> ContractsWithTypeAndStatusIndex(ContractType type, int statusIndex)
        {
            return _contractsDictionaryByTypeAndStatus[type].Values.ElementAt(statusIndex);
        }
        public List<ConsultantContract> ContractsWithTypeIndexAndStatusIndex(int typeIndex, int statusIndex)
        {
            return _contractsDictionaryByTypeAndStatus.Values.ElementAt(typeIndex).Values.ElementAt(statusIndex);
        }

        public int NumberOfStatusesWithContractsOfType( ContractType type )
        {
            return _contractsDictionaryByTypeAndStatus[type].Keys.Count;
        }
        public int NumberOfStatusesWithContractsOfTypeIndex(int section)
        {
            return _contractsDictionaryByTypeAndStatus.Values.ElementAt(section).Keys.Count;
        }

        public int NumberOfContractTypes()
        {
            return _contractsDictionaryByTypeAndStatus.Keys.Count;
        }

        public bool HasContracts()
        {
            if (NumberOfContractTypes() > 0)
                return true;
            return false;
        }


//Private Methods
        private void PopulateContractsDictionaryByTypeAndStatus(IEnumerable<ConsultantContract> contracts)
        {
            var fsDict = DictionaryWithContractsByStatus(contracts, ContractType.FullySourced);
            var ftDict = DictionaryWithContractsByStatus(contracts, ContractType.FloThru);

            _contractsDictionaryByTypeAndStatus =
                new Dictionary<ContractType, Dictionary<ContractStatusType, List<ConsultantContract>>>();
            _contractsDictionaryByTypeAndStatus[ContractType.FullySourced] = fsDict;
            _contractsDictionaryByTypeAndStatus[ContractType.FloThru] = ftDict;
        }

        private Dictionary<ContractStatusType, List<ConsultantContract>> DictionaryWithContractsByStatus(IEnumerable<ConsultantContract> contracts, ContractType type)
        {
            Dictionary<ContractStatusType, List<ConsultantContract>> typeDict =
                new Dictionary<ContractStatusType, List<ConsultantContract>>();

            List<ConsultantContract> endingContracts = contracts.Where(contract => contract.ContractType == type && contract.StatusType == ContractStatusType.Ending).ToList();
            List<ConsultantContract> startingContracts = contracts.Where(contract => contract.ContractType == type && contract.StatusType == ContractStatusType.Starting).ToList();
            List<ConsultantContract> activeContracts = contracts.Where(contract => contract.ContractType == type && contract.StatusType == ContractStatusType.Active).ToList();

            if (endingContracts.Count > 0)
                typeDict[ContractStatusType.Ending] = endingContracts;
            if (startingContracts.Count > 0)
                typeDict[ContractStatusType.Starting] = startingContracts;
            if (activeContracts.Count > 0)
                typeDict[ContractStatusType.Active] = activeContracts;

            return typeDict;
        }

        public ContractType ContractTypeAtIndex( int section)
        {
            return _contractsDictionaryByTypeAndStatus.Keys.ElementAt(section);
        }
    }
}