/*
	************************************************************ Create Activity Type Table ******************************************************
*/

CREATE TABLE [dbo].[ActivityType](
	[ActivityTypeID] [int] IDENTITY(1,1) NOT NULL,
	[ActivityTypeName] [nvarchar](50) NULL,
	[PreExecutionDescription] [nvarchar](255) NULL,
	[PostExecutionDescription] [nvarchar](255) NULL,
	[Inactive] [bit] NOT NULL,
	[Viewable] [bit] NULL,
	[Searchable] [bit] NOT NULL,
	[ToMigrateTo] [int] NULL,
	[verticalid] [int] NULL,
 CONSTRAINT [PK_ActivityType_2] PRIMARY KEY CLUSTERED 
(
	[ActivityTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 97) ON [PRIMARY]
) ON [PRIMARY]

GO


/*
	************************************************************ Create Activity Transaction Table ******************************************************
*/
CREATE TABLE [dbo].[ActivityTransaction](
	[ActivityTransactionID] [int] IDENTITY(1,1) NOT NULL,
	[ActivityTypeID] [int] NOT NULL,
	[CompanyID] [int] NULL,
	[CandidateUserID] [int] NULL,
	[ContactUserID] [int] NULL,
	[AgreementID] [int] NULL,
	[Complete] [bit] NULL,
	[StatusType] [int] NULL,
	[ActivityStatusChangeDateTime] [datetime] NULL,
	[CreateDateTime] [datetime] NOT NULL,
	[CreateUserID] [int] NOT NULL,
	[UpdateDateTime] [datetime] NULL,
	[UpdateUserID] [int] NULL,
	[FTMAgreementID] [int] NULL,
	[FTContractID] [int] NULL,
	[DueDateTime] [datetime] NULL,
	[AssociatedActivityTransactionID] [int] NULL,
	[inactive] [int] NOT NULL,
	[fromstatusid] [int] NULL,
	[tostatusid] [int] NULL,
	[verticalid] [int] NULL,
 CONSTRAINT [PK_ActivityTransaction] PRIMARY KEY CLUSTERED 
(
	[ActivityTransactionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO

/*
	************************************************************ Create Activity Transaction Note Table ******************************************************
*/

CREATE TABLE [dbo].[ActivityTransaction_Note](
	[ActivityTransactionID] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[AuthorUserID] [int] NULL,
	[TransactionSubject] [varchar](255) NULL,
	[Note] [text] NULL,
	[verticalid] [int] NULL,
 CONSTRAINT [PK_ActivityTransaction_Note] PRIMARY KEY CLUSTERED 
(
	[ActivityTransactionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/*
	************************************************************ Create Activity Type Constraints ******************************************************
*/

ALTER TABLE [dbo].[ActivityType] ADD  CONSTRAINT [DF_ActivityType_Inactive]  DEFAULT (0) FOR [Inactive]
GO

ALTER TABLE [dbo].[ActivityType] ADD  CONSTRAINT [DF_ActivityType_Viewable]  DEFAULT (0) FOR [Viewable]
GO

ALTER TABLE [dbo].[ActivityType] ADD  CONSTRAINT [DF_ActivityType_ToMigrateTo]  DEFAULT (0) FOR [ToMigrateTo]
GO

ALTER TABLE [dbo].[ActivityType]  WITH CHECK ADD  CONSTRAINT [chkverticalid_ActivityType] CHECK  (([verticalid] IS NOT NULL AND [verticalid]<>(0)))
GO

ALTER TABLE [dbo].[ActivityType] CHECK CONSTRAINT [chkverticalid_ActivityType]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identity for Activity Type' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ActivityType', @level2type=N'COLUMN',@level2name=N'ActivityTypeID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description of activity' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ActivityType', @level2type=N'COLUMN',@level2name=N'ActivityTypeName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description pre-execution' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ActivityType', @level2type=N'COLUMN',@level2name=N'PreExecutionDescription'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description post-execution' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ActivityType', @level2type=N'COLUMN',@level2name=N'PostExecutionDescription'
GO


/*
	************************************************************ Create Activity Transaction Constraints ******************************************************
*/

ALTER TABLE [dbo].[ActivityTransaction] ADD  CONSTRAINT [DF_ActivityTransaction_ActivityStatus]  DEFAULT (1) FOR [Complete]
GO

ALTER TABLE [dbo].[ActivityTransaction] ADD  CONSTRAINT [DF_ActivityTransaction_StatusType]  DEFAULT (659) FOR [StatusType]
GO

ALTER TABLE [dbo].[ActivityTransaction] ADD  CONSTRAINT [DF_ActivityTransaction_CreateDateTime]  DEFAULT (getdate()) FOR [CreateDateTime]
GO

ALTER TABLE [dbo].[ActivityTransaction] ADD  CONSTRAINT [DF_ActivityTransaction_DueDateTime]  DEFAULT (getdate()) FOR [DueDateTime]
GO

ALTER TABLE [dbo].[ActivityTransaction] ADD  CONSTRAINT [DF_ActivityTransaction_inactive]  DEFAULT ((0)) FOR [inactive]
GO

ALTER TABLE [dbo].[ActivityTransaction]  WITH NOCHECK ADD  CONSTRAINT [FK_ActivityTransaction_ActivityType] FOREIGN KEY([ActivityTypeID])
REFERENCES [dbo].[ActivityType] ([ActivityTypeID])
GO

ALTER TABLE [dbo].[ActivityTransaction] NOCHECK CONSTRAINT [FK_ActivityTransaction_ActivityType]
GO

ALTER TABLE [dbo].[ActivityTransaction]  WITH CHECK ADD  CONSTRAINT [chkverticalid_ActivityTransaction] CHECK  (([verticalid] IS NOT NULL AND [verticalid]<>(0)))
GO

ALTER TABLE [dbo].[ActivityTransaction] CHECK CONSTRAINT [chkverticalid_ActivityTransaction]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identity column' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ActivityTransaction', @level2type=N'COLUMN',@level2name=N'ActivityTransactionID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID relating to ActivityType table' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ActivityTransaction', @level2type=N'COLUMN',@level2name=N'ActivityTypeID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'UserID from Users table of candidate involved in transaction' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ActivityTransaction', @level2type=N'COLUMN',@level2name=N'CandidateUserID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'UserID from Users table of contact involved in transaction' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ActivityTransaction', @level2type=N'COLUMN',@level2name=N'ContactUserID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'AgreementtID from Agreements table for transaction' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ActivityTransaction', @level2type=N'COLUMN',@level2name=N'AgreementID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Pending = 0, Complete = 1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ActivityTransaction', @level2type=N'COLUMN',@level2name=N'Complete'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date status changed from previous status' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ActivityTransaction', @level2type=N'COLUMN',@level2name=N'ActivityStatusChangeDateTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date transaction was created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ActivityTransaction', @level2type=N'COLUMN',@level2name=N'CreateDateTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'UserID for internal user who created the transaction' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ActivityTransaction', @level2type=N'COLUMN',@level2name=N'CreateUserID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date transaction was updated' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ActivityTransaction', @level2type=N'COLUMN',@level2name=N'UpdateDateTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'UserID for internal user who updated the transaction' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ActivityTransaction', @level2type=N'COLUMN',@level2name=N'UpdateUserID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Flow through master agreement ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ActivityTransaction', @level2type=N'COLUMN',@level2name=N'FTMAgreementID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Flow through contract id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ActivityTransaction', @level2type=N'COLUMN',@level2name=N'FTContractID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Due date/time for transaction to occur' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ActivityTransaction', @level2type=N'COLUMN',@level2name=N'DueDateTime'
GO

/*
	************************************************************ Create Activity Transaction Note Constraints ******************************************************
*/

ALTER TABLE [dbo].[ActivityTransaction_Note] ADD  CONSTRAINT [DF_ActivityTransaction_Note_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO

ALTER TABLE [dbo].[ActivityTransaction_Note]  WITH NOCHECK ADD  CONSTRAINT [FK_ActivityTransaction_Note_ActivityTransaction] FOREIGN KEY([ActivityTransactionID])
REFERENCES [dbo].[ActivityTransaction] ([ActivityTransactionID])
GO

ALTER TABLE [dbo].[ActivityTransaction_Note] NOCHECK CONSTRAINT [FK_ActivityTransaction_Note_ActivityTransaction]
GO

ALTER TABLE [dbo].[ActivityTransaction_Note]  WITH NOCHECK ADD  CONSTRAINT [chkverticalid_ActivityTransaction_Note] CHECK  (([verticalid] IS NOT NULL AND [verticalid]<>(0)))
GO

ALTER TABLE [dbo].[ActivityTransaction_Note] CHECK CONSTRAINT [chkverticalid_ActivityTransaction_Note]
GO
