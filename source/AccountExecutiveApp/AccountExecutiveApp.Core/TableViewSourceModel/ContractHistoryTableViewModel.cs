using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Core;
using SiSystems.SharedModels;

namespace AccountExecutiveApp.Core.ViewModel
{
    public class ContractHistoryTableViewModel
    {
        private readonly IMatchGuideApi _api;

        private IEnumerable<ConsultantContract> _contracts;
        public IEnumerable<ConsultantContract> Contracts
        {
            get { return _contracts ?? Enumerable.Empty<ConsultantContract>(); }
            set { _contracts = value ?? Enumerable.Empty<ConsultantContract>(); }
        }
        
        public ContractHistoryTableViewModel( IEnumerable<ConsultantContract> contracts )
        {
            Contracts = contracts;
        }
        
        public bool IndexIsInBounds(int index)
        {
            return index < Contracts.Count() && index >= 0;
        }
        
        public string CompanyNameByRowNumber( int row )
        {
           return IndexIsInBounds(row) 
                ? Contracts.ElementAtOrDefault(row).ClientName
                : string.Empty;
        }

        public string ContractPeriodByRowNumber( int row )
        {
            return IndexIsInBounds(row)
                ? string.Format("{0} - {1}", Contracts.ElementAt(row).StartDate.ToString("MMM, yyyy"),
                    Contracts.ElementAt(row).EndDate.ToString("MMM, yyyy"))
                : string.Empty;
        }
        public string FormattedStartDateByRowNumber( int row )
        {
            return IndexIsInBounds(row) 
                ? string.Format("{0}", Contracts.ElementAt(row).StartDate.ToString("MMM dd, yyyy"))
                : string.Empty;
        }
        public string FormattedEndDateByRowNumber( int row )
        {
            return IndexIsInBounds(row) 
                ? string.Format("{0}", Contracts.ElementAt(row).EndDate.ToString("MMM dd, yyyy"))
                : string.Empty;
        }

        public string ContractTitleByRowNumber( int row )
        {
            return IndexIsInBounds(row) 
                ? Contracts.ElementAt(row).Title
                : string.Empty;
        }
        
        public string ConsultantsFullNameByRowNumber( int row )
        {
           return IndexIsInBounds(row) 
                ? Contracts.ElementAt(row).Contractor.FullName
                : string.Empty;
        }

        public int NumberOfContracts()
        {
            if (Contracts == null)
                return 0;
            return Contracts.Count();
        }

        public int ContractIdByRowNumber(int row)
        {
            return IndexIsInBounds(row)
                ? Contracts.ElementAt(row).ContractId
                : 0;
        }
    }
}
