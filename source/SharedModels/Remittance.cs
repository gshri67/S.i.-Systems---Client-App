using System;

namespace SiSystems.SharedModels
{
	public class Remittance
	{
		public DateTime DepositDate { get; set; }
		public float Amount { get; set; }
		public string DocumentNumber { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
	}
}

