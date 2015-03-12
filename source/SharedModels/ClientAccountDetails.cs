using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiSystems.ClientApp.SharedModels
{

    public class ClientAccountDetails
    {
        public decimal FloThruFee { get; set; }
        public decimal MspFeePercentage { get; set; }
        public decimal MaxVisibleRate { get; set; }
        public MatchGuideConstants.FloThruFeeType FloThruFeeType { get; set; }
        public MatchGuideConstants.FloThruFeePayment FloThruFeePayment { get; set; }
        public MatchGuideConstants.FloThruMspPayment FloThruMspPayment { get; set; }
        public int InvoiceFormat { get; set; }
        public int InvoiceFrequency { get; set; }
    }
}
