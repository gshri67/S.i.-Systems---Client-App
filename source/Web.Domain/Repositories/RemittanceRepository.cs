using System;
using System.Collections.Generic;
using System.Linq;
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

                        SELECT  tempTable.vchrnmbr as VoucherNumber,
		                        tempTable.docdate as DepositDate,
		                        tempTable.docnumbr as DocumentNumber,
                                tempTable.docamnt as Amount,
                                tempTable.source as Source,
                                tempTable.dbsource as DBSource
                                @candidateId as CandidateId

                        FROM @tablevar tempTable";

                var remittancesFromDB = db.Connection.Query<Remittance>(query, new
                {
                    candidateid = candidateId
                }).ToList();

                return remittancesFromDB;
            }
        }

        public IEnumerable<Remittance> GetRemittanceDataFromNonGP(int candidateId)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {

                const string query =
                      @"Declare @tablevar table(CandidateFullName VARCHAR(1000), CustomerFullName VARCHAR(1000), VendorID VARCHAR(200), SiRefNo VARCHAR(200), VoucherNumber VARCHAR(200), Depositdate DATETIME, ActualHours DECIMAL(10,2), PayRate VARCHAR(250), QuickPay VARCHAR(5), BaseSalary DECIMAL(10,2), BaseGST DECIMAL(10,2), BaseDiscountAmount DECIMAL(10,2), CreditBaseSalary DECIMAL(10,2), CreditGST DECIMAL(10,2), CreditDiscountAmount DECIMAL(10,2), Source VARCHAR(50), CandidateCompany VARCHAR(500))
                        insert into @tablevar(CandidateFullName, CustomerFullName, VendorID, SiRefNo, VoucherNumber, Depositdate, ActualHours, PayRate, QuickPay, BaseSalary, BaseGST, BaseDiscountAmount, CreditBaseSalary, CreditGST, CreditDiscountAmount, Source, CandidateCompany ) EXECUTE [dbo].[UspGetEREmittancesFromNonGP_TSAPP] 
                           @candidateid

                        SELECT  tempTable.CandidateFullName as CandidateFullName,
						tempTable.CustomerFullName as CustomerFullName,
						tempTable.VendorID as VendorID,
						tempTable.SiRefNo as SiRefNo,
						tempTable.VoucherNumber as VoucherNumber,
						tempTable.Depositdate as DepositDate,
						tempTable.ActualHours as ActualHours,
						tempTable.PayRate as PayRate,
						tempTable.QuickPay as QuickPay,
						tempTable.BaseSalary as BaseSalary,
						tempTable.BaseGST as BaseGST,
						tempTable.BaseDiscountAmount as BaseDiscountAmount,
						tempTable.CreditBaseSalary as CreditBaseSalary,
						tempTable.CreditGST as CreditGST,
						tempTable.CreditDiscountAmount as CreditDiscountAmount,
						tempTable.Source as Source,
						tempTable.CandidateCompany as CandidateCompany

                        FROM @tablevar tempTable";

                var remittancesFromDB = db.Connection.Query<Remittance>(query, new
                {
                    candidateid = candidateId
                }).ToList();

                return remittancesFromDB;
            }

        }


        public IEnumerable<Remittance> GetRemittancesForUser(int userId)
        {
            #if LOCAL 
                return TempRemittances;
            
            #endif

          


            var remittancesGP = GetRemittanceDataFromGP(userId);
            var remittancesNonGP = GetRemittanceDataFromNonGP(userId);

            /*
            foreach ( Remittance rmGP in remittancesGP.ToList() )
            {
                Remittance remittance = remittancesNonGP.Select(rm => rm.VoucherNumber = rmGP.VoucherNumber);

                if( remittance != null )
                    rmGp
            }
            */
            //Match pay rate with pay rate repo call and figure out start and end dates?

            return remittancesGP;

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
                        DocumentNumber = "6C94239",
                        CandidateId = "191844",
                        VoucherNumber = "330567",
                        Source = "pam",
                        DBSource = "sipar"
                    }
                    ,new Remittance
                    {
                        StartDate = Convert.ToDateTime("2015-07-16"),
                        EndDate =  Convert.ToDateTime("2015-07-31"),
                        DepositDate = Convert.ToDateTime("2015-08-03"),
                        Amount = (float) 2340.00,
                        DocumentNumber = "6D23490",
                        CandidateId = "191844",
                        VoucherNumber = "330567",
                        Source = "pam",
                        DBSource = "sipar"
                    }
                };
            }
        }
    }
}