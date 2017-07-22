USE [ShadowMonsters]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [Users].[Characters](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AccountId] [int] NOT NULL,
	[Name] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_Characters_Id] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)
GO

ALTER TABLE [Users].[Characters]  WITH CHECK ADD  CONSTRAINT [FK_Characters_Account_AccountId] FOREIGN KEY([AccountId])
REFERENCES [Users].[Account] ([Id])
GO

ALTER TABLE [Users].[Characters] CHECK CONSTRAINT [FK_Characters_Account_AccountId]
GO


