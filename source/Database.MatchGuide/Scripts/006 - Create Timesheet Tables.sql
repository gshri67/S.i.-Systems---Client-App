﻿CREATE TABLE [dbo].[TimeSheet](
	[TimeSheetID] [int] IDENTITY(1,1) NOT NULL,
	--[CreateDate] [datetime] NOT NULL,
	[CandidateUserID] [int] NOT NULL,
	[AgreementID] [int] NOT NULL,
	[TimeSheetAvailablePeriodID] [int] NULL,
	--[QuickPay] [bit] NOT NULL,
	--[Vacation] [bit] NULL,
	[StatusID] [int] NULL,
	[ResubmittedFromID] [int] NULL,
	[ResubmittedToID] [int] NULL,
	[ApproverRejectorUserID] [int] NULL,
	--[BatchID] [int] NULL,
	[submittedpdf] [varchar](200) NULL,
	[submitted_date] [datetime] NULL,
	[approvedpdf] [varchar](200) NULL,
	[approved_date] [datetime] NULL,
	[rejectedpdf] [varchar](200) NULL,
	[rejected_date] [datetime] NULL,
	[cancelledpdf] [varchar](200) NULL,
	[cancelled_date] [datetime] NULL,
	--[IsOverride] [bit] NOT NULL,
	--[OverrideValue] [decimal](6, 2) NULL,
	--[IsCPGSubmission] [bit] NOT NULL,
	--[Inactive] [bit] NOT NULL,
	--[SubmittedUserID] [int] NULL,
	--[IsManualPAMEntry] [bit] NOT NULL,
	--[verticalid] [int] NULL,
	--[Timesheettype] [int] NULL,
	--[IsMobile] [bit] NOT NULL,
	--[JobTitle] [varchar](100) NULL,
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
	--[ContractProjectPoID] [int] NULL,
	[Day] [varchar](50) NULL,
	[UnitValue] [varchar](50) NULL,
	[Description] [varchar](250) NULL,
	[TimesheetID] [int] NULL,
	--[verticalid] [int] NULL,
	--[InvoiceCodeId] [int] NULL,
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
	--[TimeSheetPaymentType] [int] NOT NULL,
	--[verticalid] [int] NULL,
 CONSTRAINT [PK_TimeSheetAvailablePeriod] PRIMARY KEY CLUSTERED 
(
	[TimeSheetAvailablePeriodID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 99) ON [PRIMARY]
) ON [PRIMARY]

GO

/*
	************************************************************ TimeSheet Constraints ******************************************************
*/

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

ALTER TABLE [dbo].[TimeSheet]  WITH NOCHECK ADD  CONSTRAINT [FK_TimeSheet_TimeSheetAvailablePeriodID__TimeSheetAvailablePeriod_TimeSheetAvailablePeriodID] FOREIGN KEY([TimeSheetAvailablePeriodID])
REFERENCES [dbo].[TimeSheetAvailablePeriod] ([TimeSheetAvailablePeriodID])
GO

ALTER TABLE [dbo].[TimeSheet] NOCHECK CONSTRAINT [FK_TimeSheet_TimeSheetAvailablePeriodID__TimeSheetAvailablePeriod_TimeSheetAvailablePeriodID]
GO

/*
	************************************************************ TimeSheetDetail Constraints ******************************************************
*/

ALTER TABLE [dbo].[TimeSheetDetail]  WITH NOCHECK ADD  CONSTRAINT [FK_TimeSheetDetail_ContractRateID__Agreement_ContractRateDetail_ContractRateID] FOREIGN KEY([ContractRateID])
REFERENCES [dbo].[Agreement_ContractRateDetail] ([ContractRateID])
GO

ALTER TABLE [dbo].[TimeSheetDetail] NOCHECK CONSTRAINT [FK_TimeSheetDetail_ContractRateID__Agreement_ContractRateDetail_ContractRateID]
GO
