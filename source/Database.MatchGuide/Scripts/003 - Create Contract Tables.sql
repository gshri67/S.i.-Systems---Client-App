

CREATE TABLE [dbo].[Agreement](
	[AgreementID] [int] IDENTITY(1,1) NOT NULL,
	[AgreementSubID] [int] NOT NULL,
	[AgreementType] [int] NOT NULL,
	[AgreementSubType] [varchar](50) NULL CONSTRAINT [DF_Agreements_SubType]  DEFAULT (0),
	--[BranchID] [int] NOT NULL,
	--[BranchSubID] [int] NULL,
	[AccountExecID] [int] NOT NULL,
	--[RecruiterID] [int] NULL,
	[CandidateID] [int] NULL,
	[CompanyID] [int] NOT NULL,
	[ContactID] [int] NULL,
	--[CreateUserID] [int] NOT NULL,
	--[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_Agreements_CreateDate]  DEFAULT (getdate()),
	--[UpdateUserID] [int] NULL,
	--[UpdateDate] [datetime] NULL,
	[StatusType] [int] NOT NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	--[LockAccount] [bit] NULL CONSTRAINT [DF_Agreement_LockAccount]  DEFAULT (0),
	[Inactive] [bit] NOT NULL CONSTRAINT [DF_Agreement_Inactive]  DEFAULT (0),
	--[verticalid] [int] NULL,
	--[pushtopam] [bit] NULL,
 CONSTRAINT [PK_Agreements] PRIMARY KEY CLUSTERED 
(
	[AgreementID] ASC
) ON [PRIMARY]);

GO

CREATE TABLE [dbo].[Agreement_ContractDetail](
	[AgreementID] [int] NOT NULL,
	[JobTitle] [varchar](4000) NULL,
	--[RevenueAccountExecID] [int] NULL,
	--[GP_ProjectID] [nvarchar](20) NULL,
	--[CancellationNotice] [smallint] NULL,
	--[Active_TimeSheet] [bit] NOT NULL,
	--[Active_Project] [bit] NOT NULL,
	--[LockAccount] [int] NULL,
	[PreceedingContractID] [int] NULL,
	--[SucceedingContractID] [int] NULL,
	--[TimeFactorType] [int] NULL,
	--[NumberDays] [int] NULL,
	--[GPID] [int] NULL,
	[ContractPaymentPlanType] [int] NULL,
	--[InvoiceDetails] [text] NULL,
	--[CandidateLegal] [bit] NOT NULL CONSTRAINT [DF__agreement__Candi__5498ECEA]  DEFAULT ((0)),
	--[CandidateLegalDocType] [int] NULL,
	--[CompanyLegal] [bit] NOT NULL CONSTRAINT [DF__agreement__Compa__558D1123]  DEFAULT ((0)),
	--[CompanyLegalDocType] [int] NULL,
	--[Expenditure] [money] NULL,
	--[CandidateLegalDocStatus] [int] NULL,
	--[CompanyLegalDocStatus] [int] NULL,
	--[CandidatePrimaryEmail] [bit] NULL,
	[TimeSheetSubmitted] [bit] NOT NULL CONSTRAINT [DF_Agreement_ContractDetail_TimeSheetSubmitted]  DEFAULT ((0)),
	--[ContractCandidateWith] [varchar](100) NULL,
	--[ContractClientContactWith] [varchar](100) NULL,
	--[LimitationofExpense] [money] NULL,
	--[Gaurantee_Instructions] [varchar](8000) NULL,
	--[Candidate_Verification] [bit] NULL CONSTRAINT [DF_Agreement_ContractDetail_Candidate_Verification]  DEFAULT ((0)),
	--[Client_Verification] [bit] NULL CONSTRAINT [DF_Agreement_ContractDetail_Client_Verification]  DEFAULT ((0)),
	[SpecializationID] [int] NULL,
	--[CancelledContractID] [int] NULL,
	[TimeSheetType] [int] NULL,
	--[IsThirdPartyBilling] [bit] NULL,
	--[IsThirdPartyPay] [bit] NULL,
	--[ThirdPartyBillingCompanyID] [int] NULL,
	--[ThirdPartyPayCandidateID] [int] NULL,
	--[InvoiceFormatID] [int] NULL,
	--[CommissionAccountExecID] [int] NULL,
	--[GMType] [int] NULL,
	--[cancelledfromid] [int] NULL,
	--[cancelledtoid] [int] NULL,
	--[CandidatePaymentType] [int] NULL,
	--[AdminFee] [int] NULL,
	--[ROEStatus] [int] NULL,
	--[ApprovedStatus] [int] NULL,
	--[DisApprovalReason] [text] NULL,
	--[verticalid] [int] NULL,
	--[taxapplicable] [int] NULL,
	--[LimitationofContracttype] [int] NULL,
	--[InvoiceFrequencyId] [int] NULL,
	--[HasProjectPO] [bit] NULL,
	--[QuickPay] [int] NOT NULL DEFAULT ((0)),
	--[QuickPayCount] [int] NOT NULL DEFAULT ((0)),
	--[CancellationReasonID] [int] NULL,
	--[CandidateCorpName] [nvarchar](250) NULL,
	--[CandidateGSTNumber] [nvarchar](100) NULL,
	--[PayCompanyVerificationStatus] [bit] NULL,
	--[TPCandidateCorpName] [nvarchar](250) NULL,
	--[PayCompanyVerifiedBy] [int] NULL,
	--[PayCompanyVerifyDate] [datetime] NULL,
	--[GrandParentId] [int] NULL DEFAULT (NULL),
	--[DPPaymenttype] [int] NULL DEFAULT ((0)),
 CONSTRAINT [PK_Agreement_ContractAdminDetail] PRIMARY KEY CLUSTERED 
(
	[AgreementID] ASC
) ON [PRIMARY]);

GO

CREATE TABLE [dbo].[Agreement_ContractRateDetail](
	[ContractRateID] [int] IDENTITY(1,1) NOT NULL,
	[RateDescription] [varchar](255) NULL,
	--[RateTermType] [int] NULL,
	[AgreementID] [int] NOT NULL,
	[StartDate] [smalldatetime] NULL,
	[EndDate] [smalldatetime] NOT NULL,
	[BillRate] [money] NULL,
	[PayRate] [money] NULL,
	--[MinSalary] [money] NULL,
	--[MaxSalary] [money] NULL,
	--[PlacementFeeFixed] [money] NULL,
	--[PlacementFeeVariable] [decimal](5, 2) NULL,
	--[CalloutFeeFixed] [money] NULL,
	--[CalloutFeeVariable] [decimal](5, 2) NULL,
	[HoursPerDay] [decimal](18, 2) NULL CONSTRAINT [DF_Contract_RateAdministration_HoursPerDay]  DEFAULT ((7.5)),
	--[CreateDate] [smalldatetime] NOT NULL CONSTRAINT [DF_Contract_RateAdministration_CreateDate]  DEFAULT (getdate()),
	--[CreateUserID] [int] NOT NULL,
	--[UpdateDate] [smalldatetime] NULL,
	--[UpdateUserID] [int] NULL,
	[PrimaryRateTerm] [bit] NOT NULL CONSTRAINT [DF_Agreement_ContractRateDetail_PrimaryRateTerm]  DEFAULT ((0)),
	--[AdminFee] [money] NULL,
	--[AdminFeeType] [int] NULL,
	--[AdminBilling] [varchar](50) NULL,
	--[AdminFeeCandidate] [decimal](18, 3) NULL,
	--[AdminFeeCompany] [decimal](18, 3) NULL,
	[Inactive] [bit] NOT NULL CONSTRAINT [DF_Contract_RateAdministration_Inactive]  DEFAULT ((0)),
	--[PAMRateID] [varchar](50) NULL,
	--[verticalid] [int] NULL,
 CONSTRAINT [PK_Contract_RateDetails] PRIMARY KEY CLUSTERED 
(
	[ContractRateID] ASC
) ON [PRIMARY]);

GO

CREATE TABLE [dbo].[Agreement_ContractAdminContactMatrix](
	[ContractAdminMatrixID] [int] IDENTITY(1,1) NOT NULL,
	[AgreementID] [int] NOT NULL,
	[BillingUserID] [int] NOT NULL,
	[DirectReportUserID] [int] NOT NULL,
	--[CreateDate] [datetime] NULL,
	--[CreateUserID] [int] NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateUserID] [int] NULL,
	[Inactive] [bit] NOT NULL CONSTRAINT [DF__Agreement__Inact__7FB85519]  DEFAULT (0),
	--[verticalid] [int] NULL,
 CONSTRAINT [PK__Agreement_Contra__7EC430E0] PRIMARY KEY CLUSTERED 
(
	[ContractAdminMatrixID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Agreement_ContractAdminContactMatrix]  WITH CHECK ADD  CONSTRAINT [FK_Agreement_ContractAdminContactMatrix_Agreement] FOREIGN KEY([AgreementID])
REFERENCES [dbo].[Agreement] ([AgreementID])
GO

ALTER TABLE [dbo].[Agreement_ContractAdminContactMatrix] CHECK CONSTRAINT [FK_Agreement_ContractAdminContactMatrix_Agreement]
GO

CREATE TABLE [dbo].[ContractInvoiceCode](
	[InvoiceCodeID] [int] IDENTITY(1,1) NOT NULL,
	[InvoiceCodeText] [nvarchar](max) NOT NULL,
	--[FieldDelimiter] [varchar](5) NOT NULL,
	--[FieldCounter] [int] NOT NULL,
	--[IsDefault] [bit] NOT NULL,
	[ContractID] [int] NOT NULL,
	--[IsActive] [bit] NOT NULL,
	--[InactiveForUser] [bit] NOT NULL,
	--[CreatedBy] [int] NOT NULL,
	--[CreatedDate] [datetime] NOT NULL,
	--[UpdatedBy] [int] NULL,
	--[UpdatedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[InvoiceCodeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[ContractInvoiceCode]  WITH CHECK ADD FOREIGN KEY([ContractID])
REFERENCES [dbo].[Agreement] ([AgreementID])
GO

CREATE TABLE [dbo].[Agreement_OpportunityCandidateMatrix](
	[OpportunityCandidateMatrixID] [int] IDENTITY(1,1) NOT NULL,
	[AgreementID] [int] NOT NULL,
	[CandidateUserID] [int] NOT NULL,
	[StatusType] [int] NOT NULL,
	[StatusSubType] [int] NOT NULL,
	[ProposedBillRate] [money] NULL CONSTRAINT [DF_Opportunity_CandidateMatrix_ProposedBillRate]  DEFAULT (0),
	[ProposedPayRate] [money] NULL CONSTRAINT [DF_Opportunity_CandidateMatrix_ProposedPayRate]  DEFAULT (0),
	[Note] [text] NULL,
	[CreateDateTime] [datetime] NOT NULL CONSTRAINT [DF_Opportunity_CandidateMatrix_CreateDateTime]  DEFAULT (getdate()),
	[CreateUserID] [int] NOT NULL,
	[UpdateDateTime] [datetime] NULL,
	[UpdateUserID] [int] NULL,
	[ShortlistUserID] [int] NULL,
	[Availability] [varchar](250) NULL,
	--[ReferenceComplete] [bit] NOT NULL CONSTRAINT [DF_Agreement_OpportunityCandidateMatrix_ReferenceComplete]  DEFAULT (0),
	--[ResumeComplete] [bit] NOT NULL CONSTRAINT [DF_Agreement_OpportunityCandidateMatrix_ResumeComplete]  DEFAULT (0),
	--[UpcomingVacation] [varchar](250) NULL,
	--[WebResponse] [bit] NOT NULL CONSTRAINT [DF_Agreement_OpportunityCandidateMatrix_WebResponse]  DEFAULT (0),
	--[Inactive] [bit] NOT NULL CONSTRAINT [DF_Agreement_OpportunityCandidateMatrix_Inactive]  DEFAULT (0),
	[ProposedSalary] [money] NULL,
	[ProposedPlacementFee] [money] NULL,
	[IsMDApprovedGM] [bit] NULL CONSTRAINT [DF_Agreement_OpportunityCandidateMatrix_IsMDApprovedGM]  DEFAULT ((0)),
	[InterviewsJobOfferPending] [varchar](250) NULL,
	--[verticalid] [int] NULL,
	--[DisplayOrder] [tinyint] NOT NULL CONSTRAINT [DF__AgrOppCanMat__DisplayOrder]  DEFAULT ((0)),
	--[IsAppliedFromMobile] [bit] NOT NULL DEFAULT ((0)),
 CONSTRAINT [PK_OpportunityCandidate] PRIMARY KEY CLUSTERED 
(
	[AgreementID] ASC,
	[CandidateUserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 97) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[Agreement_OpportunityCandidateMatrix]  WITH NOCHECK ADD  CONSTRAINT [FK_Agreement_OpportunityCandidateMatrix_Agreement] FOREIGN KEY([AgreementID])
REFERENCES [dbo].[Agreement] ([AgreementID])
GO

ALTER TABLE [dbo].[Agreement_OpportunityCandidateMatrix] NOCHECK CONSTRAINT [FK_Agreement_OpportunityCandidateMatrix_Agreement]
GO

--ALTER TABLE [dbo].[Agreement_OpportunityCandidateMatrix]  WITH CHECK ADD  CONSTRAINT [chkverticalid_Agreement_OpportunityCandidateMatrix] CHECK  (([verticalid] IS NOT NULL AND [verticalid]<>(0)))
--GO

--ALTER TABLE [dbo].[Agreement_OpportunityCandidateMatrix] CHECK CONSTRAINT [chkverticalid_Agreement_OpportunityCandidateMatrix]
--GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'AgreementID for opportunity from Agreements table' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agreement_OpportunityCandidateMatrix', @level2type=N'COLUMN',@level2name=N'AgreementID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'UserID of Candidate from users table' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agreement_OpportunityCandidateMatrix', @level2type=N'COLUMN',@level2name=N'CandidateUserID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Status of record activity from PickList' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agreement_OpportunityCandidateMatrix', @level2type=N'COLUMN',@level2name=N'StatusType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Bill rate proposed to client' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agreement_OpportunityCandidateMatrix', @level2type=N'COLUMN',@level2name=N'ProposedBillRate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Pay rate proposed to candidate' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agreement_OpportunityCandidateMatrix', @level2type=N'COLUMN',@level2name=N'ProposedPayRate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Misc. notes' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agreement_OpportunityCandidateMatrix', @level2type=N'COLUMN',@level2name=N'Note'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date record was created in the table' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agreement_OpportunityCandidateMatrix', @level2type=N'COLUMN',@level2name=N'CreateDateTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'UserID for individual who created the record in the table' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agreement_OpportunityCandidateMatrix', @level2type=N'COLUMN',@level2name=N'CreateUserID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date record was updated in the table' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agreement_OpportunityCandidateMatrix', @level2type=N'COLUMN',@level2name=N'UpdateDateTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'UserID for individual who updated the record in the table' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agreement_OpportunityCandidateMatrix', @level2type=N'COLUMN',@level2name=N'UpdateUserID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'UserID of individual who shortlisted the candidate' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agreement_OpportunityCandidateMatrix', @level2type=N'COLUMN',@level2name=N'ShortlistUserID'
GO

--EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Candidate’s availability' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agreement_OpportunityCandidateMatrix', @level2type=N'COLUMN',@level2name=N'Availability'
--GO

--EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Flag indicating that the reference checks are complete' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agreement_OpportunityCandidateMatrix', @level2type=N'COLUMN',@level2name=N'ReferenceComplete'
--GO

--EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Flag indicating that the resume is complete' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agreement_OpportunityCandidateMatrix', @level2type=N'COLUMN',@level2name=N'ResumeComplete'
--GO

--EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Details of any upcoming vacation the candidate has planned' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agreement_OpportunityCandidateMatrix', @level2type=N'COLUMN',@level2name=N'UpcomingVacation'
--GO







CREATE TABLE [dbo].[Agreement_OpportunityDetail](
	[AgreementID] [int] NOT NULL,
	[Description] [ntext] NULL,
	[JobTitle] [nvarchar](4000) NULL,
	[SpecializationID] [int] NULL,
	[NumberRequired] [smallint] NOT NULL,
	[NiceHave] [ntext] NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[ResumeDueDate] [datetime] NULL,
	[InterviewProcess] [ntext] NULL,
	[TimeFactorType] [int] NULL,
	[HoursPerDay] [decimal](4, 2) NULL,
	[WorkEnvironment] [nvarchar](1000) NULL,
	--[Post_Rate] [int] NOT NULL,
	--[Post_Web] [bit] NULL,
	--[Post_Branch] [int] NULL,
	--[NotifyContactNotes] [bit] NOT NULL CONSTRAINT [DF_Agreement_OpportunityDetail_NotifyContactNotes]  DEFAULT (0),
	--[SolicitationReference] [varchar](250) NULL,
	--[TaskAuthName] [varchar](250) NULL,
	--[OppVersion] [int] NULL,
	[verticalid] [int] NULL,
	--[AverageTenure] [int] NULL,
	--[SurveyUrlId] [int] NOT NULL DEFAULT ((0)),
	[IsExistingJob] [int] NULL CONSTRAINT [DF_agreement_opportunitydetail_IsexistingJob]  DEFAULT ((2)),
	--[Web_PostedDate] [datetime] NULL DEFAULT (NULL),
 CONSTRAINT [PK_Opportunity_Details] PRIMARY KEY CLUSTERED 
(
	[AgreementID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 97) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[Agreement_OpportunityDetail]  WITH NOCHECK ADD  CONSTRAINT [FK_Agreement_OpportunityDetail_Agreement] FOREIGN KEY([AgreementID])
REFERENCES [dbo].[Agreement] ([AgreementID])
GO

ALTER TABLE [dbo].[Agreement_OpportunityDetail] NOCHECK CONSTRAINT [FK_Agreement_OpportunityDetail_Agreement]
GO

ALTER TABLE [dbo].[Agreement_OpportunityDetail]  WITH CHECK ADD  CONSTRAINT [chkverticalid_Agreement_OpportunityDetail] CHECK  (([verticalid] IS NOT NULL AND [verticalid]<>(0)))
GO

ALTER TABLE [dbo].[Agreement_OpportunityDetail] CHECK CONSTRAINT [chkverticalid_Agreement_OpportunityDetail]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Agreement ID from Agreements table' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agreement_OpportunityDetail', @level2type=N'COLUMN',@level2name=N'AgreementID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description of the opportunity' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agreement_OpportunityDetail', @level2type=N'COLUMN',@level2name=N'Description'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Job title of the position offered in the opportunity' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agreement_OpportunityDetail', @level2type=N'COLUMN',@level2name=N'JobTitle'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Specialty area from the Specialty table' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agreement_OpportunityDetail', @level2type=N'COLUMN',@level2name=N'SpecializationID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Number of consultants required for this opportunity' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agreement_OpportunityDetail', @level2type=N'COLUMN',@level2name=N'NumberRequired'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Skills the applicant would have to to have an advantage for this opportunity' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agreement_OpportunityDetail', @level2type=N'COLUMN',@level2name=N'NiceHave'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Opportunity estimated start date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agreement_OpportunityDetail', @level2type=N'COLUMN',@level2name=N'StartDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Opportunity estimated end date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agreement_OpportunityDetail', @level2type=N'COLUMN',@level2name=N'EndDate'
GO

--EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date resume submission ends' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agreement_OpportunityDetail', @level2type=N'COLUMN',@level2name=N'ResumeDueDate'
--GO

--EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Interview process description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agreement_OpportunityDetail', @level2type=N'COLUMN',@level2name=N'InterviewProcess'
--GO

--EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Time factor type from PickList' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agreement_OpportunityDetail', @level2type=N'COLUMN',@level2name=N'TimeFactorType'
--GO

--EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Work environment comment' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agreement_OpportunityDetail', @level2type=N'COLUMN',@level2name=N'WorkEnvironment'
--GO

--EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indicates that the opportunity is posted on the web' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agreement_OpportunityDetail', @level2type=N'COLUMN',@level2name=N'Post_Rate'
--GO

--EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Flag to indicate that the rate for this opportunity is posted to the web' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agreement_OpportunityDetail', @level2type=N'COLUMN',@level2name=N'Post_Web'
--GO

--EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Flag to indicate that this opportunity is posted to the branch' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agreement_OpportunityDetail', @level2type=N'COLUMN',@level2name=N'Post_Branch'
--GO


