
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
	[verticalid] [int] NULL,
	--[pushtopam] [bit] NULL,
 CONSTRAINT [PK_Agreements] PRIMARY KEY CLUSTERED 
(
	[AgreementID] ASC
) ON [PRIMARY]);

GO

CREATE TABLE [dbo].[Company] (
    [CompanyID]            INT           IDENTITY (1, 1) NOT NULL,
    [CompanyName]          VARCHAR (150) NOT NULL,
    --[StatusType]           INT           NULL,
    --[EstimatedCandidates]  INT           NULL,
    --[MainAreaCode]         DECIMAL (3)   NULL,
    --[MainPhoneNumber]      DECIMAL (7)   NULL,
    --[IndustryType]         INT           NULL,
    --[LegalID]              INT           NULL,
    --[WebSite]              VARCHAR (100) NULL,
    --[CreateDate]           SMALLDATETIME NOT NULL,
    --[CreateUserID]         INT           NOT NULL,
    --[UpdateDate]           SMALLDATETIME NULL,
    --[UpdateUserID]         INT           NULL,
    [Inactive]             BIT           NOT NULL,
    --[PAM]                  BIT           NULL,
    --[GST_Applicable]       BIT           NOT NULL,
    --[FTFeeType]            INT           NULL,
    --[FTFeeValue]           MONEY         NULL,
    --[FTFloorRate]          MONEY         NULL,
    --[taxapplicable]        INT           NULL,
    --[IndustryId]           INT           NULL,
    --[SubIndustryId]        INT           NULL,
    --[InvoiceFormatId]      INT           NULL,
    --[InvoiceFrequencyId]   INT           NULL,
    --[HasProjectPO]         BIT           NULL,
    --[LimitationOfContract] BIT           NOT NULL,
    --[SendEPerformance]     BIT           NOT NULL,
    --[TargetPayTime]        INT           NULL,
	--[SendeContractTerms] [bit] NULL DEFAULT ((1)),
	[MSPFeePercentage] [decimal](6, 2) NULL DEFAULT (NULL),
	[FloThruFeeTypeID] [int] NULL DEFAULT (NULL),
	[FloThruFee] [decimal](6, 2) NULL DEFAULT (NULL),
	[FloThruFeePaymentID] [int] NULL DEFAULT (NULL),
	[FloThruMSPPaymentID] [int] NULL DEFAULT (NULL),
	[CompanyInvoiceFrequencyID] [int] NULL DEFAULT (NULL),
	[IsHavingFTAlumni] [bit] NULL DEFAULT (NULL),
	[MaxVisibleRatePerHour] [money] NULL DEFAULT (NULL),
	[MaxVisibleRatePerDay] [money] NULL DEFAULT (NULL),
    CONSTRAINT [PK_Clients] PRIMARY KEY CLUSTERED ([CompanyID] ASC) WITH (FILLFACTOR = 97)
);

GO

ALTER TABLE [dbo].[Company] ADD  CONSTRAINT [DF_Company_InActive]  DEFAULT ((0)) FOR [InActive]
GO

CREATE TABLE [dbo].[Company_ParentChildRelationship](
	[RelationshipID] [int] IDENTITY(1,1) NOT NULL,
	[ParentID] [int] NOT NULL,
	[ChildID] [int] NOT NULL,
	--[verticalid] [int] NULL,
 CONSTRAINT [PK_ClientRelationships] PRIMARY KEY CLUSTERED 
(
	[ParentID] ASC,
	[ChildID] ASC
) ON [PRIMARY]
);

GO

ALTER TABLE [dbo].[Company_ParentChildRelationship]  WITH NOCHECK ADD  CONSTRAINT [FK_ClientRelationships_Companies] FOREIGN KEY([ParentID])
REFERENCES [dbo].[Company] ([CompanyID])
GO

ALTER TABLE [dbo].[Company_ParentChildRelationship]  WITH NOCHECK ADD  CONSTRAINT [FK_ClientRelationships_Companies1] FOREIGN KEY([ChildID])
REFERENCES [dbo].[Company] ([CompanyID])
GO

CREATE TABLE [dbo].[CompanyDomain] (
	CompanyDomainID	INT	NOT NULL IDENTITY(1,1) PRIMARY KEY,
	CompanyID INT NOT NULL	,
	EmailDomain NVARCHAR(255) NOT NULL,
	--CreatedUserId INT NOT NULL,
	--CreatedDateTime DATETIME DEFAULT GETDATE(),
	--UpdatedUserId INT,
	--UpdatedDateTime DATETIME,
	IsActive BIT DEFAULT 1
	CONSTRAINT FKComDomCompanyId FOREIGN KEY (CompanyID) REFERENCES Company(CompanyID)
);

CREATE TABLE [dbo].[CompanyProject](
	[CompanyProjectID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [varchar](250) NOT NULL,
	[Description] [varchar](250) NULL,
	--[CreateDate] [datetime] NOT NULL,
	--[CreateUserID] [int] NOT NULL,
	--[UpdateDate] [datetime] NULL,
	--[UpdateUserID] [int] NULL,
	[CompanyID] [int] NOT NULL,
	[InActive] [bit] NULL,
	--[verticalid] [int] NULL,
 CONSTRAINT [PK_CompanyProject] PRIMARY KEY CLUSTERED 
(
	[CompanyProjectID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[CompanyProject] ADD  CONSTRAINT [DF_CompanyProject_InActive]  DEFAULT ((0)) FOR [InActive]
GO



SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[CompanyPO](
	[CompanyPOID] [int] IDENTITY(1,1) NOT NULL,
	[PONumber] [varchar](250) NOT NULL,
	[Description] [varchar](250) NULL,
	[CompanyID] [int] NOT NULL,
	[InActive] [bit] NOT NULL CONSTRAINT [DF_CompanyPurchaseOrder_InActive]  DEFAULT ((0)),
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_CompanyPO_CreateDate]  DEFAULT (getdate()),
	[CreateUserID] [int] NOT NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateUserID] [int] NULL,
	[verticalid] [int] NULL,
 CONSTRAINT [PK_CompanyPurchaseOrder] PRIMARY KEY CLUSTERED 
(
	[CompanyPOID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[CompanyPO]  WITH CHECK ADD  CONSTRAINT [chkverticalid_CompanyPO] CHECK  (([verticalid] IS NOT NULL AND [verticalid]<>(0)))
GO

ALTER TABLE [dbo].[CompanyPO] CHECK CONSTRAINT [chkverticalid_CompanyPO]
GO



CREATE TABLE [dbo].[CompanyPOProjectMatrix](
	[CompanyPOProjectMatrixID] [int] IDENTITY(1,1) NOT NULL,
	[CompanyPOID] [int] NOT NULL,
	[CompanyProjectID] [int] NOT NULL,
	[InActive] [bit] NOT NULL CONSTRAINT [DF_CompanyPOProjectMatrix_IsActive]  DEFAULT ((0)),
	[verticalid] [int] NULL,
 CONSTRAINT [PK_CompanyPOProjectMatrix] PRIMARY KEY CLUSTERED 
(
	[CompanyPOProjectMatrixID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[CompanyPOProjectMatrix]  WITH CHECK ADD  CONSTRAINT [chkverticalid_CompanyPOProjectMatrix] CHECK  (([verticalid] IS NOT NULL AND [verticalid]<>(0)))
GO

ALTER TABLE [dbo].[CompanyPOProjectMatrix] CHECK CONSTRAINT [chkverticalid_CompanyPOProjectMatrix]
GO


USE [MatchGuideDev]
GO

/****** Object:  Table [dbo].[ContractProjectPO]    Script Date: 2/12/2016 12:24:26 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ContractProjectPO](
	[ContractProjectPOID] [int] IDENTITY(1,1) NOT NULL,
	[CompanyProjectID] [int] NULL,
	[CompanyPOID] [int] NULL,
	[CompanyPOProjectMatrixID] [int] NULL,
	[AgreementID] [int] NOT NULL,
	[InActive] [bit] NOT NULL CONSTRAINT [DF_ContractProjectPO_InActive]  DEFAULT ((0)),
	[Counter] [int] NULL CONSTRAINT [DF_ContractProjectPO_Counter]  DEFAULT ((0)),
	[CreateUserID] [int] NULL,
	[UpdateUserID] [int] NULL,
	[CreateDate] [datetime] NULL CONSTRAINT [DF_ContractProjectPO_CreateDate]  DEFAULT (getdate()),
	[UpdateDate] [datetime] NULL CONSTRAINT [DF_ContractProjectPO_UpdateDate]  DEFAULT (getdate()),
	[verticalid] [int] NULL,
	[IsGeneralProjectPO] [bit] NOT NULL DEFAULT ((0)),
	[InactiveForUser] [bit] NOT NULL DEFAULT ((0)),
	[ProjLimit] [float] NULL DEFAULT ((0)),
	[PoLimit] [float] NULL DEFAULT ((0)),
	[MatrixLimit] [float] NULL DEFAULT ((0)),
 CONSTRAINT [PK_ContractProjectPO] PRIMARY KEY CLUSTERED 
(
	[ContractProjectPOID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[ContractProjectPO]  WITH NOCHECK ADD  CONSTRAINT [FK_ContractProjectPO_AgreementID__Agreement_AgreementID] FOREIGN KEY([AgreementID])
REFERENCES [dbo].[Agreement] ([AgreementID])
GO

ALTER TABLE [dbo].[ContractProjectPO] NOCHECK CONSTRAINT [FK_ContractProjectPO_AgreementID__Agreement_AgreementID]
GO

ALTER TABLE [dbo].[ContractProjectPO]  WITH CHECK ADD  CONSTRAINT [FK_ContractProjectPO_CompanyPOID__CompanyPurchaseOrder_CompanyPOID] FOREIGN KEY([CompanyPOID])
REFERENCES [dbo].[CompanyPO] ([CompanyPOID])
GO

ALTER TABLE [dbo].[ContractProjectPO] CHECK CONSTRAINT [FK_ContractProjectPO_CompanyPOID__CompanyPurchaseOrder_CompanyPOID]
GO

ALTER TABLE [dbo].[ContractProjectPO]  WITH CHECK ADD  CONSTRAINT [FK_ContractProjectPO_CompanyPOProjectMatrixID__CompanyPOProjectMatrix_CompanyPOProjectMatrixID] FOREIGN KEY([CompanyPOProjectMatrixID])
REFERENCES [dbo].[CompanyPOProjectMatrix] ([CompanyPOProjectMatrixID])
GO

ALTER TABLE [dbo].[ContractProjectPO] CHECK CONSTRAINT [FK_ContractProjectPO_CompanyPOProjectMatrixID__CompanyPOProjectMatrix_CompanyPOProjectMatrixID]
GO

ALTER TABLE [dbo].[ContractProjectPO]  WITH CHECK ADD  CONSTRAINT [FK_ContractProjectPO_CompanyProjectID__CompanyProject_CompanyProjectID] FOREIGN KEY([CompanyProjectID])
REFERENCES [dbo].[CompanyProject] ([CompanyProjectID])
GO

ALTER TABLE [dbo].[ContractProjectPO] CHECK CONSTRAINT [FK_ContractProjectPO_CompanyProjectID__CompanyProject_CompanyProjectID]
GO

ALTER TABLE [dbo].[ContractProjectPO]  WITH CHECK ADD  CONSTRAINT [chkverticalid_ContractProjectPO] CHECK  (([verticalid] IS NOT NULL AND [verticalid]<>(0)))
GO

ALTER TABLE [dbo].[ContractProjectPO] CHECK CONSTRAINT [chkverticalid_ContractProjectPO]
GO




