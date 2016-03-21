
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SiSystems.SharedModels
{

    public class TimesheetSupport
	{
        public List<ProjectCodeRateDetails> ProjectCodeOptions { get; set; }

        public TimesheetSupport()
        {
            ProjectCodeOptions = new List<ProjectCodeRateDetails>();
        }
	}
}

