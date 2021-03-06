﻿CREATE TABLE [dbo].[TimeSheet](
	[TimeSheetID] [int] IDENTITY(1,1) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[CandidateUserID] [int] NOT NULL,
	[AgreementID] [int] NOT NULL,
	[TimeSheetAvailablePeriodID] [int] NULL,
	[QuickPay] [bit] NOT NULL,
	[Vacation] [bit] NULL,
	[StatusID] [int] NULL,
	[ResubmittedFromID] [int] NULL,
	[ResubmittedToID] [int] NULL,
	[ApproverRejectorUserID] [int] NULL,
	[BatchID] [int] NULL,
	[submittedpdf] [varchar](200) NULL,
	[submitted_date] [datetime] NULL,
	[approvedpdf] [varchar](200) NULL,
	[approved_date] [datetime] NULL,
	[rejectedpdf] [varchar](200) NULL,
	[rejected_date] [datetime] NULL,
	[cancelledpdf] [varchar](200) NULL,
	[cancelled_date] [datetime] NULL,
	[IsOverride] [bit] NOT NULL,
	[OverrideValue] [decimal](6, 2) NULL,
	[IsCPGSubmission] [bit] NOT NULL,
	[Inactive] [bit] NOT NULL,
	[SubmittedUserID] [int] NULL,
	[IsManualPAMEntry] [bit] NOT NULL,
	[verticalid] [int] NULL,
	[Timesheettype] [int] NULL,
	[IsMobile] [bit] NOT NULL,
	[JobTitle] [varchar](100) NULL,
	[isSubmittedEmailSent] [bit] NULL,
	[DirectReportUserId] [int] NULL,
 CONSTRAINT [PK_TimeSheet] PRIMARY KEY CLUSTERED 
(
	[TimeSheetID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 99) ON [PRIMARY]

) ON [PRIMARY]
GO

CREATE TABLE [dbo].[TimeSheetDetail](
	[TimeSheetDetailID] [int] IDENTITY(1,1) NOT NULL,
	[ContractRateID] [int] NOT NULL,
	[PONumber] [varchar](250) NULL,
	[ProjectID] [int] NULL,
	[ContractProjectPoID] [int] NULL,
	[Day] [varchar](50) NULL,
	[UnitValue] [varchar](50) NULL,
	[Description] [varchar](250) NULL,
	[TimesheetID] [int] NULL,
	[verticalid] [int] NULL,
	[InvoiceCodeId] [int] NULL,
 CONSTRAINT [PK_TimeSheetDetail_1] PRIMARY KEY CLUSTERED 
(
	[TimeSheetDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 99) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[TimeSheetAvailablePeriod](
	[TimeSheetAvailablePeriodID] [int] IDENTITY(1,1) NOT NULL,
	[TimeSheetAvailablePeriodStartDate] [datetime] NULL,
	[TimeSheetAvailablePeriodEndDate] [datetime] NULL,
	[TimeSheetPaymentType] [int] NOT NULL,
	--[verticalid] [int] NULL,
 CONSTRAINT [PK_TimeSheetAvailablePeriod] PRIMARY KEY CLUSTERED 
(
	[TimeSheetAvailablePeriodID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 99) ON [PRIMARY]
) ON [PRIMARY]

GO


CREATE TABLE [dbo].[TimeSheetTemp](
	[TimeSheetTempID] [int] IDENTITY(1,1) NOT NULL,
	[TimeSheetID] [int] NULL,
	[CreateDate] [datetime] NOT NULL,
	[CandidateUserID] [int] NOT NULL,
	[AgreementID] [int] NOT NULL,
	[TimeSheetAvailablePeriodID] [int] NULL,
	[QuickPay] [bit] NULL,
	[Vacation] [bit] NULL,
	[StatusID] [int] NULL,
	[ResubmittedFromID] [int] NULL,
	[ResubmittedToID] [int] NULL,
	[ApproverRejectorUserID] [int] NULL,
	[BatchID] [int] NULL,
	[submittedpdf] [varchar](200) NULL,
	[submitted_date] [datetime] NULL,
	[approvedpdf] [varchar](200) NULL,
	[approved_date] [datetime] NULL,
	[rejectedpdf] [varchar](200) NULL,
	[rejected_date] [datetime] NULL,
	[cancelledpdf] [varchar](200) NULL,
	[cancelled_date] [datetime] NULL,
	[IsOverride] [bit] NULL,
	[OverrideValue] [decimal](6, 2) NULL,
	[IsCPGSubmission] [bit] NULL,
	[Inactive] [bit] NOT NULL,
	[SubmittedUserID] [int] NULL,
	[IsManualPAMEntry] [bit] NULL,
	[verticalid] [int] NULL,
 CONSTRAINT [PK_TimeSheetTemp] PRIMARY KEY CLUSTERED 
(
	[TimeSheetTempID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[TimeSheetDetailTemp](
	[TimeSheetDetailTempID] [int] IDENTITY(1,1) NOT NULL,
	[ContractRateID] [int] NOT NULL,
	[PONumber] [varchar](250) NULL,
	[ProjectID] [int] NULL,
	[ContractProjectPoID] [int] NULL,
	[Day] [varchar](50) NULL,
	[UnitValue] [varchar](50) NULL,
	[Description] [varchar](250) NULL,
	[TimesheetTempID] [int] NULL,
	[Inactive] [bit] NOT NULL,
	[verticalid] [int] NULL,
	[InvoiceCodeId] [int] NULL,
 CONSTRAINT [PK_TimeSheetDetailTemp] PRIMARY KEY CLUSTERED 
(
	[TimeSheetDetailTempID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


CREATE TABLE [dbo].[TimeSheetActivity](
	[TimeSheetActivityID] [int] IDENTITY(1,1) NOT NULL,
	[TimeSheetID] [int] NOT NULL,
	[Action] [varchar](100) NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedDate] [datetime] NULL,
	[DirectReportID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[TimeSheetActivityID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


CREATE TABLE [dbo].[pammanualtimesheet](
	[timesheetentereddate] [datetime] NULL,
	[candidateid] [int] NULL,
	[agreementid] [int] NULL,
	[TimeSheetAvailablePeriodID] [int] NULL
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[TimeSheetNote](
	[TimeSheetNoteID] [int] IDENTITY(1,1) NOT NULL,
	[TimeSheetID] [int] NOT NULL,
	[Comment] [varchar](8000) NOT NULL,
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_TimeSheetNote_CreateDate]  DEFAULT (getdate()),
	[CreateUserID] [int] NOT NULL,
	[verticalid] [int] NULL,
 CONSTRAINT [PK_TimeSheetNote] PRIMARY KEY CLUSTERED 
(
	[TimeSheetNoteID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 99) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[TimeSheetAdminDetail](
	[TimeSheetAdminDetailID] [int] IDENTITY(1,1) NOT NULL,
	[ContractRateID] [int] NOT NULL,
	[PONumber] [varchar](250) NULL,
	[ProjectID] [varchar](50) NULL,
	[ContractProjectPOID] [int] NOT NULL,
	[BulkHours] [decimal](6, 2) NOT NULL,
	[TimeSheetID] [int] NOT NULL,
	[Inactive] [bit] NOT NULL CONSTRAINT [DF_TimeSheetAdminDetail_Inactive]  DEFAULT ((0)),
	[verticalid] [int] NULL,
	[InvoiceCodeId] [int] NULL
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[MobileAppTimeSheet](
	[MobileAppTimeSheetID] [int] IDENTITY(1,1) NOT NULL,
	[TimeSheetID] [int] NULL,
	[CreatedDate] [datetime] NULL DEFAULT (getdate())
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[MobileAppTimeSheetTemp](
	[MobileAppTimeSheetTempID] [int] IDENTITY(1,1) NOT NULL,
	[TimeSheetTempID] [int] NULL,
	[CreatedDate] [datetime] NULL DEFAULT (getdate())
) ON [PRIMARY]


/*
	************************************************************ TimeSheet Constraints ******************************************************
*/

ALTER TABLE [dbo].[TimeSheet] ADD  CONSTRAINT [DF_TimeSheet_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO

ALTER TABLE [dbo].[TimeSheet] ADD  CONSTRAINT [DF_TimeSheet_QuickPay]  DEFAULT ((0)) FOR [QuickPay]
GO

ALTER TABLE [dbo].[TimeSheet] ADD  CONSTRAINT [DF_TimeSheet_IsOverride]  DEFAULT ((0)) FOR [IsOverride]
GO

ALTER TABLE [dbo].[TimeSheet] ADD  CONSTRAINT [DF_TimeSheet_IsCPGSubmission]  DEFAULT ((0)) FOR [IsCPGSubmission]
GO

ALTER TABLE [dbo].[TimeSheet] ADD  CONSTRAINT [DF_TimeSheet_Inactive]  DEFAULT ((0)) FOR [Inactive]
GO

ALTER TABLE [dbo].[TimeSheet] ADD  CONSTRAINT [DF_TimeSheet_IsManualPAMEntry]  DEFAULT ((0)) FOR [IsManualPAMEntry]
GO

ALTER TABLE [dbo].[TimeSheet] ADD  DEFAULT ((0)) FOR [IsMobile]
GO

ALTER TABLE [dbo].[TimeSheet] ADD  DEFAULT ((1)) FOR [isSubmittedEmailSent]
GO

ALTER TABLE [dbo].[TimeSheet] ADD  DEFAULT (NULL) FOR [DirectReportUserId]
GO

ALTER TABLE [dbo].[TimeSheet]  WITH CHECK ADD  CONSTRAINT [FK_TimeSheet_AgreementID__Agreement_AgreementID] FOREIGN KEY([AgreementID])
REFERENCES [dbo].[Agreement] ([AgreementID])
GO

ALTER TABLE [dbo].[TimeSheet] CHECK CONSTRAINT [FK_TimeSheet_AgreementID__Agreement_AgreementID]
GO

ALTER TABLE [dbo].[TimeSheet]  WITH CHECK ADD  CONSTRAINT [FK_TimeSheet_ApproverRejectorUserID__Users_UserID] FOREIGN KEY([ApproverRejectorUserID])
REFERENCES [dbo].[Users] ([UserID])
GO

ALTER TABLE [dbo].[TimeSheet] CHECK CONSTRAINT [FK_TimeSheet_ApproverRejectorUserID__Users_UserID]
GO

ALTER TABLE [dbo].[TimeSheet]  WITH CHECK ADD  CONSTRAINT [FK_TimeSheet_CandidateUserID__Users_UserID] FOREIGN KEY([CandidateUserID])
REFERENCES [dbo].[Users] ([UserID])
GO

ALTER TABLE [dbo].[TimeSheet] CHECK CONSTRAINT [FK_TimeSheet_CandidateUserID__Users_UserID]
GO

--ALTER TABLE [dbo].[TimeSheet]  WITH CHECK ADD  CONSTRAINT [FK_TimeSheet_StatusID__PickList_PickListID] FOREIGN KEY([StatusID])
--REFERENCES [dbo].[PickList] ([PickListID])
--GO

--ALTER TABLE [dbo].[TimeSheet] CHECK CONSTRAINT [FK_TimeSheet_StatusID__PickList_PickListID]
--GO

ALTER TABLE [dbo].[TimeSheet]  WITH NOCHECK ADD  CONSTRAINT [FK_TimeSheet_TimeSheetAvailablePeriodID__TimeSheetAvailablePeriod_TimeSheetAvailablePeriodID] FOREIGN KEY([TimeSheetAvailablePeriodID])
REFERENCES [dbo].[TimeSheetAvailablePeriod] ([TimeSheetAvailablePeriodID])
GO

ALTER TABLE [dbo].[TimeSheet] NOCHECK CONSTRAINT [FK_TimeSheet_TimeSheetAvailablePeriodID__TimeSheetAvailablePeriod_TimeSheetAvailablePeriodID]
GO

--ALTER TABLE [dbo].[TimeSheet]  WITH CHECK ADD  CONSTRAINT [chkverticalid_TimeSheet] CHECK  (([verticalid] IS NOT NULL AND [verticalid]<>(0)))
--GO

--ALTER TABLE [dbo].[TimeSheet] CHECK CONSTRAINT [chkverticalid_TimeSheet]
--GO

--ALTER TABLE [dbo].[TimeSheet]  WITH NOCHECK ADD  CONSTRAINT [ck_submittimesheet] CHECK  (([dbo].[timesheet_validate_submit]([AgreementID],[CandidateUserID],[TimeSheetAvailablePeriodID],[StatusID])=(0)))
--GO

--ALTER TABLE [dbo].[TimeSheet] CHECK CONSTRAINT [ck_submittimesheet]
--GO

/*
	************************************************************ TimeSheetDetail Constraints ******************************************************
*/

ALTER TABLE [dbo].[TimeSheetDetail]  WITH NOCHECK ADD  CONSTRAINT [FK_TimeSheetDetail_ContractRateID__Agreement_ContractRateDetail_ContractRateID] FOREIGN KEY([ContractRateID])
REFERENCES [dbo].[Agreement_ContractRateDetail] ([ContractRateID])
GO

ALTER TABLE [dbo].[TimeSheetDetail] NOCHECK CONSTRAINT [FK_TimeSheetDetail_ContractRateID__Agreement_ContractRateDetail_ContractRateID]
GO

/*
	************************************************************ TimeSheetTemp Constraints ******************************************************
*/

ALTER TABLE [dbo].[TimeSheetTemp] ADD  CONSTRAINT [DF_TimeSheetTemp_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO

ALTER TABLE [dbo].[TimeSheetTemp] ADD  CONSTRAINT [DF_TimeSheetTemp_QuickPay]  DEFAULT ((0)) FOR [QuickPay]
GO

ALTER TABLE [dbo].[TimeSheetTemp] ADD  CONSTRAINT [DF_TimeSheetTemp_IsOverride]  DEFAULT ((0)) FOR [IsOverride]
GO

ALTER TABLE [dbo].[TimeSheetTemp] ADD  CONSTRAINT [DF_TimeSheetTemp_IsCPGSubmission]  DEFAULT ((0)) FOR [IsCPGSubmission]
GO

ALTER TABLE [dbo].[TimeSheetTemp] ADD  CONSTRAINT [DF_TimeSheetTemp_Inactive]  DEFAULT ((0)) FOR [Inactive]
GO

ALTER TABLE [dbo].[TimeSheetTemp] ADD  CONSTRAINT [DF_TimeSheetTemp_IsManualPAMEntry]  DEFAULT ((0)) FOR [IsManualPAMEntry]
GO

--ALTER TABLE [dbo].[TimeSheetTemp]  WITH CHECK ADD  CONSTRAINT [chkverticalid_TimeSheetTemp] CHECK  (([verticalid] IS NOT NULL AND [verticalid]<>(0)))
--GO

--ALTER TABLE [dbo].[TimeSheetTemp] CHECK CONSTRAINT [chkverticalid_TimeSheetTemp]
--GO

/*
	************************************************************ TimeSheetDetailTemp Constraints ******************************************************
*/

ALTER TABLE [dbo].[TimeSheetDetailTemp] ADD  CONSTRAINT [DF_TimeSheetDetailTemp_Inactive]  DEFAULT ((0)) FOR [Inactive]
GO

--ALTER TABLE [dbo].[TimeSheetDetailTemp]  WITH CHECK ADD  CONSTRAINT [chkverticalid_TimeSheetDetailTemp] CHECK  (([verticalid] IS NOT NULL AND [verticalid]<>(0)))
--GO

--ALTER TABLE [dbo].[TimeSheetDetailTemp] CHECK CONSTRAINT [chkverticalid_TimeSheetDetailTemp]
--GO

/*
	************************************************************ TimeSheetActivity Constraints ******************************************************
*/

ALTER TABLE [dbo].[TimeSheetActivity] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO

ALTER TABLE [dbo].[TimeSheetActivity]  WITH CHECK ADD  CONSTRAINT [fk_RefId] FOREIGN KEY([TimeSheetID])
REFERENCES [dbo].[TimeSheet] ([TimeSheetID])
GO

ALTER TABLE [dbo].[TimeSheetActivity] CHECK CONSTRAINT [fk_RefId]
GO


/*
	************************************************************ TimeSheetNote Constraints ******************************************************
*/

ALTER TABLE [dbo].[TimeSheetNote]  WITH CHECK ADD  CONSTRAINT [FK_TimeSheetNote_CreateUserID__Users_UserID] FOREIGN KEY([CreateUserID])
REFERENCES [dbo].[Users] ([UserID])
GO

ALTER TABLE [dbo].[TimeSheetNote] CHECK CONSTRAINT [FK_TimeSheetNote_CreateUserID__Users_UserID]
GO

ALTER TABLE [dbo].[TimeSheetNote]  WITH NOCHECK ADD  CONSTRAINT [FK_TimeSheetNote_TimeSheet] FOREIGN KEY([TimeSheetID])
REFERENCES [dbo].[TimeSheet] ([TimeSheetID])
GO

ALTER TABLE [dbo].[TimeSheetNote] NOCHECK CONSTRAINT [FK_TimeSheetNote_TimeSheet]
GO

ALTER TABLE [dbo].[TimeSheetNote]  WITH CHECK ADD  CONSTRAINT [chkverticalid_TimeSheetNote] CHECK  (([verticalid] IS NOT NULL AND [verticalid]<>(0)))
GO

ALTER TABLE [dbo].[TimeSheetNote] CHECK CONSTRAINT [chkverticalid_TimeSheetNote]
GO

/*
	************************************************************ TimeSheetAdminDetail Constraints ******************************************************
*/

ALTER TABLE [dbo].[TimeSheetAdminDetail]  WITH NOCHECK ADD  CONSTRAINT [chkverticalid_TimeSheetAdminDetail] CHECK  (([verticalid] IS NOT NULL AND [verticalid]<>(0)))
GO

ALTER TABLE [dbo].[TimeSheetAdminDetail] CHECK CONSTRAINT [chkverticalid_TimeSheetAdminDetail]
GO


/*
	************************************************************ MobileAppTimesheet (Analytics Table) ******************************************************
*/

ALTER TABLE [dbo].[MobileAppTimeSheet]  WITH CHECK ADD  CONSTRAINT [FK_TimeSheet_MobileApp] FOREIGN KEY([TimeSheetID])
REFERENCES [dbo].[TimeSheet] ([TimeSheetID])
GO

ALTER TABLE [dbo].[MobileAppTimeSheet] CHECK CONSTRAINT [FK_TimeSheet_MobileApp]
GO

/*
	************************************************************ MobileAppTimesheetTemp (Analytics Table) ******************************************************
*/

ALTER TABLE [dbo].[MobileAppTimeSheetTemp]  WITH CHECK ADD  CONSTRAINT [FK_TimeSheet_MobileAppTemp] FOREIGN KEY([TimeSheetTempID])
REFERENCES [dbo].[TimeSheetTemp] ([TimeSheetTempID])
GO

ALTER TABLE [dbo].[MobileAppTimeSheetTemp] CHECK CONSTRAINT [FK_TimeSheet_MobileAppTemp]
GO


