using System;
using System.Collections.Generic;

namespace SiSystems.SharedModels
{
    public class ProjectCode
	{
        public string Description { get; set; }
        public IEnumerable<PayRate> PayRates { get; set; }
	}
}