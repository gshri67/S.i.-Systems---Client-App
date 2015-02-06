using System;
using System.Collections.Generic;

namespace SiSystems.ClientApp.SharedModels
{
    public class Consultant
    {
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName { get { return FirstName + " " + LastName; } }
        
        public IList<Specialization> Specializations { get; set; }

        public Resume Resume { get; set; }
        public IList<Contract> Contracts { get; set; }

        public Consultant()
        {
            Specializations = new List<Specialization>();
            Contracts = new List<Contract>();
        }

    }
}
