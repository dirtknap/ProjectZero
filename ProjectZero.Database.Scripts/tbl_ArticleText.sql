CREATE TABLE [dbo].[ArticleText]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY, 
    [ArticleId] INT NOT NULL, 
    [Text] NVARCHAR(MAX) NOT NULL, 
    CONSTRAINT [FK_ArticleText_ToArticles] FOREIGN KEY ([ArticleId]) REFERENCES [Articles]([Id])
)
