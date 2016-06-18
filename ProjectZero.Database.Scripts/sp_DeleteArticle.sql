﻿CREATE PROCEDURE [dbo].[sp_DeleteArticle]
	@Id int = 0
AS
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
BEGIN TRANSACTION
	DELETE FROM [ArticleTags] WHERE [ArticleId] = @Id
	DELETE FROM [ArticleText] WHERE [ArticleId] = @Id
	DELETE FROM [Articles] WHERE [Id] = @Id
COMMIT TRANSACTION
