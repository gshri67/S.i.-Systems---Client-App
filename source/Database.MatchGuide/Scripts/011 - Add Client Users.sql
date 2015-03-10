
SET IDENTITY_INSERT [dbo].[Users] ON;

INSERT INTO [dbo].[Users]
(
	[UserID], [FirstName], [Middle], [LastName], [UserType], [CompanyID], [Inactive], [ClientPortalTypeID], [ClientPortalFTAlumniTypeID]
)
VALUES
(1, 'Bob', 'H.', 'Smith', 491, 1, 0, 833, 2469),
(2, 'Tom', 'H.','Smith', 491, 2, 0, 833, 2469),
(3, 'Sally', 'J.','Smitherson', 491, 3, 0, 833, 2469);

SET IDENTITY_INSERT [dbo].[Users] OFF;

INSERT INTO [dbo].[User_Login]
(
	[UserID], [Login], [Password]
)
VALUES
(1, 'bob.smith@email.com', '5F4DCC3B5AA765D61D8327DEB882CF99'),
(2, 'tom.smith@email.com', '5F4DCC3B5AA765D61D8327DEB882CF99'),
(3, 'sally.smitherson@email.com', '5F4DCC3B5AA765D61D8327DEB882CF99');


INSERT INTO [dbo].[User_Email]
(
	[UserID], [PrimaryEmail]
)
VALUES
(1, 'bob.smith@email.com'),
(2, 'tom.smith@email.com'),
(3, 'sally.smitherson@email.com');