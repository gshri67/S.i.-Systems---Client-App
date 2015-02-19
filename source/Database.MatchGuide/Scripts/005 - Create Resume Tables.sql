
CREATE TABLE [dbo].[Candidate_ResumeInfo](
	[UserID] [int] NOT NULL,
	[ResumeText] [text] NULL,
	--[HourlyRate] [money] NULL,

	-- I THINK THAT THIS DEFAULT(5) MIGHT BE A BUG IN MATCHGUIDE.. dbo.picklist item with id=5 is 'Contact', which is used for user type...
	[ReferenceValue] [int] NULL CONSTRAINT [DF_CandidateResumeInfo_ReferenceValue]  DEFAULT (5), 
	
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


CREATE FULLTEXT CATALOG [SysFullText]WITH ACCENT_SENSITIVITY = OFF
AS DEFAULT

GO

-- Create a Full Text Index on Resume Text
CREATE FULLTEXT INDEX ON [dbo].[Candidate_ResumeInfo](
[ResumeText] LANGUAGE 'English')
KEY INDEX [PK_CandidateResumeInfo] ON ([SysFullText])
WITH (CHANGE_TRACKING = AUTO, STOPLIST = SYSTEM)


GO