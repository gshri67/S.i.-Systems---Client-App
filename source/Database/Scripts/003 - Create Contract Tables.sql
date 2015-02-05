

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
	--[StatusType] [int] NOT NULL,
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
