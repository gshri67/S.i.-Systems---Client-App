using System;

namespace SiSystems.ClientApp.SharedModels
{
    public class ConsultantSummary
    {
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName { get { return FirstName + " " + LastName; } }
        
        //Is this going to come from a contract??
        public string StartDate { get; set; }
        public string EndDate { get; set; }

        public int Rating { get; set; }
        public Decimal Rate { get; set; }
        
        public ConsultantSummary(Consultant contractor)
        {
            Id = contractor.Id;
            FirstName = contractor.FirstName;
            LastName = contractor.LastName;
            StartDate = contractor.StartDate;
            EndDate = contractor.EndDate;
            Rating = contractor.MostRecentContractRating;
            Rate = contractor.MostRecentContractRate;
        }
    }
}
