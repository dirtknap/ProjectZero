CREATE PROCEDURE [dbo].[sp_UpdateArticle]
	@Id int = 0,
	@Name nvarchar(50),
	@LastEdited datetimeoffset,
	@Teaser nvarchar(240),
	@Active bit,
	@Text nvarchar(Max)
AS
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
BEGIN TRANSACTION
	DELETE FROM [ArticleTags] WHERE [ArticleId] = @Id
	UPDATE [Articles] SET Name = @Name, LastEdited = @LastEdited, Teaser = @Teaser, Active = @Active WHERE [Id] = @Id
	UPDATE [ArticleText] SET Text = @Text WHERE [ArticleId] = @Id
COMMIT TRANSACTION

