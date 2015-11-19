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
        public UserContact ClientContact { get; set; }
        public UserContact DirectReport { get; set; }

        public IEnumerable<Contractor> Shortlisted { get; set; }
        public IEnumerable<Contractor> Proposed { get; set; }
        public IEnumerable<Contractor> Callouts{ get; set; }

        public JobDetails()
        {
            Id = 0;
            Title = string.Empty;
            ClientName = string.Empty;
            ClientContact = new UserContact();
            DirectReport = new UserContact();

            Shortlisted = Enumerable.Empty<Contractor>();
            Proposed = Enumerable.Empty<Contractor>();
            Callouts = Enumerable.Empty<Contractor>();
        }
    }
}
