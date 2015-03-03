using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiSystems.ClientApp.SharedModels
{

    public class ClientAccountDetails
    {
        //public FloThruAgreement CleintFloThruAgreement { get; set; }
        //public AccountExecutive ClientAccountExecutive { get; set; }

        public decimal FloThruFee { get; set; }
        public string FloThruFeePayer { get; set; }
        public string FloThruFeeType { get; set; }
        public string InvoiceFormat { get; set; }
        public string InvoiceFrequency { get; set; }
    }
}
