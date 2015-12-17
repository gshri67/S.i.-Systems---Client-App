using System;
using System.Collections.Generic;
using System.Linq;
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
        int SaveTimesheet(Timesheet timesheet, int userId);
        int SubmitZeroTimeForUser(Timesheet timesheet, int userId);
        Timesheet GetTimesheetsById(int timesheetId);
        int SubmitTimesheet(Timesheet timesheet, int userId);
        DirectReport GetDirectReportByTimesheetId(int timesheetId);
        TimesheetSummarySet GetTimesheetSummaryByAccountExecutiveId(int id);
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
                            ORDER BY EndDate DESC, CompanyName";

                var timesheets = db.Connection.Query<Timesheet, DirectReport, Timesheet>(query, (timesheet, directReport) =>
                {
                    timesheet.TimesheetApprover = directReport;
                    return timesheet;
                } ,new { UserId = userId});

                return timesheets;
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

            return
                new TimesheetSummarySet()
                {
                    Id = 1,
                    NumOpen = 5,
                    NumCancelled = 2,
                    NumRejected = 0,
                    NumSubmitted = 11
                };
            /*
            return new List<TimesheetSummary>
            {
                new TimesheetSummary
                {
                    Id = 1,
                    ClientContact = new UserContact
                    {
                        Id = 3,
                        FirstName = "Joe",
                        LastName = "Dirt",
                        EmailAddresses = new List<EmailAddress>() { new EmailAddress { Email = "jd@email.com", Title = "Primary"} }.AsEnumerable(),
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
                    },
                    StartDate = DateTime.UtcNow.AddDays(-30),
                    EndDate = DateTime.UtcNow.AddDays(-15),
                    Status = MatchGuideConstants.TimesheetStatus.Approved
                },
                new TimesheetSummary
                {
                    Id = 2,
                    ClientContact = new UserContact
                    {
                        Id = 3,
                        FirstName = "Joe",
                        LastName = "Dirt",
                        EmailAddresses = new List<EmailAddress>() { new EmailAddress { Email = "jd@email.com", Title = "Primary"} }.AsEnumerable(),
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
                    },
                    StartDate = DateTime.UtcNow.AddDays(-14),
                    EndDate = DateTime.UtcNow,
                    Status = MatchGuideConstants.TimesheetStatus.Submitted
                },
                new TimesheetSummary
                {
                    Id = 3,
                    ClientContact = new UserContact
                    {
                        Id = 4,
                        FirstName = "Marlin",
                        LastName = "Monrow",
                        EmailAddresses = new List<EmailAddress>() { new EmailAddress { Email = "mm@email.com", Title = "Primary"} }.AsEnumerable(),
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
                    },
                    StartDate = DateTime.UtcNow.AddDays(-30),
                    EndDate = DateTime.UtcNow.AddDays(-15),
                    Status = MatchGuideConstants.TimesheetStatus.Rejected
                },
                new TimesheetSummary
                {
                    Id = 4,
                    ClientContact = new UserContact
                    {
                        Id = 4,
                        FirstName = "Marlin",
                        LastName = "Monrow",
                        EmailAddresses = new List<EmailAddress>() { new EmailAddress { Email = "mm@email.com", Title = "Primary"} }.AsEnumerable(),
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
                    },
                    StartDate = DateTime.UtcNow.AddDays(-14),
                    EndDate = DateTime.UtcNow,
                    Status = MatchGuideConstants.TimesheetStatus.Open
                }
            };*/
        }
    }
}
