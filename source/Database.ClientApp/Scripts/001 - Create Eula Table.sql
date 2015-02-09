
CREATE TABLE [dbo].[Eula](
	[Version] [int] IDENTITY(1,1) NOT NULL,
	[Text] [nvarchar](MAX) NOT NULL,
	[PublishedDate] [datetime] NOT NULL
	CONSTRAINT [PK_Eulas] PRIMARY KEY CLUSTERED 
	(
		[Version] ASC
	) ON [PRIMARY]
);
GO
