SET IDENTITY_INSERT [dbo].[Company] ON;

INSERT INTO [dbo].[Company]
(
	[CompanyID], [CompanyName]
)
VALUES
(1, 'Company One'),
(2, 'Company Two'),
(3, 'Company Three'),
(4, 'Company One Division 1');

SET IDENTITY_INSERT [dbo].[Company] OFF;

SET IDENTITY_INSERT [dbo].[Company_ParentChildRelationship] ON;

INSERT INTO [dbo].[Company_ParentChildRelationship]
(
	[RelationshipID], [ParentID], [ChildID]
)
VALUES
(1, 1, 4);


SET IDENTITY_INSERT [dbo].[Company_ParentChildRelationship] OFF;
