using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using Dapper;
using SiSystems.ClientApp.Web.Domain;
using SiSystems.ClientApp.Web.Domain.Repositories;
using SiSystems.ClientApp.Web.Domain.Repositories.Search;
using SiSystems.SharedModels;

namespace SiSystems.ConsultantApp.Web.Domain.Repositories
{
    public interface ITimesheetRepository
    {
        IEnumerable<Timesheet> GetTimesheetsForUser(int userId);
        IEnumerable<Timesheet> GetNonOpenTimesheetsForUser(int userId);
        IEnumerable<Timesheet> GetOpenTimesheetsForUser(int userId);

        int SaveTimesheet(Timesheet timesheet, int userId);
        int SubmitZeroTimeForUser(Timesheet timesheet, int userId);
        Timesheet GetTimesheetsById(int timesheetId);
        int SubmitTimesheet(Timesheet timesheet, int userId);
        void WithdrawTimesheet(int timesheetId, string cancelType, int createUserId, string cancelledPDFName, string cancelReason);
        DirectReport GetDirectReportByTimesheetId(int timesheetId);
        TimesheetSummarySet GetTimesheetSummaryByAccountExecutiveId(int id);
        IEnumerable<TimesheetDetails> GetOpenTimesheetDetailsByAccountExecutiveId(int id);
        IEnumerable<TimesheetDetails> GetSubmittedTimesheetDetailsByAccountExecutiveId(int id);
        IEnumerable<TimesheetDetails> GetCancelledTimesheetDetailsByAccountExecutiveId(int id);
        IEnumerable<TimesheetDetails> GetRejectedTimesheetDetailsByAccountExecutiveId(int id);
        TimesheetContact GetTimesheetContactById(int id);
        TimesheetContact GetOpenTimesheetContactByAgreementId(int id);
        string GetSubmittedPDFFromTimesheet(int timesheetId);
    }

    public class TimesheetRepository : ITimesheetRepository
    {
        public Timesheet GetTimesheetsById(int timesheetId)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string query =
                        @"SELECT TimeSheetID AS Id
                                ,Company.CompanyName AS ClientName
                                ,Times.AgreementId as ContractId
                                ,StatusID AS Status
                                ,Periods.TimeSheetAvailablePeriodStartDate AS StartDate
	                            ,Periods.TimeSheetAvailablePeriodEndDate AS EndDate
                                ,Periods.TimeSheetAvailablePeriodID AS AvailableTimePeriodId
	                            ,DirectReport.UserID Id
	                            ,DirectReport.FirstName FirstName
	                            ,DirectReport.LastName LastName
	                            ,DirectReportEmail.PrimaryEmail Email
                            FROM TimeSheet Times
                            LEFT JOIN Agreement ON Agreement.AgreementID = Times.AgreementID
                            LEFT JOIN Company ON Agreement.CompanyID = Company.CompanyID
                            LEFT JOIN TimeSheetAvailablePeriod Periods ON Times.TimeSheetAvailablePeriodID = Periods.TimeSheetAvailablePeriodID
                            LEFT JOIN Users DirectReport ON Times.DirectReportUserId = DirectReport.UserID
                            LEFT JOIN User_Email DirectReportEmail ON Times.DirectReportUserId = DirectReportEmail.UserID
                        WHERE Times.TimeSheetId = @TimesheetId
                        ORDER BY EndDate DESC, CompanyName";

                var retrievedTimesheet = db.Connection.Query<Timesheet, DirectReport, Timesheet>(query, (timesheet, directReport) =>
                {
                    timesheet.TimesheetApprover = directReport;
                    return timesheet;
                }, new { TimesheetId = timesheetId }).FirstOrDefault();

                return retrievedTimesheet;
            }
        }

        public IEnumerable<Timesheet> GetOpenTimesheetsForUser(int userId)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string query =
                    @"DECLARE @RC int
                    DECLARE @TimesheetType varchar(5)
                    SET @TimesheetType = 'ETS'
                    --Get a users Open Timesheets
                    EXECUTE @RC = [dbo].[UspGetOpenTSForCandidate_TSAPP] 
                       @candidateID
                      ,@TimesheetType";

                var timesheetsFromDb = db.Connection.Query<DbOpenTimesheetFromForMapping>(query, new { CandidateId = userId });

                return timesheetsFromDb.Select(ts => new Timesheet
                {
                    Id = ts.timesheetid,
                    OpenStatusId = ts.TimesheetTempID,
                    Status = MatchGuideConstants.TimesheetStatus.Open,
                    ClientName = ts.ClientName,
                    ContractId = ts.agreementid, //note that with this SP we could get ContractID or AgreementID
                    StartDate = ts.tsStartDate,
                    EndDate = ts.tsEndDate,
                    TimesheetApprover = ts.directreportname != null 
                        ? GetTimeSheetApproverByName(ts.directreportname) 
                        : GetTimeSheetApproverByName(ts.ContractDirectReportName) 
                }).ToList();
            }
        }



        public DirectReport GetTimeSheetApproverByName(string name)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string query =
                        @"SELECT DirectReport.UserID Id
	                        ,DirectReport.FirstName FirstName
	                        ,DirectReport.LastName LastName
	                        ,DirectReportEmail.PrimaryEmail Email
                        FROM Users DirectReport 
                        LEFT JOIN User_Email DirectReportEmail ON DirectReport.UserID= DirectReportEmail.UserID
                        WHERE Concat(DirectReport.FirstName,' ',DirectReport.LastName) LIKE @DirectReportName
                        ORDER BY DirectReport.UserID DESC";

                var directReport = db.Connection.Query<DirectReport>(query, new { DirectReportName = name }).FirstOrDefault();

                return directReport;
            }
        }

        public IEnumerable<Timesheet> GetNonOpenTimesheetsForUser(int userId)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string query =
                    @"DECLARE @RC int
                    EXECUTE @RC = [dbo].[UspViewTSForCandidate_TSAPP] 
                       @CandidateID";

                var timesheetsFromDb = db.Connection.Query<DbTimesheetForMapping>(query, new { CandidateId = userId });

                return timesheetsFromDb.Where(ts => ts.timesheetStatus != "Saved").Select(ts =>
                    new Timesheet
                    {
                        Id = ts.TimesheetID,
                        ContractId = GetAgreementIdByTimeSheetId(ts.TimesheetID), 
                        Status = (MatchGuideConstants.TimesheetStatus) ts.timesheetStatus, 
                        ClientName = ts.CompanyName, StartDate = StartDateFromPeriodDate(ts.payPeriod), 
                        EndDate = EndDateFromPeriodDate(ts.payPeriod)
                    }
                );
            }
        }

        public int GetAgreementIdByTimeSheetId(int timesheetId)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string query =
                    @"SELECT AgreementID FROM TimeSheet WHERE TimeSheetID = @TimesheetId";

                var agreementId = db.Connection.Query<int>(query, new { TimesheetId = timesheetId }).FirstOrDefault();

                return agreementId;
            }
        }

        private static string FormattedDateFromPeriodRange(string periodRange, bool shouldReturnStartDate)
        {
            //This is the format that we get from the DB: Feb 1-15 2016 and we want the first date from that. Feb 1, 2016
            var spaceSplit = periodRange.Split(' ');
            var month = spaceSplit.FirstOrDefault();
            var year = spaceSplit.LastOrDefault();
            var days = spaceSplit[1].Split('-');
            var dayToReturn = shouldReturnStartDate 
                ? days.FirstOrDefault() 
                : days.LastOrDefault();

            return string.Format("{0}-{1}-{2}", year, month, dayToReturn);
        }

        private static DateTime StartDateFromPeriodDate(string periodRange)
        {
            var startDateAsString = FormattedDateFromPeriodRange(periodRange, true);
            return DateTime.ParseExact(startDateAsString, "yyyy-MMM-d", System.Globalization.CultureInfo.InvariantCulture);
        }

        private static DateTime EndDateFromPeriodDate(string periodRange)
        {
            var endDateAsString = FormattedDateFromPeriodRange(periodRange, false);
            return DateTime.ParseExact(endDateAsString, "yyyy-MMM-d", System.Globalization.CultureInfo.InvariantCulture);
        }

        public IEnumerable<Timesheet> GetTimesheetsForUser(int userId)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string query =
                        @"SELECT TimeSheetID AS Id
                                ,Company.CompanyName AS ClientName
                                ,Times.AgreementId as ContractId
                                ,StatusID AS Status
                                ,Periods.TimeSheetAvailablePeriodStartDate AS StartDate
	                            ,Periods.TimeSheetAvailablePeriodEndDate AS EndDate
                                ,Periods.TimeSheetAvailablePeriodID AS AvailableTimePeriodId
	                            ,DirectReport.UserID Id
	                            ,DirectReport.FirstName FirstName
	                            ,DirectReport.LastName LastName
	                            ,DirectReportEmail.PrimaryEmail Email
                            FROM TimeSheet Times
                            LEFT JOIN Agreement ON Agreement.AgreementID = Times.AgreementID
                            LEFT JOIN Company ON Agreement.CompanyID = Company.CompanyID
                            LEFT JOIN TimeSheetAvailablePeriod Periods ON Times.TimeSheetAvailablePeriodID = Periods.TimeSheetAvailablePeriodID
                            LEFT JOIN Users DirectReport ON Times.DirectReportUserId = DirectReport.UserID
                            LEFT JOIN User_Email DirectReportEmail ON Times.DirectReportUserId = DirectReportEmail.UserID
                            WHERE CandidateUserID = 12
                            AND ResubmittedToID IS NULL
                            AND Periods.TimeSheetAvailablePeriodEndDate > DATEADD(MONTH, -6, GETDATE())
                            ORDER BY EndDate DESC, CompanyName";

                var timesheets = db.Connection.Query<Timesheet, DirectReport, Timesheet>(query, (timesheet, directReport) =>
                {
                    timesheet.TimesheetApprover = directReport;
                    return timesheet;
                } ,new { UserId = userId});

                return timesheets;
            }
        }

        public string GetSubmittedPDFFromTimesheet(int timesheetId)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string query =
                    @"SELECT submittedpdf
                      FROM TimeSheet
                      WHERE TimeSheetID = @timesheetId";

                var submittedpdf = db.Connection.Query<string>(query, new { TimesheetId = timesheetId }).FirstOrDefault();

                return submittedpdf;
            }
        }

        public void WithdrawTimesheet(int timesheetId, string cancelType, int createUserId, string cancelledPDFName, string cancelReason)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string query =
                    @"DECLARE @RC int
                        EXECUTE @RC = [dbo].[UspSetCancelTS_TSAPP] 
	                    @TimesheetId,
	                    @Canceltype,
	                    @createuserid,
	                    @CancelledPdfName,
	                    @timesheetcancelreason,
	                    @verticalId";

                db.Connection.Query(query, new
                {
                    TimesheetId = timesheetId,
                    Canceltype = cancelType,
                    createuserid = createUserId,
                    CancelledPdfName = cancelledPDFName,
                    timesheetcancelreason = cancelReason,
                    verticalId = MatchGuideConstants.VerticalId.IT
                }, commandType:CommandType.StoredProcedure);
            }
        }

        public int SaveTimesheet(Timesheet timesheet, int userId)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string query =
                    @"DECLARE @RC int
                        EXECUTE @RC = [dbo].[sp_TimesheetTemp_Insert] 
                            @aCandidateUserId
                            ,@aContractID
                            ,@aTSAvailablePeriodID
                            ,@aQuickPay
                            ,@aTSID
                            ,@aTSTempID
                            ,@verticalId";

                var savedTimesheetTempId = db.Connection.Query<int>(query, new
                {
                    aCandidateUserId  = userId,
                    aContractID = timesheet.ContractId,
                    aTSAvailablePeriodID = timesheet.AvailableTimePeriodId,
                    aQuickPay = (int?)null,
                    aTSID = IntegerOrNullIfZero(timesheet.Id),
                    aTSTempID = IntegerOrNullIfZero(timesheet.OpenStatusId),
                    verticalId = MatchGuideConstants.VerticalId.IT
                }).FirstOrDefault();

                return savedTimesheetTempId;
            }
        }

        public int SubmitZeroTimeForUser(Timesheet timesheet, int userId)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string query =
                    @"DECLARE @newTSID INT
                        DECLARE @RC int
                        EXECUTE @RC = [dbo].[sp_Timesheet_Update] 
                           @aTimesheetId
                          ,@aCandidateUserId
                          ,@aContractID
                          ,@aTimesheetavailableperiodid
                          ,@aTSSubmittedName
                          ,@verticalid
                          ,@aTimesheetType
                          ,@TSstatus
                     SET @newTSID =  @@identity                  
                     SELECT @newTSID as TimesheetId";

                var savedTimesheetTempId = db.Connection.Query<int>(query, new
                {
                    aTimesheetId = IntegerOrNullIfZero(timesheet.Id),
                    aCandidateUserId = userId,
                    aContractID = timesheet.ContractId,
                    aTimesheetavailableperiodid = timesheet.AvailableTimePeriodId,
                    aTSSubmittedName = (string)null, //Name of the Submitted PDF
                    verticalId = MatchGuideConstants.VerticalId.IT,
                    aTimesheetType = MatchGuideConstants.TimesheetType.ETimesheet.ToString(),
                    TSstatus = MatchGuideConstants.TimesheetStatus.Approved.ToString()
                }).FirstOrDefault();

                return savedTimesheetTempId;
            }
        }
        
        public int SubmitTimesheet(Timesheet timesheet, int userId)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string query =
                    @"DECLARE @RC int
                        EXECUTE @RC = [dbo].[sp_Timesheet_Insert] 
                           @aCandidateUserId
                          ,@aContractID
                          ,@aTSType
                          ,@aTSAvailablePeriodID
                          ,@aQuickPay
                          ,@aSubmittedPdfName
                          ,@aTSID
                          ,@aIsCPGSubmission
                          ,@verticalId
                          ,@aTimesheetType
                          ,@aSubmittedBy
                          ,@isSubmittedEmailSent
                          ,@aDirectReportid";

                var submittedTimesheetId = db.Connection.Query<int>(query, new
                {
                    aCandidateUserId = userId,
                    aContractID = timesheet.ContractId,
                    aTSType = MatchGuideConstants.TimesheetType.ETimesheet.ToString(), 
                    aTSAvailablePeriodID = timesheet.AvailableTimePeriodId,
                    aQuickPay = 0, 
                    aSubmittedPdfName = (string)null, //Name of the Submitted PDF
                    aTSID = IntegerOrNullIfZero(timesheet.Id),
                    aIsCPGSubmission = (bool?)null, 
                    verticalId = MatchGuideConstants.VerticalId.IT,
                    aTimesheetType = MatchGuideConstants.TimesheetType.ETimesheet.ToString(),
                    aSubmittedBy = userId,
                    isSubmittedEmailSent = true, 
                    aDirectReportid = timesheet.TimesheetApprover.Id
                }).FirstOrDefault();

                return submittedTimesheetId;
            }
        }

        public DirectReport GetDirectReportByTimesheetId(int timesheetId)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string query =
                        @"SELECT DirectReport.UserID Id
	                            ,DirectReport.FirstName FirstName
	                            ,DirectReport.LastName LastName
	                            ,DirectReportEmail.PrimaryEmail Email
                            FROM TimeSheet Times
                            LEFT JOIN Users DirectReport ON Times.DirectReportUserId = DirectReport.UserID
                            LEFT JOIN User_Email DirectReportEmail ON Times.DirectReportUserId = DirectReportEmail.UserID
                        WHERE Times.TimeSheetId = @TimesheetId";

                var retrievedTimesheet = db.Connection.Query<DirectReport>(query, new { TimesheetId = timesheetId }).FirstOrDefault();

                return retrievedTimesheet;
            }
        }

        private static int? IntegerOrNullIfZero(int number)
        {
            return number == 0 ? (int?)null : number;
        }

        public TimesheetSummarySet GetTimesheetSummaryByAccountExecutiveId(int id)
        {

            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                var openTimesheets = db.Connection.Query<int>(AccountExecutiveTimesheetQueries.OpenTimesheetsCountByAccountExecutiveId, new { Id = id }).FirstOrDefault();

                var cancelledTimesheets = db.Connection.Query<int>(AccountExecutiveTimesheetQueries.CancelledTimesheetsCountByAccountExecutiveId, new { Id = id }).FirstOrDefault();

                var rejectedTimesheets = db.Connection.Query<int>(AccountExecutiveTimesheetQueries.RejectedTimesheetsCountByAccountExecutiveId, new { Id = id }).FirstOrDefault();

                var submittedTimesheets = db.Connection.Query<int>(AccountExecutiveTimesheetQueries.SubmittedTimesheetsCountByAccountExecutiveId, new { Id = id }).FirstOrDefault();


                return
                    new TimesheetSummarySet()
                    {
                        NumOpen = openTimesheets,
                        NumCancelled = cancelledTimesheets,
                        NumRejected = rejectedTimesheets,
                        NumSubmitted = submittedTimesheets
                    };
            }
        }

        public IEnumerable<TimesheetDetails> GetOpenTimesheetDetailsByAccountExecutiveId(int id)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                var timesheets = db.Connection.Query<TimesheetDetails>(
                    AccountExecutiveTimesheetQueries.OpenTimesheetDetailsByAccountExecutiveId, new { Id = id });

                return timesheets;
            }
        }

        public IEnumerable<TimesheetDetails> GetSubmittedTimesheetDetailsByAccountExecutiveId(int id)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                var timesheets = db.Connection.Query<TimesheetDetails>(
                    AccountExecutiveTimesheetQueries.SubmittedTimesheetsByAccountExecutiveId, new { Id = id });

                return timesheets;
            }
        }

        public IEnumerable<TimesheetDetails> GetCancelledTimesheetDetailsByAccountExecutiveId(int id)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                var timesheets = db.Connection.Query<TimesheetDetails>(
                    AccountExecutiveTimesheetQueries.CancelledTimesheetsByAccountExecutiveId, new { Id = id });

                return timesheets;
            }
        }

        public IEnumerable<TimesheetDetails> GetRejectedTimesheetDetailsByAccountExecutiveId(int id)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                var timesheets = db.Connection.Query<TimesheetDetails>(
                    AccountExecutiveTimesheetQueries.RejectedTimesheetsByAccountExecutiveId, new { Id = id });

                return timesheets;
            }
        }

        //Get Open Timesheet with Agreement Id
        public TimesheetContact GetOpenTimesheetContactByAgreementId(int id)
        {
            const string timesheetContactQuery =
                @"SELECT Agreement.AgreementID AS AgreementId, 
	                    Company.CompanyName,
	                    PeriodDetails.TimeSheetAvailablePeriodStartDate AS StartDate,
	                    PeriodDetails.TimeSheetAvailablePeriodEndDate AS EndDate
                    FROM  Agreement
                    LEFT JOIN (
	                    /* Get the most recent TimeSheetAvailablePeriodId for the contract, if it exists */
	                    SELECT Agreement.AgreementID, MAX(MaxRecentPeriod.TimeSheetAvailablePeriodID) AS MostRecentApplicablePeriodId
	                    FROM Agreement
	                    INNER JOIN agreement_contractdetail acd on agreement.agreementid=acd.agreementid
	                    LEFT JOIN TimeSheetAvailablePeriod MaxRecentPeriod ON MaxRecentPeriod.TimeSheetPaymentType = acd.ContractPaymentPlanType
	                    WHERE Agreement.AgreementID = @Id
		                    AND 
		                    (
			                    MaxRecentPeriod.TimeSheetAvailablePeriodStartDate BETWEEN Agreement.StartDate AND Agreement.EndDate
			                    OR 
			                    Agreement.StartDate BETWEEN MaxRecentPeriod.TimeSheetAvailablePeriodStartDate  AND MaxRecentPeriod.TimeSheetAvailablePeriodEndDate
			                    OR
			                    Agreement.EndDate BETWEEN MaxRecentPeriod.TimeSheetAvailablePeriodStartDate  AND MaxRecentPeriod.TimeSheetAvailablePeriodEndDate
		                    )
	                    GROUP BY Agreement.AgreementID
                    ) Recent ON Agreement.AgreementID = Recent.AgreementID
                    LEFT JOIN TimeSheetAvailablePeriod PeriodDetails ON PeriodDetails.TimeSheetAvailablePeriodID = Recent.MostRecentApplicablePeriodId
                    LEFT JOIN Agreement_ContractDetail acd2 ON Agreement.AgreementID = acd2.AgreementID
                    LEFT JOIN TimeSheetAvailablePeriod StartingPeriod ON StartingPeriod.TimeSheetPaymentType = acd2.ContractPaymentPlanType
                    LEFT JOIN
                    (
	                    SELECT MaxTimeSheets.*
	                    FROM 
	                    (
		                    SELECT MAX(Timesheet.TimeSheetID) AS MaxTimeSheetIds
		                    FROM TimeSheet
		                    JOIN Agreement ON Agreement.AgreementID = TimeSheet.AgreementID
		                    WHERE Agreement.AgreementID = @Id
		                    GROUP BY Timesheet.AgreementID
	                    ) MaxIds
	                    LEFT JOIN TimeSheet MaxTimeSheets ON MaxTimeSheets.TimeSheetID = MaxIds.MaxTimeSheetIds
	                    LEFT JOIN PickList ON PickList.PickListID = MaxTimeSheets.StatusID
	                    WHERE PickList.Title NOT IN ('rejected','cancelled') --Ignore Rejected and Cancelled TimeSheets
                    ) MaxTimeSheets ON MaxTimeSheets.AgreementID = Agreement.AgreementID
                    LEFT JOIN pammanualtimesheet Pam ON Pam.agreementid = Agreement.AgreementID AND Pam.TimeSheetAvailablePeriodID = MostRecentApplicablePeriodId -- join Manual Entries on the Agreement and latest time period
                    LEFT JOIN Company ON Company.CompanyID = Agreement.CompanyID
                    WHERE Agreement.AgreementID = @Id
	                    AND agreement.agreementsubtype not in (select picklistid from dbo.udf_getpicklistids('contracttype','permanent',-1)) --The Agreement is not permanent
	                    AND acd2.timesheettype in (select picklistid from dbo.udf_getpicklistids('TimesheetType','ETimesheet',-1)) -- The agreement is set for ETimesheets
	                    AND StartingPeriod.TimeSheetAvailablePeriodID IS NOT NULL -- The Agreement has started (the first timesheet period has started)
	                    AND CONVERT(DATETIME, CONVERT(VARCHAR(10), Agreement.StartDate, 101)) -- 
		                    BETWEEN StartingPeriod.TimeSheetAvailablePeriodStartDate
			                    AND  StartingPeriod.TimeSheetAvailablePeriodEndDate
	                    AND Pam.agreementid IS NULL -- The latest available period has not been manually entered
	                    AND
	                    (
		                    MaxTimeSheets.TimeSheetID IS NULL -- No timesheet has been added for this agreement
		                    OR
		                    MaxTimeSheets.TimeSheetAvailablePeriodID < Recent.MostRecentApplicablePeriodId -- The latest timesheet added is not for the most recent timesheet period
	                    )";

            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                var contact = db.Connection.Query<TimesheetContact>(timesheetContactQuery, param: new { Id = id }).FirstOrDefault();

                return contact;
            }
        }

        //Get unopen timesheet with timesheet Id
        public TimesheetContact GetTimesheetContactById(int id)
        {
            const string timesheetContactQuery =
                @"SELECT TimeSheet.TimeSheetID AS Id, Agreement.AgreementID AS AgreementId, Company.CompanyName, Period.TimeSheetAvailablePeriodStartDate AS StartDate, Period.TimeSheetAvailablePeriodEndDate AS EndDate, TimeSheet.StatusID AS Status
                FROM TimeSheet 
                LEFT JOIN Agreement ON Agreement.AgreementID = TimeSheet.AgreementID
                LEFT JOIN Company on Company.CompanyID = Agreement.CompanyID
                LEFT JOIN TimeSheetAvailablePeriod Period ON Period.TimeSheetAvailablePeriodID = TimeSheet.TimeSheetAvailablePeriodID
                WHERE TimeSheet.TimeSheetID = @Id";

            const string consultantFromTimesheetId = 
                @"SELECT TimeSheet.CandidateUserID AS Id
                FROM TimeSheet 
                WHERE TimeSheet.TimeSheetID = @Id";

            const string directReportFromTimesheetId =
                @"SELECT DirectReport.UserID AS Id
                FROM TimeSheet 
                LEFT JOIN Agreement_ContractAdminContactMatrix Matrix ON Matrix.AgreementID = TimeSheet.AgreementID
                LEFT JOIN Users DirectReport ON DirectReport.UserID = Matrix.DirectReportUserID
                WHERE TimeSheet.TimeSheetID = @Id";

            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                var contact = db.Connection.Query<TimesheetContact>(timesheetContactQuery, param: new { Id = id }).FirstOrDefault();
                if (contact == null)
                    return new TimesheetContact();

                contact.Contractor = new UserContact{Id = db.Connection.Query<int>(consultantFromTimesheetId, param: new { Id = id }).FirstOrDefault()};
                contact.DirectReport = new UserContact { Id = db.Connection.Query<int>(directReportFromTimesheetId, param: new { Id = id }).FirstOrDefault() };

                return contact;
            }
        }
    }

    internal static class AccountExecutiveTimesheetQueries
    {
        private const string TimesheetCountByAccountExecutiveBaseQuery = 
            @"SELECT COUNT(TimeSheetId)
            FROM Agreement 
            JOIN PickList ON Agreement.StatusType = PickList.PickListID
            LEFT JOIN Timesheet ON Timesheet.AgreementID = Agreement.AgreementID
            WHERE Agreement.AccountExecID = @Id
            AND Agreement.AgreementType IN (SELECT PickListId FROM udf_GetPickListIds('agreementtype','contract',4))
            AND (
	            Agreement.AgreementSubType IN (SELECT PickListId FROM udf_GetPickListIds('contracttype','consultant,contract to hire',4))
	            OR
	            Agreement.AgreementSubType IN (SELECT PickListId FROM udf_GetPickListIds('contracttype','Flo Thru',4))
            )
            AND Timesheet.TimeSheetAvailablePeriodID IN (
	            SELECT TimeSheetAvailablePeriodID 
	            FROM TimeSheetAvailablePeriod Periods
	            WHERE DATEDIFF(MONTH, Periods.TimeSheetAvailablePeriodEndDate, GETDATE()) < 3
            )";

        private const string TimesheetDetailsByAccountExecutiveBaseQuery =
            @"SELECT TimeSheet.TimeSheetID as Id, Period.TimeSheetAvailablePeriodStartDate as StartDate, Period.TimeSheetAvailablePeriodEndDate as EndDate, Company.CompanyName, Users.FirstName +' '+ Users.LastName AS ContractorFullName
            FROM Agreement 
            JOIN PickList ON Agreement.StatusType = PickList.PickListID
            LEFT JOIN Timesheet ON Timesheet.AgreementID = Agreement.AgreementID
            LEFT JOIN TimeSheetAvailablePeriod Period ON Period.TimeSheetAvailablePeriodID = TimeSheet.TimeSheetAvailablePeriodID
            LEFT JOIN Company ON Company.CompanyID = Agreement.CompanyID
            LEFT JOIN Users ON Users.UserID = Agreement.CandidateID
            WHERE Agreement.AccountExecID = @Id
            AND Agreement.AgreementType IN (SELECT PickListId FROM udf_GetPickListIds('agreementtype','contract',4))
            AND (
	            Agreement.AgreementSubType IN (SELECT PickListId FROM udf_GetPickListIds('contracttype','consultant,contract to hire',4))
	            OR
	            Agreement.AgreementSubType IN (SELECT PickListId FROM udf_GetPickListIds('contracttype','Flo Thru',4))
            )
            AND Timesheet.TimeSheetAvailablePeriodID IN (
	            SELECT TimeSheetAvailablePeriodID 
	            FROM TimeSheetAvailablePeriod Periods
	            WHERE DATEDIFF(MONTH, Periods.TimeSheetAvailablePeriodEndDate, GETDATE()) < 3
            )";

        private const string TimesheetStatusSubmittedFilter = 
                @"AND Timesheet.StatusID IN (
	            SELECT PickListID
	            FROM PickList 
	            WHERE PickList.PickTypeID IN (SELECT PickTypeID from PickType WHERE Type='TimeSheetStatusType')
	            AND PickList.Title IN ('Submitted')
            )";

        private const string TimesheetStatusRejectedFilter =
            @"AND Timesheet.StatusID IN (
	            SELECT PickListID
	            FROM PickList 
	            WHERE PickList.PickTypeID IN (SELECT PickTypeID from PickType WHERE Type='TimeSheetStatusType')
	            AND PickList.Title IN ('Rejected')
            )";

        private const string TimesheetStatusCancelledFilter =
            @"AND Timesheet.StatusID IN (
	            SELECT PickListID
	            FROM PickList 
	            WHERE PickList.PickTypeID IN (SELECT PickTypeID from PickType WHERE Type='TimeSheetStatusType')
	            AND PickList.Title IN ('Cancelled')
            )";

        private const string OpenTimeSheetsCountByAEQuery = 
            @"SELECT COUNT(DISTINCT(Agreement.AgreementID))
            FROM  Agreement
            LEFT JOIN (
	            /* Get the most recent TimeSheetAvailablePeriodId for the contract, if it exists */
	            SELECT Agreement.AgreementID, MAX(MaxRecentPeriod.TimeSheetAvailablePeriodID) AS MostRecentApplicablePeriodId
	            FROM Agreement
	            INNER JOIN agreement_contractdetail acd on agreement.agreementid=acd.agreementid
	            LEFT JOIN TimeSheetAvailablePeriod MaxRecentPeriod ON MaxRecentPeriod.TimeSheetPaymentType = acd.ContractPaymentPlanType
	            WHERE Agreement.AccountExecID = @Id
		            AND MaxRecentPeriod.TimeSheetAvailablePeriodStartDate BETWEEN Agreement.StartDate AND Agreement.EndDate
	            GROUP BY Agreement.AgreementID
            ) Recent ON Agreement.AgreementID = Recent.AgreementID
            LEFT JOIN Agreement_ContractDetail acd2 ON Agreement.AgreementID = acd2.AgreementID
            LEFT JOIN TimeSheetAvailablePeriod StartingPeriod ON StartingPeriod.TimeSheetPaymentType = acd2.ContractPaymentPlanType
            LEFT JOIN
            (
	            SELECT MaxTimeSheets.*
	            FROM 
	            (
		            SELECT MAX(Timesheet.TimeSheetID) AS MaxTimeSheetIds
		            FROM TimeSheet
		            JOIN Agreement ON Agreement.AgreementID = TimeSheet.AgreementID
		            WHERE Agreement.AccountExecID = @Id
		            GROUP BY Timesheet.AgreementID
	            ) MaxIds
	            LEFT JOIN TimeSheet MaxTimeSheets ON MaxTimeSheets.TimeSheetID = MaxIds.MaxTimeSheetIds
	            LEFT JOIN PickList ON PickList.PickListID = MaxTimeSheets.StatusID
	            WHERE PickList.Title NOT IN ('rejected','cancelled') --Ignore Rejected and Cancelled TimeSheets
            ) MaxTimeSheets ON MaxTimeSheets.AgreementID = Agreement.AgreementID
            LEFT JOIN pammanualtimesheet Pam ON Pam.agreementid = Agreement.AgreementID AND Pam.TimeSheetAvailablePeriodID = MostRecentApplicablePeriodId -- join Manual Entries on the Agreement and latest time period
            WHERE Agreement.AccountExecID = @Id
	            AND agreement.agreementsubtype not in (select picklistid from dbo.udf_getpicklistids('contracttype','permanent',-1)) --The Agreement is not permanent
	            AND acd2.timesheettype in (select picklistid from dbo.udf_getpicklistids('TimesheetType','ETimesheet',-1)) -- The agreement is set for ETimesheets
	            AND StartingPeriod.TimeSheetAvailablePeriodID IS NOT NULL -- The Agreement has started (the first timesheet period has started)
	            AND CONVERT(DATETIME, CONVERT(VARCHAR(10), Agreement.StartDate, 101)) -- 
		            BETWEEN StartingPeriod.TimeSheetAvailablePeriodStartDate
			            AND  StartingPeriod.TimeSheetAvailablePeriodEndDate
	            AND Pam.agreementid IS NULL -- The latest available period has not been manually entered
	            AND
	            (
		            MaxTimeSheets.TimeSheetID IS NULL -- No timesheet has been added for this agreement
		            OR
		            MaxTimeSheets.TimeSheetAvailablePeriodID < Recent.MostRecentApplicablePeriodId -- The latest timesheet added is not for the most recent timesheet period
	            )";

        public static string OpenTimesheetsCountByAccountExecutiveId
        {
            get { return string.Format("{0}", OpenTimeSheetsCountByAEQuery); }
        }

        private static string OpenTimesheetDetailsByAccountExecutiveIdQuery =
            @"SELECT Agreement.AgreementID AS AgreementId, 
	            Users.FirstName + ' ' + Users.LastName AS ContractorFullName,
	            Company.CompanyName,
	            PeriodDetails.TimeSheetAvailablePeriodStartDate AS StartDate,
	            PeriodDetails.TimeSheetAvailablePeriodEndDate AS EndDate
            FROM  Agreement
            LEFT JOIN (
	            /* Get the most recent TimeSheetAvailablePeriodId for the contract, if it exists */
	            SELECT Agreement.AgreementID, MAX(MaxRecentPeriod.TimeSheetAvailablePeriodID) AS MostRecentApplicablePeriodId
	            FROM Agreement
	            INNER JOIN agreement_contractdetail acd on agreement.agreementid=acd.agreementid
	            LEFT JOIN TimeSheetAvailablePeriod MaxRecentPeriod ON MaxRecentPeriod.TimeSheetPaymentType = acd.ContractPaymentPlanType
	            WHERE Agreement.AccountExecID = @Id
		            AND 
		            (
			            MaxRecentPeriod.TimeSheetAvailablePeriodStartDate BETWEEN Agreement.StartDate AND Agreement.EndDate
			            OR 
			            Agreement.StartDate BETWEEN MaxRecentPeriod.TimeSheetAvailablePeriodStartDate  AND MaxRecentPeriod.TimeSheetAvailablePeriodEndDate
			            OR
			            Agreement.EndDate BETWEEN MaxRecentPeriod.TimeSheetAvailablePeriodStartDate  AND MaxRecentPeriod.TimeSheetAvailablePeriodEndDate
		            )
	            GROUP BY Agreement.AgreementID
            ) Recent ON Agreement.AgreementID = Recent.AgreementID
            LEFT JOIN TimeSheetAvailablePeriod PeriodDetails ON PeriodDetails.TimeSheetAvailablePeriodID = Recent.MostRecentApplicablePeriodId
            LEFT JOIN Agreement_ContractDetail acd2 ON Agreement.AgreementID = acd2.AgreementID
            LEFT JOIN TimeSheetAvailablePeriod StartingPeriod ON StartingPeriod.TimeSheetPaymentType = acd2.ContractPaymentPlanType
            LEFT JOIN
            (
	            SELECT MaxTimeSheets.*
	            FROM 
	            (
		            SELECT MAX(Timesheet.TimeSheetID) AS MaxTimeSheetIds
		            FROM TimeSheet
		            JOIN Agreement ON Agreement.AgreementID = TimeSheet.AgreementID
		            WHERE Agreement.AccountExecID = @Id
		            GROUP BY Timesheet.AgreementID
	            ) MaxIds
	            LEFT JOIN TimeSheet MaxTimeSheets ON MaxTimeSheets.TimeSheetID = MaxIds.MaxTimeSheetIds
	            LEFT JOIN PickList ON PickList.PickListID = MaxTimeSheets.StatusID
	            WHERE PickList.Title NOT IN ('rejected','cancelled') --Ignore Rejected and Cancelled TimeSheets
            ) MaxTimeSheets ON MaxTimeSheets.AgreementID = Agreement.AgreementID
            LEFT JOIN pammanualtimesheet Pam ON Pam.agreementid = Agreement.AgreementID AND Pam.TimeSheetAvailablePeriodID = MostRecentApplicablePeriodId -- join Manual Entries on the Agreement and latest time period
            LEFT JOIN Users ON Users.UserID = Agreement.CandidateID
            LEFT JOIN Company ON Company.CompanyID = Agreement.CompanyID
            WHERE Agreement.AccountExecID = @Id
	            AND agreement.agreementsubtype not in (select picklistid from dbo.udf_getpicklistids('contracttype','permanent',-1)) --The Agreement is not permanent
	            AND acd2.timesheettype in (select picklistid from dbo.udf_getpicklistids('TimesheetType','ETimesheet',-1)) -- The agreement is set for ETimesheets
	            AND StartingPeriod.TimeSheetAvailablePeriodID IS NOT NULL -- The Agreement has started (the first timesheet period has started)
	            AND CONVERT(DATETIME, CONVERT(VARCHAR(10), Agreement.StartDate, 101)) -- 
		            BETWEEN StartingPeriod.TimeSheetAvailablePeriodStartDate
			            AND  StartingPeriod.TimeSheetAvailablePeriodEndDate
	            AND Pam.agreementid IS NULL -- The latest available period has not been manually entered
	            AND
	            (
		            MaxTimeSheets.TimeSheetID IS NULL -- No timesheet has been added for this agreement
		            OR
		            MaxTimeSheets.TimeSheetAvailablePeriodID < Recent.MostRecentApplicablePeriodId -- The latest timesheet added is not for the most recent timesheet period
	            )";

        public static string OpenTimesheetDetailsByAccountExecutiveId
        {
            get { return string.Format("{0}", OpenTimesheetDetailsByAccountExecutiveIdQuery); }
        }

        public static string CancelledTimesheetsCountByAccountExecutiveId
        {
            get { return string.Format("{1}{0}{2}", Environment.NewLine, TimesheetCountByAccountExecutiveBaseQuery, TimesheetStatusCancelledFilter); }
        }

        public static string RejectedTimesheetsCountByAccountExecutiveId
        {
            get { return string.Format("{1}{0}{2}", Environment.NewLine, TimesheetCountByAccountExecutiveBaseQuery, TimesheetStatusRejectedFilter); }
        }

        public static string SubmittedTimesheetsCountByAccountExecutiveId
        {
            get { return string.Format("{1}{0}{2}", Environment.NewLine, TimesheetCountByAccountExecutiveBaseQuery, TimesheetStatusSubmittedFilter); }
        }

        public static string SubmittedTimesheetsByAccountExecutiveId
        {
            get { return string.Format("{1}{0}{2}", Environment.NewLine, TimesheetDetailsByAccountExecutiveBaseQuery, TimesheetStatusSubmittedFilter); }
        }

        public static string CancelledTimesheetsByAccountExecutiveId
        {
            get { return string.Format("{1}{0}{2}", Environment.NewLine, TimesheetDetailsByAccountExecutiveBaseQuery, TimesheetStatusCancelledFilter); }
        }

        public static string RejectedTimesheetsByAccountExecutiveId
        {
            get { return string.Format("{1}{0}{2}", Environment.NewLine, TimesheetDetailsByAccountExecutiveBaseQuery, TimesheetStatusRejectedFilter); }
        }
    }
}
