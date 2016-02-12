using System;
using System.Collections.Generic;

namespace SiSystems.SharedModels
{
    public class ProjectCodeRateDetails
	{
        public string ProjectCodeDescription { get; set; }
        public int ProjectCodeId { get; set; }

        public int PayRateId { get; set; }
        public string RateAmount { get; set; }
        public string RateDescription { get; set; }
        public string RateType { get; set; }
	}
}