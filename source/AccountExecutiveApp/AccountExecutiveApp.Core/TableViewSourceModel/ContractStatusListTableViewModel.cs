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
        private Dictionary<MatchGuideConstants.AgreementSubTypes, Dictionary<ContractStatusType, List<ConsultantContractSummary>>>
            _contractsDictionaryByTypeAndStatus;

        public ContractStatusListTableViewModel( IEnumerable<ConsultantContractSummary> contracts )
        {
            PopulateContractsDictionaryByTypeAndStatus(contracts);
        }

        public List<ConsultantContractSummary> ContractsWithTypeAndStatus(MatchGuideConstants.AgreementSubTypes type, ContractStatusType status)
        {
            return _contractsDictionaryByTypeAndStatus[type][status];
        }
        public List<ConsultantContractSummary> ContractsWithTypeAndStatusIndex(MatchGuideConstants.AgreementSubTypes type, int statusIndex)
        {
            return _contractsDictionaryByTypeAndStatus[type].Values.ElementAt(statusIndex);
        }
        public List<ConsultantContractSummary> ContractsWithTypeIndexAndStatusIndex(int typeIndex, int statusIndex)
        {
            return _contractsDictionaryByTypeAndStatus.Values.ElementAt(typeIndex).Values.ElementAt(statusIndex);
        }

        public int NumberOfStatusesWithContractsOfType(MatchGuideConstants.AgreementSubTypes type)
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
        private void PopulateContractsDictionaryByTypeAndStatus(IEnumerable<ConsultantContractSummary> contracts)
        {
            var fsDict = DictionaryWithContractsByStatus(contracts, MatchGuideConstants.AgreementSubTypes.Consultant);
            var ftDict = DictionaryWithContractsByStatus(contracts, MatchGuideConstants.AgreementSubTypes.FloThru);

            _contractsDictionaryByTypeAndStatus =
                new Dictionary<MatchGuideConstants.AgreementSubTypes, Dictionary<ContractStatusType, List<ConsultantContractSummary>>>();
            if (fsDict.Any())
                _contractsDictionaryByTypeAndStatus[MatchGuideConstants.AgreementSubTypes.Consultant] = fsDict;
            if (ftDict.Any())
                _contractsDictionaryByTypeAndStatus[MatchGuideConstants.AgreementSubTypes.FloThru] = ftDict;
        }

        private Dictionary<ContractStatusType, List<ConsultantContractSummary>> DictionaryWithContractsByStatus(IEnumerable<ConsultantContractSummary> contracts, MatchGuideConstants.AgreementSubTypes type)
        {
            var typeDict = new Dictionary<ContractStatusType, List<ConsultantContractSummary>>();

            var endingContracts = contracts.Where(contract => contract.AgreementSubType == type && contract.StatusType == ContractStatusType.Ending).ToList();
            var startingContracts = contracts.Where(contract => contract.AgreementSubType == type && contract.StatusType == ContractStatusType.Starting).ToList();
            var activeContracts = contracts.Where(contract => contract.AgreementSubType == type && contract.StatusType == ContractStatusType.Active).ToList();

            if (endingContracts.Count > 0)
                typeDict[ContractStatusType.Ending] = endingContracts;
            if (startingContracts.Count > 0)
                typeDict[ContractStatusType.Starting] = startingContracts;
            if (activeContracts.Count > 0)
                typeDict[ContractStatusType.Active] = activeContracts;

            return typeDict;
        }

        public MatchGuideConstants.AgreementSubTypes ContractTypeAtIndex(int section)
        {
            return _contractsDictionaryByTypeAndStatus.Keys.ElementAt(section);
        }
    }
}