using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using SiSystems.SharedModels;

namespace SiSystems.ClientApp.Web.Domain.Repositories.AccountExecutive
{
    public interface IPickListValuesRepository
    {
        IEnumerable<string> InvoiceFormatOptions();
        IEnumerable<string> InvoiceFrequencyOptions();
        IEnumerable<string> ContractLimitationOptions();
        IEnumerable<string> ContractPaymentPlans();
        IEnumerable<string> CandidatePaymentPlans();
        IEnumerable<string> RateTermTypes();
    }

    public class PickListValuesRepository : IPickListValuesRepository
    {
        private IEnumerable<string> GetPickListDisplayTitlesForType(string type)
        {
            const string pickListDisplayTitleForTypeQuery =
                @"SELECT PickList.DisplayTitle 
                FROM PickList
                LEFT JOIN PickType ON PickType.PickTypeID = PickList.PickTypeID
                WHERE PickType.Type = @Type
                AND PickType.Inactive = 0
                AND PickList.Inactive = 0
                ORDER BY PickList.isOrder";

            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                var values = db.Connection.Query<string>(pickListDisplayTitleForTypeQuery, new { Type = type });

                return values;
            }
        }

        public IEnumerable<string> InvoiceFormatOptions()
        {
            return GetPickListDisplayTitlesForType("InvoiceFormat");
        }

        public IEnumerable<string> InvoiceFrequencyOptions()
        {
            return GetPickListDisplayTitlesForType("ClientInvoiceFrequency");
        }

        public IEnumerable<string> ContractLimitationOptions()
        {
            return GetPickListDisplayTitlesForType("ContractLimitationType");
        }

        public IEnumerable<string> ContractPaymentPlans()
        {
            return GetPickListDisplayTitlesForType("ContractPaymentPlanType");
        }

        public IEnumerable<string> CandidatePaymentPlans()
        {
            return GetPickListDisplayTitlesForType("CandidatePaymentType");
        }

        public IEnumerable<string> RateTermTypes()
        {
            return GetPickListDisplayTitlesForType("RateTermType");
        }
    }

    public class MockPickListValuesRepository : IPickListValuesRepository
    {
        public IEnumerable<string> InvoiceFormatOptions()
        {
            return new List<string> {"1 Invoice Per Client", "1 Credit Per Client"};
        }

        public IEnumerable<string> InvoiceFrequencyOptions()
        {
            return new List<string> {"Monthly", "Weekly"};
        }

        public IEnumerable<string> ContractLimitationOptions()
        {
            return new List<string> {"Hours", "Amount"};
        }

        public IEnumerable<string> ContractPaymentPlans()
        {
            return new List<string> {"Monthly Standard Last Business Day","Semi-monthly 7/22","Term","I&SP"};
        }

        public IEnumerable<string> CandidatePaymentPlans()
        {
            return new List<string> {"Incorporated", "Sole-proprietorship", "Term"};
        }

        public IEnumerable<string> RateTermTypes()
        {
            return new List<string> {"Per Day", "Per Hour"};
        }
    }
}
