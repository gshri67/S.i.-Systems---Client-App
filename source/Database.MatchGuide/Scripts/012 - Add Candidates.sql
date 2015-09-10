
SET IDENTITY_INSERT [dbo].[Users] ON;


INSERT INTO [dbo].[Users]
(
	[UserID], [FirstName], [Middle], [LastName], [UserType], [Inactive]
)
VALUES
-- Contracts With Company One Only
(10, 'Tommy', 'H.', 'Contractor', 490, 0),
-- Contracts With Company Two Only
(11, 'Bill', 'H.','Contractasaurus', 490, 0),
-- Active With Comp Three And Alumni With One
(12, 'Candice', 'J.','Consulty', 490, 0),
-- Alumni with division of company one
(13, 'Sally', 'P', 'Divisioner', 490, 0),
-- Active for Contractor Application
(14, 'Fred', 'W', 'Flintstone', 490, 0);

SET IDENTITY_INSERT [dbo].[Users] OFF;

INSERT INTO [dbo].[User_Login]
(
	[UserID], [Login], [Password]
)
VALUES
(12, 'candice.consulty@email.com', '5F4DCC3B5AA765D61D8327DEB882CF99'),
(14, 'fred.flintstone@email.com', '5F4DCC3B5AA765D61D8327DEB882CF99');



INSERT INTO [dbo].[User_Email]
(
	[UserID], [PrimaryEmail]
)
VALUES
(10, 'tommy.contractor@email.com'),
(11, 'bill.contractasaurus@email.com'),
(12, 'candice.consulty@email.com'),
(13, 'sally.divisioner@email.com'),
(14, 'fred.flintstone@email.com');

INSERT INTO [dbo].[Candidate_Corporation]
(
	[UserID], [CorpName]
)
VALUES
(12, 'Candice Consulty Ltd.');