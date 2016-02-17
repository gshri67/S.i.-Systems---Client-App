
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SiSystems.SharedModels
{

    public class TimesheetSupport
	{
        public int TimesheetId { get; set; }
        public IEnumerable<ProjectCode> ProjectCodeOptions { get; set;  }

        public TimesheetSupport()
        {
            TimesheetId = 1;
            ProjectCodeOptions = Enumerable.Empty<ProjectCode>();
        }
	}
}

