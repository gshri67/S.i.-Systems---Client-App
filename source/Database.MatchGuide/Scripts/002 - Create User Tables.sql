
CREATE TABLE [dbo].[Users](
	[UserID] [int] IDENTITY(500,1) NOT NULL,
	--[HonorificType] [int] NULL CONSTRAINT [DF_Users_Honorific]  DEFAULT (0),
	[FirstName] [nvarchar](30) NOT NULL CONSTRAINT [DF_Users_FirstName]  DEFAULT ('Need First Name'),
	[Middle] [nvarchar](25) NULL,
	[LastName] [nvarchar](30) NOT NULL CONSTRAINT [DF_Users_LastName]  DEFAULT ('Need Last Name'),
	--[Nickname] [varchar](50) NULL,
	[UserType] [int] NOT NULL,
	--[Title] [nvarchar](250) NULL,
	[CompanyID] [int] NULL,
	--[PrefLangType] [int] NULL CONSTRAINT [DF_Users_PrefLang]  DEFAULT (196),
	--[UserOfficeID] [int] NULL CONSTRAINT [DF_Users_OfficeID]  DEFAULT (0),
	--[StatusType] [int] NULL,
	--[JoinCandidateID] [int] NULL,
	--[JoinClientContactID] [int] NULL,
	--[CreateUserID] [int] NOT NULL,
	--[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_Users_CreateDate]  DEFAULT (getdate()),
	--[UpdateUserID] [int] NULL,
	--[UpdateDate] [datetime] NULL,
	--[SelfUpdateDate] [datetime] NULL,
	--[Old_CandidateID] [int] NULL CONSTRAINT [DF_Users_Old_CandidateID]  DEFAULT (0),
	--[Old_ClientContactID] [int] NULL CONSTRAINT [DF_Users_Old_ClientContactID]  DEFAULT (0),
	--[Old_UserProfileID] [int] NULL CONSTRAINT [DF_Users_Old_UserProfileID]  DEFAULT (0),
	--[Old_CareerCandidateID] [int] NULL CONSTRAINT [DF_Users_Old_CareerCandidateID]  DEFAULT (0),
	--[Old_UGA] [int] NULL CONSTRAINT [DF_Users_Old_UGA]  DEFAULT (0),
	--[Old_TimesheetContactID] [int] NULL CONSTRAINT [DF_Users_Old_TimesheetContactID]  DEFAULT (0),
	--[MigrationSource] [varchar](50) NULL,
	[Inactive] [bit] NOT NULL CONSTRAINT [DF_Users_Inactive]  DEFAULT (0),
	--[GP_UserID] [int] NOT NULL,
	--[PAM] [bit] NULL,
	--[old_aeuserid] [int] NULL,
	--[ActiveDirectoryLogin] [varchar](200) NULL,
	[ClientPortalTypeID] [int] NULL,
	--[IsEmpInPAM] [bit] NULL CONSTRAINT [DF_Users_IsEmpInPAM]  DEFAULT ((0)),
	--[TaxableProvinceType] [int] NULL,
	--[verticalid] [int] NULL,
	--[Locationid] [int] NULL,
	--[IsInCloud] [int] NULL DEFAULT (NULL),
	[ClientPortalFTAlumniTypeID] [int] NULL DEFAULT (NULL),
	CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([UserID] ASC)
);
GO

CREATE TABLE [dbo].[User_Login](
	[UserID] [int] NOT NULL,
	[Login] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](200) NULL,
	--[CreateUserID] [int] NOT NULL,
	--[CreateDateTime] [smalldatetime] NOT NULL CONSTRAINT [DF_User_Login_CreateDateTime]  DEFAULT (getdate()),
	--[UpdateUserID] [int] NULL,
	--[UpdateDateTime] [smalldatetime] NULL,
	--[Password_Expiry] [smalldatetime] NULL,
	--[Old_Password] [nvarchar](50) NULL,
	--[ForceUpdate] [bit] NULL CONSTRAINT [DF_User_Login_ForceUpdate]  DEFAULT ((0)),
	--[ActiveDirectoryLogin] [varchar](100) NULL,
	--[verticalid] [int] NULL,
	CONSTRAINT [PK_UserLogin] PRIMARY KEY CLUSTERED 
	(
		[UserID] ASC
	) ON [PRIMARY],
	CONSTRAINT [FK_User_Login_Users] FOREIGN KEY ([UserID]) REFERENCES [dbo].[Users] ([UserID])
	)
GO

CREATE TABLE [dbo].[User_Email](
	[UserID] [int] NOT NULL CONSTRAINT [DF_UserEmail_UserID]  DEFAULT ((0)),
	[PrimaryEmail] [nvarchar](50) NOT NULL,
	--[SecondaryEmail] [nvarchar](50) NULL,
	--[EmailType] [int] NULL CONSTRAINT [DF_UserEmail_EmailPref]  DEFAULT ((1)),
	--[EmailVerified] [bit] NULL CONSTRAINT [DF_UserEmail_EmailVerified]  DEFAULT ((0)),
	--[EmailVerifiedDate] [smalldatetime] NULL,
	--[EmailOverRide] [bit] NULL,
	--[verticalid] [int] NULL,
 CONSTRAINT [PK_User_Email] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
) ON [PRIMARY]
);

GO