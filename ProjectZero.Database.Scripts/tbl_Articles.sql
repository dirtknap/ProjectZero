﻿CREATE TABLE [dbo].[Articles]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY, 
    [Name] NVARCHAR(50) NOT NULL, 
    [Author] UNIQUEIDENTIFIER NOT NULL, 
    [Published] DATETIMEOFFSET NOT NULL, 
    [LastEdited] DATETIMEOFFSET NOT NULL, 
    [Teaser] NVARCHAR(240) NOT NULL, 
    [Active] BIT NULL
)
