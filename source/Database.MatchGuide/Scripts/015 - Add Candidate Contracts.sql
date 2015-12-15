
SET IDENTITY_INSERT [dbo].[Agreement] ON;

-- Contracts
-- Tommy -> 2 old contracts with company 1
-- Bill -> 1 old contract with company 2
-- Candice -> Active contract with company 3 and old contract with company 1
INSERT INTO [dbo].[Agreement](
	[AgreementID],
	[AgreementType],
	[AgreementSubType],
	[AccountExecID],
	[CandidateID],
	[CompanyID],
	[ContactID],
	[StatusType],
	[StartDate],
	[EndDate]
)
VALUES
(
	1, 459, 172, 7, 10, 1, 1, 575, '2013-12-11', '2014-1-1'
),
(
	2, 459, 172, 7, 10, 1, 1, 575, '2012-12-11', '2013-12-10'
),
(
	3, 459, 172, 7, 11, 2, 2, 575, '2012-12-11', '2013-12-10'
),
(
	4, 459, 172, 7, 12, 1, 1, 575, '2012-12-11', '2016-12-10'
),
(
	5, 459, 172, 7, 12, 3, 3, 573, '2013-12-11', '2050-12-10'
),
(
	6, 459, 172, 7, 13, 4, 3, 573, '2012-12-11', '2016-12-10'
),
--Expired Jobs
(
	7, 458, 172, 7, NULL, 1, 1, 575, '2013-12-11', '2016-01-01'
),
--Cancelled Jobs
(
	8, 458, 171, 7, NULL, 1, 1, 574, '2012-12-11', '2016-01-01'
),
--Active Jobs
(
	9, 458, 171, 7, NULL, 2, 2, 580, '2016-01-01', '2017-06-15'
),
(
	10, 458, 172, 7, NULL, 1, 1, 580, '2016-01-01', '2016-02-15'
),
(
	11, 458, 172, 7, NULL, 3, 3, 580, '2016-01-01', '2050-12-10'
),
(
	12, 458, 172, 7, NULL, 4, 3, 580, '2016-01-01', '2016-12-31'
);

SET IDENTITY_INSERT [dbo].[Agreement] OFF;

SET IDENTITY_INSERT [dbo].[Agreement_ContractRateDetail] ON;

INSERT INTO [dbo].[Agreement_ContractRateDetail](
	[ContractRateID],
	[AgreementID],
	[RateDescription],
	[BillRate],
	[PayRate],
	[StartDate],
	[EndDate]
)
VALUES
(1, 1, 'Regular Hours', 98.00, 88.00, '2014-12-01', '2017-12-01'),
(2, 2, NULL, 109.00, 99.00, '2014-12-01', '2015-12-01'),
(3, 3, NULL, 107.00, 97.00, '2014-12-01', '2015-12-01'),
(4, 4, NULL, 110.00, 100.00, '2014-12-01', '2017-12-01'),
(5, 5, NULL, 121.00, 111.00, '2014-12-01', '2017-12-01'),
(6, 6, NULL, 201.00, 191.00, '2014-12-01', '2017-12-01'),
(7, 1, 'Overtime', 128.00, 118.00, '2014-12-01', '2015-12-01'),
(8, 1, 'Special Rate', 88.00, 78.00, '2014-12-01', '2017-12-01');

SET IDENTITY_INSERT [dbo].[Agreement_ContractRateDetail] OFF;

INSERT INTO [dbo].[Agreement_ContractDetail](
	[AgreementID],
	[JobTitle],
	[SpecializationID]
)
VALUES
(1, 'Senior Project Manager',  4),
(2, 'Junior Java Jiver', 5),
(3, 'Project Management Consultant', 4),
(4, 'C# Afficionado', 4),
(5, 'Humble Developer', 4),
(6, 'SUPER Developer', 4),
(7, 'Senior Project Manager',  4),
(8, 'Junior Java Jiver', 5),
(9, 'Project Management Consultant', 4),
(10, 'C# Afficionado', 4),
(11, 'Humble Developer', 4),
(12, 'SUPER Developer', 4);

SET IDENTITY_INSERT [dbo].[Agreement_ContractAdminContactMatrix] ON;

INSERT INTO [dbo].[Agreement_ContractAdminContactMatrix]
           ([ContractAdminMatrixID]
		   ,[AgreementID]
           ,[DirectReportUserID]
		   ,[BillingUserID]
           ,[Inactive]
)
VALUES
(1, 1, 1, 1, 0),
(2, 2, 2, 2, 0),
(3, 3, 1, 1, 0),
(4, 4, 1, 1, 0),
(5, 5, 2, 2, 0),
(6, 6, 3, 3, 0)
          

SET IDENTITY_INSERT [dbo].[Agreement_ContractAdminContactMatrix] OFF;

SET IDENTITY_INSERT [dbo].[ContractInvoiceCode] ON;

INSERT INTO [dbo].[ContractInvoiceCode](
	InvoiceCodeID,
	InvoiceCodeText,
	ContractID
)
VALUES
(1, '[CC:57846][G:7030.334][WO:00830721W][A:-]', 4),
(2, '[A:15161088][G:7030.334][WO:00830369][CC:-]', 5),
(3, '[CC:55131][G:7030.334][A:-][WO:-]', 6);

SET IDENTITY_INSERT [dbo].[ContractInvoiceCode] OFF;


SET IDENTITY_INSERT [dbo].[Agreement_OpportunityCandidateMatrix] ON;

INSERT INTO [dbo].[Agreement_OpportunityCandidateMatrix](
	OpportunityCandidateMatrixID,
	AgreementID,
	CandidateUserID,
	StatusType,
	StatusSubType,
	CreateDateTime,
	UpdateDateTime,
	CreateUserID
)
VALUES
(1, 1, 1, 0, 0, '2014-12-01',  '2014-12-01', 0),
(2, 2, 2, 1, 1, '2014-12-01',  '2014-12-01', 0),
(3, 3, 3, 731, 583, '2014-12-01', '2014-12-01', 0);

SET IDENTITY_INSERT [dbo].[Agreement_OpportunityCandidateMatrix] OFF;



INSERT INTO [dbo].[Agreement_OpportunityDetail](
	[AgreementID]
	,[NumberRequired]
	,[verticalid]
)
VALUES
(1, 1, 4),
(2, 2, 4),
(3, 3, 4);
