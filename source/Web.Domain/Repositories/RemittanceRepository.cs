using System;
using System.Collections.Generic;
using Dapper;
using SiSystems.ClientApp.Web.Domain.Repositories;
using SiSystems.SharedModels;

namespace SiSystems.ConsultantApp.Web.Domain.Repositories
{
    public interface IRemittanceRepository
    {
        IEnumerable<Remittance> GetRemittancesForUser(int userId);
    }

    public class RemittanceRepository : IRemittanceRepository
    {
        public IEnumerable<Remittance> GetRemittanceDataFromGP( int candidateId )
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {

              const string query =
                    @"Declare @tablevar table(vchrnmbr VARCHAR(50), docdate datetime, docnumbr VARCHAR(21), docamnt INT, source VARCHAR(250), dbsource VARCHAR(250))
                        insert into @tablevar(vchrnmbr, docdate, docnumbr, docamnt, source, dbsource ) EXECUTE [dbo].[UspGetEREmittancesFromGP_TSAPP] 
                           @candidateid

                        SELECT  tempTable.vchrnmbr as vchrnmbr,
		                        tempTable.docdate as docdate,
		                        tempTable.docnumbr as docnumbr,
                                tempTable.docamnt as docamnt,
                                tempTable.source as source,
                                tempTable.dbsource as dbsource

                        FROM @tablevar tempTable";

                var remittances = db.Connection.Query<Remittance>(query, new
                {
                    candidateid = candidateId
                });

                return remittances;
            }
             
        }


        public IEnumerable<Remittance> GetRemittancesForUser(int userId)
        {
            #if LOCAL 
                return TempRemittances;
            
            #endif

          
            return GetRemittanceDataFromGP(userId);

            /*
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                //const string query = 
                //        @"";
                
                //var remittances = db.Connection.Query<Remittance>(query, new { UserId = userId});
                //todo actually get the remittances from the DB
                if (userId != 12)
                {
                    return new List<Remittance>();
                }

                return TempRemittances;
            }*/
        }

        private static IEnumerable<Remittance> TempRemittances
        {
            get
            {
                return new List<Remittance>
                {
                    new Remittance
                    {
                        StartDate = Convert.ToDateTime("2015-07-01"),
                        EndDate =  Convert.ToDateTime("2015-07-15"),
                        DepositDate = Convert.ToDateTime("2015-07-17"),
                        Amount = (float) 2653.50,
                        DocumentNumber = "6C94239"
                    }
                    ,new Remittance
                    {
                        StartDate = Convert.ToDateTime("2015-07-16"),
                        EndDate =  Convert.ToDateTime("2015-07-31"),
                        DepositDate = Convert.ToDateTime("2015-08-03"),
                        Amount = (float) 2340.00,
                        DocumentNumber = "6D23490"
                    }
                };
            }
        }
    }
}
