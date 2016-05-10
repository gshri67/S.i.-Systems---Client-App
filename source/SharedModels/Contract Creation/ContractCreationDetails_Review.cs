using System;
using System.Collections.Generic;
using System.Linq;

namespace SiSystems.SharedModels
{
    public class ContractCreationDetails_Review
    {
        //Contract Terms
        public bool WebTimeSheetAccess { get; set; }
        public bool WebTimeSheetProjectAccess { get; set; }
        public string TimesheetType { get; set; }
        public string Vertical { get; set; }

        //Invoice Information
        public IEnumerable<string> InvoiceInformation { get; set; }

        //Associated Project and POs to New Contract
        public IEnumerable<string> AssociatedProjectAndPOs { get; set; }


        public ContractCreationDetails_Review()
        {
            WebTimeSheetAccess = true;
            WebTimeSheetProjectAccess = true;
            TimesheetType = "S.i. E-Timesheets";
            Vertical = "IT";
        }
    }
}