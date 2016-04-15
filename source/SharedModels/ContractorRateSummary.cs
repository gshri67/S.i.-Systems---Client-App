using System;

namespace SiSystems.SharedModels
{
    public class ContractorRateSummary
    {
        public int RateId { get; set; }
        public int ContractorId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get { return string.Format("{0} {1}", FirstName, LastName).Trim(); } }

        public float BillRate { get; set; }
        public float PayRate { get; set; }
        public float GrossMargin { get; set; }
        public float Markup { get; set; }
        public float Margin { get; set; }

        public string Description { get; set; }
        public string RateType { get; set; }
        public float HoursPerDay { get; set; }
        public bool IsPrimaryRate { get; set; }

        public DateTime Date { get; set; } 

        public ContractorRateSummary()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            BillRate = 0;
            PayRate = 0;
            GrossMargin = 0;
            Markup = 0;
            Margin = 0;

            Date = new DateTime();
        }
    }
}