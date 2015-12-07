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
        public bool IsProposed { get; set; }
        public bool HasCallout { get; set; }
        public DateTime IssueDate { get; set; }

		public JobStatus Status { get; set; }

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
            Status = JobStatus.Open;
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
