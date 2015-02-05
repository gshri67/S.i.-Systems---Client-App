

CREATE TABLE [dbo].[Skill](
	[SkillID] [int] IDENTITY(1,1) NOT NULL,
	[SkillName] [varchar](50) NOT NULL,
	--[isRole] [bit] NULL CONSTRAINT [DF_Skill_isRole]  DEFAULT (0),
	--[Name_Old] [varchar](50) NULL,
	--[isNew] [bit] NOT NULL CONSTRAINT [DF_Skill_isNew]  DEFAULT (0),
	[Inactive] [int] NOT NULL CONSTRAINT [DF_Skill_Inactive]  DEFAULT (0),
	--[verticalid] [int] NULL,
	--[SkillTypeID] [int] NULL,
 CONSTRAINT [PK_Skill] PRIMARY KEY CLUSTERED 
(
	[SkillID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 97) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[Specialization](
	[SpecializationID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Description] [varchar](255) NULL,
	--[Name_Old] [varchar](50) NULL,
	--[DisplayLegacy] [bit] NOT NULL CONSTRAINT [DF_Specialization_displayLegacy_1]  DEFAULT (0),
	[Inactive] [int] NOT NULL CONSTRAINT [DF_Specialization_Inactive]  DEFAULT (0),
	--[verticalid] [int] NULL,
 CONSTRAINT [PK_Specialization] PRIMARY KEY CLUSTERED 
(
	[SpecializationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 97) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[Candidate_SkillsMatrix](
	[SkillsMatrixID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[SpecID] [int] NOT NULL,
	[SkillID] [int] NULL,
	--[ExpID] [int] NULL,
	--[KeySkill] [bit] NULL CONSTRAINT [DF_Candidate_SkillsMatrix_KeySkill]  DEFAULT (0),
	[Inactive] [bit] NULL CONSTRAINT [DF_Candidate_SkillsMatrix_Inactive]  DEFAULT ((0)),
	--[CreateUserID] [int] NULL,
	--[CreateDateTime] [datetime] NULL,
	--[UpdateUserID] [int] NULL,
	--[UpdateDateTime] [datetime] NULL,
	--[verticalid] [int] NULL,
 CONSTRAINT [PK_Candidate_SkillsMatrix] PRIMARY KEY CLUSTERED 
(
	[SkillsMatrixID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 97) ON [PRIMARY]
) ON [PRIMARY]
