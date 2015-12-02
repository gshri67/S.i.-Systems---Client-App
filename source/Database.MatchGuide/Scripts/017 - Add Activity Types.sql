SET IDENTITY_INSERT [dbo].[ActivityType] ON;

INSERT INTO [dbo].[ActivityType]
           ([ActivityTypeID]
		   ,[ActivityTypeName]
           ,[PreExecutionDescription]
           ,[PostExecutionDescription]
           ,[Inactive]
           ,[Viewable]
           ,[Searchable]
           ,[ToMigrateTo]
           ,[verticalid])
     VALUES
           (284, 'DirectReportChange', 'Change of Direct Report', 'Direct Report Changed', 0, 1, 1, 0, -1)
GO

SET IDENTITY_INSERT [dbo].[ActivityType] OFF;
