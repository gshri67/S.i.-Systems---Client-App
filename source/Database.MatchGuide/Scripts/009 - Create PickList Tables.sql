/*
	************************************************************ Create Pick Type Table ******************************************************
*/

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[PickType](
	[PickTypeID] [int] IDENTITY(1,1) NOT NULL,
	[Type] [varchar](50) NOT NULL,
	[Title] [varchar](50) NOT NULL,
	[Inactive] [bit] NOT NULL CONSTRAINT [DF_PickType_Inactive]  DEFAULT (0),
	[verticalid] [int] NULL,
 CONSTRAINT [PK_PickType] PRIMARY KEY CLUSTERED 
(
	[PickTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[PickType]  WITH CHECK ADD  CONSTRAINT [chkverticalid_PickType] CHECK  (([verticalid] IS NOT NULL AND [verticalid]<>(0)))
GO

ALTER TABLE [dbo].[PickType] CHECK CONSTRAINT [chkverticalid_PickType]
GO



/*
	************************************************************ Create Pick List Table ******************************************************
*/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[PickList](
	[PickListID] [int] IDENTITY(1,1) NOT NULL,
	[PickTypeID] [int] NOT NULL,
	[Title] [varchar](100) NOT NULL,
	[isDefault] [bit] NOT NULL,
	[isOrder] [int] NOT NULL,
	[Var1] [varchar](100) NULL,
	[Var2] [varchar](300) NULL,
	[Var3] [int] NULL,
	[Var4] [int] NULL,
	[Var5] [varchar](100) NULL,
	[Var6] [int] NULL,
	[Inactive] [bit] NOT NULL CONSTRAINT [DF_PickList_Inactive]  DEFAULT ((0)),
	[verticalid] [int] NULL,
	[DisplayTitle] [varchar](100) NULL,
 CONSTRAINT [PK_PickList] PRIMARY KEY CLUSTERED 
(
	[PickListID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[PickList]  WITH CHECK ADD  CONSTRAINT [FK_PickList_PickType] FOREIGN KEY([PickTypeID])
REFERENCES [dbo].[PickType] ([PickTypeID])
GO

ALTER TABLE [dbo].[PickList] CHECK CONSTRAINT [FK_PickList_PickType]
GO

ALTER TABLE [dbo].[PickList]  WITH CHECK ADD  CONSTRAINT [chkverticalid_PickList] CHECK  (([verticalid] IS NOT NULL AND [verticalid]<>(0)))
GO

ALTER TABLE [dbo].[PickList] CHECK CONSTRAINT [chkverticalid_PickList]
GO
