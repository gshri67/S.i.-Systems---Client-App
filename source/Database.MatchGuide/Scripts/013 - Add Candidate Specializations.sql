﻿
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
	[Description],
	[VerticalId]
)
VALUES
(4, 'Project Management', 'PM', 4),
(5, 'Software Development', 'SD', 4);

SET IDENTITY_INSERT [dbo].[Specialization] OFF;


-- Fill out Tommy Consultant's Skill Matrix

SET IDENTITY_INSERT [dbo].[Candidate_SkillsMatrix] ON;
INSERT INTO [dbo].[Candidate_SkillsMatrix](
	[SkillsMatrixID],
	[UserID],
	[SpecID],
	[SkillID],
	[ExpID]
)
VALUES
(1, 10, 4, 3, 448), --PM -> 7 Sigma
(2, 10, 5, 1, 450), --SD -> Java
(3, 10, 5, 2, 451), --SD -> C#
(4, 10, 5, 4, 452); --SD -> ColdFusion
SET IDENTITY_INSERT [dbo].[Candidate_SkillsMatrix] OFF;