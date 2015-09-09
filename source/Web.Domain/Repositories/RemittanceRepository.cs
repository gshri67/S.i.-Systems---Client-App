using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using SiSystems.ClientApp.Web.Domain;
using SiSystems.ClientApp.Web.Domain.Repositories;
using SiSystems.ClientApp.Web.Domain.Repositories.Search;
using SiSystems.SharedModels;

namespace SiSystems.ConsultantApp.Web.Domain.Repositories
{
    public interface IRemittanceRepository
    {
        IEnumerable<Remittance> GetRemittancesForUser(int userId);
    }

    public class RemittanceRepository : IRemittanceRepository
    {
        public IEnumerable<Remittance> GetRemittancesForUser(int userId)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string query = 
                        @"";
                
                //var remittances = db.Connection.Query<Remittance>(query, new { UserId = userId});



                return tempRemittances;
            }
        }

        private static IEnumerable<Remittance> tempRemittances
        {
            get
            {
                return new List<Remittance>
                {
                    new Remittance
                    {
                        StartDate = Convert.ToDateTime("2015-06-01"),
                        EndDate =  Convert.ToDateTime("2015-06-15"),
                        DepositDate = Convert.ToDateTime("2015-06-17"),
                        Amount = (float) 2653.50,
                        DocumentNumber = "6C94239"
                    }
                    ,new Remittance
                    {
                        StartDate = Convert.ToDateTime("2015-06-16"),
                        EndDate =  Convert.ToDateTime("2015-06-30"),
                        DepositDate = Convert.ToDateTime("2015-07-03"),
                        Amount = (float) 2653.50,
                        DocumentNumber = "6D23490"
                    }
                    ,new Remittance
                    {
                        StartDate = Convert.ToDateTime("2015-06-16"),
                        EndDate =  Convert.ToDateTime("2015-06-30"),
                        DepositDate = Convert.ToDateTime("2015-07-03"),
                        Amount = (float) 2653.50,
                        DocumentNumber = "6D23490"
                    }
                };
            }
        }
    }
}
