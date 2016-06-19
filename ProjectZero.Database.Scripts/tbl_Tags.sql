CREATE TABLE [dbo].[Tags]
(
	[Id] INT NOT NULL  IDENTITY(1,1) PRIMARY KEY, 
    [Text] NVARCHAR(50) NOT NULL, 
    CONSTRAINT [CK_Tags_Text] Unique (Text)
)
