using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using SiSystems.SharedModels;
using Dapper;
using SiSystems.ClientApp.Web.Domain;
using SiSystems.ClientApp.Web.Domain.Repositories;
using SiSystems.ClientApp.Web.Domain.Repositories.Search;

namespace SiSystems.ClientApp.Web.Domain.Repositories.AccountExecutive
{
    public interface IConsultantContractRepository
    {
        IEnumerable<ConsultantContractSummary> GetContractSummaryByAccountExecutiveId(int id);
        ContractSummarySet GetFloThruSummaryByAccountExecutiveId(int id);
        ContractSummarySet GetFullySourcedSummaryByAccountExecutiveId(int id);
        ConsultantContract GetContractDetailsById(int id);
        IEnumerable<ConsultantContract> GetContractsByContractorId(int id);

        int SubmitContract(Job job, ContractCreationDetails contract, Timesheet timesheet, int candidateUserId,
            string candidateEmail, int internalUserId, int timeFactorId, int paymentPlanId, int invoiceFormatId,
            int invoiceFrequencyId, int candidatePaymentId, int SAID);

        int SubmitRateTermDetailsForJob(User user, int rateTypeId, int agreementId, Rate rate, DateTime startDate,
            DateTime endDate, string PAMRateId);

        int SubmitAdminContactDetailsForJob(int agreementId, int internalUserId, int directReportId,
            int billingContactId);
    }

    public class ConsultantContractRepository : IConsultantContractRepository
    {
        public IEnumerable<ConsultantContractSummary> GetContractSummaryByAccountExecutiveId(int id)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string contractSummaryQuery = @"declare @ctdate datetime
		                                            declare @ctdate datetime = convert(datetime,convert(varchar(10),getdate(),101)) --Today in correct format
                                                    declare @date1 datetime = (select date1 from dbo.getttmdates_tvl(DATEPART(m, GETDATE()), DATEPART(YY, GETDATE()),@ctdate,1,'NTTM')) --The first of this month
                                                    declare @date2 datetime = (select date2 from dbo.getttmdates_tvl(DATEPART(m, GETDATE()), DATEPART(YY, GETDATE()),@ctdate,1,'NTTM')) --Today

                                                    SELECT Agreement.AgreementID AS ContractId,
	                                                    Candidate.FirstName + ' '+ Candidate.LastName AS ContractorName,
	                                                    Company.CompanyName AS ClientName,
	                                                    Details.JobTitle AS Title,
	                                                    Agreement.StartDate,
	                                                    Agreement.EndDate,
	                                                    CASE WHEN ISNUMERIC(Agreement.AgreementSubType) = 1 THEN CAST(Agreement.AgreementSubType AS INT) ELSE 0 END AS AgreementSubType
                                                    FROM 
                                                    (Select * 
                                                    from
	                                                    (select distinct agreement.agreementid
	                                                    from agreement 
	                                                    inner join users on users.userid=agreement.accountexecid
	                                                    left join agreement_contractdetail on agreement.agreementid=agreement_contractdetail.agreementid
	                                                    inner join Agreement_ContractRateDetail on Agreement_ContractRateDetail.agreementid=agreement.agreementid
		                                                    and Agreement_ContractRateDetail.primaryrateterm=1  
		                                                    and Agreement_ContractrateDetail.inactive = 0 
		                                                    and agreement.enddate is not null
	                                                    where isnull(agreement_contractdetail.SucceedingContractID,0)=0
		                                                    and users.UserID = @Id
		                                                    and convert(datetime,convert(varchar(10),agreement.enddate,101)) 
			                                                    between (select date1+1 from dbo.getttmdates_tvl(DATEPART(m, GETDATE()), DATEPART(YY, GETDATE()),@ctdate,1,'cmgpf'))
			                                                    and (select date2 from dbo.getttmdates_tvl(DATEPART(M, GETDATE()), DATEPART(YY, GETDATE()),@ctdate,2,'cmgpf'))
		                                                    and agreementtype in (select picklistid from dbo.udf_getpicklistids( 'agreementtype', 'contract',-1))
		                                                    AND Agreement.AgreementSubType IN (
			                                                    SELECT PickListId FROM udf_GetPickListIds('contracttype', 'consultant,contract to hire', 4)
		                                                    )
	                                                    ) as FullySourcedEnding
	                                                    UNION
	                                                    (
		                                                    select distinct agreement.agreementid
		                                                    from agreement 
			                                                    inner join users on users.userid=agreement.accountexecid
			                                                    left join agreement_contractdetail on agreement.agreementid=agreement_contractdetail.agreementid
			                                                    inner join Agreement_ContractRateDetail on Agreement_ContractRateDetail.agreementid=agreement.agreementid
				                                                    and Agreement_ContractRateDetail.primaryrateterm=1  	
				                                                    and Agreement_ContractrateDetail.inactive = 0 
				                                                    and agreement.enddate is not null
		                                                    where 
			                                                    isnull(agreement_contractdetail.preceedingcontractid,0)=0
			                                                    AND agreement.AccountExecID = @Id
			                                                    and convert(datetime,convert(varchar(10),agreement.StartDate,101)) 
				                                                    between (select date1+1 from dbo.getttmdates_tvl(DATEPART(m, GETDATE()), DATEPART(YY, GETDATE()),@ctdate,1,'cmgpf'))
				                                                    and (select date2 from dbo.getttmdates_tvl(DATEPART(M, GETDATE()), DATEPART(YY, GETDATE()),@ctdate,2,'cmgpf'))
			                                                    and agreementtype    in (select picklistid from dbo.udf_getpicklistids( 'agreementtype', 'contract',-1))
			                                                    and agreement.agreementsubtype not  in  (select picklistid from dbo.udf_getpicklistids( 'contracttype', 'permanent',-1))
			                                                    AND Agreement.AgreementSubType IN (
				                                                    SELECT PickListId FROM udf_GetPickListIds('contracttype', 'consultant,contract to hire', 4)
			                                                    )
	                                                    )
	                                                    UNION
	                                                    (
		                                                    select distinct agreement.agreementid
			                                                    from	agreement 
			                                                    inner join users on users.userid=agreement.accountexecid
			                                                    left join agreement_contractdetail on agreement.agreementid=agreement_contractdetail.agreementid
			                                                    inner join Agreement_ContractRateDetail on Agreement_ContractRateDetail.agreementid=agreement.agreementid
				                                                    and Agreement_ContractRateDetail.primaryrateterm=1  
				                                                    and Agreement_ContractrateDetail.inactive = 0 
				                                                    and agreement.enddate is not null
		                                                    where  isnull(agreement_contractdetail.SucceedingContractID,0)=0
				                                                    and users.UserID = @Id
				                                                    and convert(datetime,convert(varchar(10),agreement.enddate,101)) 
					                                                    between (select date1+1 from dbo.getttmdates_tvl(DATEPART(m, GETDATE()), DATEPART(YY, GETDATE()),@ctdate,1,'cmgpf'))
					                                                    and (select date2 from dbo.getttmdates_tvl(DATEPART(M, GETDATE()), DATEPART(YY, GETDATE()),@ctdate,2,'cmgpf'))
				                                                    and agreementtype in (select picklistid from dbo.udf_getpicklistids( 'agreementtype', 'contract',-1))
				                                                    AND Agreement.AgreementSubType IN (
					                                                    SELECT PickListId FROM udf_GetPickListIds('contracttype', 'consultant,contract to hire', 4)
				                                                    )
	                                                    )
	                                                    UNION
	                                                    (
		                                                    select distinct agreement.agreementid
			                                                    from	agreement 
			                                                    inner join users on users.userid=agreement.accountexecid
			                                                    left join agreement_contractdetail on agreement.agreementid=agreement_contractdetail.agreementid
			                                                    inner join Agreement_ContractRateDetail on Agreement_ContractRateDetail.agreementid=agreement.agreementid
				                                                    and Agreement_ContractRateDetail.primaryrateterm=1  
				                                                    and Agreement_ContractrateDetail.inactive = 0 
				                                                    and agreement.enddate is not null
		                                                    where  isnull(agreement_contractdetail.SucceedingContractID,0)=0
				                                                    and users.UserID = @Id
				                                                    and convert(datetime,convert(varchar(10),agreement.enddate,101)) 
					                                                    between (select date1+1 from dbo.getttmdates_tvl(DATEPART(m, GETDATE()), DATEPART(YY, GETDATE()),@ctdate,1,'cmgpf'))
					                                                    and (select date2 from dbo.getttmdates_tvl(DATEPART(M, GETDATE()), DATEPART(YY, GETDATE()),@ctdate,2,'cmgpf'))
				                                                    and agreementtype in (select picklistid from dbo.udf_getpicklistids( 'agreementtype', 'contract',-1))
			                                                    AND Agreement.AgreementSubType IN (
				                                                    SELECT PickListId FROM udf_GetPickListIds('contracttype', 'Flo Thru', 4)
			                                                    )
	                                                    )
	                                                    UNION
	                                                    (
                                                            SELECT DISTINCT Agreement.AgreementID
                                                                FROM agreement 
	                                                            inner join users on users.userid=agreement.AccountExecID
	                                                            inner join user_office on user_office.UserOfficeID=users.userofficeid
	                                                            inner join location on location.locationid = user_office.locationid
	                                                            inner join Agreement_ContractRateDetail on Agreement_ContractRateDetail.agreementid=agreement.agreementid
		                                                                and Agreement_ContractRateDetail.primaryrateterm=1  
		                                                                and Agreement_ContractrateDetail.inactive = 0 
	                                                            left join activitytransaction on agreement.agreementid=activitytransaction.agreementid
		                                                            and activitytransaction.ActivityTypeID in 
		                                                            (
			                                                            select ActivityTypeID 
			                                                            from  activitytype where ActivityTypeName='ContractCancel'
		                                                            )
                                                            where 
                                                            agreement.AccountExecID = @Id
                                                            and (
	                                                            agreement.StatusType in (select picklistid from dbo.udf_getpicklistids('ContractStatusType','Active,cancelled',-1))
	                                                            and convert(datetime,convert(varchar(10),agreement.enddate,101)) >= @date1 
	                                                            and convert(datetime,convert(varchar(10),agreement.startdate,101)) <= @date2
	                                                            and convert(datetime,convert(varchar(10),agreement.enddate,101)) >= @date2
                                                            )
                                                            and AgreementType    in (select picklistid from dbo.udf_GetPickListIds( 'AgreementType', 'Contract',-1))
                                                            and 
                                                            (
	                                                            AgreementSubType in (select picklistid from dbo.udf_GetPickListIds('ContractType','Flo Thru',-1))
	                                                            or
	                                                            AgreementSubType in (SELECT PickListId FROM udf_GetPickListIds('contracttype', 'consultant,contract to hire', 4))
                                                            )
	                                                    )) AS AllContracts
                                                    LEFT JOIN Agreement ON Agreement.AgreementID = AllContracts.AgreementID	
                                                    JOIN PickList ON  Agreement.StatusType = PickList.PickListID
                                                    JOIN Users AE ON Agreement.AccountExecID = AE.UserID
                                                    JOIN Agreement_ContractDetail Details ON Agreement.AgreementID = Details.AgreementID
                                                    JOIN Users Candidate ON Agreement.CandidateID = Candidate.UserID
                                                    JOIN Company ON Agreement.CompanyID = Company.CompanyID";


                var contracts = db.Connection.Query<ConsultantContractSummary>(contractSummaryQuery, new { Id = id });

                return contracts;
            }
        }
        
        public int GetNumberOfActiveFloThruContracts(int userId )
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string query =
                    @"declare @ctdate datetime = convert(datetime,convert(varchar(10),getdate(),101)) --Today in correct format
                        declare @date1 datetime = (select date1 from dbo.getttmdates_tvl(DATEPART(m, GETDATE()), DATEPART(YY, GETDATE()),@ctdate,1,'NTTM')) --The first of this month
                        declare @date2 datetime = (select date2 from dbo.getttmdates_tvl(DATEPART(m, GETDATE()), DATEPART(YY, GETDATE()),@ctdate,1,'NTTM')) --Today

                        SELECT COUNT(Distinct(Agreement.AgreementID))
                         FROM agreement 
	                        inner join users on users.userid=agreement.AccountExecID
	                        inner join user_office on user_office.UserOfficeID=users.userofficeid
	                        inner join location on location.locationid = user_office.locationid
	                        inner join Agreement_ContractRateDetail on Agreement_ContractRateDetail.agreementid=agreement.agreementid
		                         and Agreement_ContractRateDetail.primaryrateterm=1  
		                         and Agreement_ContractrateDetail.inactive = 0 
	                        left join activitytransaction on agreement.agreementid=activitytransaction.agreementid
		                        and activitytransaction.ActivityTypeID in 
		                        (
			                        select ActivityTypeID 
			                        from  activitytype where ActivityTypeName='ContractCancel'
		                        )
                        where 
                        agreement.AccountExecID = @UserId
                        and (
	                        agreement.StatusType in (select picklistid from dbo.udf_getpicklistids('ContractStatusType','Active,cancelled',-1))
	                        and convert(datetime,convert(varchar(10),agreement.enddate,101)) >= @date1 
	                        and convert(datetime,convert(varchar(10),agreement.startdate,101)) <= @date2
	                        and convert(datetime,convert(varchar(10),agreement.enddate,101)) >= @date2
                        )
                        and AgreementType    in (select picklistid from dbo.udf_GetPickListIds( 'AgreementType', 'Contract',-1))
                        and 
                        (
	                        AgreementSubType in (select picklistid from dbo.udf_GetPickListIds('ContractType','Flo Thru',-1))
                        )
                        ";

                var result = db.Connection.Query<int>(query, new
                {
                    UserId = userId

                }).FirstOrDefault();

                return result;
            }
        }

        public int GetNumberOfActiveFullySourcedContracts(int userId)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string query =
                    @"declare @ctdate datetime = convert(datetime,convert(varchar(10),getdate(),101)) --Today in correct format
                        declare @date1 datetime = (select date1 from dbo.getttmdates_tvl(DATEPART(m, GETDATE()), DATEPART(YY, GETDATE()),@ctdate,1,'NTTM')) --The first of this month
                        declare @date2 datetime = (select date2 from dbo.getttmdates_tvl(DATEPART(m, GETDATE()), DATEPART(YY, GETDATE()),@ctdate,1,'NTTM')) --Today

                        SELECT COUNT(Distinct(Agreement.AgreementID))
                         FROM agreement 
	                        inner join users on users.userid=agreement.AccountExecID
	                        inner join user_office on user_office.UserOfficeID=users.userofficeid
	                        inner join location on location.locationid = user_office.locationid
	                        inner join Agreement_ContractRateDetail on Agreement_ContractRateDetail.agreementid=agreement.agreementid
		                         and Agreement_ContractRateDetail.primaryrateterm=1  
		                         and Agreement_ContractrateDetail.inactive = 0 
	                        left join activitytransaction on agreement.agreementid=activitytransaction.agreementid
		                        and activitytransaction.ActivityTypeID in 
		                        (
			                        select ActivityTypeID 
			                        from  activitytype where ActivityTypeName='ContractCancel'
		                        )
                        where 
                        agreement.AccountExecID = @UserId
                        and (
	                        agreement.StatusType in (select picklistid from dbo.udf_getpicklistids('ContractStatusType','Active,cancelled',-1))
	                        and convert(datetime,convert(varchar(10),agreement.enddate,101)) >= @date1 
	                        and convert(datetime,convert(varchar(10),agreement.startdate,101)) <= @date2
	                        and convert(datetime,convert(varchar(10),agreement.enddate,101)) >= @date2
                        )
                        and AgreementType    in (select picklistid from dbo.udf_GetPickListIds( 'AgreementType', 'Contract',-1))
                        and 
                        (
	                        AgreementSubType in (SELECT PickListId FROM udf_GetPickListIds('contracttype', 'consultant,contract to hire', 4))
                        )
                        ";

                var result = db.Connection.Query<int>(query, new
                {
                    UserId = userId

                }).FirstOrDefault();

                return result;
            }
        }

        public ContractSummarySet GetFloThruSummaryByAccountExecutiveId(int id)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                var numActive = GetNumberOfActiveFloThruContracts(id);

                var numEnding = db.Connection.Query<int>(AccountExecutiveContractsQueries.NumberEndingFloThruContractsQuery, new { Id = id }).FirstOrDefault();

                var numStarting = db.Connection.Query<int>(AccountExecutiveContractsQueries.NumberStartingFloThruContractsQuery, new { Id = id }).FirstOrDefault();

                return new ContractSummarySet
                {
                    Current = numActive,
                    Starting = numStarting,
                    Ending = numEnding
                };
            }
        }

        public ContractSummarySet GetFullySourcedSummaryByAccountExecutiveId(int id)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                var numActive = GetNumberOfActiveFullySourcedContracts(id);

                var numEnding = db.Connection.Query<int>(AccountExecutiveContractsQueries.NumberEndingFullySourcedContractsQuery, new { Id = id }).FirstOrDefault();

                var numStarting = db.Connection.Query<int>(AccountExecutiveContractsQueries.NumberStartingFullySourcedContractsQuery, new { Id = id }).FirstOrDefault();
                
                return new ContractSummarySet
                {
                    Current = numActive,
                    Starting = numStarting,
                    Ending =  numEnding
                };
            }
        }

        public ConsultantContract GetContractDetailsById(int id)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string contractsQuery =
                    @"SELECT Agreement.AgreementID AS ContractId,
	                    Candidate.FirstName + ' '+ Candidate.LastName AS ContractorName,
	                    Company.CompanyName AS ClientName,
	                    Details.JobTitle AS Title,
	                    Agreement.StartDate,
	                    Agreement.EndDate,
	                    CASE WHEN ISNUMERIC(Agreement.AgreementSubType) = 1 THEN CAST(Agreement.AgreementSubType AS INT) ELSE 0 END AS AgreementSubType,
	                    Company.CompanyID ClientId,
	                    RateDetails.BillRate,
	                    RateDetails.PayRate,
	                    RateDetails.BillRate - RateDetails.PayRate AS GrossMargin
                    FROM Agreement
                    JOIN PickList ON  Agreement.StatusType = PickList.PickListID
                    JOIN Agreement_ContractDetail Details ON Agreement.AgreementID = Details.AgreementID
                    JOIN Users Candidate ON Agreement.CandidateID = Candidate.UserID
                    JOIN Company ON Agreement.CompanyID = Company.CompanyID
                    JOIN Agreement_ContractRateDetail RateDetails on Agreement.AgreementID = RateDetails.AgreementID
                    WHERE Agreement.AgreementID = @Id";

                var contract = db.Connection.Query<ConsultantContract>(contractsQuery, param: new { Id = id }).FirstOrDefault();

                return contract;
            }
        }

        public IEnumerable<ConsultantContract> GetContractsByContractorId(int id)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string contractsQuery =
                    @"SELECT Agreement.AgreementID AS ContractId,
	                    Candidate.FirstName + ' '+ Candidate.LastName AS ContractorName,
	                    Company.CompanyName AS ClientName,
	                    Details.JobTitle AS Title,
	                    Agreement.StartDate,
	                    Agreement.EndDate,
	                    CASE WHEN ISNUMERIC(Agreement.AgreementSubType) = 1 THEN CAST(Agreement.AgreementSubType AS INT) ELSE 0 END AS AgreementSubType,
	                    Company.CompanyID ClientId,
	                    RateDetails.BillRate,
	                    RateDetails.PayRate,
	                    RateDetails.BillRate - RateDetails.PayRate AS GrossMargin
                    FROM Agreement
                    JOIN PickList ON  Agreement.StatusType = PickList.PickListID
                    JOIN Agreement_ContractDetail Details ON Agreement.AgreementID = Details.AgreementID
                    JOIN Users Candidate ON Agreement.CandidateID = Candidate.UserID
                    JOIN Company ON Agreement.CompanyID = Company.CompanyID
                    JOIN Agreement_ContractRateDetail RateDetails on Agreement.AgreementID = RateDetails.AgreementID
                    WHERE Agreement.CandidateID = @Id";

                var contracts = db.Connection.Query<ConsultantContract>(contractsQuery, param: new { Id = id });

                return contracts;
            }
        }

        public int SubmitContract( Job job, ContractCreationDetails contract, Timesheet timesheet, int candidateUserId, string candidateEmail, int internalUserId, int timeFactorId, int paymentPlanId, int invoiceFormatId, int invoiceFrequencyId, int candidatePaymentId, int SAID )
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string query =
                    @"DECLARE @RC int
                        EXECUTE @RC = [dbo].[USPSetCreateNewFSContractInsert] 
                           @opportunityid
                          ,@internaluserid
                          ,@startdate
                          ,@enddate
                          ,@accountexec
                          ,@revenueaccountexec
                          ,@Commissionaccountexec
                          ,@contactuserid
                          ,@candidateuserid
                          ,@candidateemail
                          ,@timefactortype
                          ,@paymentplantype
                          ,@cancellation
                          ,@jobtitle
                          ,@activetimesheet
                          ,@activeproject
                          ,@expenditure
                          ,@expenseexpenditure
                          ,@contractcandidatename
                          ,@contractclientcontactname
                          ,@TimeSheetType
                          ,@IsThirdPartyBilling
                          ,@IsThridPartyPay
                          ,@ThirdPartyBillingCompany
                          ,@ThirdPartyPayCandidate
                          ,@InvoiceFrequency
                          ,@CandidatePaymentType
                          ,@VerticalID
                          ,@Limitationofcontracttype
                          ,@InvoiceFormatId
                          ,@HasProjectPO
                          ,@Quickpay
                          ,@SAID";

                var result = db.Connection.Query<int>(query, new
                {
                    opportunityid = job.Id,
                    internaluserid = internalUserId,
                    startdate = contract.StartDate,
                    enddate = contract.EndDate,
                    accountexec = contract.AccountExecutive.Id,
                    revenueaccountexec = contract.BillingContact.Id,
                    Commissionaccountexec = contract.ComissionAssigned.Id,
                    contactuserid = job.ClientContact.Id,
                    candidateuserid = candidateUserId,
                    candidateemail = candidateEmail,
                    timefactortype = timeFactorId,
                    paymentplantype = paymentPlanId,
                    cancellation = contract.DaysCancellation,
                    jobtitle = job.Title,
                    activetimesheet = 1,
                    activeproject = 1,
                    /*
                     @activetimesheet – A bit value for Timesheet Access field
@activeproject – A bit value for Timesheet Project Access field

                     */
                    expenditure = contract.LimitationExpense,
                    expenseexpenditure = contract.LimitationOfContractValue,
                    contractcandidatename = job.ClientName,
                    contractclientcontactname = job.ClientContact.ClientName,
                    TimeSheetType = MatchGuideConstants.TimesheetType.ETimesheet,
                    IsThirdPartyBilling = 0,
                    IsThridPartyPay = 0,
                    ThirdPartyBillingCompany = 0,
                    ThirdPartyPayCandidate = 0,
                    InvoiceFrequency = contract.InvoiceFrequency,
                    CandidatePaymentType = 714,
                    VerticalID = MatchGuideConstants.VerticalId.IT,
                    Limitationofcontracttype = contract.LimitationOfContractType,
                    InvoiceFormatId = invoiceFormatId,
                    HasProjectPO = contract.UsingProjectCode,
                    Quickpay = contract.UsingQuickPay,
                    SAID = SAID
                }).FirstOrDefault();

                return result;
            }
        }

        public int SubmitAdminContactDetailsForJob( int agreementId, int internalUserId, int directReportId, int billingContactId )
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string query =
                    @"DECLARE @RC int
                        EXECUTE @RC = [dbo].[sp_CON_Contract_AdminContact_Insert] 
                           @agreementid
                          ,@internaluserid
                          ,@directreport
                          ,@billingcontact
                          ,@VerticalID";

                var result = db.Connection.Query<int>(query, new
                {
                    agreementid = agreementId,
                    internaluserid = internalUserId,
                    directreport = directReportId,
                    billingContact = billingContactId,
                    VerticalID = MatchGuideConstants.VerticalId.IT

                }).FirstOrDefault();

                return result;
            }
        }

   
        public int SubmitRateTermDetailsForJob(User user, int rateTypeId, int agreementId, Rate rate, DateTime startDate, DateTime endDate, string PAMRateId )
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string query =
                    @"DECLARE @RC int
                        EXECUTE @RC = [dbo].[sp_CON_Contract_RateTerm_Insert] 
                       @internaluserid
                      ,@ratetermtype
                      ,@nbrhours
                      ,@description
                      ,@billrate
                      ,@payrate
                      ,@startdate
                      ,@enddate
                      ,@agreementid
                      ,@header
                      ,@comments
                      ,'ContractRateTerm'
                      ,@IsPrimary
                      ,@PAMRateID
                      ,@InActive
                      ,@AdminFee
                      ,@VerticalID";

                var result = db.Connection.Query<int>(query, new
                {
                    internaluserid = user.Id,
                    ratetermtype = rateTypeId,
                    nbrhours = rate.HoursPerDay,//todo: implement conditional logic around 7.5 hrs default or null -> need more information
                    description = rate.Description,
                    billrate = rate.BillRate,
                    payrate = rate.PayRate,
                    startdate = startDate,
                    enddate = endDate,
                    agreementid = agreementId,
                    header = string.Format( "Contract Created by {0}", user.FullName),
                    comments = string.Format("Rate Term for Contract ID"),//todo: add action Contract ID in correct position in string
                    IsPrimary = rate.IsPrimaryRate,
                    PAMRateID = PAMRateId,
                    InActive = 0,
                    AdminFee = 0,//<NetPay> for sole-prop and term candidates otherwise NULL
                    VerticalID = MatchGuideConstants.VerticalId.IT
                    //@TotalMarginContract - NULL ??? what is this field. It does not seem to be in our stored procedure
                }).FirstOrDefault();

                return result;
            }
        }
    }

    internal static class AccountExecutiveContractsQueries
    {
        public static string NumberStartingFullySourcedContractsQuery
        {
            get
            {
                return @"declare @ctdate datetime
                        select @ctdate=convert(datetime,convert(varchar(10),getdate(),101))      

                        select count(distinct agreement.agreementid) 
                        from agreement 
	                        inner join users on users.userid=agreement.accountexecid
	                        left join agreement_contractdetail on agreement.agreementid=agreement_contractdetail.agreementid
	                        inner join Agreement_ContractRateDetail on Agreement_ContractRateDetail.agreementid=agreement.agreementid
		                        and Agreement_ContractRateDetail.primaryrateterm=1  	
		                        and Agreement_ContractrateDetail.inactive = 0 
		                        and agreement.enddate is not null
                        where 
	                        isnull(agreement_contractdetail.preceedingcontractid,0)=0
	                        AND agreement.AccountExecID = @Id
	                        and convert(datetime,convert(varchar(10),agreement.StartDate,101)) 
		                        between (select date1+1 from dbo.getttmdates_tvl(DATEPART(m, GETDATE()), DATEPART(YY, GETDATE()),@ctdate,1,'cmgpf'))
		                        and (select date2 from dbo.getttmdates_tvl(DATEPART(M, GETDATE()), DATEPART(YY, GETDATE()),@ctdate,2,'cmgpf'))
	                        and agreementtype    in (select picklistid from dbo.udf_getpicklistids( 'agreementtype', 'contract',-1))
	                        and agreement.agreementsubtype not  in  (select picklistid from dbo.udf_getpicklistids( 'contracttype', 'permanent',-1))
                            AND Agreement.AgreementSubType IN (
	                            SELECT PickListId FROM udf_GetPickListIds('contracttype', 'consultant,contract to hire', 4)
                            )";
            }
        }

        public static string NumberEndingFullySourcedContractsQuery
        {
            get
            {
                return @"declare @ctdate datetime
                        select @ctdate=convert(datetime,convert(varchar(10),getdate(),101))      

                        select 
	                        count(distinct agreement.agreementid) as NumberOfFullySourcedContracts
                            from	agreement 
	                        inner join users on users.userid=agreement.accountexecid
                            left join agreement_contractdetail on agreement.agreementid=agreement_contractdetail.agreementid
	                        inner join Agreement_ContractRateDetail on Agreement_ContractRateDetail.agreementid=agreement.agreementid
		                        and Agreement_ContractRateDetail.primaryrateterm=1  
		                        and Agreement_ContractrateDetail.inactive = 0 
		                        and agreement.enddate is not null
                        where  isnull(agreement_contractdetail.SucceedingContractID,0)=0
		                        and users.UserID = @Id
		                        and convert(datetime,convert(varchar(10),agreement.enddate,101)) 
			                        between (select date1+1 from dbo.getttmdates_tvl(DATEPART(m, GETDATE()), DATEPART(YY, GETDATE()),@ctdate,1,'cmgpf'))
			                        and (select date2 from dbo.getttmdates_tvl(DATEPART(M, GETDATE()), DATEPART(YY, GETDATE()),@ctdate,2,'cmgpf'))
		                        and agreementtype in (select picklistid from dbo.udf_getpicklistids( 'agreementtype', 'contract',-1))
		                        AND Agreement.AgreementSubType IN (
	                                SELECT PickListId FROM udf_GetPickListIds('contracttype', 'consultant,contract to hire', 4)
                                )";
            }
        }

        public static string NumberStartingFloThruContractsQuery
        {
            get
            {
                return @"declare @ctdate datetime
                        select @ctdate=convert(datetime,convert(varchar(10),getdate(),101))      

                        select count(distinct agreement.agreementid) 
                        from agreement 
	                        inner join users on users.userid=agreement.accountexecid
	                        left join agreement_contractdetail on agreement.agreementid=agreement_contractdetail.agreementid
	                        inner join Agreement_ContractRateDetail on Agreement_ContractRateDetail.agreementid=agreement.agreementid
		                        and Agreement_ContractRateDetail.primaryrateterm=1  	
		                        and Agreement_ContractrateDetail.inactive = 0 
		                        and agreement.enddate is not null
                        where 
	                        isnull(agreement_contractdetail.preceedingcontractid,0)=0
	                        AND agreement.AccountExecID = @Id
	                        and convert(datetime,convert(varchar(10),agreement.StartDate,101)) 
		                        between (select date1+1 from dbo.getttmdates_tvl(DATEPART(m, GETDATE()), DATEPART(YY, GETDATE()),@ctdate,1,'cmgpf'))
		                        and (select date2 from dbo.getttmdates_tvl(DATEPART(M, GETDATE()), DATEPART(YY, GETDATE()),@ctdate,2,'cmgpf'))
	                        and agreementtype    in (select picklistid from dbo.udf_getpicklistids( 'agreementtype', 'contract',-1))
	                        and agreement.agreementsubtype not  in  (select picklistid from dbo.udf_getpicklistids( 'contracttype', 'permanent',-1))
                            AND Agreement.AgreementSubType IN (
	                            SELECT PickListId FROM udf_GetPickListIds('contracttype', 'Flo Thru', 4)
                            )";
            }
        }

        public static string NumberEndingFloThruContractsQuery
        {
            get
            {
                return @"declare @ctdate datetime
                        select @ctdate=convert(datetime,convert(varchar(10),getdate(),101))      

                        select 
	                        count(distinct agreement.agreementid) as NumberOfFloThruContracts
                            from	agreement 
	                        inner join users on users.userid=agreement.accountexecid
                            left join agreement_contractdetail on agreement.agreementid=agreement_contractdetail.agreementid
	                        inner join Agreement_ContractRateDetail on Agreement_ContractRateDetail.agreementid=agreement.agreementid
		                        and Agreement_ContractRateDetail.primaryrateterm=1  
		                        and Agreement_ContractrateDetail.inactive = 0 
		                        and agreement.enddate is not null
                        where  isnull(agreement_contractdetail.SucceedingContractID,0)=0
		                        and users.UserID = @Id
		                        and convert(datetime,convert(varchar(10),agreement.enddate,101)) 
			                        between (select date1+1 from dbo.getttmdates_tvl(DATEPART(m, GETDATE()), DATEPART(YY, GETDATE()),@ctdate,1,'cmgpf'))
			                        and (select date2 from dbo.getttmdates_tvl(DATEPART(M, GETDATE()), DATEPART(YY, GETDATE()),@ctdate,2,'cmgpf'))
		                        and agreementtype in (select picklistid from dbo.udf_getpicklistids( 'agreementtype', 'contract',-1))
		                    AND Agreement.AgreementSubType IN (
	                            SELECT PickListId FROM udf_GetPickListIds('contracttype', 'Flo Thru', 4)
                            )";
            }
        }
    }

    public class MockedConsultantContractRepository : IConsultantContractRepository
    {
        public IEnumerable<ConsultantContractSummary> GetContractSummaryByAccountExecutiveId(int id)
        {
            
            return new List<ConsultantContractSummary>
            {
                FredAtNexenActiveFloThruContract,FredAtNexenEndingFloThruContract, FredAtNexenEndingFullySourcedContract,
                BarneyAtNexenActiveFloThruContract, BarneyAtNexenStartingFloThruContract, BarneyAtNexenStartingFullySourcedContract,
                BamBamAtCenovusActiveFloThruContract, BamBamAtCenovusActiveFullySourcedContract
            }.AsEnumerable();
        }

        private ConsultantContractSummary FredAtNexenActiveFloThruContract = new ConsultantContractSummary
        {
            ContractId = 1,
            ClientName = "Nexen",
            ContractorName = "Fred Flintstone",
            Title = string.Format("{0} - Job title with indepth description to indicate length", 60123),
            AgreementSubType = MatchGuideConstants.AgreementSubTypes.FloThru,
            StartDate = DateTime.UtcNow.AddMonths(-2).AddDays(-14),
            EndDate = DateTime.UtcNow.AddMonths(2).AddDays(10)
        };

        private ConsultantContractSummary BarneyAtNexenActiveFloThruContract = new ConsultantContractSummary
        {
            ContractId = 2,
            ClientName = "Nexen",
            ContractorName = "Barney Rubble",
            Title = string.Format("{0} - Project Manager", 59326),
            AgreementSubType = MatchGuideConstants.AgreementSubTypes.FloThru,
            StartDate = DateTime.UtcNow.AddDays(-15),
            EndDate = DateTime.UtcNow.AddMonths(3).AddDays(5)
        };

        private ConsultantContractSummary BamBamAtCenovusActiveFloThruContract = new ConsultantContractSummary
        {
            ContractId = 3,
            ClientName = "Cenovus",
            ContractorName = "BamBam Rubble",
            Title = string.Format("{0} - Solution Architect", 59950),
            AgreementSubType = MatchGuideConstants.AgreementSubTypes.FloThru,
            StartDate = DateTime.UtcNow.AddMonths(-1),
            EndDate = DateTime.UtcNow.AddMonths(2)
        };

        private ConsultantContractSummary FredAtNexenEndingFloThruContract = new ConsultantContractSummary
        {
            ContractId = 4,
            ClientName = "Nexen",
            ContractorName = "Fred Flantstone",
            Title = string.Format("{0} - Telecom Analyst/ Implementation Specialist", 63321),
            AgreementSubType = MatchGuideConstants.AgreementSubTypes.FloThru,
            StartDate = DateTime.UtcNow.AddMonths(-7),
            EndDate = DateTime.UtcNow.AddDays(10)
        };

        private ConsultantContractSummary BarneyAtNexenStartingFloThruContract = new ConsultantContractSummary
        {
            ContractId = 5,
            ClientName = "Nexen",
            ContractorName = "Barney Rabble",
            Title = string.Format("{0} - Project Analyst", 62123),
            AgreementSubType = MatchGuideConstants.AgreementSubTypes.FloThru,
            StartDate = DateTime.UtcNow.AddDays(15),
            EndDate = DateTime.UtcNow.AddMonths(3)
        };

        private ConsultantContractSummary BamBamAtCenovusActiveFullySourcedContract = new ConsultantContractSummary
        {
            ContractId = 6,
            ClientName = "Cenovus",
            ContractorName = "BamBam Rabble",
            Title = string.Format("{0} - ASP.NET MVC Developer", 59950),
            AgreementSubType = MatchGuideConstants.AgreementSubTypes.Consultant,
            StartDate = DateTime.UtcNow.AddMonths(-1),
            EndDate = DateTime.UtcNow.AddMonths(2)
        };

        private ConsultantContractSummary FredAtNexenEndingFullySourcedContract = new ConsultantContractSummary
        {
            ContractId = 7,
            ClientName = "Nexen",
            ContractorName = "Fred Flantstone",
            Title = string.Format("{0} - Job title with indepth description to indicate length", 63321),
            AgreementSubType = MatchGuideConstants.AgreementSubTypes.Consultant,
            StartDate = DateTime.UtcNow.AddMonths(-3),
            EndDate = DateTime.UtcNow.AddDays(15)
        };

        private ConsultantContractSummary BarneyAtNexenStartingFullySourcedContract = new ConsultantContractSummary
        {
            ContractId = 8,
            ClientName = "Nexen",
            ContractorName = "Barney Rabble",
            Title = string.Format("{0} - Project Analyst", 62123),
            AgreementSubType = MatchGuideConstants.AgreementSubTypes.Consultant,
            StartDate = DateTime.UtcNow.AddDays(15),
            EndDate = DateTime.UtcNow.AddMonths(3)
        };

        public ContractSummarySet GetFloThruSummaryByAccountExecutiveId(int id)
        {
            return new ContractSummarySet
            {
                Current = 3,
                Starting = 1,
                Ending = 1
            };
        }

        public ContractSummarySet GetFullySourcedSummaryByAccountExecutiveId(int id)
        {
            return new ContractSummarySet
            {
                Current = 1,
                Starting = 1,
                Ending = 1
            };
        }

        public ConsultantContract GetContractDetailsById(int id)
        {
            return ActiveMarketerAtNexenContract;
        }

        public IEnumerable<ConsultantContract> GetContractsByContractorId(int id)
        {
            return new List<ConsultantContract> {ActiveMarketerAtNexenContract};
        }

        public int SubmitContract(Job job, ContractCreationDetails contract, Timesheet timesheet, int candidateUserId,
            string candidateEmail, int internalUserId, int timeFactorId, int paymentPlanId, int invoiceFormatId,
            int invoiceFrequencyId, int candidatePaymentId, int SAID)
        {
            throw new NotImplementedException();
        }

        public int SubmitRateTermDetailsForJob(User user, int rateTypeId, int agreementId, Rate rate, DateTime startDate, DateTime endDate, string PAMRateId)
        {
            throw new NotImplementedException();
        }

        public int SubmitAdminContactDetailsForJob(int agreementId, int internalUserId, int directReportId, int billingContactId)
        {
            throw new NotImplementedException();
        }

        private static readonly Contractor StandardMarketer = new Contractor
        {
            ContactInformation = new UserContact
            {
                Id = 1,
                FirstName = "Jon",
                LastName = "Marketer",
                EmailAddresses = new List<EmailAddress>() { new EmailAddress { Email = "jm@email.com", Title = "Primary" } }.AsEnumerable(),
                PhoneNumbers = Enumerable.Empty<PhoneNumber>()
            },
            Rating = MatchGuideConstants.ResumeRating.Standard
        };

        private static readonly UserContact ContactJanice = new UserContact
        {
            FirstName = "Janice",
            LastName = "McContact",
            EmailAddresses = new List<EmailAddress>() { new EmailAddress { Email = "cc@email.com", Title = "Primary" } }.AsEnumerable(),
            PhoneNumbers = new List<PhoneNumber> 
                { 
                    new PhoneNumber
                    {
                        Title = "Work",
                        AreaCode = 555,
                        Prefix = 555,
                        LineNumber = 1231
                    }, new PhoneNumber
                    {
                        Title = "Cell",
                        AreaCode = 555,
                        Prefix = 222,
                        LineNumber = 2212
                    }
                }.AsEnumerable(),
        };

        private static readonly UserContact BillingContactWilliam = new UserContact
        {
            FirstName = "William",
            LastName = "Payerson",
            EmailAddresses = new List<EmailAddress>() { new EmailAddress { Email = "will.payerson@email.com", Title = "Primary" } }.AsEnumerable(),
            PhoneNumbers = new List<PhoneNumber> 
                { 
                    new PhoneNumber
                    {
                        Title = "Work",
                        AreaCode = 555,
                        Prefix = 555,
                        LineNumber = 1231
                    }, new PhoneNumber
                    {
                        Title = "Cell",
                        AreaCode = 555,
                        Prefix = 222,
                        LineNumber = 2212
                    }
                }.AsEnumerable(),
        };

        private static readonly UserContact DirectReportCandice = new UserContact
        {
            FirstName = "Candice",
            LastName = "Consulty",
            EmailAddresses = new List<EmailAddress>() { new EmailAddress { Email = "candice.consulty@email.com", Title = "Primary" } }.AsEnumerable(),
            PhoneNumbers = new List<PhoneNumber> 
                { 
                    new PhoneNumber
                    {
                        Title = "Work",
                        AreaCode = 555,
                        Prefix = 555,
                        LineNumber = 1231
                    }, new PhoneNumber
                    {
                        Title = "Cell",
                        AreaCode = 555,
                        Prefix = 222,
                        LineNumber = 2212
                    }
                }.AsEnumerable(),
        };

        private static readonly ConsultantContract ActiveMarketerAtNexenContract = new ConsultantContract
        {
            ContractId = 1,
            ClientName = "Nexen",
            ClientId = 1,
            AgreementSubType = MatchGuideConstants.AgreementSubTypes.Consultant,
            PayRate = 125,
            BillRate = 150,
            GrossMargin = 50,
            Title = "123456 - Telecom Analyst/ Implementation Specialist",
            Contractor = StandardMarketer,
            ClientContact = ContactJanice,
            BillingContact = BillingContactWilliam,
            DirectReport = DirectReportCandice,
            StartDate = DateTime.UtcNow.AddDays(-7),
            EndDate = DateTime.UtcNow.AddMonths(3)
        };
    }
}
