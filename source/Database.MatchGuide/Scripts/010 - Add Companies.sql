SET IDENTITY_INSERT [dbo].[Company] ON;

INSERT INTO [dbo].[Company]
(
	[CompanyID], [CompanyName], [FloThruFee]
)
VALUES
(1, 'Company One', 3),
(2, 'Company Two', 3),
(3, 'Company Three', 3),
(4, 'Company One Division 1', 3);

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


SET IDENTITY_INSERT [dbo].[PAMInvoiceFormat] ON

INSERT INTO [dbo].[PAMInvoiceFormat] ([InvoiceFormatId], [Title]) 
VALUES 
(1, N'1 Credit per Client'),
(2, N'1 Credit per Consultant/PO'),
(3, N'1 Credit per PO or Project'),
(4, N'1 Credit per Project/PO Multi'),
(5, N'1 Credit per Reporting Manager'),
(6, N'1 Credit pr Contract (Job)'),
(7, N'1 Invoice per Client'),
(8, N'1 Invoice per Consultant/PO'),
(9, N'1 Invoice per Contract (Job)'),
(10, N'1 Invoice per Perm'),
(11, N'1 Invoice per PO or Project'),
(12, N'1 Invoice per Project/ PO Mult'),
(13, N'1 Invoice per Reporting Manage'),
(14, N'Contracts Renewed'),
(15, N'credit'),
(16, N'Credit for Consultant Renewal'),
(17, N'CREDIT FOR FED GOV INV'),
(18, N'Fed Govern with SI Systems'),
(19, N'Invoice information section'),
(20, N'One Credit per Temporary ID'),
(21, N'One Invoice per Temporary ID'),
(22, N'opamperm'),
(23, N'Perm Credit Note'),
(24, N'Weekly Invoicing'),
(25, N'Weekly Invoicing Credit'),
(26, N'PAM Perm Credit Note'),
(27, N'1 invoice per job'),
(28, N'1 invoice per job'),
(29, N'Nexstaf 1 Credit per Job'),
(30, N'Nexstaf 1 Invoice per Job')

SET IDENTITY_INSERT [dbo].[PAMInvoiceFormat] OFF

SET IDENTITY_INSERT [dbo].[PAMInvoiceFrequency] ON

INSERT INTO [dbo].[PAMInvoiceFrequency] ([InvoiceFrequencyId], [Title]) 
VALUES
(1, 'Monthly'),
(2, 'Semi Monthly'),
(3, 'Weekly Invoicing')

SET IDENTITY_INSERT [dbo].[PAMInvoiceFrequency] OFF
