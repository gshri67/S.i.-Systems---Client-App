﻿
SET IDENTITY_INSERT [dbo].[Users] ON;

INSERT INTO [dbo].[Users]
(
	[UserID], [FirstName], [Middle], [LastName], [UserType], [CompanyID], [Inactive], [ClientPortalTypeID], [ClientPortalFTAlumniTypeID], [GP_UserID]
)
VALUES
(1, 'Bob', 'H.', 'Smith', 491, 1, 0, 833, 2469, 7),
(2, 'Tom', 'H.','Smith', 491, 2, 0, 833, 2469, 8),
(3, 'Sally', 'J.','Smitherson', 491, 3, 0, 833, 2469, 9),
(4, 'Joe', 'M.','Johnson', 491, 1, 0, 833, 2469, 10),
(5, 'Jessica', 'N.','Li', 491, 1, 0, 833, 2469, 11),
(6, 'Ed', 'H.','Maron', 491, 1, 0, 833, 2469, 12);

SET IDENTITY_INSERT [dbo].[Users] OFF;

INSERT INTO [dbo].[User_Login]
(
	[UserID], [Login], [Password], [ForceUpdate]
)
VALUES
(1, 'bob.smith@email.com', '5F4DCC3B5AA765D61D8327DEB882CF99', 0),
(2, 'tom.smith@email.com', '5F4DCC3B5AA765D61D8327DEB882CF99', 0),
(3, 'sally.smitherson@email.com', '5F4DCC3B5AA765D61D8327DEB882CF99', 0);


INSERT INTO [dbo].[User_Email]
(
	[UserID], [PrimaryEmail], [SecondaryEmail]
)
VALUES
(1, 'bob.smith@email.com', 'b.s@email.com'),
(2, 'tom.smith@email.com', 't.s@email.com'),
(3, 'sally.smitherson@email.com', 's.s@email.com'),
(4, 'joe.johnson@email.com', null),
(5, 'jessica.li@email.com', null),
(6, 'ed.maron@email.com', null);



SET IDENTITY_INSERT [dbo].[Users] ON;

INSERT INTO [dbo].[Users]
(
	[UserID], [FirstName], [Middle], [LastName], [UserType], [CompanyID], [Inactive], [ClientPortalTypeID], [ClientPortalFTAlumniTypeID], [verticalid], [Title], [UserOfficeID], [GP_UserID]
)
VALUES
(7, 'Adam', 'H.', 'Si', 693, null, 0, null, null, 4, 'Account Executive', 1, 13),
(20, 'Pauline', '', 'Frazzle', 693, null, 0, null, null, 4, 'Account Executive', 1, 13),
(21, 'Frank', '', 'Drabble', 693, null, 0, null, null, 4, 'Managing Director', 1, 13),
(22, 'Jacob', '', 'Ladders', 693, null, 0, null, null, 4, 'Account Executive', 2, 13);

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

INSERT INTO [dbo].[User_Phone]
(
	[UserID], [Home_AreaCode], [Home_Number]
	, [Work_AreaCode], [Work_Number] ,[Work_Extension]
    , [Cell_AreaCode], [Cell_Number]
	, [Fax_AreaCode] ,[Fax_Number]
    , [Other_AreaCode], [Other_Number], [Other_Extension]
    ,[verticalid]
)
VALUES
	(1, 555, 5551234
	, 555, 5559876, 456
	, 555, 2221234
	, 555, 5555555
	, 555, 5553333, null, 4),
	(10, 555, 5551234
	, 555, 5559876, 456
	, 555, 2221234
	, 555, 5555555
	, 555, 5553333, null, 4),
	(7, null, null
	, null, null, null
	, null, null
	, null, null
	, null, null, null, 4),
	(4, 555, 5551234
	, 555, 5559876, 456
	, null, null
	, null, null
	, null, null, null, 4);