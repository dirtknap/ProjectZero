CREATE TABLE [dbo].[ArticleTags]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY, 
    [ArticleId] INT NOT NULL, 
    [TagId] INT NOT NULL, 
    CONSTRAINT [FK_ArticleTags_ToArticles] FOREIGN KEY ([ArticleId]) REFERENCES [Articles]([Id]), 
    CONSTRAINT [FK_ArticleTags_ToTags] FOREIGN KEY ([TagId]) REFERENCES [Tags]([Id])
)
