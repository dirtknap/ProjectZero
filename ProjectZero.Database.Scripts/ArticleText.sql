CREATE TABLE [dbo].[ArticleText]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [ArticleId] INT NOT NULL, 
    [Text] VARCHAR(MAX) NOT NULL, 
    CONSTRAINT [FK_ArticleText_ToArticles] FOREIGN KEY ([ArticleId]) REFERENCES [Articles]([Id])
)
