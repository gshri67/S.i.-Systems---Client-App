using System;

namespace SiSystems.SharedModels
{
	public class TimesheetContact
	{
		public int Id;
        public int AgreementId { get; set; }
        
        public string CompanyName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
	    public MatchGuideConstants.TimesheetStatus Status { get; set; }public UserContact Contractor { get; set; }
        public UserContact DirectReport { get; set; }


	    public TimesheetContact ()
		{
            Contractor = new UserContact();
            DirectReport = new UserContact();
            CompanyName = string.Empty;
            StartDate = new DateTime();
            EndDate = new DateTime();
		}
	}
}

