/*
	************************************************************ Candidate_Corporation Table ******************************************************
*/
CREATE TABLE [dbo].[Candidate_Corporation](
	[UserID] [int] NOT NULL,
	[CorpName] [nvarchar](125) NULL,
	--[GST_Number] [nvarchar](50) NULL,
	--[CorpDocsRecd] [bit] NULL,
	--[ChkVoid] [bit] NULL,
	--[SameAddress] [bit] NOT NULL,
	--[Inactive] [bit] NOT NULL,
	--[SinNumber] [int] NULL,
	--[TD1Form] [int] NULL,
	--[GSTNumberVerified] [int] NULL,
	--[SolePropProof] [int] NULL,
	--[verticalid] [int] NULL,
	--[DeclarationofPartnership] [bit] NOT NULL,
	--[RegistrationofPartnership] [bit] NOT NULL,
	--[LegalCompany] [bit] NULL,
	--[Documentsubmitted] [int] NOT NULL,
	--[DocumentApprovedDate] [datetime] NULL,
	--[OnHs] [bit] NULL,
 CONSTRAINT [PK_CandidateCorporations] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 97) ON [PRIMARY]
) ON [PRIMARY]

GO

/*
	************************************************************ Candidate_Corporation Constraints ******************************************************
*/
ALTER TABLE [dbo].[Candidate_Corporation]  WITH CHECK ADD  CONSTRAINT [FK_CandidateCorporations_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO

ALTER TABLE [dbo].[Candidate_Corporation] CHECK CONSTRAINT [FK_CandidateCorporations_Users]
GO
