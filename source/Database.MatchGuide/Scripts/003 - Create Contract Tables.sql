

CREATE TABLE [dbo].[Agreement](
	[AgreementID] [int] IDENTITY(1,1) NOT NULL,
	--[AgreementSubID] [int] NOT NULL,
	[AgreementType] [int] NOT NULL,
	[AgreementSubType] [varchar](50) NULL CONSTRAINT [DF_Agreements_SubType]  DEFAULT (0),
	--[BranchID] [int] NOT NULL,
	--[BranchSubID] [int] NULL,
	--[AccountExecID] [int] NOT NULL,
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
	--[PreceedingContractID] [int] NULL,
	--[SucceedingContractID] [int] NULL,
	--[TimeFactorType] [int] NULL,
	--[NumberDays] [int] NULL,
	--[GPID] [int] NULL,
	--[ContractPaymentPlanType] [int] NULL,
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
	--[TimeSheetType] [int] NULL,
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
	--[HoursPerDay] [decimal](18, 2) NULL CONSTRAINT [DF_Contract_RateAdministration_HoursPerDay]  DEFAULT ((7.5)),
	--[CreateDate] [smalldatetime] NOT NULL CONSTRAINT [DF_Contract_RateAdministration_CreateDate]  DEFAULT (getdate()),
	--[CreateUserID] [int] NOT NULL,
	--[UpdateDate] [smalldatetime] NULL,
	--[UpdateUserID] [int] NULL,
	--[PrimaryRateTerm] [bit] NOT NULL CONSTRAINT [DF_Agreement_ContractRateDetail_PrimaryRateTerm]  DEFAULT ((0)),
	--[AdminFee] [money] NULL,
	--[AdminFeeType] [int] NULL,
	--[AdminBilling] [varchar](50) NULL,
	--[AdminFeeCandidate] [decimal](18, 3) NULL,
	--[AdminFeeCompany] [decimal](18, 3) NULL,
	--[Inactive] [bit] NOT NULL CONSTRAINT [DF_Contract_RateAdministration_Inactive]  DEFAULT ((0)),
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
	--[BillingUserID] [int] NOT NULL,
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


