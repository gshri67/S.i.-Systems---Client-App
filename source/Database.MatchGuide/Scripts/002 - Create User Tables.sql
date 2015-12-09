
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
	[verticalid] [int] NULL,
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
CREATE TABLE [dbo].[User_Phone](
	[UserID] [int] NOT NULL,
	[Home_AreaCode] [decimal](3, 0) NULL CONSTRAINT [DF_User_Phone_Home_AreaCode]  DEFAULT (null),
	[Home_Number] [decimal](7, 0) NULL CONSTRAINT [DF_User_Phone_Home_Number]  DEFAULT (null),
	[Work_AreaCode] [decimal](3, 0) NULL CONSTRAINT [DF_User_Phone_Work_AreaCode]  DEFAULT (null),
	[Work_Number] [decimal](7, 0) NULL CONSTRAINT [DF_User_Phone_Work_Number]  DEFAULT (null),
	[Work_Extension] [int] NULL CONSTRAINT [DF_User_Phone_Work_Extension]  DEFAULT (null),
	[Cell_AreaCode] [decimal](3, 0) NULL CONSTRAINT [DF_User_Phone_Cell_AreaCode]  DEFAULT (null),
	[Cell_Number] [decimal](7, 0) NULL CONSTRAINT [DF_User_Phone_Cell_Number]  DEFAULT (null),
	[Fax_AreaCode] [decimal](3, 0) NULL CONSTRAINT [DF_User_Phone_Fax_AreaCode]  DEFAULT (null),
	[Fax_Number] [decimal](7, 0) NULL CONSTRAINT [DF_User_Phone_Fax_Number]  DEFAULT (null),
	[Other_AreaCode] [decimal](3, 0) NULL CONSTRAINT [DF_User_Phone_Other_AreaCode]  DEFAULT (null),
	[Other_Number] [decimal](7, 0) NULL CONSTRAINT [DF_User_Phone_Other_Number]  DEFAULT (null),
	[Other_Extension] [int] NULL CONSTRAINT [DF_User_Phone_Other_Extension]  DEFAULT (null),
	[verticalid] [int] NULL,
 CONSTRAINT [PK_User_Phone] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[User_Phone]  WITH NOCHECK ADD  CONSTRAINT [FK_User_Phone_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO

ALTER TABLE [dbo].[User_Phone] NOCHECK CONSTRAINT [FK_User_Phone_Users]
GO

ALTER TABLE [dbo].[User_Phone]  WITH CHECK ADD  CONSTRAINT [chkverticalid_User_Phone] CHECK  (([verticalid] IS NOT NULL AND [verticalid]<>(0)))
GO

ALTER TABLE [dbo].[User_Phone] CHECK CONSTRAINT [chkverticalid_User_Phone]
GO




CREATE TABLE [dbo].[Address](
	[AddressID] [int] IDENTITY(1,1) NOT NULL,
	[Address1] [varchar](100) NULL CONSTRAINT [DF_Address_Address1]  DEFAULT ('Need Address Information'),
	[Address2] [varchar](100) NULL,
	[Address3] [varchar](100) NULL,
	[Address4] [varchar](100) NULL,
	[City] [varchar](100) NULL,
	[PostalCode] [varchar](6) NULL,
	[ZipCode] [varchar](10) NULL,
	[ProvinceType] [int] NULL,
	[Country] [int] NULL CONSTRAINT [DF_Address_Country]  DEFAULT ((6)),
	[AddressType] [int] NULL,
	[CreateDate] [smalldatetime] NOT NULL CONSTRAINT [DF_Address_CreateDate]  DEFAULT (getdate()),
	[CreateUserID] [int] NOT NULL,
	[Updatedate] [smalldatetime] NULL,
	[UpdateUserID] [int] NULL,
	[Inactive] [bit] NOT NULL CONSTRAINT [DF_Address_Inactive]  DEFAULT ((0)),
	[UserID] [int] NULL,
	[verticalid] [int] NULL,
 CONSTRAINT [PK_Addresses_1] PRIMARY KEY CLUSTERED 
(
	[AddressID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 97) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[Address]  WITH CHECK ADD  CONSTRAINT [chkverticalid_Address] CHECK  (([verticalid] IS NOT NULL AND [verticalid]<>(0)))
GO

ALTER TABLE [dbo].[Address] CHECK CONSTRAINT [chkverticalid_Address]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identity for Addresses' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Address', @level2type=N'COLUMN',@level2name=N'AddressID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Line 1 of address' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Address', @level2type=N'COLUMN',@level2name=N'Address1'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Line 2 of address' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Address', @level2type=N'COLUMN',@level2name=N'Address2'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Line 3 of address' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Address', @level2type=N'COLUMN',@level2name=N'Address3'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Line 4 of address' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Address', @level2type=N'COLUMN',@level2name=N'Address4'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'City' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Address', @level2type=N'COLUMN',@level2name=N'City'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PostalCode' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Address', @level2type=N'COLUMN',@level2name=N'PostalCode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID from PickList of Province name' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Address', @level2type=N'COLUMN',@level2name=N'ProvinceType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Country PickListID from PickList' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Address', @level2type=N'COLUMN',@level2name=N'Country'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID from PickList address types' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Address', @level2type=N'COLUMN',@level2name=N'AddressType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date address added to table' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Address', @level2type=N'COLUMN',@level2name=N'CreateDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'UserID for internal user who created the address record ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Address', @level2type=N'COLUMN',@level2name=N'CreateUserID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date record updated' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Address', @level2type=N'COLUMN',@level2name=N'Updatedate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'UserID for internal user who updated the address record' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Address', @level2type=N'COLUMN',@level2name=N'UpdateUserID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date address was deleted (marked as invalid)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Address', @level2type=N'COLUMN',@level2name=N'Inactive'
GO


CREATE TABLE [dbo].[User_Address](
	[UserID] [int] NOT NULL,
	[AddressID] [int] NOT NULL,
	[MainAddress] [bit] NULL CONSTRAINT [DF_User_Address_MainAddress]  DEFAULT (0),
	[Inactive] [bit] NOT NULL CONSTRAINT [DF_User_Address_Inactive]  DEFAULT (0),
	[verticalid] [int] NULL,
 CONSTRAINT [PK_UserAddressIDs] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC,
	[AddressID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 97) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[User_Address]  WITH NOCHECK ADD  CONSTRAINT [FK_UserAddressIDs_Addresses] FOREIGN KEY([AddressID])
REFERENCES [dbo].[Address] ([AddressID])
GO

ALTER TABLE [dbo].[User_Address] NOCHECK CONSTRAINT [FK_UserAddressIDs_Addresses]
GO

ALTER TABLE [dbo].[User_Address]  WITH CHECK ADD  CONSTRAINT [FK_UserAddressIDs_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO

ALTER TABLE [dbo].[User_Address] CHECK CONSTRAINT [FK_UserAddressIDs_Users]
GO

ALTER TABLE [dbo].[User_Address]  WITH CHECK ADD  CONSTRAINT [chkverticalid_User_Address] CHECK  (([verticalid] IS NOT NULL AND [verticalid]<>(0)))
GO

ALTER TABLE [dbo].[User_Address] CHECK CONSTRAINT [chkverticalid_User_Address]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'UserID from Users table of individual with the address' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'User_Address', @level2type=N'COLUMN',@level2name=N'UserID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'AddressID from Address table of the location address' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'User_Address', @level2type=N'COLUMN',@level2name=N'AddressID'
GO


