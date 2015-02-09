
SET IDENTITY_INSERT [dbo].[Agreement] ON;

-- Adds Contracts between Tommy Consultant and Company One
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
	1, 459, 172, 10, 1,  1, 575, '2013-12-11', '2014-1-1'
);
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
	2, 459, 172, 10, 1,  1, 575, '2012-12-11', '2013-12-10'
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
(2, 2, 99.00);

SET IDENTITY_INSERT [dbo].[Agreement_ContractRateDetail] OFF;

INSERT INTO [dbo].[Agreement_ContractDetail](
	[AgreementID],
	[SpecializationID]
)
VALUES
(1, 4),
(2, 5);