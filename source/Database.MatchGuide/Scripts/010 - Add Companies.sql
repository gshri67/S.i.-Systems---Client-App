SET IDENTITY_INSERT [dbo].[Company] ON;

INSERT INTO [dbo].[Company]
(
	[CompanyID], [CompanyName], [FloThruFee], [MaxVisibleRatePerHour]
)
VALUES
(1, 'Company One', 3, 100),
(2, 'Company Two', 3, 100),
(3, 'Company Three', 3, 100),
(4, 'Company One Division 1', 3, 100);

SET IDENTITY_INSERT [dbo].[Company] OFF;

SET IDENTITY_INSERT [dbo].[Company_ParentChildRelationship] ON;

INSERT INTO [dbo].[Company_ParentChildRelationship]
(
	[RelationshipID], [ParentID], [ChildID]
)
VALUES
(1, 1, 4);


SET IDENTITY_INSERT [dbo].[Company_ParentChildRelationship] OFF;

SET IDENTITY_INSERT [dbo].[CompanyDomain] ON;

INSERT INTO [dbo].[CompanyDomain]
(
	CompanyDomainID, CompanyID, EmailDomain
)
VALUES
(1, 1, 'companyone'),
(2, 2, 'companytwo'),
(3, 3, 'companythree'),
(4, 4, 'division1.companyone');

SET IDENTITY_INSERT [dbo].[CompanyDomain] OFF;

