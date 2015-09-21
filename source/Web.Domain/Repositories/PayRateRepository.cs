using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiSystems.SharedModels;
using Dapper;
using SiSystems.ClientApp.Web.Domain;
using SiSystems.ClientApp.Web.Domain.Repositories;
using SiSystems.ClientApp.Web.Domain.Repositories.Search;

namespace SiSystems.ConsultantApp.Web.Domain.Repositories
{
    public interface IPayRateRepository
    {
        IEnumerable<PayRate> GetPayRates();
    }

    public class PayRateRepository : IPayRateRepository
    {
        public IEnumerable<PayRate> GetPayRates()
        {
            
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
              const string query =
                        @"SELECT RateDescription AS RateDescription
                              ,PayRate AS Rate
                          FROM Agreement_ContractRateDetail Dets";
                    

                var payRates = db.Connection.Query<PayRate>(query);

                return payRates;
             }
        }
    }
}
