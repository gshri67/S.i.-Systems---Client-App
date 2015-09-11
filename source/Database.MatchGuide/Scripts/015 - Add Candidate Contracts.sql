
SET IDENTITY_INSERT [dbo].[Agreement] ON;

-- Contracts
-- Tommy -> 2 old contracts with company 1
-- Bill -> 1 old contract with company 2
-- Candice -> Active contract with company 3 and old contract with company 1
INSERT INTO [dbo].[Agreement](
	[AgreementID],
	[AgreementType],
	[AgreementSubType],
	[CandidateID],
	[CompanyID],
	[ContactID],
	[StatusType],
	[StartDate],
	[EndDate]
)
VALUES
(
	1, 459, 172, 10, 1, 1, 575, '2013-12-11', '2014-1-1'
),
(
	2, 459, 172, 10, 1, 1, 575, '2012-12-11', '2013-12-10'
),
(
	3, 459, 172, 11, 2, 2, 575, '2012-12-11', '2013-12-10'
),
(
	4, 459, 172, 12, 1, 1, 575, '2012-12-11', '2013-12-10'
),
(
	5, 459, 172, 12, 3, 3, 573, '2013-12-11', '2050-12-10'
),
(
	6, 459, 172, 13, 4, 3, 573, '2012-12-11', '2013-12-10'
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
(1, 1, 'Regular Hours', 98.00, 88.00, '2014-12-01', '2015-12-01'),
(2, 2, NULL, 109.00, 99.00, '2014-12-01', '2015-12-01'),
(3, 3, NULL, 107.00, 97.00, '2014-12-01', '2015-12-01'),
(4, 4, NULL, 110.00, 100.00, '2014-12-01', '2015-12-01'),
(5, 5, NULL, 121.00, 111.00, '2014-12-01', '2015-12-01'),
(6, 6, NULL, 201.00, 191.00, '2014-12-01', '2015-12-01'),
(7, 1, 'Overtime', 128.00, 118.00, '2014-12-01', '2015-12-01'),
(8, 1, 'Special Rate', 88.00, 78.00, '2014-12-01', '2015-12-01');

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
(6, 'SUPER Developer', 4);

SET IDENTITY_INSERT [dbo].[Agreement_ContractAdminContactMatrix] ON;

INSERT INTO [dbo].[Agreement_ContractAdminContactMatrix]
           ([ContractAdminMatrixID]
		   ,[AgreementID]
           ,[DirectReportUserID]
           ,[Inactive]
)
VALUES
(1, 1, 1, 0),
(2, 2, 2, 0),
(3, 3, 1, 0),
(4, 4, 1, 0),
(5, 5, 2, 0),
(6, 6, 3, 0)
          

SET IDENTITY_INSERT [dbo].[Agreement_ContractAdminContactMatrix] OFF;