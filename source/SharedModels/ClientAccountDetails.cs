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
        public int FloThruFeePayer { get; set; }
        public int FloThruFeeType { get; set; }
        public int InvoiceFormat { get; set; }
        public int InvoiceFrequency { get; set; }
        public decimal MspPercent { get; set; }
    }
}
