
SET IDENTITY_INSERT [dbo].[Skill] ON;

INSERT INTO [dbo].[Skill]
(
	[SkillID],
	[SkillName]
)
VALUES
(1, 'Java'),
(2, 'C#'),
(3, '7 Sigma'),
(4, 'ColdFusion');

SET IDENTITY_INSERT [dbo].[Skill] OFF;


SET IDENTITY_INSERT [dbo].[Specialization] ON;


INSERT INTO [dbo].[Specialization]
(
	[SpecializationID],
	[Name],
	[Description]
)
VALUES
(4, 'Project Management', 'PM'),
(5, 'Software Development', 'SD');

SET IDENTITY_INSERT [dbo].[Specialization] OFF;


-- Fill out Tommy Consultant's Skill Matrix

SET IDENTITY_INSERT [dbo].[Candidate_SkillsMatrix] ON;
INSERT INTO [dbo].[Candidate_SkillsMatrix](
	[SkillsMatrixID],
	[UserID],
	[SpecID],
	[SkillID]
)
VALUES
(1, 10, 4, 3), --PM -> 7 Sigma
(2, 10, 5, 1), --SD -> Java
(3, 10, 5, 2), --SD -> C#
(4, 10, 5, 4); --SD -> ColdFusion
SET IDENTITY_INSERT [dbo].[Candidate_SkillsMatrix] OFF;