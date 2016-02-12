
using System;
using System.Collections.Generic;

namespace SiSystems.SharedModels
{

    public class TimesheetSupport
	{
        public int TimesheetId { get; set; }
        public IEnumerable<ProjectCode> ProjectCodeOptions { get; set;  }
	}
}

