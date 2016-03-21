SET IDENTITY_INSERT [dbo].[Company] ON;

INSERT INTO [dbo].[Company]
(
	[CompanyID], [CompanyName], [FloThruFee], [MaxVisibleRatePerHour], [IsHavingFTAlumni]
)
VALUES
(1, 'Company One', 3, 100, 1),
(2, 'Company Two', 3, 100, 0),
(3, 'Company Three', 3, 100, 0),
(4, 'Company One Division 1', 3, 100, 0);

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

SET IDENTITY_INSERT [dbo].[CompanyProject] ON;

INSERT INTO [dbo].[CompanyProject]
(
	CompanyProjectID,
	ProjectID,
	Description,
	CompanyID
)
VALUES
(1, '', 'Project One', 1),
(2, '', 'Project Two', 3),
(3, '', 'General Project', 3),
(4, '', 'Operations Mobile Field Inspection', 1);

SET IDENTITY_INSERT [dbo].[CompanyProject] OFF;


SET IDENTITY_INSERT [dbo].[ContractProjectPO] ON;

INSERT INTO [dbo].[ContractProjectPO]
(
	ContractProjectPOID,
	CompanyProjectID,
	AgreementID,
	InActive,
	IsGeneralProjectPO,
	InactiveForUser,
	verticalid
)
VALUES
(1, 1, 1, 0, 0, 0, 4),
(2, 2, 1, 0, 0, 0, 4),
(3, 3, 1, 0, 0, 0, 4);

SET IDENTITY_INSERT [dbo].[ContractProjectPO] OFF;


SET IDENTITY_INSERT [dbo].[CompanyPO] ON;

INSERT INTO [dbo].[CompanyPO]
(
	CompanyPOID,
	PONumber,
	Description,
	CompanyID,
	InActive,
	CreateDate,
	CreateUserID,
	verticalid
)
VALUES
(1, 'PO Number', 'Description', 1, 0, '2013-12-11', 1, 4),
(2, 'PO Number', 'Description', 2, 0, '2013-12-11', 2, 4),
(3, 'PO Number', 'Description', 3, 0, '2013-12-11', 3, 4);

SET IDENTITY_INSERT [dbo].[CompanyPO] OFF;



SET IDENTITY_INSERT [dbo].[CompanyPOProjectMatrix] ON;

INSERT INTO [dbo].[CompanyPOProjectMatrix]
(
	CompanyPOProjectMatrixID,
	CompanyPOID,
	CompanyProjectID,
	InActive,
	verticalid
)
VALUES
(1, 1, 1, 0, 4),
(2, 2, 2, 0, 4),
(3, 3, 3, 0, 4);

SET IDENTITY_INSERT [dbo].[CompanyPOProjectMatrix] OFF;
