using System.Collections.Generic;
using System.Linq;

namespace SiSystems.SharedModels
{
	public class Contractor : UserContact
	{
		public MatchGuideConstants.ResumeRating? Rating { get; set; }

		public string ResumeText { get; set; }

		public IEnumerable<Specialization> Specializations { get; set; }

		public IEnumerable<ConsultantContract> Contracts { get; set; }

		public Contractor()
		{
			Specializations = Enumerable.Empty<Specialization>();
			Contracts = Enumerable.Empty<ConsultantContract>();
		}
	}
}
