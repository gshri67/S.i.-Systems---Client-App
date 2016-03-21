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
    public interface IActivityRepository
    {
        int InsertUpdateRecordActivity(Timesheet timesheet, int userId);
    }

    public class ActivityRepository : IActivityRepository
    {
        public int InsertUpdateRecordActivity(Timesheet timesheet, int userId)
        {
            var typeId = GetActivityTypeId("DirectReportChange");
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string query =
                    @"DECLARE @RC int
                        EXECUTE @RC = [dbo].[sp_SC_ActivityTransaction_Generic] 
                           @internaluserid
                          ,@header
                          ,@note
                          ,@activitytype
                          ,@complete
                          ,@status
                          ,@AgreementID
                          ,@VerticalID";

                var insertedRecordId = db.Connection.Query<int>(query, new
                {
                    internaluserid = userId,
                    header = (string)null,
                    note = (string)null,
                    activitytype = typeId,
                    complete = (bool?)null,
                    status = (int?)null,
                    AgreementID = timesheet.AgreementId,
                    VerticalID = MatchGuideConstants.VerticalId.IT
                }).FirstOrDefault();

                return insertedRecordId;
            }
        }

        private int GetActivityTypeId(string typeName)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string query =
                        @"SELECT [ActivityTypeID]
                          FROM [dbo].[ActivityType]
                          WHERE [ActivityTypeName]=@TypeName";

                var typeId = db.Connection.Query<int>(query
                    , new
                    {
                        TypeName = typeName
                    }).FirstOrDefault();

                return typeId;
            }
        }
    }
}
