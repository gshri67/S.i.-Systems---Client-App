
SET IDENTITY_INSERT [dbo].[Users] ON;

INSERT INTO [dbo].[Users]
(
	[UserID], [FirstName], [Middle], [LastName], [UserType], [CompanyID], [Inactive], [ClientPortalTypeID], [ClientPortalFTAlumniTypeID]
)
VALUES
(4, 'Fred', 'H.', 'Flintstone', 490, NULL, 0, NULL, NULL);

SET IDENTITY_INSERT [dbo].[Users] OFF;

INSERT INTO [dbo].[User_Login]
(
	[UserID], [Login], [Password]
)
VALUES
(4, 'fred.flintstone@email.com', '5F4DCC3B5AA765D61D8327DEB882CF99');


INSERT INTO [dbo].[User_Email]
(
	[UserID], [PrimaryEmail]
)
VALUES
(4, 'fred.flintstone@email.com');