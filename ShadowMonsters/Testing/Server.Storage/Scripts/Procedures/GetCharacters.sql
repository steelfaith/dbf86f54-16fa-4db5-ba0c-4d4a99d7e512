USE [ShadowMonsters]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [Users].[GetCharacters]
       @AccountId				int
AS

BEGIN
	SET NOCOUNT ON;

	SELECT * FROM Users.Characters WHERE AccountId =  @AccountId
END
GO


