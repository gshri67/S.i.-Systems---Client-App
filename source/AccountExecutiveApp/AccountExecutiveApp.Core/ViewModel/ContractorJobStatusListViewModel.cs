using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiSystems.SharedModels;

namespace AccountExecutiveApp.Core.ViewModel
{
    public class ContractorJobStatusListViewModel
    {
        private IEnumerable<IM_Consultant> _consultants;

        private IEnumerable<IM_Consultant> Consultants
        {
            get { return _consultants ?? Enumerable.Empty<IM_Consultant>(); }
            set { _consultants = value ?? Enumerable.Empty<IM_Consultant>(); }
        }

        public ContractorJobStatusListViewModel()
        {

        }

        public void LoadConsultants(IEnumerable<IM_Consultant> consultants)
        {
            Consultants = consultants ?? Enumerable.Empty<IM_Consultant>();
        }

        public int NumberOfContractors()
        {
            return Consultants.Count();
        }

        public string ContractorNameByRowNumber(int rowNumber)
        {
            var contractor = Consultants.ElementAtOrDefault(rowNumber);
            
            if (contractor == null || string.IsNullOrEmpty(contractor.FullName))
                return string.Empty;

            return contractor.FullName;
        }

        public string ContractorStatusByRowNumber(int rowNumber)
        {
            //var contractor = Consultants.ElementAtOrDefault(rowNumber);
            //return contractor == null ? string.Empty : contractor.FullName;
            return string.Empty;
        }
    }
}
