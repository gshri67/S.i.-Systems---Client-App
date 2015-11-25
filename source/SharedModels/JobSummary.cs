using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiSystems.SharedModels
{
    public class JobSummary
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public int NumJobs { get; set; }
        public int NumProposed { get; set; }
        public int NumCallouts { get; set; }

        public JobSummary()
        {
            ClientId = 0;
            ClientName = string.Empty;
        }
    }
}
