CREATE PROCEDURE [dbo].[sp_UpdateArticle]
	@Id int = 0,
	@Name nvarchar(50),
	@Author uniqueidentifier,
	@Published datetimeoffset,
	@LastEdited datetimeoffset,
	@Teaser nvarchar(240),
	@Active bit,
	@Text nvarchar(Max),
	@Tags nvarchar(1000)
AS
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
BEGIN TRANSACTION
	DELETE FROM [ArticleTags] WHERE [ArticleId] = @Id
	UPDATE [Articles] SET WHERE [Id] = @Id
	UPDATE [ArticleText] WHERE [ArticleId] = @Id
COMMIT TRANSACTION

