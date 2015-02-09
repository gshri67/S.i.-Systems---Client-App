
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
);

SET IDENTITY_INSERT [dbo].[Agreement] OFF;

SET IDENTITY_INSERT [dbo].[Agreement_ContractRateDetail] ON;

INSERT INTO [dbo].[Agreement_ContractRateDetail](
	[ContractRateID],
	[AgreementID],
	[BillRate]
)
VALUES
(1, 1, 88.00),
(2, 2, 99.00),
(3, 3, 97.00),
(4, 4, 100.00),
(5, 5, 111.00);

SET IDENTITY_INSERT [dbo].[Agreement_ContractRateDetail] OFF;

INSERT INTO [dbo].[Agreement_ContractDetail](
	[AgreementID],
	[SpecializationID]
)
VALUES
(1, 4),
(2, 5),
(3, 4),
(4, 4),
(5, 4);