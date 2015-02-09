
SET IDENTITY_INSERT [dbo].[Agreement] ON;

-- Adds Resume for Tommy Consultant
INSERT INTO [dbo].[Candidate_ResumeInfo](
	[UserID],
	[ResumeText]
)
VALUES
(
	10, 'Tommy Consultant Resume text lorem ipsum and such..'
);