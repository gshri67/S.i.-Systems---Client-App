
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
    --[Inactive]             BIT           NOT NULL,
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

