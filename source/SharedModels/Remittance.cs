using System;

namespace SiSystems.SharedModels
{
	public class Remittance
	{
		public float Amount { get; set; }
		public string DocumentNumber { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
	    public DateTime DepositDate { get; set; }

        public string VoucherNumber { get; set; }
        public string Source { get; set; }
        public string DBSource { get; set; }
	}
}

