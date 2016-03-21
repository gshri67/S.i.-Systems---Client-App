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
	************************************************************ Create Historical Pam Table ******************************************************
*/

CREATE TABLE [dbo].[Historical_SISGP_PAM10501](
	[BTCHID] [char](15) NOT NULL,
	[SEQNUMBR] [int] NOT NULL,
	[TEMPID] [char](15) NOT NULL,
	[CUSTNMBR] [char](15) NOT NULL,
	[JOBNUMBR] [char](17) NOT NULL,
	[DATE1] [datetime] NOT NULL,
	[HOURSWKD] [int] NOT NULL,
	[TIMEWRKD] [int] NOT NULL,
	[TIMEID] [char](9) NOT NULL,
	[PAYRATE] [numeric](19, 5) NOT NULL,
	[PAYTYPID] [char](11) NOT NULL,
	[PAYAMT] [numeric](19, 5) NOT NULL,
	[PAYTXAMT] [numeric](19, 5) NOT NULL,
	[OPAYAMT] [numeric](19, 5) NOT NULL,
	[OPYTXAMT] [numeric](19, 5) NOT NULL,
	[INVCDDT] [datetime] NOT NULL,
	[INVCNMBR] [char](21) NOT NULL,
	[RMDTYPAL] [smallint] NOT NULL,
	[TOTALFEE] [numeric](19, 5) NOT NULL,
	[FEETXAMT] [numeric](19, 5) NOT NULL,
	[OTOTALFE] [numeric](19, 5) NOT NULL,
	[OTFTXAMT] [numeric](19, 5) NOT NULL,
	[DOCTYPE] [smallint] NOT NULL,
	[VCHRNMBR] [char](21) NOT NULL,
	[ONCOSTAM] [numeric](19, 5) NOT NULL,
	[DAYSWRDK] [int] NOT NULL,
	[STATE] [char](29) NOT NULL,
	[PAM_Invoice_Grouping] [char](61) NOT NULL,
	[USERDEF1] [char](21) NOT NULL,
	[USERDEF2] [char](21) NOT NULL,
	[USRDEF03] [char](21) NOT NULL,
	[NOTEINDX] [numeric](19, 5) NOT NULL,
	[POSITION_ID] [char](15) NOT NULL,
	[PYTIMEID] [char](9) NOT NULL,
	[PAM_Time_Billed] [int] NOT NULL,
	[PAM_Hours_Billed] [int] NOT NULL,
	[JRNENTRY] [int] NOT NULL,
	[CHEKNMBR] [char](21) NOT NULL,
	[PAM_GL_Audit_Trail_Code] [char](13) NOT NULL,
	[PAM_RM_TRX_Source] [char](13) NOT NULL,
	[PAM_PM_TRX_Source] [char](13) NOT NULL,
	[PAM_UPR_Audit_Control_Co] [char](13) NOT NULL,
	[PAM_Bill_Rate] [numeric](19, 5) NOT NULL,
	[CREATDDT] [datetime] NOT NULL,
	[CRUSRID] [char](15) NOT NULL,
	[MODIFDT] [datetime] NOT NULL,
	[MODTIME] [datetime] NOT NULL,
	[MDFUSRID] [char](15) NOT NULL,
	[INVOICBY] [char](15) NOT NULL,
	[CURNCYID] [char](15) NOT NULL,
	[CURRNIDX] [smallint] NOT NULL,
	[EXCHDATE] [datetime] NOT NULL,
	[TIME1] [datetime] NOT NULL,
	[XCHGRATE] [numeric](19, 7) NOT NULL,
	[EXGTBLID] [char](15) NOT NULL,
	[RATETPID] [char](15) NOT NULL,
	[RTCLCMTD] [smallint] NOT NULL,
	[DENXRATE] [numeric](19, 7) NOT NULL,
	[Creditor_Currency_ID] [char](15) NOT NULL,
	[Creditor_Currency_Index] [smallint] NOT NULL,
	[Creditor_EXCHDATE] [datetime] NOT NULL,
	[Creditor_TIME1] [datetime] NOT NULL,
	[Creditor_XCHGRATE] [numeric](19, 7) NOT NULL,
	[Creditor_EXGTBLID] [char](15) NOT NULL,
	[Creditor_RATETPID] [char](15) NOT NULL,
	[Creditor_RTCLCMTD] [smallint] NOT NULL,
	[Creditor_DENXRATE] [numeric](19, 7) NOT NULL,
	[TAXSCHID] [char](15) NOT NULL,
	[PORDNMBR] [char](21) NOT NULL,
	[SHFTCODE] [char](7) NOT NULL,
	[SHFTPREM] [numeric](19, 5) NOT NULL,
	[PAM_Payroll_Tax_Costs] [numeric](19, 5) NOT NULL,
	[PAM_Payroll_Benefit_Cost] [numeric](19, 5) NOT NULL,
	[PAM_Vendor_Costs] [numeric](19, 5) NOT NULL,
	[INVFORMT] [char](15) NOT NULL,
	[COMPTRNM] [int] NOT NULL,
	[HOLDCDID] [char](15) NOT NULL,
	[DCAMOUNT] [numeric](19, 5) NOT NULL,
	[PAM_Original_Cost] [numeric](19, 5) NOT NULL,
	[PAM_Discount_ID] [char](15) NOT NULL,
	[DISCAMNT] [numeric](19, 5) NOT NULL,
	[ORDDLRAT] [numeric](19, 5) NOT NULL,
	[Flat_Rate_Bill_Amount] [numeric](19, 5) NOT NULL,
	[ODCAAmount] [numeric](19, 5) NOT NULL,
	[OFRBAMT] [numeric](19, 5) NOT NULL,
	[PRCSSQNC] [smallint] NOT NULL,
	[Originating_OC_Amount] [numeric](19, 5) NOT NULL,
	[PAM_OriginalOnCost] [numeric](19, 5) NOT NULL,
	[DEX_ROW_ID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL
) ON [PRIMARY]

GO

/*
	************************************************************ Create Replication Pam Table ******************************************************
*/

CREATE TABLE [dbo].[Replication_PAM10501](
	[BTCHID] [char](15) NOT NULL,
	[SEQNUMBR] [int] NOT NULL,
	[TEMPID] [char](15) NOT NULL,
	[CUSTNMBR] [char](15) NOT NULL,
	[JOBNUMBR] [char](17) NOT NULL,
	[DATE1] [datetime] NOT NULL,
	[HOURSWKD] [int] NOT NULL,
	[TIMEWRKD] [int] NOT NULL,
	[TIMEID] [char](9) NOT NULL,
	[PAYRATE] [numeric](19, 5) NOT NULL,
	[PAYTYPID] [char](11) NOT NULL,
	[PAYAMT] [numeric](19, 5) NOT NULL,
	[PAYTXAMT] [numeric](19, 5) NOT NULL,
	[OPAYAMT] [numeric](19, 5) NOT NULL,
	[OPYTXAMT] [numeric](19, 5) NOT NULL,
	[INVCDDT] [datetime] NOT NULL,
	[INVCNMBR] [char](21) NOT NULL,
	[RMDTYPAL] [smallint] NOT NULL,
	[TOTALFEE] [numeric](19, 5) NOT NULL,
	[FEETXAMT] [numeric](19, 5) NOT NULL,
	[OTOTALFE] [numeric](19, 5) NOT NULL,
	[OTFTXAMT] [numeric](19, 5) NOT NULL,
	[DOCTYPE] [smallint] NOT NULL,
	[VCHRNMBR] [char](21) NOT NULL,
	[ONCOSTAM] [numeric](19, 5) NOT NULL,
	[DAYSWRDK] [int] NOT NULL,
	[STATE] [char](29) NOT NULL,
	[PAM_Invoice_Grouping] [char](129) NOT NULL,
	[USERDEF1] [char](21) NOT NULL,
	[USERDEF2] [char](21) NOT NULL,
	[USRDEF03] [char](21) NOT NULL,
	[NOTEINDX] [numeric](19, 5) NOT NULL,
	[POSITION_ID] [char](15) NOT NULL,
	[PYTIMEID] [char](9) NOT NULL,
	[PAM_Time_Billed] [int] NOT NULL,
	[PAM_Hours_Billed] [int] NOT NULL,
	[JRNENTRY] [int] NOT NULL,
	[CHEKNMBR] [char](21) NOT NULL,
	[PAM_GL_Audit_Trail_Code] [char](13) NOT NULL,
	[PAM_RM_TRX_Source] [char](13) NOT NULL,
	[PAM_PM_TRX_Source] [char](13) NOT NULL,
	[PAM_UPR_Audit_Control_Co] [char](13) NOT NULL,
	[PAM_Bill_Rate] [numeric](19, 5) NOT NULL,
	[CREATDDT] [datetime] NOT NULL,
	[CRUSRID] [char](15) NOT NULL,
	[MODIFDT] [datetime] NOT NULL,
	[MODTIME] [datetime] NOT NULL,
	[MDFUSRID] [char](15) NOT NULL,
	[INVOICBY] [char](15) NOT NULL,
	[CURNCYID] [char](15) NOT NULL,
	[CURRNIDX] [smallint] NOT NULL,
	[EXCHDATE] [datetime] NOT NULL,
	[TIME1] [datetime] NOT NULL,
	[XCHGRATE] [numeric](19, 7) NOT NULL,
	[EXGTBLID] [char](15) NOT NULL,
	[RATETPID] [char](15) NOT NULL,
	[RTCLCMTD] [smallint] NOT NULL,
	[DENXRATE] [numeric](19, 7) NOT NULL,
	[Creditor_Currency_ID] [char](15) NOT NULL,
	[Creditor_Currency_Index] [smallint] NOT NULL,
	[Creditor_EXCHDATE] [datetime] NOT NULL,
	[Creditor_TIME1] [datetime] NOT NULL,
	[Creditor_XCHGRATE] [numeric](19, 7) NOT NULL,
	[Creditor_EXGTBLID] [char](15) NOT NULL,
	[Creditor_RATETPID] [char](15) NOT NULL,
	[Creditor_RTCLCMTD] [smallint] NOT NULL,
	[Creditor_DENXRATE] [numeric](19, 7) NOT NULL,
	[TAXSCHID] [char](15) NOT NULL,
	[PORDNMBR] [char](21) NOT NULL,
	[SHFTCODE] [char](7) NOT NULL,
	[SHFTPREM] [numeric](19, 5) NOT NULL,
	[PAM_Payroll_Tax_Costs] [numeric](19, 5) NOT NULL,
	[PAM_Payroll_Benefit_Cost] [numeric](19, 5) NOT NULL,
	[PAM_Vendor_Costs] [numeric](19, 5) NOT NULL,
	[INVFORMT] [char](15) NOT NULL,
	[COMPTRNM] [int] NOT NULL,
	[HOLDCDID] [char](15) NOT NULL,
	[DCAMOUNT] [numeric](19, 5) NOT NULL,
	[PAM_Original_Cost] [numeric](19, 5) NOT NULL,
	[PAM_Discount_ID] [char](15) NOT NULL,
	[DISCAMNT] [numeric](19, 5) NOT NULL,
	[ORDDLRAT] [numeric](19, 5) NOT NULL,
	[Flat_Rate_Bill_Amount] [numeric](19, 5) NOT NULL,
	[ODCAAmount] [numeric](19, 5) NOT NULL,
	[OFRBAMT] [numeric](19, 5) NOT NULL,
	[PRCSSQNC] [smallint] NOT NULL,
	[Originating_OC_Amount] [numeric](19, 5) NOT NULL,
	[PAM_OriginalOnCost] [numeric](19, 5) NOT NULL,
	[PAMTSNUM] [char](15) NOT NULL,
	[Consolidated_Invoice] [char](21) NOT NULL,
	[State_ID] [char](15) NOT NULL,
	[SC_JournalEntry] [int] NOT NULL,
	[DEX_ROW_ID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL
) ON [PRIMARY]

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
