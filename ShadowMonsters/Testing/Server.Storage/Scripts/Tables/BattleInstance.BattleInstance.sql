USE [ShadowMonsters]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [BattleInstance].[BattleInstance](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CharacterId] [int] NOT NULL,
 CONSTRAINT [PK_BattleInstance_Id] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)
GO

ALTER TABLE [BattleInstance].[BattleInstance]  WITH CHECK ADD  CONSTRAINT [FK_BattleInstance_Character_CharacterId] FOREIGN KEY([Id])
REFERENCES [Users].[Account] ([Id])
GO

ALTER TABLE [BattleInstance].[BattleInstance] CHECK CONSTRAINT [FK_BattleInstance_Character_CharacterId]
GO


