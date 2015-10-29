using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiSystems.SharedModels;

namespace SiSystems.ClientApp.Web.Domain.Repositories.AccountExecutive
{
    public interface IContractsRepository
    {
        ContractSummarySet GetFlowThruSummary();
        ContractSummarySet GetFullySourcedSummary();
    }

    public class ContractsRepository : IContractsRepository
    {
        public ContractSummarySet GetFlowThruSummary()
        {
            return new ContractSummarySet
            {
                Current = 55,
                Starting = 17,
                Ending = 12
            };
        }

        public ContractSummarySet GetFullySourcedSummary()
        {
            return new ContractSummarySet
            {
                Current = 30,
                Starting = 15,
                Ending = 10
            };
        }
    }
}
