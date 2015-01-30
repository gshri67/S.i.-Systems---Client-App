using System;
using System.Collections.Generic;

namespace SiSystems.ClientApp.SharedModels
{
    public class Consultant
    {
        public int Id { get; set; }

        //this might come from contracts?
        public int ClientId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName { get { return FirstName + " " + LastName; } }
        
        //Is this going to come from a contract??
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        //How about these? contracts?
        public int Rating { get; set; }
        public Decimal Rate { get; set; }

        public Specialization Specialization { get; set; }
        public Resume Resume { get; set; }
        public IEnumerable<Contract> Contracts { get; set; }
        
    }
}
