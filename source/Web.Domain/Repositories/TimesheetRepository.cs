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
        IEnumerable<TimesheetDetails> GetOpenTimesheetDetailsByAccountExecutiveId(int id);
        IEnumerable<TimesheetDetails> GetSubmittedTimesheetDetailsByAccountExecutiveId(int id);
        IEnumerable<TimesheetDetails> GetCancelledTimesheetDetailsByAccountExecutiveId(int id);
        IEnumerable<TimesheetDetails> GetRejectedTimesheetDetailsByAccountExecutiveId(int id);
        TimesheetContact GetTimesheetContactById(int id);
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

            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                var openTimesheets = 0; //db.Connection.Query<int>(AccountExecutiveTimesheetQueries.OpenTimesheetsCountByAccountExecutiveId, new { Id = id }).FirstOrDefault();

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
            return new List<TimesheetDetails>()
            {
                new TimesheetDetails()
                {
                    Id = 1,
                    CompanyName = "Cenovus",
                    ContractorFullName = "Bob Smith",
                    StartDate = new DateTime(2015, 2, 12),
                    EndDate = new DateTime(2015, 2, 12)
                }
            }.AsEnumerable();
        }

        public IEnumerable<TimesheetDetails> GetSubmittedTimesheetDetailsByAccountExecutiveId(int id)
        {
            return new List<TimesheetDetails>()
            {
                new TimesheetDetails()
                {
                    Id = 1,
                    CompanyName = "Cenovus",
                    ContractorFullName = "Bob Smith",
                    StartDate = new DateTime(2015, 2, 12),
                    EndDate = new DateTime(2015, 2, 12)
                }
            }.AsEnumerable();
        }

        public IEnumerable<TimesheetDetails> GetCancelledTimesheetDetailsByAccountExecutiveId(int id)
        {
            return new List<TimesheetDetails>()
            {
                new TimesheetDetails()
                {
                    Id = 1,
                    CompanyName = "Cenovus",
                    ContractorFullName = "Bob Smith",
                    StartDate = new DateTime(2015, 2, 12),
                    EndDate = new DateTime(2015, 2, 12)
                }
            }.AsEnumerable();
        }

        public IEnumerable<TimesheetDetails> GetRejectedTimesheetDetailsByAccountExecutiveId(int id)
        {
            return new List<TimesheetDetails>()
            {
                new TimesheetDetails()
                {
                    Id = 1,
                    CompanyName = "Cenovus",
                    ContractorFullName = "Bob Smith",
                    StartDate = new DateTime(2015, 2, 12),
                    EndDate = new DateTime(2015, 2, 12)
                }
            }.AsEnumerable();
        }

        public TimesheetContact GetTimesheetContactById(int id)
        {
            return new TimesheetContact()
            {
                Id = 1,
                CompanyName = "Cenovus",
                StartDate = new DateTime(2015, 2, 2),
                EndDate = new DateTime(2015, 2, 9),
                Status = MatchGuideConstants.TimesheetStatus.Open,
                Contractor = new Contractor
                {
                    ContactInformation = new UserContact
                    {
                        Id = 1,
                        FirstName = "Robert",
                        LastName = "Paulson",
                        EmailAddresses = new List<EmailAddress>() { new EmailAddress { Email = "rp.clientcontact@email.com", Title = "Primary" } }.AsEnumerable(),
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
                    Rating = MatchGuideConstants.ResumeRating.Standard,
                    ResumeText = string.Empty,
                    Specializations = new List<Specialization>
                    {
                    },
                    Contracts = new List<ConsultantContract>
                    {
                        new ConsultantContract
                        {
                            ClientId = 1,
                            ContractId = 2,
                            ClientName = "Cenovus",
                            ContractorName = "Robert Paulson",
                            Title = string.Format("{0} - Project Manager", 59326),
                            AgreementSubType = MatchGuideConstants.AgreementSubTypes.FloThru,
                            StartDate = DateTime.UtcNow.AddDays(-15),
                            EndDate = DateTime.UtcNow.AddMonths(3).AddDays(5)
                        }
                    }
                },
                    DirectReport = new UserContact
                    {
                        Id = id,
                        FirstName = "Robert",
                        LastName = "Paulson",
                        EmailAddresses = new List<EmailAddress>() { new EmailAddress { Email = "rp.clientcontact@email.com", Title = "Primary" } }.AsEnumerable(),
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
                        ClientName = "Cenovus",
                        Address = "999 Rainbow Road SE, Calgary, AB"
                    }
            };
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

        public static string OpenTimesheetsCountByAccountExecutiveId
        {
            get { return string.Format("{1}{0}{2}", Environment.NewLine, TimesheetCountByAccountExecutiveBaseQuery); }
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
    }
}
