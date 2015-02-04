SET IDENTITY_INSERT [dbo].[Company] ON;

INSERT INTO [dbo].[Company]
(
	[CompanyID], [CompanyName]
)
VALUES
(
	1, 'Company One'
);

SET IDENTITY_INSERT [dbo].[Company] OFF;

SET IDENTITY_INSERT [dbo].[Users] ON;

INSERT INTO [dbo].[Users]
(
	[UserID]
    ,[FirstName]
    ,[Middle]
    ,[LastName]           
    ,[UserType]
    ,[CompanyID]
	,[Inactive]
)
VALUES
(
	1
	,'Bob'
	,'H.'
	,'Smith'
	,490
	,1
	,0
);

SET IDENTITY_INSERT [dbo].[Users] OFF;

INSERT INTO [dbo].[User_Login]
(
	[UserID],
	[Login],
	[Password]
)
VALUES
(
	1
	,'bob.smith@email.com'
	,'5F4DCC3B5AA765D61D8327DEB882CF99'
);