SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Users] (
    [Id]					INT            IDENTITY (1, 1) NOT NULL,
    [ClientId]				INT            NOT NULL,
    [FirstName]				NVARCHAR (MAX) NOT NULL,
    [LastName]				NVARCHAR (MAX) NOT NULL,
    [EmailAddress]			NVARCHAR (MAX) NOT NULL,
    [PasswordHash]			NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_Person] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO
