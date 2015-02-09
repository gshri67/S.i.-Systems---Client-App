
SET IDENTITY_INSERT [dbo].[Users] ON;


INSERT INTO [dbo].[Users]
(
	[UserID], [FirstName], [Middle], [LastName], [UserType], [CompanyID], [Inactive]
)
VALUES
-- Contracts With Company One Only
(10, 'Tommy', 'H.', 'Contractor', 490, 1, 0),
-- Contracts With Company Two Only
(11, 'Bill', 'H.','Contractasaurus', 490, 2, 0),
-- Active With Comp Three And Alumni With One
(12, 'Candice', 'J.','Consulty', 490, 3, 0);

SET IDENTITY_INSERT [dbo].[Users] OFF;
