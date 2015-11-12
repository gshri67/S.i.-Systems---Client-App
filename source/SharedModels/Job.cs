using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiSystems.SharedModels
{
    public class Job
    {
        public int Id { get; set; }
        public string ClientName { get; set; }
        public string JobTitle { get; set; }
        public bool IsProposed { get; set; }
        public bool HasCallout { get; set; }
        public DateTime IssueDate { get; set; }
    }
}
