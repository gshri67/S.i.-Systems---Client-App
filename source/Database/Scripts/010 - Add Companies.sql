SET IDENTITY_INSERT [dbo].[Company] ON;

INSERT INTO [dbo].[Company]
(
	[CompanyID], [CompanyName]
)
VALUES
(1, 'Company One'),
(2, 'Company Two'),
(3, 'Company Three');

SET IDENTITY_INSERT [dbo].[Company] OFF;
