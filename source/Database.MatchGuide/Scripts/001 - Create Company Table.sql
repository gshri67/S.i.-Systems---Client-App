
CREATE TABLE [dbo].[Company](
	[CompanyID] [int] IDENTITY(1,1) NOT NULL,
	[CompanyName] [varchar](150) NOT NULL
	CONSTRAINT [PK_Clients] PRIMARY KEY CLUSTERED 
	(
		[CompanyID] ASC
	) ON [PRIMARY]
);
GO
