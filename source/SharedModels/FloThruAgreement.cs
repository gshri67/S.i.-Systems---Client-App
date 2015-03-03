using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiSystems.ClientApp.SharedModels
{

    public enum FloThruFeePayer
    {
        Client,
        Contractor
    }

    public enum FloThruFeeType
    {
        Percentage,
        PerHour,
        PerDay
    }

    public class FloThruAgreement
    {
        public decimal Fee { get; set; }
        public FloThruFeePayer Payer { get; set; }
        public FloThruFeeType Type { get; set; }
    }
}
