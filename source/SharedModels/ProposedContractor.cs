using System;

namespace SiSystems.SharedModels
{
	public class ProposedContractor : Contractor
	{
		public float BillRate { get; set; }
		public float PayRate { get; set; }
		public float GrossMargin { get; set; }
		public float Markup { get; set; }
	}
}

