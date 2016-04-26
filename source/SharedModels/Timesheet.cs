using System;
using System.Collections.Generic;

namespace SiSystems.SharedModels
{
    public class TimesheetSummary
    {
        public int Id { get; set; }
        
        public MatchGuideConstants.TimesheetStatus Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string TimePeriod { get { return string.Format("{0:MMM d} - {1:MMM d}", StartDate, EndDate); } }

        public UserContact ClientContact { get; set; }

        public TimesheetSummary()
        {
            ClientContact = new UserContact();
        }
    }

    public class DbTimesheetForMapping
    {
        public int TimesheetID;
        public string timesheettype;
        public string timesheetStatus;
        public string contractDesc;
        public int ContractID;
        public string payPeriod;
        public string CompanyName;
        public bool iscpgsubmission;
        public bool vacation;
        public string pdfQueryString;
        public string submittedpdf;
        public string timesheet_note;
        public bool isoverride;
        public bool isResendEnabled;
        public float tsHours;
        public float varHrs;
        public string Overridecolor;
        public string statusTitleString;
    }

    public class DbOpenTimesheetFromForMapping
    {
        public string timesheetstatus;
        public int TimesheetTempID;
        public DateTime tsStartDate;
        public DateTime tsEndDate;
        public string timesheetPeriod;
        public int timesheetid;
        public string directreportname ;
        public string ContractDirectReportName;
        public int billperiodsorder;
        public string timesheet_note;
        public string timesheettype;
        public int agreementid;
        public int contractid;
        public string ClientName;
        public float TSSaveHours;
        public bool IsEnabled;
        public string valueString;
        public string valueStringZero;
        public string pdfQueryString;
        public int chk_count;
        public bool TSSubmitOrSavedFlag;
        public int GetContracts;
        public int GetContracts_Pending;
    }

	public class Timesheet
	{
        public int Id { get; set; }
        public string ClientName { get; set; }
        public int AgreementId { get; set; }
        public int ContractId { get; set; }
        public DateTime AgreementStartDate { get; set; }
        public DateTime AgreementEndDate { get; set; }
        public float TotalHours { get; set; }

        public MatchGuideConstants.TimesheetStatus Status { get; set; }
		public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DirectReport TimesheetApprover { get; set; }

        //todo: should time period properties be contained within a seperate object?
		public string TimePeriod { get { return string.Format("{0:MMM d} - {1:MMM d}", StartDate, EndDate); } }
        public int AvailableTimePeriodId { get; set; }

        public int OpenStatusId { get; set; }

	    public List<TimeEntry> TimeEntries { get; set; }
	}

    public class DirectReport
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public bool IsFrequentlyUsed { get; set; }
    }

    public class PayPeriod
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string TimePeriod { get { return string.Format("{0:MMM d} - {1:MMM d}", StartDate, EndDate); } }

        public IEnumerable<Timesheet> Timesheets { get; set; }
    }
}

