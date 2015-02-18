
CREATE TABLE [dbo].[Company](
	[CompanyID] [int] IDENTITY(1,1) NOT NULL,
	[CompanyName] [varchar](150) NOT NULL,
	--[StatusType] [int] NULL,
	--[EstimatedCandidates] [int] NULL CONSTRAINT [DF_Company_EstimatedCandidates]  DEFAULT (0),
	--[MainAreaCode] [decimal](3, 0) NULL,
	--[MainPhoneNumber] [decimal](7, 0) NULL,
	--[IndustryType] [int] NULL,
	--[LegalID] [int] NULL,
	--[WebSite] [varchar](100) NULL,
	--[CreateDate] [smalldatetime] NOT NULL CONSTRAINT [DF_Company_CreateDate]  DEFAULT (getdate()),
	--[CreateUserID] [int] NOT NULL,
	--[UpdateDate] [smalldatetime] NULL,
	--[UpdateUserID] [int] NULL,
	--[Inactive] [bit] NOT NULL CONSTRAINT [DF_Company_Inactive]  DEFAULT (0),
	--[PAM] [bit] NULL,
	--[GST_Applicable] [bit] NOT NULL CONSTRAINT [DF_Company_GST_Applicable]  DEFAULT ((1)),
	--[FTFeeType] [int] NULL,
	--[FTFeeValue] [money] NULL,
	--[FTFloorRate] [money] NULL,
	--[taxapplicable] [int] NULL,
	--[IndustryId] [int] NULL,
	--[SubIndustryId] [int] NULL,
	--[InvoiceFormatId] [int] NULL,
	--[InvoiceFrequencyId] [int] NULL,
	--[HasProjectPO] [bit] NULL,
	--[LimitationOfContract] [bit] NOT NULL DEFAULT ((0)),
	--[SendEPerformance] [bit] NOT NULL CONSTRAINT [df_company_sendeperformance]  DEFAULT ((1)),
	--[TargetPayTime] [int] NULL DEFAULT ((0)),
 CONSTRAINT [PK_Clients] PRIMARY KEY CLUSTERED 
(
	[CompanyID] ASC
) ON [PRIMARY]
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