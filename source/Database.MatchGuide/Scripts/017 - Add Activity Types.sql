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
           (284, 'DirectReportChange', 'Change of Direct Report', 'Direct Report Changed', 0, 1, 1, 0, -1),
		   (282, 'CandidatePropose', 'Proposing Candidate', 'Candidate Proposed', 0, 1, 1, 0, -1)
		   
GO

SET IDENTITY_INSERT [dbo].[ActivityType] OFF;


SET IDENTITY_INSERT [dbo].[ActivityTransaction] ON;

INSERT INTO [dbo].[ActivityTransaction]
           ([ActivityTransactionID]
		   ,ActivityTypeID
		   ,[CreateDateTime]
           ,[CreateUserID]
           ,[inactive]
		   ,verticalid)
     VALUES
		   (0, 282, '2013-12-11', 0, 0, 4)
		   
GO

SET IDENTITY_INSERT [dbo].[ActivityTransaction] OFF;