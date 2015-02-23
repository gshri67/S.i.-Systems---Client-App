
SET IDENTITY_INSERT [dbo].[Users] ON;

INSERT INTO [dbo].[Users]
(
	[UserID], [FirstName], [Middle], [LastName], [UserType], [CompanyID], [Inactive]
)
VALUES
(1, 'Bob', 'H.', 'Smith', 490, 1, 0),
(2, 'Tom', 'H.','Smith', 490, 2, 0),
(3, 'Sally', 'J.','Smitherson', 490, 3, 0);

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