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
        private IEnumerable<IM_Consultant> Consultants { get; set; }
        public ContractorJobStatusListViewModel()
        {

        }

        public void LoadConsultants(IEnumerable<IM_Consultant> consultants)
        {
            Consultants = consultants;
        }
    }
}
