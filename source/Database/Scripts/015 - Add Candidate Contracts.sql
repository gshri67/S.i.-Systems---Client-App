
SET IDENTITY_INSERT [dbo].[Agreement] ON;

-- Adds Contracts between Tommy Consultant and Company One
INSERT INTO [dbo].[Agreement](
	[AgreementID],
	[AgreementType],
	[AgreementSubType],
	[CandidateID],
	[CompanyID],
	[ContactID],
	[StartDate],
	[EndDate]
)
VALUES
(
	1, 1337, 1337, 10, 1,  1, '2013-12-11', '2014-1-1'
);
INSERT INTO [dbo].[Agreement](
	[AgreementID],
	[AgreementType],
	[AgreementSubType],
	[CandidateID],
	[CompanyID],
	[ContactID],
	[StartDate],
	[EndDate]
)
VALUES
(
	2, 1337, 1337, 10, 1,  1, '2012-12-11', '2013-12-10'
);

SET IDENTITY_INSERT [dbo].[Agreement] OFF;