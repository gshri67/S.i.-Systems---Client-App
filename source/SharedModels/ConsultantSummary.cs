using System;
using System.Linq;

namespace SiSystems.ClientApp.SharedModels
{
    public class ConsultantSummary
    {
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName { get { return FirstName + " " + LastName; } }
        
        //Is this going to come from a contract??
        public string MostRecentContractStartDate { get; set; }
        public string MostRecentContractEndDate { get; set; }

        public int MostRecentContractRating { get; set; }
        public Decimal MostRecentContractRate { get; set; }
        
        public ConsultantSummary(Consultant contractor)
        {
            Id = contractor.Id;
            FirstName = contractor.FirstName;
            LastName = contractor.LastName;

            var mostRecentContract = contractor.Contracts.OrderByDescending(c => c.EndDate).FirstOrDefault();

            if (mostRecentContract != null)
            {
                MostRecentContractStartDate = mostRecentContract.StartDate;
                MostRecentContractEndDate = mostRecentContract.EndDate;
                MostRecentContractRate = mostRecentContract.Rate;
                MostRecentContractRating = mostRecentContract.Rating;
            }

        }
    }
}
