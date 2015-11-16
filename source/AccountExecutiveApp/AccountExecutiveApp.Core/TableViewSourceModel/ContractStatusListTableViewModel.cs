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
            ContractsDictionaryByTypeAndStatus;

        public ContractStatusListTableViewModel( IEnumerable<ConsultantContract> contracts )
        {
            PopulateContractsDictionaryByTypeAndStatus(contracts);
        }

        public List<ConsultantContract> contractsWithTypeAndStatus(ContractType type, ContractStatusType status)
        {
            return ContractsDictionaryByTypeAndStatus[type][status];
        }
        public List<ConsultantContract> contractsWithTypeAndStatusIndex(ContractType type, int statusIndex)
        {
            return ContractsDictionaryByTypeAndStatus[type].Values.ElementAt(statusIndex);
        }
        public List<ConsultantContract> contractsWithTypeIndexAndStatusIndex(int typeIndex, int statusIndex)
        {
            return ContractsDictionaryByTypeAndStatus.Values.ElementAt(typeIndex).Values.ElementAt(statusIndex);
        }

        public int NumberOfStatusesWithContractsOfType( ContractType type )
        {
            return ContractsDictionaryByTypeAndStatus[type].Keys.Count;
        }

        public int NumberOfContractTypes()
        {
            return ContractsDictionaryByTypeAndStatus.Keys.Count;
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

            ContractsDictionaryByTypeAndStatus =
                new Dictionary<ContractType, Dictionary<ContractStatusType, List<ConsultantContract>>>();
            ContractsDictionaryByTypeAndStatus[ContractType.FullySourced] = fsDict;
            ContractsDictionaryByTypeAndStatus[ContractType.FloThru] = ftDict;
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
    }
}