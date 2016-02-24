using System;
using System.Collections.Generic;

namespace SiSystems.SharedModels
{
    public class ProjectCodeRateDetails
	{
        //public string ProjectCodeDescription { get; set; }
        //public int ProjectCodeId { get; set; }

        //public int PayRateId { get; set; }
        //public string RateAmount { get; set; }
        //public string RateDescription { get; set; }
        //public string RateType { get; set; }

        public int AgreementId { get; set; }
        public int ProjectId { get; set; }
        public int EinvoiceId { get; set; }
        public string ProjectDescription { get; set; }
        public string DisplayProjectId { get; set; }

        public int CompanyProjectID { get; set; }
        public int ContractProjectPOID { get; set; }
        public int POID { get; set; }
        public string PODescription { get; set; }
        public string DisplayPONumber { get; set; }
        public string PONumber { get; set; }
        public int companypoprojectmatrixid { get; set; }

        public int contractrateid { get; set; }
        public bool primaryrateterm { get; set; }
        public string ratedescription { get; set; }
        public string rateAmount { get; set; }
        public string ratetype { get; set; }
        public int EinvoiceType { get; set; }
    }
}