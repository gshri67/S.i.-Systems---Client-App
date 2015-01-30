using System;

namespace SiSystems.ClientApp.SharedModels
{
    public class Contract
    {
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Decimal Rate { get; set; }
        public Contact Contact { get; set; }
    }
}
