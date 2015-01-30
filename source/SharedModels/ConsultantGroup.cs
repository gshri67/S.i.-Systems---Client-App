using System;
using System.Collections.Generic;

namespace SiSystems.ClientApp.SharedModels
{
    public class ConsultantGroup
    {
        public string GroupTitle { get; set; }
        public IEnumerable<ConsultantSummary> Contractors { get; set; } 
    }
}
