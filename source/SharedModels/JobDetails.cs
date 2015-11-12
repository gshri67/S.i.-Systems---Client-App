using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiSystems.SharedModels
{
    public class JobDetails
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ClientName { get; set; }
        public ClientContact ClientContact { get; set; }
        public ClientContact DirectReport { get; set; }

        public IEnumerable<IM_Consultant> Shortlisted { get; set; }
        public IEnumerable<IM_Consultant> Proposed { get; set; }
        public IEnumerable<IM_Consultant> Callouts{ get; set; }

        public JobDetails()
        {
            Id = 0;
            Title = string.Empty;
            ClientName = string.Empty;
            ClientContact = new ClientContact();
            DirectReport = new ClientContact();

            Shortlisted = Enumerable.Empty<IM_Consultant>();
            Proposed = Enumerable.Empty<IM_Consultant>();
            Callouts = Enumerable.Empty<IM_Consultant>();
        }
    }
}
