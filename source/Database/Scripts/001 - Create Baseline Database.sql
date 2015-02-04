SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Company](
	[CompanyID] [int] IDENTITY(1,1) NOT NULL,
	[CompanyName] [varchar](150) NOT NULL
	CONSTRAINT [PK_Clients] PRIMARY KEY CLUSTERED 
	(
		[CompanyID] ASC
	) ON [PRIMARY]
);
GO

CREATE TABLE [dbo].[Users](
	[UserID] [int] IDENTITY(500,1) NOT NULL,
	[FirstName] [nvarchar](30) NOT NULL,
	[Middle] [nvarchar](25) NULL,
	[LastName] [nvarchar](30) NOT NULL,
	[UserType] [int] NOT NULL,
	[CompanyID] [int] NULL,
	[Inactive] [bit] NOT NULL
	CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([UserID] ASC)
);
GO

CREATE TABLE [dbo].[User_Login](
	[UserID] [int] NOT NULL,
	[Login] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](200) NULL
	CONSTRAINT [PK_UserLogin] PRIMARY KEY CLUSTERED 
	(
		[UserID] ASC
	) ON [PRIMARY],
	CONSTRAINT [FK_User_Login_Users] FOREIGN KEY ([UserID]) REFERENCES [dbo].[Users] ([UserID])
	)
GO

SET ANSI_PADDING OFF
GO
