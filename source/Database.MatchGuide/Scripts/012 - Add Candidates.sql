
SET IDENTITY_INSERT [dbo].[Users] ON;


INSERT INTO [dbo].[Users]
(
	[UserID], [FirstName], [Middle], [LastName], [UserType], [CompanyID], [Inactive]
)
VALUES
(10, 'Tommy', 'H.', 'Consultant', 490, 1, 0),
(11, 'Bill', 'H.','Contractor', 490, 2, 0),
(12, 'Candice', 'J.','Candidate', 490, 3, 0);

SET IDENTITY_INSERT [dbo].[Users] OFF;
