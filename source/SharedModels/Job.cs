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
        public string Title { get; set; }

        public bool IsProposed { get { return NumProposed > 0; } }
        public bool HasCallout { get { return NumCallouts > 0; } }
        public DateTime IssueDate { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }


        public JobStatus Status
        {
            get
            {
                if (IsProposed)
                    return HasCallout
                        ? JobStatus.Callout
                        : JobStatus.Proposed;
                
                return JobStatus.Open;
            }
        }

        public int NumShortlisted { get; set; }
		public int NumProposed { get; set; }
		public int NumCallouts { get; set; }

        public UserContact ClientContact { get; set; }

        public Job()
        {
            Id = 0;
            Title = string.Empty;
            ClientName = string.Empty;
            ClientContact = new UserContact();
        }
    }

	public enum JobStatus
	{
		Open,
		Shortlisted,
		Proposed,
		Callout
	}
}
