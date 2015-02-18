
SET IDENTITY_INSERT [dbo].[Agreement] ON;

-- Adds Resume for Tommy Consultant
INSERT INTO [dbo].[Candidate_ResumeInfo](
	[UserID],
	[ResumeText],
	-- 314 = 'Above Standard', 315 = 'Standard', 316 = 'Below Standard', 317 = 'Not Checked'
	[ReferenceValue]
)
VALUES
(10, 'Tommy Contractor Resume text lorem ipsum and such..', 315),
-- Assume Bill does not have a resume in order to test LEFT JOIN.
--(11, 'Bill Contractasaurus Resume text lorem ipsum and such..', 316),
(12, 'Candice Consulty Resume text lorem ipsum and such..', 314),
(13, 'Sally P Divisioner blah blah blah', 314);