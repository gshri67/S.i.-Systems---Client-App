
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

	[MSPFeePercentage] DECIMAL(6,2) DEFAULT NULL,
	[FloThruFeeTypeID] INT DEFAULT NULL,
	[FloThruFee] DECIMAL(6,2) DEFAULT NULL,
	[FloThruFeePaymentID] INT DEFAULT NULL,
	[FloThruMSPPaymentID] INT DEFAULT NULL,
	[CompanyInvoiceFrequencyID] INT DEFAULT NULL,
	[FloThruAlumni] BIT DEFAULT NULL,
	[MaxVisibleRatePerHour] MONEY DEFAULT NULL,
	[MaxVisibleRatePerDay] MONEY DEFAULT NULL,
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

GO
