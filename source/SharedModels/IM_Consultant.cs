using System.Collections.Generic;

namespace SiSystems.SharedModels
{
	public class IM_Consultant : UserContact
	{
		public MatchGuideConstants.ResumeRating? Rating { get; set; }


		public string ResumeText { get; set; }

		public IEnumerable<Specialization> Specializations { get; set; }

		public IEnumerable<ConsultantContract> Contracts { get; set; }

		public IM_Consultant()
		{
			Specializations = new List<Specialization>();
			Contracts = new List<ConsultantContract>();
		}

	}
}
