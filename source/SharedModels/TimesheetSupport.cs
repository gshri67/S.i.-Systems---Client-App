
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SiSystems.SharedModels
{

    public class TimesheetSupport
	{
        public IEnumerable<ProjectCode> ProjectCodeOptions { get; set;  }

        public TimesheetSupport()
        {
            ProjectCodeOptions = Enumerable.Empty<ProjectCode>();
        }
	}
}

