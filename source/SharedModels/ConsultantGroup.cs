using System;
using System.Collections.Generic;

namespace SiSystems.ClientApp.SharedModels
{
    public class ConsultantGroup
    {
        public string Specialization { get; set; }
        public IList<ConsultantSummary> Consultants { get; set; }

        public ConsultantGroup()
        {
            Consultants = new List<ConsultantSummary>();
        }
    }
}
