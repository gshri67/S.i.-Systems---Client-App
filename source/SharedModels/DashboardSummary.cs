﻿using System;

namespace SiSystems.SharedModels
{
    public class DashboardSummary
    {
        public ContractSummarySet FullySourcedContracts { get; set; }
        public ContractSummarySet FlowThruContracts { get; set; }
        public JobsSummarySet Jobs { get; set; }
		public string UserName { get; set; }
        public TimesheetSummarySet Timesheets { get; set; }

        public DashboardSummary()
        {
            FullySourcedContracts = new ContractSummarySet();
            FlowThruContracts = new ContractSummarySet();
            Jobs = new JobsSummarySet();
			UserName = string.Empty;
            Timesheets = new TimesheetSummarySet();
        }
    }

    public class ContractSummarySet
    {
        public int Current { get; set; }
        public int Ending { get; set; }
        public int Starting { get; set; }
    }

    public class JobsSummarySet
    {
        public int All { get; set; }
        public int Proposed { get; set; }
        public int Callouts { get; set; }
    }
}

