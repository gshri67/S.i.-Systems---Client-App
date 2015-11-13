﻿
SET IDENTITY_INSERT [dbo].[Users] ON;

INSERT INTO [dbo].[Users]
(
	[UserID], [FirstName], [Middle], [LastName], [UserType], [CompanyID], [Inactive], [ClientPortalTypeID], [ClientPortalFTAlumniTypeID]
)
VALUES
(1, 'Bob', 'H.', 'Smith', 491, 1, 0, 833, 2469),
(2, 'Tom', 'H.','Smith', 491, 2, 0, 833, 2469),
(3, 'Sally', 'J.','Smitherson', 491, 3, 0, 833, 2469),
(4, 'Joe', 'M.','Johnson', 491, 1, 0, 833, 2469),
(5, 'Jessica', 'N.','Li', 491, 1, 0, 833, 2469),
(6, 'Ed', 'H.','Maron', 491, 1, 0, 833, 2469);

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
(3, 'sally.smitherson@email.com'),
(4, 'joe.johnson@email.com'),
(5, 'jessica.li@email.com'),
(6, 'ed.maron@email.com');



SET IDENTITY_INSERT [dbo].[Users] ON;

INSERT INTO [dbo].[Users]
(
	[UserID], [FirstName], [Middle], [LastName], [UserType], [CompanyID], [Inactive], [ClientPortalTypeID], [ClientPortalFTAlumniTypeID]
)
VALUES
(7, 'Adam', 'H.', 'Si', 693, null, 0, null, null);

SET IDENTITY_INSERT [dbo].[Users] OFF;

INSERT INTO [dbo].[User_Login]
(
	[UserID], [Login], [Password]
)
VALUES
(7, 'adam.si', '5F4DCC3B5AA765D61D8327DEB882CF99');


INSERT INTO [dbo].[User_Email]
(
	[UserID], [PrimaryEmail]
)
VALUES
(7, 'adam.si@email.com');