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
        public MatchGuideConstants.ResumeRating? Rating { get; set; }

        public DateTime MostRecentContractStartDate { get; set; }
        public DateTime MostRecentContractEndDate { get; set; }

        public Decimal MostRecentContractRate { get; set; }
        public bool RateWitheld { get; set; }

        public ConsultantSummary()
        {
            //Required for JSON deserializer
        }

        public ConsultantSummary(Consultant contractor, string specializationName=null)
        {
            Id = contractor.Id;
            FirstName = contractor.FirstName;
            LastName = contractor.LastName;
            Rating = contractor.Rating;


            var contracts = contractor.Contracts.AsEnumerable();
            if (specializationName != null)
                contracts = contracts.Where(c => c.SpecializationName == specializationName);
                
            var mostRecentContract=contracts.OrderByDescending(c => c.EndDate).FirstOrDefault();

            if (mostRecentContract != null)
            {
                MostRecentContractStartDate = mostRecentContract.StartDate;
                MostRecentContractEndDate = mostRecentContract.EndDate;
                MostRecentContractRate = mostRecentContract.Rate;
            }

        }
    }
}
