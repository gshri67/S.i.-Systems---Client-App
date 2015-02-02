using System;

namespace SiSystems.ClientApp.SharedModels
{
    public class Contract
    {
        public int ConsultantId { get; set; }
        public int ClientId { get; set; }

        public string Title { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }

        public Decimal Rate { get; set; }

        public Contact Contact { get; set; }

        public int Rating { get; set; }
    }
}
