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
		   (282, 'CandidatePropose', 'Proposing Candidate', 'Candidate Proposed', 0, 1, 1, 0, -1),
		   (280, 'OpportunityCallout', 'Calling Out Candidate', 'Candidate Called Out', 0, 1, 1, 0, -1)
		   
GO

SET IDENTITY_INSERT [dbo].[ActivityType] OFF;


SET IDENTITY_INSERT [dbo].[ActivityTransaction] ON;

INSERT INTO [dbo].[ActivityTransaction]
           ([ActivityTransactionID]
		   ,[ActivityTypeID]
		   ,[AgreementID]
		   ,[CandidateUserID]
		   ,[CreateDateTime]
		   ,[UpdateDateTime]
           ,[CreateUserID]
           ,[inactive]
		   ,[verticalid])
     VALUES
		   (1 , 282, 1, 12, '2013-12-11', '2013-12-11', 0, 0, 4),
		   (2, 280, 2, 13, '2013-12-11', '2013-12-11', 0, 0, 4),
		   (3, 278, 3, 14, '2013-12-11', '2013-12-11', 0, 0, 4)
		   
GO

SET IDENTITY_INSERT [dbo].[ActivityTransaction] OFF;