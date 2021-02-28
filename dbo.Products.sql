CREATE TABLE [dbo].[Table]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Наименование] VARCHAR(50) NOT NULL, 
    [Категория] VARCHAR(50) NOT NULL, 
    [Производитель] VARCHAR(50) NOT NULL, 
    [Вес] INT NOT NULL, 
    [Дата изготовления] DATE NOT NULL, 
    [Стоимость] MONEY NOT NULL, 
    [Поступило] INT NOT NULL, 
    [Продано] INT NOT NULL
)
