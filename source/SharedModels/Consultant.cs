using System.Collections.Generic;

namespace SiSystems.ClientApp.SharedModels
{
    public class Consultant
    {
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName { get { return FirstName + " " + LastName; } }

        public string EmailAddress { get; set; }
        
        public MatchGuideConstants.ResumeRating? Rating { get; set; }

        
        public string ResumeText { get; set; }

        public IEnumerable<Specialization> Specializations { get; set; }

        public IList<Contract> Contracts { get; set; }

        public Consultant()
        {
            Contracts = new List<Contract>();
        }

    }
}
