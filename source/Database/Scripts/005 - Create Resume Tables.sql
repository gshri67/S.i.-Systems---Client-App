
CREATE TABLE [dbo].[Candidate_ResumeInfo](
	[UserID] [int] NOT NULL,
	[ResumeText] [text] NULL,
	--[HourlyRate] [money] NULL,
	--[ReferenceValue] [int] NULL CONSTRAINT [DF_CandidateResumeInfo_ReferenceValue]  DEFAULT (5),
	--[ResumeVerified] [bit] NULL,
	--[InterviewedByUserID] [int] NULL,
	--[SkillsVerified] [bit] NULL,
	--[WorkedForSI] [bit] NULL,
	--[ResumeUpdateDate] [datetime] NOT NULL CONSTRAINT [DF_Candidate_ResumeInfo_ResumeUpdateDate]  DEFAULT (getdate()),
	--[ResumeUpdateUserID] [int] NOT NULL,
	--[zzDateAvailable] [smalldatetime] NULL,
	--[verticalid] [int] NULL,
	--[VideoBioUrl] [varchar](150) NULL,
 CONSTRAINT [PK_CandidateResumeInfo] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
) ON [PRIMARY]);

GO