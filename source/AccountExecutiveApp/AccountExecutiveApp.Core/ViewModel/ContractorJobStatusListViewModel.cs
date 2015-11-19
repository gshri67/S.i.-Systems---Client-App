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
        private IEnumerable<Contractor> _contractors;

        private IEnumerable<Contractor> Contractors
        {
            get { return _contractors ?? Enumerable.Empty<Contractor>(); }
            set { _contractors = value ?? Enumerable.Empty<Contractor>(); }
        }

        public ContractorJobStatusListViewModel()
        {

        }

        public void LoadContractors(IEnumerable<Contractor> contractors)
        {
            Contractors = contractors ?? Enumerable.Empty<Contractor>();
        }

        public int NumberOfContractors()
        {
            return Contractors.Count();
        }

        public string ContractorNameByRowNumber(int rowNumber)
        {
            var contractor = Contractors.ElementAtOrDefault(rowNumber);
            
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
