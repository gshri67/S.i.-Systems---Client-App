using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiSystems.SharedModels
{
    public class Job
    {
        public string ClientName { get; set; }
        public string JobTitle { get; set; }
        public bool isProposed { get; set; }
        public bool hasCallout { get; set; }
    }
}
